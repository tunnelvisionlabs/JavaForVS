namespace Tvl.VisualStudio.Language.Intellisense.Implementation
{
    using System.ComponentModel.Composition;
    using IMouseProcessor = Microsoft.VisualStudio.Text.Editor.IMouseProcessor;
    using IMouseProcessorProvider = Microsoft.VisualStudio.Text.Editor.IMouseProcessorProvider;
    using IWpfTextView = Microsoft.VisualStudio.Text.Editor.IWpfTextView;

    [Export(typeof(IMouseProcessorProvider))]
    public class IntellisenseMouseProcessorProvider : IMouseProcessorProvider
    {
        public IMouseProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            return wpfTextView.Properties.GetOrCreateSingletonProperty(() => new IntellisenseMouseProcessor(wpfTextView));
        }
    }
}
