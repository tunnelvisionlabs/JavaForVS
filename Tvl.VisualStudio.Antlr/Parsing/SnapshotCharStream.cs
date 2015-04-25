namespace Tvl.VisualStudio.Language.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Antlr.Runtime;
    using Microsoft.VisualStudio.Text;

    public class SnapshotCharStream : ICharStream
    {
        private bool _explicitCache;

        private int _currentSnapshotLineStartIndex;

        private string _currentSnapshotLine;

        /** <summary>tracks how deep mark() calls are nested</summary> */
        private int _markDepth = 0;

        /** <summary>
         *  A list of CharStreamState objects that tracks the stream state
         *  values line, charPositionInLine, and p that can change as you
         *  move through the input stream.  Indexed from 1..markDepth.
         *  A null is kept @ index 0.  Create upon first call to mark().
         *  </summary>
         */
        private List<CharStreamState> _markers;

        /** <summary>Track the last mark() call result value for use in rewind().</summary> */
        private int _lastMarker;

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

        public int Line
        {
            get;
            set;
        }

        public int CharPositionInLine
        {
            get;
            set;
        }

        public int Count
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

        public int LT(int i)
        {
            return LA(i);
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
            int la = LA(1);
            if (la < 0)
                return;

            if (la == '\n')
            {
                Line++;
                CharPositionInLine = 0;
            }
            else
            {
                CharPositionInLine++;
            }

            Index++;
            UpdateCachedLine();
        }

        public virtual int LA(int i)
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
                    return CharStreamConstants.EndOfFile;
                }
            }

            if ((Index + i - 1) >= Count)
            {
                return CharStreamConstants.EndOfFile;
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
            if (_markers == null)
            {
                _markers = new List<CharStreamState>();
                // depth 0 means no backtracking, leave blank
                _markers.Add(null);
            }

            _markDepth++;
            CharStreamState state = null;
            if (_markDepth >= _markers.Count)
            {
                state = new CharStreamState();
                _markers.Add(state);
            }
            else
            {
                state = _markers[_markDepth];
            }

            state.p = Index;
            state.line = Line;
            state.charPositionInLine = CharPositionInLine;
            _lastMarker = _markDepth;
            return _markDepth;
        }

        public void Release(int marker)
        {
            // unwind any other markers made after m and release m
            _markDepth = marker;
            // release this marker
            _markDepth--;
        }

        public void Rewind()
        {
            Rewind(_lastMarker);
        }

        public void Rewind(int marker)
        {
            CharStreamState state = _markers[marker];

            // Restore stream state (don't use Seek because it calls UpdateCachedLine() unnecessarily).
            Index = state.p;
            Line = state.line;
            CharPositionInLine = state.charPositionInLine;
            Release(marker);

            UpdateCachedLine();
        }

        public void Seek(int index)
        {
            if (index == Index)
                return;

            Index = index;
            var line = Snapshot.GetLineFromPosition(Index);
            Line = line.LineNumber;
            CharPositionInLine = Index - line.Start.Position;
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
                if (Index >= 0 && Index < Count)
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
