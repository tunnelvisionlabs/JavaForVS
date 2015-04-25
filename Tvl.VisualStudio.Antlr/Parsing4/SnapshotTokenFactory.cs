namespace Tvl.VisualStudio.Language.Parsing4
{
    using System;
    using Antlr4.Runtime;
    using ITextSnapshot = Microsoft.VisualStudio.Text.ITextSnapshot;

    public class SnapshotTokenFactory : ITokenFactory
    {
        private readonly ITextSnapshot snapshot;
        private readonly Tuple<ITokenSource, ICharStream> effectiveSource;

        public SnapshotTokenFactory(ITextSnapshot snapshot, ITokenSource effectiveSource)
        {
            this.snapshot = snapshot;
            this.effectiveSource = Tuple.Create(effectiveSource, effectiveSource.InputStream);
        }

        public SnapshotTokenFactory(ITextSnapshot snapshot, Tuple<ITokenSource, ICharStream> effectiveSource)
        {
            this.snapshot = snapshot;
            this.effectiveSource = effectiveSource;
        }

        public IToken Create(Tuple<ITokenSource, ICharStream> source, int type, string text, int channel, int start, int stop, int line, int charPositionInLine)
        {
            if (effectiveSource != null)
            {
                source = effectiveSource;
            }

            SnapshotToken t = new SnapshotToken(snapshot, source, type, channel, start, stop);
            t.Line = line;
            t.Column = charPositionInLine;
            if (text != null)
            {
                t.Text = text;
            }
            return t;
        }

        public IToken Create(int type, string text)
        {
            return new SnapshotToken(type, text);
        }
    }
}
