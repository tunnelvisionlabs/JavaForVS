namespace Tvl.VisualStudio.Text.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Microsoft.VisualStudio.Text;

    [Export(typeof(IJavaEditorNavigationSourceAggregatorFactoryService))]
    internal class EditorNavigationSourceAggregatorFactoryService : IJavaEditorNavigationSourceAggregatorFactoryService
    {
        [ImportMany]
        private IEnumerable<Lazy<IJavaEditorNavigationSourceProvider, IEditorNavigationSourceMetadata>> NavigationSourceProviders
        {
            get;
            set;
        }

        public IEditorNavigationSourceAggregator CreateEditorNavigationSourceAggregator(ITextBuffer textBuffer)
        {
            var providers = NavigationSourceProviders.Where(provider => provider.Metadata.ContentTypes.Any(contentType => textBuffer.ContentType.IsOfType(contentType)));

            var sources =
                providers
                .Select(provider => provider.Value.TryCreateEditorNavigationSource(textBuffer))
                .Where(source => source != null)
                .ToArray();

            return new EditorNavigationSourceAggregator(sources);
        }
    }
}
