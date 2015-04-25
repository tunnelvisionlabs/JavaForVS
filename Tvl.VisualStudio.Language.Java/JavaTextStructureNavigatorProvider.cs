namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Utilities;

    //[Export(typeof(ITextStructureNavigatorProvider))]
    [ContentType(Constants.JavaContentType)]
    public sealed class JavaTextStructureNavigatorProvider : ITextStructureNavigatorProvider
    {
        [Import]
        private IContentTypeRegistryService contentTypeRegistry
        {
            get;
            set;
        }

        public ITextStructureNavigator CreateTextStructureNavigator(ITextBuffer textBuffer)
        {
            Func<JavaTextStructureNavigator> factory = () => new JavaTextStructureNavigator(textBuffer, contentTypeRegistry);
            return textBuffer.Properties.GetOrCreateSingletonProperty<JavaTextStructureNavigator>(factory);
        }
    }
}
