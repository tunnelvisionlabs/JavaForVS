namespace Tvl.VisualStudio.Language.Java.Experimental
{
    using Antlr.Runtime;
    using Tvl.VisualStudio.Language.Parsing;

    internal class JavaClassifierLexerWrapper<TState> : ITokenSourceWithState<TState>
        where TState : struct, ITextLineState<TState>, IJavaClassifierLexerState<TState>
    {
        private readonly JavaClassifierLexer _classifier;

        public JavaClassifierLexerWrapper(ICharStream input, TState state)
        {
            _classifier = new JavaClassifierLexer(input, state.LexerState);
        }

        public ICharStream CharStream
        {
            get
            {
                return _classifier.CharStream;
            }
        }

        public string SourceName
        {
            get
            {
                return _classifier.SourceName;
            }
        }

        public string[] TokenNames
        {
            get
            {
                return _classifier.TokenNames;
            }
        }

        public TState GetCurrentState()
        {
            return new TState().CreateFromLexerState(_classifier.GetCurrentState());
        }

        public IToken NextToken()
        {
            IToken token = _classifier.NextToken();
            switch (token.Type)
            {
            case JavaColorizerLexer.WS:
            case JavaColorizerLexer.NEWLINE:
            case JavaColorizerLexer.COMMENT:
            case JavaColorizerLexer.ML_COMMENT:
            case JavaDocCommentClassifierLexer.DOC_COMMENT_TEXT:
            case JavaDocCommentClassifierLexer.DOC_COMMENT_TAG:
            case JavaDocCommentClassifierLexer.DOC_COMMENT_INVALID_TAG:
                token.Channel = TokenChannels.Hidden;
                break;
            }

            return token;
        }
    }
}
