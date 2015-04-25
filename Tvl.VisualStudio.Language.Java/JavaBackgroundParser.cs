namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Antlr.Runtime;
    using Microsoft.VisualStudio.Text;
    using Tvl.VisualStudio.Language.Parsing;
    using Tvl.VisualStudio.OutputWindow.Interfaces;
    using Stopwatch = System.Diagnostics.Stopwatch;

    public class JavaBackgroundParser : BackgroundParser
    {
        private readonly JavaBackgroundParserProvider _provider;

        public JavaBackgroundParser(ITextBuffer textBuffer, JavaBackgroundParserProvider provider)
            : base(textBuffer, provider.BackgroundIntelliSenseTaskScheduler, provider.TextDocumentFactoryService, provider.OutputWindowService)
        {
            Contract.Requires<ArgumentNullException>(provider != null, "provider");

            _provider = provider;
        }

        protected override void ReParseImpl()
        {
            var outputWindow = OutputWindowService.TryGetPane(PredefinedOutputWindowPanes.TvlIntellisense);

            Stopwatch stopwatch = Stopwatch.StartNew();

            string filename = "<Unknown File>";
            ITextDocument textDocument = TextDocument;
            if (textDocument != null)
                filename = textDocument.FilePath;

            var snapshot = TextBuffer.CurrentSnapshot;
            ANTLRStringStream input = new ANTLRStringStream(snapshot.GetText());
            Java2Lexer lexer = new Java2Lexer(new JavaUnicodeStream(input));
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            Java2Parser parser = new Java2Parser(tokens);
            List<ParseErrorEventArgs> errors = new List<ParseErrorEventArgs>();
            parser.ParseError += (sender, e) =>
                {
                    errors.Add(e);

                    string message = e.Message;
                    if (message.Length > 100)
                        message = message.Substring(0, 100) + " ...";

                    ITextSnapshotLine startLine = snapshot.GetLineFromPosition(e.Span.Start);
                    int line = startLine.LineNumber;
                    int column = e.Span.Start - startLine.Start;

                    if (outputWindow != null)
                        outputWindow.WriteLine(string.Format("{0}({1},{2}): {3}", filename, line + 1, column + 1, message));

                    if (errors.Count > 100)
                        throw new OperationCanceledException();
                };

            var result = parser.compilationUnit();
            OnParseComplete(new AntlrParseResultEventArgs(snapshot, errors, stopwatch.Elapsed, tokens.GetTokens(), result));
        }
    }
}
