namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Utilities;

    internal sealed class JavaTextStructureNavigator : ITextStructureNavigator
    {
        public JavaTextStructureNavigator(ITextBuffer textBuffer, IContentTypeRegistryService contentTypeRegistry)
        {
            this.TextBuffer = textBuffer;
            this.ContentTypeRegistry = contentTypeRegistry;
        }

        public ITextBuffer TextBuffer
        {
            get;
            private set;
        }

        public IContentTypeRegistryService ContentTypeRegistry
        {
            get;
            private set;
        }

        public IContentType ContentType
        {
            get
            {
                return ContentTypeRegistry.GetContentType(Constants.JavaContentType);
            }
        }

        public TextExtent GetExtentOfWord(SnapshotPoint currentPosition)
        {
            throw new NotImplementedException();
        }

        public SnapshotSpan GetSpanOfEnclosing(SnapshotSpan activeSpan)
        {
            throw new NotImplementedException();
        }

        public SnapshotSpan GetSpanOfFirstChild(SnapshotSpan activeSpan)
        {
            throw new NotImplementedException();
        }

        public SnapshotSpan GetSpanOfNextSibling(SnapshotSpan activeSpan)
        {
            throw new NotImplementedException();
        }

        public SnapshotSpan GetSpanOfPreviousSibling(SnapshotSpan activeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
