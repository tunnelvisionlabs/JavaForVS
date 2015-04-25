namespace Tvl.VisualStudio.Language.Java
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;
    using Tvl.VisualStudio.Text.Tagging;

    [Export(typeof(IViewTaggerProvider))]
    [ContentType(Constants.JavaContentType)]
    [TagType(typeof(TextMarkerTag))]
    public sealed class BraceMatchingTaggerProvider : IViewTaggerProvider
    {
        [Import]
        private IClassifierAggregatorService AggregatorService
        {
            get;
            set;
        }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            if (textView == null)
                return null;

            var aggregator = AggregatorService.GetClassifier(buffer);
            var pairs = new KeyValuePair<char, char>[]
                {
                    new KeyValuePair<char, char>('(', ')'),
                    new KeyValuePair<char, char>('{', '}'),
                    new KeyValuePair<char, char>('[', ']')
                };
            return new BraceMatchingTagger(textView, buffer, aggregator, pairs) as ITagger<T>;
        }
    }
}
