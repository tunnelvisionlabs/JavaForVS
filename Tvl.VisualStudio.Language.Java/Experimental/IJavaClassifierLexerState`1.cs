namespace Tvl.VisualStudio.Language.Java.Experimental
{
    using Tvl.VisualStudio.Language.Parsing;

    internal interface IJavaClassifierLexerState<TState>
        where TState : struct, ITextLineState<TState>
    {
        JavaClassifierLexerState LexerState
        {
            get;
        }

        TState CreateFromLexerState(JavaClassifierLexerState lexerState);
    }
}
