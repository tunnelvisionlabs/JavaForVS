namespace Tvl.Java.BuildTasks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Tasks;
    using Microsoft.Build.Utilities;
    using Directory = System.IO.Directory;
    using File = System.IO.File;
    using Path = System.IO.Path;

    public class Javac : JavaToolTask
    {
        private readonly List<ITaskItem> _generatedClassFiles = new List<ITaskItem>();
        private readonly List<string> _previousErrorLines = new List<string>();

        public Javac()
        {
            Verbose = false;
        }

        public bool Verbose
        {
            get;
            set;
        }

        public string OutputPath
        {
            get;
            set;
        }

        public string SourceRelease
        {
            get;
            set;
        }

        public string TargetRelease
        {
            get;
            set;
        }

        public string Encoding
        {
            get;
            set;
        }

        public string DebugSymbols
        {
            get;
            set;
        }

        public string SpecificDebugSymbols
        {
            get;
            set;
        }

        public bool ShowWarnings
        {
            get;
            set;
        }

        public bool ShowAllWarnings
        {
            get;
            set;
        }

        public string BuildArgs
        {
            get;
            set;
        }

        public string[] ClassPath
        {
            get;
            set;
        }

        public ITaskItem[] References
        {
            get;
            set;
        }

        public ITaskItem[] ResponseFiles
        {
            get;
            set;
        }

        public ITaskItem[] Sources
        {
            get;
            set;
        }

        [Output]
        public ITaskItem[] GeneratedClassFiles
        {
            get
            {
                return _generatedClassFiles.ToArray();
            }
        }

        protected override string ToolName
        {
            get
            {
                return "javac.exe";
            }
        }

        protected override void AddCommandLineCommands(CommandLineBuilderExtension commandLine)
        {
        }

        protected override void AddResponseFileCommands(CommandLineBuilderExtension commandLine)
        {
            // the -verbose flag must be included or there's no way to figure out what the output files are
            commandLine.AppendSwitch("-verbose");

            if (!string.IsNullOrEmpty(Encoding))
                commandLine.AppendSwitchIfNotNull("-encoding ", Encoding);

            switch (DebugSymbols)
            {
            case "All":
                commandLine.AppendSwitch("-g");break;

            case "None":
                commandLine.AppendSwitch("-g:none");break;

            case "Specific":
                if (!string.IsNullOrEmpty(SpecificDebugSymbols))
                    commandLine.AppendSwitchIfNotNull("-g:", SpecificDebugSymbols);
                else
                    commandLine.AppendSwitch("-g:none");

                break;

            case "Default":
            default:
                break;
            }

            if (!string.IsNullOrEmpty(SourceRelease) && !string.Equals(SourceRelease, "Default", StringComparison.OrdinalIgnoreCase))
                commandLine.AppendSwitchIfNotNull("-source ", SourceRelease);
            if (!string.IsNullOrEmpty(TargetRelease) && !string.Equals(TargetRelease, "Default", StringComparison.OrdinalIgnoreCase))
                commandLine.AppendSwitchIfNotNull("-target ", TargetRelease);

            if (!string.IsNullOrEmpty(OutputPath))
                commandLine.AppendSwitchIfNotNull("-d ", OutputPath);

            if (!ShowWarnings)
            {
                commandLine.AppendSwitch("-nowarn");
            }
            else if (ShowAllWarnings)
            {
                commandLine.AppendSwitch("-Xlint");
                commandLine.AppendSwitch("-deprecation");
            }

            if (!string.IsNullOrEmpty(BuildArgs))
                commandLine.AppendTextUnquoted(" " + BuildArgs);

            // reference paths
            List<string> referencePaths = new List<string>();
            foreach (var reference in (References ?? Enumerable.Empty<ITaskItem>()))
            {
                string path = GetReferencePath(reference);
                if (!string.IsNullOrEmpty(path))
                    referencePaths.Add(path);
            }

            if (referencePaths.Count > 0)
            {
                commandLine.AppendSwitchIfNotNull("-cp ", referencePaths.ToArray(), ";");
            }

            commandLine.AppendSwitchIfNotNull("-classpath ", ClassPath, ";");

            commandLine.AppendFileNamesIfNotNull(Sources, " ");
        }

        private string GetReferencePath(ITaskItem reference)
        {
            string path = reference.ItemSpec;
            if (File.Exists(reference.ItemSpec) && Path.GetExtension(reference.ItemSpec).Equals(".jar", StringComparison.OrdinalIgnoreCase))
                return path;

            if (Directory.Exists(Path.GetDirectoryName(reference.ItemSpec)))
                return Path.GetDirectoryName(reference.ItemSpec);

            return null;
        }

        private static readonly Regex CompileMessageFormat = new Regex(@"^(?<File>[\w\\/\.\-_\:]+):(?<Line>[0-9]+):(?<Warning> warning:)? (?:\[(?<Category>\w+)\] )?(?<Message>.*)$", RegexOptions.Compiled);

        protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
        {
            if (_previousErrorLines.Count > 0)
            {
                if (singleLine.Trim() == "^")
                {
                    Match result = CompileMessageFormat.Match(_previousErrorLines[0]);
                    Contract.Assert(result.Success);

                    string subcategory = null;
                    string warningCode = null;
                    string helpKeyword = null;
                    string file = null;

                    int lineNumber = 0;
                    int columnNumber = 0;
                    int endLineNumber = 0;
                    int endColumnNumber = 0;

                    string message = null;
                    object[] messageArgs = null;

                    Group fileGroup = result.Groups["File"];
                    Group lineGroup = result.Groups["Line"];
                    Group warningGroup = result.Groups["Warning"];
                    Group categoryGroup = result.Groups["Category"];
                    Group messageGroup = result.Groups["Message"];

                    file = Path.GetFullPath(fileGroup.Value);

                    message = messageGroup.Value;
                    if (_previousErrorLines.Count > 2)
                        message += ", " + string.Join(", ", _previousErrorLines.Skip(1).Take(_previousErrorLines.Count - 2));

                    if (categoryGroup.Success)
                        subcategory = categoryGroup.Value;

                    if (!int.TryParse(lineGroup.Value, out lineNumber))
                        lineNumber = 0;
                    endLineNumber = lineNumber;

                    columnNumber = singleLine.IndexOf('^');
                    endColumnNumber = columnNumber;

                    //Log.LogWarning(subcategory, warningCode, helpKeyword, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, messageArgs);
                    Action<string, string, string, string, int, int, int, int, string, object[]> logFunction;
                    if (warningGroup.Success)
                        logFunction = Log.LogWarning;
                    else
                        logFunction = Log.LogError;

                    logFunction(subcategory, warningCode, helpKeyword, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, messageArgs);

                    _previousErrorLines.Clear();
                }
                else
                {
                    _previousErrorLines.Add(singleLine);
                }

                return;
            }

            if (Verbose && singleLine.StartsWith("[") && singleLine.EndsWith("]"))
            {
                base.LogEventsFromTextOutput(singleLine, messageImportance);
            }

            if (singleLine.StartsWith("Note: "))
            {
                Log.LogWarning("{0}", singleLine);
            }
            else if (singleLine.StartsWith("[wrote "))
            {
                int startIndex = "[wrote ".Length;
                string writeDetail = singleLine.Substring(startIndex, singleLine.Length - startIndex - 1);

                string outputFile;
                if (writeDetail.EndsWith("]"))
                {
                    // starting with Java 7, the writeDetail is actually wrapped in another set of brackets
                    startIndex = writeDetail.IndexOf('[');
                    outputFile = writeDetail.Substring(startIndex + 1, writeDetail.Length - startIndex - 2);
                }
                else
                {
                    outputFile = writeDetail;
                }

                TaskItem generated = new TaskItem(outputFile);
                generated.SetMetadata("BaseOutputDirectory", OutputPath);
                _generatedClassFiles.Add(generated);
            }
            else if (CompileMessageFormat.IsMatch(singleLine))
            {
                _previousErrorLines.Add(singleLine);
            }
            else if (!singleLine.StartsWith("[") || !singleLine.EndsWith("]"))
            {
                base.LogEventsFromTextOutput(singleLine, messageImportance);
            }
        }
    }
}
