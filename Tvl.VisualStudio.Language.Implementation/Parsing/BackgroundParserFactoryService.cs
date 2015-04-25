namespace Tvl.VisualStudio.Language.Parsing.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Microsoft.VisualStudio.Text;
    using Tvl.VisualStudio.Text;

    [Export(typeof(IJavaBackgroundParserFactoryService))]
    internal class BackgroundParserFactoryService : IJavaBackgroundParserFactoryService
    {
        [ImportMany]
        private IEnumerable<Lazy<IJavaBackgroundParserProvider, IContentTypeMetadata>> BackgroundParserProviders
        {
            get;
            set;
        }

        public IBackgroundParser GetBackgroundParser(ITextBuffer buffer)
        {
            foreach (var provider in BackgroundParserProviders)
            {
                if (provider.Metadata.ContentTypes.Any(contentType => buffer.ContentType.IsOfType(contentType)))
                {
                    var parser = provider.Value.CreateParser(buffer);
                    if (parser != null)
                        return parser;
                }
            }

            return null;
        }
    }
}
