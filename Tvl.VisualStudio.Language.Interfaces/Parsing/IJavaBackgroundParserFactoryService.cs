namespace Tvl.VisualStudio.Language.Parsing
{
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.Text;

    [ContractClass(typeof(IJavaBackgroundParserFactoryServiceContracts))]
    public interface IJavaBackgroundParserFactoryService
    {
        IBackgroundParser GetBackgroundParser(ITextBuffer buffer);
    }
}
