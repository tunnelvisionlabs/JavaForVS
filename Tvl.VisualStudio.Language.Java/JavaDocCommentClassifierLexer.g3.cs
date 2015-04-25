namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Diagnostics.Contracts;
    using Antlr.Runtime;

    partial class JavaDocCommentClassifierLexer
    {
        private const string DocCommentStartSymbols = "$@&~<>#%\"\\";

        private readonly JavaClassifierLexer _lexer;

        public JavaDocCommentClassifierLexer(ICharStream input, JavaClassifierLexer lexer)
            : this(input)
        {
            Contract.Requires<ArgumentNullException>(lexer != null, "lexer");

            _lexer = lexer;
        }

        private static bool IsDocCommentStartCharacter(int c)
        {
            if (char.IsLetter((char)c))
                return true;

            return DocCommentStartSymbols.IndexOf((char)c) >= 0;
        }
    }
}
