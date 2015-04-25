namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Diagnostics.Contracts;
    using Antlr.Runtime;

    partial class JavaColorizerLexer
    {
        private readonly JavaClassifierLexer _lexer;

        public JavaColorizerLexer(ICharStream input, JavaClassifierLexer lexer)
            : this(input)
        {
            Contract.Requires<ArgumentNullException>(lexer != null, "lexer");

            _lexer = lexer;
        }

        private bool InComment
        {
            get
            {
                return _lexer.InComment;
            }

            set
            {
                _lexer.InComment = value;
            }
        }

        public override IToken NextToken()
        {
            IToken token = base.NextToken();

            switch (token.Type)
            {
            case CONTINUE_COMMENT:
                InComment = true;
                token.Type = ML_COMMENT;
                break;

            case END_COMMENT:
                InComment = false;
                token.Type = ML_COMMENT;
                break;

            default:
                break;
            }

            return token;
        }

        protected override void ParseNextToken()
        {
            if (InComment)
            {
                int la1 = input.LA(1);
                if (la1 != '\n' && la1 != '\r')
                {
                    mCONTINUE_COMMENT();
                    return;
                }
            }

            base.ParseNextToken();
        }

        public override void Recover(IIntStream input, RecognitionException re)
        {
            base.Recover(input, re);
        }

        public override void DisplayRecognitionError(string[] tokenNames, RecognitionException e)
        {
            base.DisplayRecognitionError(tokenNames, e);
        }
    }
}
