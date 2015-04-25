namespace Tvl.VisualStudio.Language.Intellisense.Implementation
{
    using System.Collections.Concurrent;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Language.Intellisense;

    using Action = System.Action;
    using Dispatcher = System.Windows.Threading.Dispatcher;
    using ImageSource = System.Windows.Media.ImageSource;

    [Export(typeof(IJavaDispatcherGlyphService))]
    public class DispatcherGlyphService : IJavaDispatcherGlyphService
    {
        private readonly ConcurrentDictionary<uint, ImageSource> _glyphCache = new ConcurrentDictionary<uint, ImageSource>();

        public DispatcherGlyphService()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        [Import]
        public IGlyphService GlyphService
        {
            get;
            private set;
        }

        public Dispatcher Dispatcher
        {
            get;
            private set;
        }

        public ImageSource GetGlyph(StandardGlyphGroup group, StandardGlyphItem item)
        {
            uint key = ((uint)group << 16) + (uint)item;
            return _glyphCache.GetOrAdd(key, CreateGlyph);
        }

        public ImageSource CreateGlyph(uint key)
        {
            StandardGlyphGroup group = (StandardGlyphGroup)(key >> 16);
            StandardGlyphItem item = (StandardGlyphItem)(key & 0xFFFF);
            ImageSource source = null;

            // create the glyph on the UI thread
            Dispatcher dispatcher = Dispatcher;
            if (dispatcher == null)
            {
                source = null;
            }
            else
            {
                dispatcher.Invoke((Action)(
                    () =>
                    {
                        source = GlyphService.GetGlyph(group, item);
                    }));
            }

            return source;
        }
    }
}
