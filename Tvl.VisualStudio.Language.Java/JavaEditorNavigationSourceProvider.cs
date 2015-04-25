namespace Tvl.VisualStudio.Language.Java
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Utilities;
    using Tvl.VisualStudio.Language.Parsing;
    using Tvl.VisualStudio.Text.Navigation;
    using IJavaDispatcherGlyphService = Tvl.VisualStudio.Language.Intellisense.IJavaDispatcherGlyphService;
    using IOutputWindowService = Tvl.VisualStudio.OutputWindow.Interfaces.IOutputWindowService;

    [Export(typeof(IJavaEditorNavigationSourceProvider))]
    [ContentType(Constants.JavaContentType)]
    public sealed class JavaEditorNavigationSourceProvider : IJavaEditorNavigationSourceProvider
    {
        [Import]
        public IJavaBackgroundParserFactoryService BackgroundParserFactoryService
        {
            get;
            private set;
        }

        [Import]
        public IJavaEditorNavigationTypeRegistryService EditorNavigationTypeRegistryService
        {
            get;
            private set;
        }

        [Import]
        public IJavaDispatcherGlyphService GlyphService
        {
            get;
            private set;
        }

        [Import]
        public IOutputWindowService OutputWindowService
        {
            get;
            private set;
        }

        public IEditorNavigationSource TryCreateEditorNavigationSource(ITextBuffer textBuffer)
        {
            var backgroundParser = BackgroundParserFactoryService.GetBackgroundParser(textBuffer);
            if (backgroundParser == null)
                return null;

            return new JavaEditorNavigationSource(textBuffer, backgroundParser, this);
        }
    }
}
