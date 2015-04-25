namespace Tvl.VisualStudio.Language.Java
{
    using System;

    internal struct JavaClassifierLexerState : IEquatable<JavaClassifierLexerState>
    {
        public static readonly JavaClassifierLexerState Initial =
            new JavaClassifierLexerState(JavaClassifierLexerMode.JavaCode, false);

        public readonly JavaClassifierLexerMode Mode;
        public readonly bool InComment;

        public JavaClassifierLexerState(JavaClassifierLexerMode mode, bool inComment)
        {
            Mode = mode;
            InComment = inComment;
        }

        public bool Equals(JavaClassifierLexerState other)
        {
            return this.Mode == other.Mode
                && this.InComment == other.InComment;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is JavaClassifierLexerState))
                return false;

            return Equals((JavaClassifierLexerState)obj);
        }

        public override int GetHashCode()
        {
            return (int)Mode ^ (InComment ? 1 : 0);
        }
    }
}
