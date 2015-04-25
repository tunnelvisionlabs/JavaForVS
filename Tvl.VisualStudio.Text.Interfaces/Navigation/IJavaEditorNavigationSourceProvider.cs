namespace Tvl.VisualStudio.Text.Navigation
{
    using Microsoft.VisualStudio.Text;

    public interface IJavaEditorNavigationSourceProvider
    {
        IEditorNavigationSource TryCreateEditorNavigationSource(ITextBuffer textBuffer);
    }
}
