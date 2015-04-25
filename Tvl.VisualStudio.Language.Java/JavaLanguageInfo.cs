namespace Tvl.VisualStudio.Language.Java
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using Microsoft.VisualStudio.Text;
    using Tvl.VisualStudio.Language.Java.Debugger;
    using Tvl.VisualStudio.Shell;
    using Tvl.VisualStudio.Text;

    using IVsEditorAdaptersFactoryService = Microsoft.VisualStudio.Editor.IVsEditorAdaptersFactoryService;
    using IVsTextBuffer = Microsoft.VisualStudio.TextManager.Interop.IVsTextBuffer;
    using SVsServiceProvider = Microsoft.VisualStudio.Shell.SVsServiceProvider;
    using TextSpan = Microsoft.VisualStudio.TextManager.Interop.TextSpan;
    using VSConstants = Microsoft.VisualStudio.VSConstants;

    [Guid(Constants.JavaLanguageGuidString)]
    internal class JavaLanguageInfo : LanguageInfo
    {
        public JavaLanguageInfo(SVsServiceProvider serviceProvider)
            : base(serviceProvider, typeof(JavaLanguageInfo).GUID)
        {
        }

        public override string LanguageName
        {
            get
            {
                return Constants.JavaLanguageName;
            }
        }

        public override IEnumerable<string> FileExtensions
        {
            get
            {
                yield return Constants.JavaFileExtension;
            }
        }

        public override int ValidateBreakpointLocation(IVsTextBuffer buffer, int line, int col, TextSpan[] pCodeSpan)
        {
            var componentModel = ServiceProvider.GetComponentModel();
            var adapterFactoryService = componentModel.DefaultExportProvider.GetExport<IVsEditorAdaptersFactoryService>();
            ITextBuffer textBuffer = adapterFactoryService.Value.GetDataBuffer(buffer);

            ITextSnapshot snapshot = textBuffer.CurrentSnapshot;
            ITextSnapshotLine snapshotLine = snapshot.GetLineFromLineNumber(line);
            string lineText = snapshotLine.GetText();

            IList<IParseTree> statementTrees;
            IList<IToken> tokens;
            if (!LineStatementAnalyzer.TryGetLineStatements(textBuffer, line, out statementTrees, out tokens))
                return VSConstants.E_FAIL;

            IParseTree tree = null;
            for (int i = statementTrees.Count - 1; i >= 0; i--)
            {
                // want the last tree ending at or after col
                IParseTree current = statementTrees[i];
                if (current.SourceInterval.Length == 0)
                    continue;

                IToken token = tokens[current.SourceInterval.b];
                if (token.Line - 1 < line)
                    break;

                if (token.Line - 1 == line && (token.Column + token.StopIndex - token.StartIndex + 1) < col)
                    break;

                tree = current;
            }

            if (tree == null)
                return VSConstants.E_FAIL;

            IToken startToken = tokens[tree.SourceInterval.a];
            IToken stopToken = tokens[tree.SourceInterval.b];

            pCodeSpan[0].iStartLine = startToken.Line - 1;
            pCodeSpan[0].iStartIndex = startToken.Column;
            pCodeSpan[0].iEndLine = stopToken.Line - 1;
            pCodeSpan[0].iEndIndex = stopToken.Column + stopToken.StopIndex - stopToken.StartIndex + 1;
            return VSConstants.S_OK;
        }
    }
}
