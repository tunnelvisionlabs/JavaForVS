namespace Tvl.VisualStudio.Text
{
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.Text.Editor;

    [ContractClass(typeof(Contracts.IJavaCommenterProviderContracts))]
    public interface IJavaCommenterProvider
    {
        ICommenter GetCommenter(ITextView textView);
    }
}
