namespace Tvl.VisualStudio.Text.Navigation
{
    using Microsoft.VisualStudio.Text;

    public interface IJavaEditorNavigationSourceAggregatorFactoryService
    {
        IEditorNavigationSourceAggregator CreateEditorNavigationSourceAggregator(ITextBuffer textBuffer);
    }
}
