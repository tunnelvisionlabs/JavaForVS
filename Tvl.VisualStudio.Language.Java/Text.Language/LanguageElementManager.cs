namespace Tvl.VisualStudio.Language.Java.Text.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Projection;
    using Microsoft.VisualStudio.Text.Tagging;
    using Tvl.VisualStudio.Text.Tagging;

    public sealed class LanguageElementManager : ILanguageElementManager
    {
        public event EventHandler<LanguageElementsChangedEventArgs> LanguageElementsChanged;

        private bool enabled;
        private bool disposed;
        private ITextBuffer textBuffer;
        private IBufferGraph bufferGraph;
        private ITagAggregator<ILanguageElementTag> tagAggregator;

        public LanguageElementManager(ITextBuffer textBuffer, IBufferGraph bufferGraph, ITagAggregator<ILanguageElementTag> tagAggregator)
        {
            this.enabled = true;
            this.textBuffer = textBuffer;
            this.bufferGraph = bufferGraph;
            this.tagAggregator = tagAggregator;
            this.tagAggregator.TagsChanged += LanguageElementTagsChanged;
            this.textBuffer.Changed += SourceTextChanged;
        }

        public IEnumerable<ILanguageElementTag> GetAllLanguageElements(SnapshotSpan span)
        {
            return this.GetAllLanguageElementsInternal(span).Cast<ILanguageElementTag>();
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.disposed = true;
                this.textBuffer.Changed -= SourceTextChanged;
                this.tagAggregator.TagsChanged -= LanguageElementTagsChanged;
                this.tagAggregator.Dispose();
            }
        }

        private void EnsureValid(SnapshotSpan? span)
        {
            if (this.disposed)
                throw new ObjectDisposedException("LanguageElementManager");
            if (span.HasValue && span.Value.Snapshot.TextBuffer != this.textBuffer)
                throw new ArgumentException("The given span is on an invalid buffer. Spans must be generated against the view model's edit bufer.", "span");
        }

        private IEnumerable<LanguageElementTag> GetAllLanguageElementsInternal(SnapshotSpan span)
        {
            //IEnumerable<LanguageElementTag> elements;
            EnsureValid(span);
            if (!this.enabled)
                return new LanguageElementTag[0];

            throw new NotImplementedException();
        }

        private void SourceTextChanged(object sender, TextContentChangedEventArgs e)
        {
            if (this.enabled && e.Changes.Count > 0)
            {
                ITextChange firstChange = e.Changes[0];
                ITextChange lastChange = e.Changes[e.Changes.Count - 1];
                SnapshotSpan changedSpan = new SnapshotSpan(e.After, Span.FromBounds(firstChange.NewSpan.Start, lastChange.NewSpan.End));
                UpdateAfterChange(changedSpan);
            }
        }

        private void LanguageElementTagsChanged(object sender, TagsChangedEventArgs e)
        {
            if (this.enabled)
            {
                foreach (SnapshotSpan span in e.Span.GetSpans(this.textBuffer))
                    UpdateAfterChange(span);
            }
        }

        private void UpdateAfterChange(SnapshotSpan changedSpan)
        {
            throw new NotImplementedException();
        }

        private void OnLanguageElementsChanged(LanguageElementsChangedEventArgs e)
        {
            var t = LanguageElementsChanged;
            if (t != null)
                t(this, e);
        }
    }
}
