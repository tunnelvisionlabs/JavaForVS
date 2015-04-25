namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;
    using Microsoft.VisualStudio.Text;
    using Tvl.VisualStudio.Language.Parsing;
    using Interval = Tvl.VisualStudio.Language.Parsing.Collections.Interval;

    partial class IntelliSenseCacheWalker
    {
        public event EventHandler<ParseErrorEventArgs> ParseError;

        public override void DisplayRecognitionError(string[] tokenNames, RecognitionException e)
        {
            string header = GetErrorHeader(e);
            string message = GetErrorMessage(e, tokenNames);
            Span span = new Span();

            object positionNode = null;
            IPositionTrackingStream positionTrackingStream = input as IPositionTrackingStream;
            if (positionTrackingStream != null)
            {
                positionNode = positionTrackingStream.GetKnownPositionElement(false);
                if (positionNode == null)
                {
                    positionNode = positionTrackingStream.GetKnownPositionElement(true);
                }
            }

            if (positionNode != null)
            {
                IToken token = input.TreeAdaptor.GetToken(positionNode);
                if (token != null)
                    span = Span.FromBounds(token.StartIndex, token.StopIndex + 1);
            }
            else if (e.Token != null)
            {
                span = Span.FromBounds(e.Token.StartIndex, e.Token.StopIndex + 1);
            }

            ParseErrorEventArgs args = new ParseErrorEventArgs(message, span);
            OnParseError(args);

            base.DisplayRecognitionError(tokenNames, e);
        }

        protected virtual void OnParseError(ParseErrorEventArgs e)
        {
            var t = ParseError;
            if (t != null)
                t(this, e);
        }

        private static Interval GetSourceInterval(ITree tree, ITokenStream tokenStream)
        {
            Contract.Requires(tree != null);
            Contract.Requires(tokenStream != null);

            IToken firstToken = tokenStream.Get(tree.TokenStartIndex);
            IToken lastToken = tokenStream.Get(tree.TokenStopIndex);
            return Interval.FromBounds(firstToken.StartIndex, lastToken.StopIndex);
        }

        private static Interval GetSourceInterval(IToken token)
        {
            Contract.Requires(token != null);

            return Interval.FromBounds(token.StartIndex, token.StopIndex);
        }

        private Interval GetSourceInterval(IEnumerable<ITree> trees)
        {
            Contract.Requires(trees != null);

            IEnumerable<Interval> intervals = trees.Select(GetSourceInterval);
            int start = intervals.Min(i => i.Start);
            int endInclusive = intervals.Max(i => i.EndInclusive);
            return Interval.FromBounds(start, endInclusive);
        }

        private Interval GetSourceInterval(ITree tree)
        {
            Contract.Requires(tree != null);

            return GetSourceInterval(tree, this.input.TokenStream);
        }
    }
}
