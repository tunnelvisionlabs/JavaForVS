namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;
    using Tvl.VisualStudio.Language.Parsing;

    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.JavaContentType)]
    [TagType(typeof(IOutliningRegionTag))]
    public sealed class OutliningTaggerProvider : ITaggerProvider
    {
        [Import]
        private IJavaBackgroundParserFactoryService BackgroundParserFactoryService
        {
            get;
            set;
        }

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            Func<OutliningTagger> creator = () => new OutliningTagger(buffer, BackgroundParserFactoryService.GetBackgroundParser(buffer));
            return buffer.Properties.GetOrCreateSingletonProperty<OutliningTagger>(creator) as ITagger<T>;
        }
    }
}
