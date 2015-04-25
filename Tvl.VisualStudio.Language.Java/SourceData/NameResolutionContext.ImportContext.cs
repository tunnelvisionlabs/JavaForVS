namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System;

    partial class NameResolutionContext
    {
        private class ImportContext : NameResolutionContext
        {
            private readonly CodeImportStatement _importStatement;

            private readonly string _path;
            private readonly bool _importOnDemand;
            private readonly bool _staticImport;

            public ImportContext(IntelliSenseCache cache, CodeImportStatement importStatement)
                : this(cache, importStatement.FullName, importStatement.ImportOnDemand, importStatement.StaticImport)
            {
                _importStatement = importStatement;
            }

            public ImportContext(IntelliSenseCache cache, string path, bool importOnDemand, bool staticImport)
                : base(cache)
            {
                _path = path;
                _importOnDemand = importOnDemand;
                _staticImport = staticImport;
            }

            public override CodeElement[] GetMatchingElements()
            {
                throw new NotImplementedException();
            }

            public override NameResolutionContext Filter(string name, string @operator, bool caseSensitive)
            {
                throw new NotImplementedException();
            }
        }
    }
}
