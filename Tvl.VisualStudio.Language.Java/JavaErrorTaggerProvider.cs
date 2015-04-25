namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;
    using Tvl.VisualStudio.Language.Parsing;
    using Tvl.VisualStudio.Text.Tagging;

    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.JavaContentType)]
    [TagType(typeof(IErrorTag))]
    public sealed class JavaErrorTaggerProvider : ITaggerProvider
    {
        [Import]
        private IJavaBackgroundParserFactoryService BackgroundParserFactoryService
        {
            get;
            set;
        }

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            if (typeof(T) == typeof(IErrorTag))
            {
                BackgroundParserErrorTagger tagger;
                if (buffer.Properties.TryGetProperty<BackgroundParserErrorTagger>(typeof(BackgroundParserErrorTagger), out tagger))
                    return (ITagger<T>)tagger;

                var backgroundParser = BackgroundParserFactoryService.GetBackgroundParser(buffer);
                if (backgroundParser == null)
                    return null;

                Func<BackgroundParserErrorTagger> creator = () => new BackgroundParserErrorTagger(buffer, backgroundParser);
                return (ITagger<T>)buffer.Properties.GetOrCreateSingletonProperty(creator);
            }

            return null;
        }
    }
}
