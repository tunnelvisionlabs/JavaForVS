namespace Tvl.VisualStudio.Language.Parsing4
{
    using System;
    using Antlr4.Runtime;
    using Tvl.Collections;
    using Contract = System.Diagnostics.Contracts.Contract;

    public struct SimpleLexerState : IEquatable<SimpleLexerState>
    {
        private static readonly int[] EmptyModeStack = { };
        private static readonly SimpleLexerState _initial = new SimpleLexerState(Lexer.DefaultMode);

        private readonly int _mode;
        private readonly int[] _modeStack;

        public SimpleLexerState(Lexer lexer)
        {
            _mode = lexer._mode;
            if (lexer._modeStack.Count == 0)
                _modeStack = EmptyModeStack;
            else
                _modeStack = lexer._modeStack.ToArray();
        }

        public SimpleLexerState(int mode)
            : this(mode, EmptyModeStack)
        {
        }

        private SimpleLexerState(int mode, int[] modeStack)
        {
            _mode = mode;
            _modeStack = modeStack;
        }

        public static SimpleLexerState Initial
        {
            get
            {
                return _initial;
            }
        }

        public int Mode
        {
            get
            {
                return _mode;
            }
        }

        public int[] ModeStack
        {
            get
            {
                return _modeStack;
            }
        }

        public void Apply(Lexer lexer)
        {
            lexer._mode = this.Mode;
            Contract.Assert(_modeStack != null);
            lexer._modeStack.AddRange(_modeStack ?? EmptyModeStack);
        }

        public bool Equals(SimpleLexerState other)
        {
            return _mode == other._mode
                && ArrayEqualityComparers.Int32.Equals(_modeStack, other._modeStack);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SimpleLexerState))
                return false;

            return Equals((SimpleLexerState)obj);
        }

        public override int GetHashCode()
        {
            int hashCode = 1;
            hashCode = hashCode * 31 + _mode;
            hashCode = hashCode * 31 + ArrayEqualityComparers.Int32.GetHashCode(_modeStack);
            return hashCode;
        }
    }
}
