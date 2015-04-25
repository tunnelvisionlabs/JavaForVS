namespace Tvl.VisualStudio.Text.Classification.Contracts
{
    using System.Diagnostics.Contracts;
    using IClassifier = Microsoft.VisualStudio.Text.Classification.IClassifier;
    using ITextBuffer = Microsoft.VisualStudio.Text.ITextBuffer;
    using NotImplementedException = System.NotImplementedException;
    using Package = Microsoft.VisualStudio.Shell.Package;

    [ContractClassFor(typeof(LanguageClassifierProvider<>))]
    internal abstract class LanguageClassifierProviderContracts<TLanguagePackage> : LanguageClassifierProvider<TLanguagePackage>
        where TLanguagePackage : Package
    {
        protected override IClassifier GetClassifierImpl(ITextBuffer textBuffer)
        {
            Contract.Requires(textBuffer != null);

            throw new NotImplementedException();
        }
    }
}
