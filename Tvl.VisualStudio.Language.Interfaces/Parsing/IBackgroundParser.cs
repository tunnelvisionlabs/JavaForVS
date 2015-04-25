namespace Tvl.VisualStudio.Language.Parsing
{
    using System;

    public interface IBackgroundParser
    {
        event EventHandler<ParseResultEventArgs> ParseComplete;

        void RequestParse(bool forceReparse);
    }
}
