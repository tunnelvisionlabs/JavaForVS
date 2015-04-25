namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;
    using Microsoft.VisualStudio;
    using Tvl.VisualStudio.Language.Java.SourceData.Emit;
    using Tvl.VisualStudio.Language.Parsing;
    using Tvl.VisualStudio.Shell;
    using Tvl.VisualStudio.OutputWindow.Interfaces;

    using CancellationToken = System.Threading.CancellationToken;
    using Contract = System.Diagnostics.Contracts.Contract;
    using Directory = System.IO.Directory;
    using File = System.IO.File;
    using ReaderWriterLockSlim = System.Threading.ReaderWriterLockSlim;
    using SearchOption = System.IO.SearchOption;
    using SVsServiceProvider = Microsoft.VisualStudio.Shell.SVsServiceProvider;
    using TaskScheduler = System.Threading.Tasks.TaskScheduler;

    [Export]
    public class IntelliSenseCache
    {
        private readonly ReaderWriterLockSlim _updateLock = new ReaderWriterLockSlim();

        /* code files are unique on both of the following:
         *  1. The physical path of their file
         *  2. The project the file is a member of along with its item id
         *
         * since the project doesn't store different files at the same physical path, the
         * cache itself only distinguishes between files based on their physical path. files
         * which are part of a specific project are actually wrappers around the original
         * CodePhysicalFile designed to allow project-specific name resolution within the
         * file. the comparer *is* case-sensitive, so the keys of files located on
         * case-insensitive file systems should be converted to lowercase before being added.
         */
        private readonly Dictionary<string, CodePhysicalFile> _files =
            new Dictionary<string, CodePhysicalFile>(StringComparer.Ordinal);

        /* code elements within files are unique on all of the following:
         *  1. Their virtual file, consisting of:
         *     a. The file's physical path
         *     b. The project the file is a member of along with its item id
         *  2. The beginning of the seek location of the element
         */

        /* the keys in this dictionary are the case-insensitive unqualified type names
         * without any generic type parameters. the value sets are unique per the code
         * element description above.
         */
        private readonly Dictionary<string, HashSet<CodeType>> _types =
            new Dictionary<string, HashSet<CodeType>>(StringComparer.OrdinalIgnoreCase);

        /* the keys in this dictionary are the case-insensitive unqualified type names
         * without any generic type parameters. the value sets are unique per the code
         * element description above.
         */
        private readonly SortedDictionary<string, HashSet<CodePhysicalFile>> _packages =
            new SortedDictionary<string, HashSet<CodePhysicalFile>>(StringComparer.Ordinal);

        private readonly SortedDictionary<string, HashSet<CodePhysicalFile>> _packagesIgnoreCase =
            new SortedDictionary<string, HashSet<CodePhysicalFile>>(StringComparer.OrdinalIgnoreCase);

        private readonly object _backgroundParseSyncObject = new object();
        private volatile bool _backgroundParseStarted;

        [Import(PredefinedTaskSchedulers.ProjectCacheIntelliSense)]
        public TaskScheduler ProjectCacheIntelliSenseTaskScheduler
        {
            get;
            private set;
        }

        [Import]
        public IOutputWindowService OutputWindowService
        {
            get;
            private set;
        }

        [Import]
        public SVsServiceProvider GlobalServiceProvider
        {
            get;
            private set;
        }

        public string[] GetPackageNames(bool caseSensitive)
        {
            using (_updateLock.ReadLock())
            {
                IDictionary<string, HashSet<CodePhysicalFile>> packages = caseSensitive ? _packages : _packagesIgnoreCase;
                return packages.Keys.ToArray();
            }
        }

        public CodePackage[] GetPackages()
        {
            HashSet<string> packageNames = new HashSet<string>(StringComparer.Ordinal);

            using (_updateLock.ReadLock())
            {
                foreach (var packageName in _packages.Keys)
                {
                    int dot = packageName.IndexOf('.');
                    if (dot >= 0)
                        packageNames.Add(packageName.Substring(0, dot));
                    else
                        packageNames.Add(packageName);
                }
            }

            return packageNames.Select(i => new CodePackage(this, i, i, CodeElement.Intrinsic)).ToArray();
        }

        public CodePackage[] ResolvePackage(string packageName, bool caseSensitive)
        {
            if (caseSensitive)
            {
                using (_updateLock.ReadLock())
                {
                    if (!_packages.ContainsKey(packageName))
                    {
                        string prefix = packageName + '.';
                        if (!_packages.Keys.Any(i => i.StartsWith(prefix)))
                            return new CodePackage[0];
                    }
                }

                CodeElement parentPackage = CodeElement.Intrinsic;
                string name = packageName.Substring(packageName.LastIndexOf('.') + 1);
                if (name.Length < packageName.Length)
                    parentPackage = ResolvePackage(packageName.Substring(0, packageName.LastIndexOf('.')), true).Single();

                return new CodePackage[] { new CodePackage(this, name, packageName, parentPackage) };
            }
            else
            {
                // get the case-sensitive package names matching this name
                HashSet<string> matchingNames = new HashSet<string>(StringComparer.Ordinal);
                CodePhysicalFile[] files = GetPackageFiles(packageName, caseSensitive);
                foreach (var file in files)
                {
                    CodePackageStatement packageStatement = file.Children.OfType<CodePackageStatement>().FirstOrDefault();
                    if (packageStatement != null)
                        matchingNames.Add(packageStatement.FullName);
                }

                return matchingNames.SelectMany(name => ResolvePackage(name, true)).ToArray();
            }
        }

        public CodePhysicalFile[] GetPackageFiles(string packageName, bool caseSensitive)
        {
            using (_updateLock.ReadLock())
            {
                HashSet<CodePhysicalFile> files;
                if (caseSensitive && !_packages.TryGetValue(packageName, out files))
                    return new CodePhysicalFile[0];
                else if (!_packagesIgnoreCase.TryGetValue(packageName, out files))
                    return new CodePhysicalFile[0];

                return files.ToArray();
            }
        }

        public CodeType[] GetTypes(string unqualifiedName, bool caseSensitive)
        {
            using (_updateLock.ReadLock())
            {
                HashSet<CodeType> types;
                if (!_types.TryGetValue(unqualifiedName, out types))
                    return new CodeType[0];

                if (!caseSensitive)
                    return types.ToArray();

                return types.Where(i => StringComparer.Ordinal.Equals(unqualifiedName, i.Name)).ToArray();
            }
        }

        public void BeginReferenceSourceParsing()
        {
            if (_backgroundParseStarted)
                return;

            lock (_backgroundParseSyncObject)
            {
                if (_backgroundParseStarted)
                    return;

                _backgroundParseStarted = true;
            }

            JavaLanguagePackage package = JavaLanguagePackage.Instance ?? GlobalServiceProvider.GetShell().LoadPackage<JavaLanguagePackage>();
            if (!package.IntellisenseOptions.ParseJreSource || !Directory.Exists(package.IntellisenseOptions.JreSourcePath))
            {
                _backgroundParseStarted = false;
                return;
            }

            var task = Task.Factory.StartNew(QueueReferenceSourceParseTasks, package.IntellisenseOptions.JreSourcePath, CancellationToken.None, TaskCreationOptions.None, ProjectCacheIntelliSenseTaskScheduler);
            task.HandleNonCriticalExceptions();
        }

        private void QueueReferenceSourceParseTasks(object sourcePathObject)
        {
            string sourcePath = sourcePathObject as string;
            if (string.IsNullOrEmpty(sourcePath))
                return;

            IEnumerable<string> files = Directory.EnumerateFiles(sourcePath, "*.java", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                Task parseTask = Task.Factory.StartNew(ParseReferenceSourceFile, file, CancellationToken.None, TaskCreationOptions.None, ProjectCacheIntelliSenseTaskScheduler);
                parseTask.HandleNonCriticalExceptions();
            }
        }

        private void ParseReferenceSourceFile(object state)
        {
            string fileName = state as string;
            if (string.IsNullOrEmpty(fileName))
                return;

            try
            {
                if (!File.Exists(fileName))
                    return;

                string sourceText = File.ReadAllText(fileName);
                var inputStream = new ANTLRStringStream(sourceText, fileName);
                var unicodeStream = new JavaUnicodeStream(inputStream);
                var lexer = new Java2Lexer(unicodeStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new Java2Parser(tokenStream);

                int[] lineOffsets = null;

                var outputWindow = OutputWindowService.TryGetPane(PredefinedOutputWindowPanes.TvlIntellisense);
                List<ParseErrorEventArgs> errors = new List<ParseErrorEventArgs>();
                parser.ParseError += (sender, e) =>
                    {
                        errors.Add(e);

                        string message = e.Message;
                        if (message.Length > 100)
                            message = message.Substring(0, 100) + " ...";

                        if (lineOffsets == null)
                            lineOffsets = FindLineOffsets(sourceText);

                        int line = Array.BinarySearch(lineOffsets, e.Span.Start);
                        if (line < 0)
                            line = -line - 2;

                        int column;
                        if (line >= lineOffsets.Length)
                            column = 0;
                        else
                            column = e.Span.Start - lineOffsets[line];

                        if (outputWindow != null)
                            outputWindow.WriteLine(string.Format("{0}({1},{2}): {3}", fileName, line + 1, column + 1, message));

                        if (errors.Count > 20)
                            throw new OperationCanceledException();
                    };

                var result = parser.compilationUnit();

                CodeFileBuilder fileBuilder = new CodeFileBuilder(fileName);
                var treeNodeStream = new CommonTreeNodeStream(result.Tree);
                treeNodeStream.TokenStream = tokenStream;
                var walker = new IntelliSenseCacheWalker(treeNodeStream);
                List<ParseErrorEventArgs> walkerErrors = new List<ParseErrorEventArgs>();
                walker.ParseError += (sender, e) =>
                {
                    walkerErrors.Add(e);

                    string message = e.Message;
                    if (message.Length > 100)
                        message = message.Substring(0, 100) + " ...";

                    if (lineOffsets == null)
                        lineOffsets = FindLineOffsets(sourceText);

                    int line = Array.BinarySearch(lineOffsets, e.Span.Start);
                    if (line < 0)
                        line = -line - 2;

                    int column;
                    if (line >= lineOffsets.Length)
                        column = 0;
                    else
                        column = e.Span.Start - lineOffsets[line];

                    if (outputWindow != null)
                        outputWindow.WriteLine(string.Format("{0}({1},{2}): {3}", fileName, line + 1, column + 1, message));

                    if (walkerErrors.Count > 20)
                        throw new OperationCanceledException();
                };

                walker.compilationUnit(fileBuilder);

                UpdateFile(fileBuilder);
            }
            catch (Exception e)
            {
                if (ErrorHandler.IsCriticalException(e))
                    throw;
            }
        }

        private int[] FindLineOffsets(string sourceText)
        {
            List<int> offsets = new List<int>();
            offsets.Add(0);

            for (int i = 0; i < sourceText.Length; i++)
            {
                char c = sourceText[i];
                if (c != '\r' && c != '\n')
                    continue;

                if (c == '\r' && i < sourceText.Length - 1 && sourceText[i + 1] == '\n')
                    i++;

                offsets.Add(i + 1);
            }

            return offsets.ToArray();
        }

        private void UpdateFile(CodeFileBuilder fileBuilder)
        {
            Contract.Requires(fileBuilder != null);

            CodeElement fileElement = fileBuilder.BuildElement(null);
            CodePhysicalFile file = fileElement as CodePhysicalFile;
            if (file == null)
                throw new NotSupportedException();

            CodeElement[] elementsToAdd = file.GetDescendents(true).ToArray();

            // apply changes to the cache
            using (_updateLock.WriteLock())
            {
                CodePhysicalFile existingFile;
                if (_files.TryGetValue(file.FullName, out existingFile))
                {
                    // remove the file
                    CodeElement[] elementsToRemove = existingFile.GetDescendents(true).ToArray();
                    foreach (var type in elementsToRemove.OfType<CodeType>())
                        _types[type.Name].Remove(type);

                    string previousPackage = string.Empty;
                    CodePackageStatement previousPackageStatement = elementsToRemove.OfType<CodePackageStatement>().FirstOrDefault();
                    if (previousPackageStatement != null)
                        previousPackage = previousPackageStatement.FullName;

                    _packages[previousPackage].Remove(existingFile);
                    _packagesIgnoreCase[previousPackage].Remove(existingFile);
                    _files.Remove(existingFile.FullName);
                }

                // now add the new file
                foreach (var type in elementsToAdd.OfType<CodeType>())
                {
                    HashSet<CodeType> types;
                    if (!_types.TryGetValue(type.Name, out types))
                    {
                        types = new HashSet<CodeType>(CodeElementLocationEqualityComparer.Default);
                        _types.Add(type.Name, types);
                    }

                    types.Add(type);
                }

                string package = string.Empty;
                CodePackageStatement packageStatement = elementsToAdd.OfType<CodePackageStatement>().FirstOrDefault();
                if (packageStatement != null)
                    package = packageStatement.FullName;

                HashSet<CodePhysicalFile> packageFiles;
                if (!_packages.TryGetValue(package, out packageFiles))
                {
                    packageFiles = new HashSet<CodePhysicalFile>(ObjectReferenceEqualityComparer<CodePhysicalFile>.Default);
                    _packages.Add(package, packageFiles);
                }

                packageFiles.Add(file);

                if (!_packagesIgnoreCase.TryGetValue(package, out packageFiles))
                {
                    packageFiles = new HashSet<CodePhysicalFile>(ObjectReferenceEqualityComparer<CodePhysicalFile>.Default);
                    _packagesIgnoreCase.Add(package, packageFiles);
                }

                packageFiles.Add(file);

                _files.Add(file.FullName, file);
            }
        }

        private class CodeElementLocationEqualityComparer : IEqualityComparer<CodeElement>
        {
            private static readonly CodeElementLocationEqualityComparer _default = new CodeElementLocationEqualityComparer();

            private CodeElementLocationEqualityComparer()
            {
            }

            public static CodeElementLocationEqualityComparer Default
            {
                get
                {
                    return _default;
                }
            }

            public bool Equals(CodeElement x, CodeElement y)
            {
                if (object.ReferenceEquals(x, y))
                    return true;
                if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
                    return false;

                CodeLocation xloc = x.Location;
                CodeLocation yloc = y.Location;
                return xloc.FileName == yloc.FileName
                    && xloc.Seek == yloc.Seek
                    && xloc.Span == yloc.Span;
            }

            public int GetHashCode(CodeElement obj)
            {
                if (obj == null)
                    return 0;

                return obj.Location.GetHashCode() ^ obj.Location.Span.GetHashCode() ^ obj.Location.Seek.GetHashCode();
            }
        }
    }
}
