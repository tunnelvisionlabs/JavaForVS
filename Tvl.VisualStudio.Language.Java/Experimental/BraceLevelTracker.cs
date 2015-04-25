namespace Tvl.VisualStudio.Language.Java.Experimental
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Antlr.Runtime;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Tvl.VisualStudio.Language.Parsing;
    using Tvl.VisualStudio.Language.Parsing.Collections;

    internal class BraceLevelTracker : AntlrTaggerBase<BraceLevelTracker.BraceLevelAnchorState, IClassificationTag>
    {
        private readonly ITextBuffer _textBuffer;
        private readonly IntervalSet _dirtySpans = new IntervalSet();

        public BraceLevelTracker(ITextBuffer textBuffer, ClassifierOptions options)
            : base(textBuffer)
        {
            Contract.Requires<ArgumentNullException>(textBuffer != null, "textBuffer");
        }

        protected override BraceLevelAnchorState GetStartState()
        {
            return BraceLevelAnchorState.Initial;
        }

        protected override ITokenSourceWithState<BraceLevelAnchorState> CreateLexer(SnapshotSpan span, BraceLevelAnchorState state)
        {
            SnapshotCharStream input = new SnapshotCharStream(span.Snapshot, span.Span);
            return new JavaClassifierLexerWrapper<BraceLevelAnchorState>(input, state);
        }

        protected override IEnumerable<ITagSpan<IClassificationTag>> GetTagSpansForToken(IToken token, ITextSnapshot snapshot)
        {
            // TODO: support LT(x) operations here (with caching in the base class)
            return base.GetTagSpansForToken(token, snapshot);
        }

        protected override bool TryClassifyToken(IToken token, out IClassificationTag tag)
        {
            switch (token.Type)
            {
            case JavaColorizerLexer.LBRACE:
            case JavaColorizerLexer.RBRACE:
                tag = null;
                return false;

            default:
                tag = null;
                return false;
            }
        }

        public struct BraceLevelAnchorState : ITextLineState<BraceLevelAnchorState>, IJavaClassifierLexerState<BraceLevelAnchorState>
        {
            public static readonly BraceLevelAnchorState Initial = new BraceLevelAnchorState(JavaClassifierLexerState.Initial, false, false);

            private readonly JavaClassifierLexerState _lexerState;
            private readonly bool _isDirty;
            private readonly bool _isMultiline;

            public BraceLevelAnchorState(JavaClassifierLexerState lexerState, bool isDirty, bool isMultiline)
            {
                _lexerState = lexerState;
                _isDirty = isDirty;
                _isMultiline = isMultiline;
            }

            public JavaClassifierLexerState LexerState
            {
                get
                {
                    return _lexerState;
                }
            }

            public bool IsDirty
            {
                get
                {
                    return _isDirty;
                }
            }

            public bool IsMultiline
            {
                get
                {
                    return _isMultiline;
                }
            }

            public BraceLevelAnchorState CreateDirtyState()
            {
                return new BraceLevelAnchorState(this.LexerState, true, this.IsMultiline);
            }

            public BraceLevelAnchorState CreateMultilineState()
            {
                return new BraceLevelAnchorState(this.LexerState, this.IsDirty, true);
            }

            public BraceLevelAnchorState CreateFromLexerState(JavaClassifierLexerState lexerState)
            {
                return new BraceLevelAnchorState(lexerState, false, false);
            }
        }
    }
}
