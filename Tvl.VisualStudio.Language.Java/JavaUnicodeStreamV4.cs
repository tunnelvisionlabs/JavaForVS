namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using System.Collections.Generic;

    using Contract = System.Diagnostics.Contracts.Contract;
    using ICharStream = Antlr4.Runtime.ICharStream;
    using Interval = Antlr4.Runtime.Misc.Interval;

    public class JavaUnicodeStreamV4 : ICharStream
    {
        private readonly ICharStream _source;
        private readonly List<int> _escapeIndexes = new List<int>();
        private readonly List<char> _escapeCharacters = new List<char>();

        private int _escapeListIndex;
        private int _range;
        private int _slashCount;

        public JavaUnicodeStreamV4(ICharStream source)
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");

            _source = source;
        }

        public int Size
        {
            get
            {
                return _source.Size;
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

        public string GetText(Interval interval)
        {
            return _source.GetText(interval);
        }

        public void Consume()
        {
            if (_source.La(1) != '\\')
            {
                _source.Consume();
                _range = Math.Max(_range, _source.Index);
                _slashCount = 0;
                return;
            }

            // make sure the next character has been processed
            this.La(1);

            if (_escapeListIndex >= _escapeIndexes.Count || _escapeIndexes[_escapeListIndex] != Index)
            {
                _source.Consume();
                _slashCount++;
            }
            else
            {
                for (int i = 0; i < 6; i++)
                    _source.Consume();

                _escapeListIndex++;
                _slashCount = 0;
            }

            Contract.Assert(_range >= Index);
        }

        public int La(int i)
        {
            if (i <= 0)
            {
                int desiredIndex = Index + i;
                for (int j = _escapeListIndex - 1; j >= 0; j--)
                {
                    // TODO: verify this
                    if (_escapeIndexes[j] + 6 > desiredIndex)
                        desiredIndex -= 5;
                }

                return _source.La(desiredIndex - Index);
            }
            else
            {
                int desiredIndex = Index + i - 1;
                for (int j = _escapeListIndex; j < _escapeIndexes.Count; j++)
                {
                    if (_escapeIndexes[j] == desiredIndex)
                        return _escapeCharacters[j];
                    else if (_escapeIndexes[j] < desiredIndex)
                        desiredIndex += 5;
                    else
                        return _source.La(desiredIndex - Index + 1);
                }

                desiredIndex = Index + i - 1;
                int currentIndex = Index;
                int escapeListIndex = _escapeListIndex;
                int slashCount = _slashCount;
                for (int j = 0; j < i; j++)
                {
                    int previousIndex = currentIndex;
                    int c = ReadCharAt(ref currentIndex, ref slashCount);
                    if (currentIndex > _range)
                    {
                        if (currentIndex - previousIndex > 1)
                        {
                            _escapeIndexes.Add(previousIndex);
                            _escapeCharacters.Add((char)c);
                        }

                        _range = currentIndex;
                    }

                    if (j == i - 1)
                    {
                        return c;
                    }
                }

                throw new InvalidOperationException("Shouldn't be reachable");
            }
        }

        private int ReadCharAt(ref int nextIndex, ref int slashCount)
        {
            bool blockUnicodeEscape = (slashCount % 2) != 0;

            int c0 = _source.La(nextIndex - Index + 1);
            if (c0 == '\\')
            {
                slashCount++;

                if (!blockUnicodeEscape)
                {
                    int c1 = _source.La(nextIndex - Index + 2);
                    if (c1 == 'u')
                    {
                        int c2 = _source.La(nextIndex - Index + 3);
                        int c3 = _source.La(nextIndex - Index + 4);
                        int c4 = _source.La(nextIndex - Index + 5);
                        int c5 = _source.La(nextIndex - Index + 6);

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

        public void Seek(int index)
        {
            if (index > _range)
                throw new NotSupportedException();

            _source.Seek(index);

            _slashCount = 0;
            while (_source.La(-_slashCount - 1) == '\\')
                _slashCount++;

            _escapeListIndex = _escapeIndexes.BinarySearch(_source.Index);
            if (_escapeListIndex < 0)
                _escapeListIndex = -_escapeListIndex - 1;
        }
    }
}
