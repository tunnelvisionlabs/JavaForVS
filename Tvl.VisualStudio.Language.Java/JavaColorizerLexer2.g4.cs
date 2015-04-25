namespace Tvl.VisualStudio.Language.Java
{
    using Antlr4.Runtime;
    using Tvl.VisualStudio.Language.Parsing4;

    partial class JavaColorizerLexer2 : ITokenSourceWithState<SimpleLexerState>
    {
        private const string DocCommentStartSymbols = "$@&~<>#%\"\\";

        private static readonly bool AssertSupported = true;
        private static readonly bool EnumSupported = true;

        private static bool IsDocCommentStartCharacter(int c)
        {
            if (char.IsLetter((char)c))
                return true;

            return DocCommentStartSymbols.IndexOf((char)c) >= 0;
        }

        public ICharStream CharStream
        {
            get
            {
                return (ICharStream)InputStream;
            }
        }

        public SimpleLexerState GetCurrentState()
        {
            return new SimpleLexerState(this);
        }

        public override IToken NextToken()
        {
            IToken token = base.NextToken();
            while (token.Type == NEWLINE)
                token = base.NextToken();

            return token;
        }
    }
}
