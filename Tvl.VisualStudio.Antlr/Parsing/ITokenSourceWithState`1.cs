namespace Tvl.VisualStudio.Language.Parsing
{
    using Antlr.Runtime;

    public interface ITokenSourceWithState<T> : ITokenSource
    {
        ICharStream CharStream
        {
            get;
        }

        T GetCurrentState();
    }
}
