namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Antlr.Runtime;

    public class JavaUnicodeStream : ICharStream
    {
        private readonly ICharStream _source;
        private readonly List<CharInfo> _next = new List<CharInfo>();
        private int _slashCount;

        public JavaUnicodeStream(ICharStream source)
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");

            _source = source;
        }

        public int CharPositionInLine
        {
            get
            {
                return _source.CharPositionInLine;
            }

            set
            {
                _source.CharPositionInLine = value;
            }
        }

        public int Line
        {
            get
            {
                return _source.Line;
            }

            set
            {
                _source.Line = value;
            }
        }

        public int Count
        {
            get
            {
                return _source.Count;
            }
        }

        public int Index
        {
            get
            {
                return _source.Index;
            }
        }

        public string SourceName
        {
            get
            {
                return _source.SourceName;
            }
        }

        int ICharStream.LT(int i)
        {
            return LA(i);
        }

        public string Substring(int start, int length)
        {
            return _source.Substring(start, length);
        }

        public void Consume()
        {
            if (_next.Count == 0)
            {
                if (LA(1) == CharStreamConstants.EndOfFile)
                    return;
            }

            if (_next.Count > 0)
            {
                int count = _next[0].Index - Index;
                for (int i = 0; i < count; i++)
                    _source.Consume();

                if (_next[0].Char == '\\' && count == 1)
                    _slashCount++;
                else
                    _slashCount = 0;

                _next.RemoveAt(0);
            }
        }

        public int LA(int i)
        {
            if (i <= 0)
                return _source.LA(i);

            if (i - 1 >= _next.Count)
            {
                int nextIndex = Index;
                if (_next.Count > 0)
                    nextIndex = _next[_next.Count - 1].Index;

                int slashCount = _slashCount;
                if (_next.Count > 0)
                    slashCount = _next[_next.Count - 1].SlashCount;

                for (int j = _next.Count; j < i; j++)
                {
                    int c = ReadCharAt(ref nextIndex, ref slashCount);
                    _next.Add(new CharInfo(c, nextIndex, slashCount));
                }
            }

            return _next[i - 1].Char;
        }

        private int ReadCharAt(ref int nextIndex, ref int slashCount)
        {
            bool blockUnicodeEscape = (slashCount % 2) != 0;

            int c0 = _source.LA(nextIndex - Index + 1);
            if (c0 == '\\')
            {
                slashCount++;

                if (!blockUnicodeEscape)
                {
                    int c1 = _source.LA(nextIndex - Index + 2);
                    if (c1 == 'u')
                    {
                        int c2 = _source.LA(nextIndex - Index + 3);
                        int c3 = _source.LA(nextIndex - Index + 4);
                        int c4 = _source.LA(nextIndex - Index + 5);
                        int c5 = _source.LA(nextIndex - Index + 6);

                        if (IsHexDigit(c2) && IsHexDigit(c3) && IsHexDigit(c4) && IsHexDigit(c5))
                        {
                            int value = HexValue(c2);
                            value = (value << 4) + HexValue(c3);
                            value = (value << 4) + HexValue(c4);
                            value = (value << 4) + HexValue(c5);

                            nextIndex += 6;
                            slashCount = 0;
                            return value;
                        }
                    }
                }
            }

            nextIndex++;
            return c0;
        }

        private static bool IsHexDigit(int c)
        {
            return (c >= '0' && c <= '9')
                || (c >= 'a' && c <= 'f')
                || (c >= 'A' && c <= 'F');
        }

        private static int HexValue(int c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';

            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;

            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;

            throw new ArgumentOutOfRangeException("c");
        }

        public int Mark()
        {
            return _source.Mark();
        }

        public void Release(int marker)
        {
            _source.Release(marker);
        }

        public void Rewind()
        {
            _source.Rewind();
        }

        public void Rewind(int marker)
        {
            _source.Rewind(marker);
        }

        public void Seek(int index)
        {
            _source.Seek(index);
        }

        private struct CharInfo
        {
            public readonly int Char;
            public readonly int Index;
            public readonly int SlashCount;

            public CharInfo(int c, int index, int slashCount)
            {
                Char = c;
                Index = index;
                SlashCount = slashCount;
            }
        }
    }
}
