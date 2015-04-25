namespace Tvl.VisualStudio.Language.Parsing
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.Text;

    [ContractClassFor(typeof(IJavaBackgroundParserFactoryService))]
    public abstract class IJavaBackgroundParserFactoryServiceContracts : IJavaBackgroundParserFactoryService
    {
        IBackgroundParser IJavaBackgroundParserFactoryService.GetBackgroundParser(ITextBuffer buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            throw new NotImplementedException();
        }
    }
}
