namespace Tvl.VisualStudio.Language.Parsing4
{
    using System;
    using System.Diagnostics.Contracts;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Microsoft.VisualStudio.Text;

    public class SnapshotCharStream : ICharStream
    {
        private bool _explicitCache;

        private int _currentSnapshotLineStartIndex;

        private string _currentSnapshotLine;

        private int _count;

        public SnapshotCharStream(ITextSnapshot snapshot)
        {
            Contract.Requires<ArgumentNullException>(snapshot != null, "snapshot");

            this.Snapshot = snapshot;
            this._count = snapshot.Length;

            UpdateCachedLine();
        }

        public SnapshotCharStream(ITextSnapshot snapshot, Span cachedSpan)
            : this(new SnapshotSpan(snapshot, cachedSpan))
        {
            Contract.Requires<ArgumentNullException>(snapshot != null, "snapshot");
        }

        public SnapshotCharStream(SnapshotSpan cachedSpan)
        {
            Contract.Requires<ArgumentException>(cachedSpan.Snapshot != null);

            this.Snapshot = cachedSpan.Snapshot;
            _count = Snapshot.Length;
            _explicitCache = true;
            _currentSnapshotLineStartIndex = cachedSpan.Start;
            _currentSnapshotLine = cachedSpan.GetText();
        }

        public ITextSnapshot Snapshot
        {
            get;
            private set;
        }

        public int Size
        {
            get
            {
                return _count;
            }
        }

        public int Index
        {
            get;
            private set;
        }

        public string SourceName
        {
            get
            {
                return "Snapshot";
            }
        }

        public int Lt(int i)
        {
            return La(i);
        }

        public string GetText(Interval interval)
        {
            return Substring(interval.a, interval.Length);
        }

        public string Substring(int startIndex, int length)
        {
            if (_currentSnapshotLine != null)
            {
                if (startIndex >= _currentSnapshotLineStartIndex && (startIndex + length) <= _currentSnapshotLineStartIndex + _currentSnapshotLine.Length)
                    return _currentSnapshotLine.Substring(startIndex - _currentSnapshotLineStartIndex, length);
            }

            return Snapshot.GetText(startIndex, length);
        }

        public virtual void Consume()
        {
            int la = La(1);
            if (la < 0)
                return;

            Index++;
            UpdateCachedLine();
        }

        public virtual int La(int i)
        {
            if (i == 0)
            {
                // undefined
                return 0;
            }

            if (i < 0)
            {
                // e.g., translate LA(-1) to use offset i=0; then data[p+0-1]
                i++;
                if ((Index + i - 1) < 0)
                {
                    // invalid; no char before first char
                    return IntStreamConstants.Eof;
                }
            }

            if ((Index + i - 1) >= Size)
            {
                return IntStreamConstants.Eof;
            }

            int actualIndex = Index + i - 1;

            if (_currentSnapshotLine != null
                && actualIndex >= _currentSnapshotLineStartIndex
                && actualIndex < _currentSnapshotLineStartIndex + _currentSnapshotLine.Length)
            {
                return _currentSnapshotLine[actualIndex - _currentSnapshotLineStartIndex];
            }

            return Snapshot.GetText(actualIndex, 1)[0];
        }

        public int Mark()
        {
            return 0;
        }

        public void Release(int marker)
        {
        }

        public void Seek(int index)
        {
            if (index == Index)
                return;

            Index = index;
            var line = Snapshot.GetLineFromPosition(Index);
            UpdateCachedLine();
        }

        private void UpdateCachedLine()
        {
            if (_explicitCache)
                return;

            if (_currentSnapshotLine == null
                || Index < _currentSnapshotLineStartIndex
                || Index >= _currentSnapshotLineStartIndex + _currentSnapshotLine.Length)
            {
                if (Index >= 0 && Index < Size)
                {
                    ITextSnapshotLine line = Snapshot.GetLineFromPosition(Index);
                    _currentSnapshotLineStartIndex = line.Start;
                    _currentSnapshotLine = line.GetTextIncludingLineBreak();
                }
                else
                {
                    _currentSnapshotLine = null;
                    _currentSnapshotLineStartIndex = 0;
                }
            }
        }
    }
}
