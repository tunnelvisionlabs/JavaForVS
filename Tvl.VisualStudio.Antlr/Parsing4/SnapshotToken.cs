namespace Tvl.VisualStudio.Language.Parsing4
{
    using System;
    using Antlr4.Runtime;
    using ITextSnapshot = Microsoft.VisualStudio.Text.ITextSnapshot;
    using ITextSnapshotLine = Microsoft.VisualStudio.Text.ITextSnapshotLine;

    public class SnapshotToken : CommonToken
    {
        private readonly ITextSnapshot snapshot;

        public SnapshotToken(ITextSnapshot snapshot, Tuple<ITokenSource, ICharStream> source, int type, int channel, int start, int stop)
            : base(source, type, channel, start, stop)
        {
            this.snapshot = snapshot;
        }

        public SnapshotToken(int type, string text)
            : base(type, text)
        {
            snapshot = null;
        }

        public ITextSnapshot Snapshot
        {
            get
            {
                return snapshot;
            }
        }

        public override int Line
        {
            get
            {
                if (snapshot != null)
                    return snapshot.GetLineNumberFromPosition(StartIndex) + 1;

                return base.Line;
            }
        }

        public override int Column
        {
            get
            {
                if (snapshot != null)
                {
                    ITextSnapshotLine snapshotLine = snapshot.GetLineFromPosition(StartIndex);
                    return StartIndex - snapshotLine.Start.Position;
                }

                return base.Column;
            }
        }

        public override bool Equals(Object obj)
        {
            SnapshotToken other = obj as SnapshotToken;
            if (other == null)
                return false;

            return this.Snapshot.Equals(other.Snapshot)
                && this.StartIndex == other.StartIndex
                && this.StopIndex == other.StopIndex
                && this.Type == other.Type;
        }

        public override int GetHashCode()
        {
            int hash = 5;
            hash = 31 * hash + (this.snapshot != null ? this.snapshot.GetHashCode() : 0);
            hash = 31 * hash + StartIndex;
            hash = 31 * hash + StopIndex;
            hash = 31 * hash + Type;
            return hash;
        }
    }
}
