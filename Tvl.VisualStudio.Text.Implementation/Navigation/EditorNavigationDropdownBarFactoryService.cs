namespace Tvl.VisualStudio.Text.Navigation.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Projection;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Tvl.VisualStudio.Shell;

    using IWpfTextView = Microsoft.VisualStudio.Text.Editor.IWpfTextView;

    [Export(typeof(IJavaEditorNavigationDropdownBarFactoryService))]
    public class EditorNavigationDropdownBarFactoryService : IJavaEditorNavigationDropdownBarFactoryService
    {
        [ImportMany]
        private IEnumerable<Lazy<IJavaEditorNavigationSourceProvider, IEditorNavigationSourceMetadata>> NavigationSourceProviders
        {
            get;
            set;
        }

        [Import]
        private IJavaEditorNavigationTypeRegistryService EditorNavigationTypeRegistryService
        {
            get;
            set;
        }

        [Import]
        private IBufferGraphFactoryService BufferGraphFactoryService
        {
            get;
            set;
        }

        public IEditorNavigationDropdownBarClient CreateEditorNavigationDropdownBar(IVsCodeWindow codeWindow, IVsEditorAdaptersFactoryService editorAdaptersFactory)
        {
            // a code window can only be associated with a single buffer, so the primary view will get us the correct information
            IVsTextView primaryViewAdapter = codeWindow.GetPrimaryView();
            IWpfTextView textView = editorAdaptersFactory.GetWpfTextView(primaryViewAdapter);

            IBufferGraph bufferGraph = BufferGraphFactoryService.CreateBufferGraph(textView.TextBuffer);
            Collection<ITextBuffer> buffers = bufferGraph.GetTextBuffers(i => true);

            List<IEditorNavigationSource> sources = new List<IEditorNavigationSource>();
            foreach (ITextBuffer buffer in buffers)
            {
                var bufferProviders = NavigationSourceProviders.Where(provider => provider.Metadata.ContentTypes.Any(contentType => buffer.ContentType.IsOfType(contentType)));

                var bufferSources =
                    bufferProviders
                    .Select(provider => provider.Value.TryCreateEditorNavigationSource(buffer))
                    .Where(source => source != null);

                sources.AddRange(bufferSources);
            }

            return new EditorNavigationDropdownBar(codeWindow, editorAdaptersFactory, sources, BufferGraphFactoryService, EditorNavigationTypeRegistryService);
        }
    }
}
