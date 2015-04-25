namespace Tvl.VisualStudio.Language.Parsing4
{
    using Antlr4.Runtime;

    public interface ITokenSourceWithState<T> : ITokenSource
    {
        ICharStream CharStream
        {
            get;
        }

        T GetCurrentState();
    }
}
