namespace Tvl.VisualStudio.Language.Java
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;
    using Tvl.VisualStudio.Text.Classification;

    [Export(typeof(IClassifierProvider))]
    [ContentType(Constants.JavaContentType)]
    public sealed class JavaClassifierProvider : LanguageClassifierProvider<JavaLanguagePackage>
    {
        protected override IClassifier GetClassifierImpl(ITextBuffer textBuffer)
        {
            return new JavaClassifier(textBuffer, StandardClassificationService, ClassificationTypeRegistryService);
        }
    }
}
