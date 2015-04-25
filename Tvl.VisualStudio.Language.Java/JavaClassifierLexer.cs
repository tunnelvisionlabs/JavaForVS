namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Diagnostics.Contracts;
    using Antlr.Runtime;
    using Tvl.VisualStudio.Language.Parsing;

    internal class JavaClassifierLexer : ITokenSourceWithState<JavaClassifierLexerState>
    {
        private readonly ICharStream _input;
        private readonly JavaColorizerLexer _languageLexer;
        private readonly JavaDocCommentClassifierLexer _commentLexer;

        private JavaClassifierLexerMode _mode;
        private bool _inComment;

        public JavaClassifierLexer(ICharStream input)
            : this(input, JavaClassifierLexerState.Initial)
        {
            Contract.Requires(input != null);
        }

        public JavaClassifierLexer(ICharStream input, JavaClassifierLexerState state)
        {
            Contract.Requires<ArgumentNullException>(input != null, "input");

            _input = input;
            _languageLexer = new JavaColorizerLexer(input, this);
            _commentLexer = new JavaDocCommentClassifierLexer(input, this);

            State = state;
        }

        public ICharStream CharStream
        {
            get
            {
                return _input;
            }
        }

        public string SourceName
        {
            get
            {
                return "Java Classifier";
            }
        }

        public string[] TokenNames
        {
            get
            {
                return _languageLexer.TokenNames;
            }
        }

        internal JavaClassifierLexerState State
        {
            get
            {
                return new JavaClassifierLexerState(_mode, _inComment);
            }

            set
            {
                _mode = value.Mode;
                _inComment = value.InComment;
            }
        }

        internal JavaClassifierLexerMode Mode
        {
            get
            {
                return _mode;
            }

            set
            {
                _mode = value;
            }
        }

        internal bool InComment
        {
            get
            {
                return _inComment;
            }

            set
            {
                _inComment = value;
            }
        }

        public JavaClassifierLexerState GetCurrentState()
        {
            return State;
        }

        public IToken NextToken()
        {
            IToken token = null;
            do
            {
                int position = _input.Index;
                token = NextTokenCore();
                // ensure progress
                if (position == _input.Index)
                    _input.Consume();
            } while (token == null || token.Type == JavaColorizerLexer.NEWLINE);

            return token;
        }

        private IToken NextTokenCore()
        {
            IToken token = null;

            switch (Mode)
            {
            case JavaClassifierLexerMode.JavaDocComment:
                token = _commentLexer.NextToken();

                switch (token.Type)
                {
                case JavaDocCommentClassifierLexer.END_COMMENT:
                    _mode = JavaClassifierLexerMode.JavaCode;
                    token.Type = JavaDocCommentClassifierLexer.DOC_COMMENT_TEXT;
                    break;

                default:
                    break;
                }

                break;

            case JavaClassifierLexerMode.JavaCode:
            default:
                token = _languageLexer.NextToken();

                switch (token.Type)
                {
                case JavaColorizerLexer.DOC_COMMENT_START:
                    _mode = JavaClassifierLexerMode.JavaDocComment;
                    token.Type = JavaDocCommentClassifierLexer.DOC_COMMENT_TEXT;
                    break;

                default:
                    break;
                }

                break;
            }

            return token;
        }
    }
}
