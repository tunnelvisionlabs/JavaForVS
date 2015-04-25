namespace Tvl.VisualStudio.Language.Parsing.Collections
{
    using ArgumentException = System.ArgumentException;
    using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;
    using Math = System.Math;

    public struct Interval : System.IEquatable<Interval>
    {
        private readonly int _start;
        private readonly int _length;

        public Interval(int start, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "The length cannot be less than 0.");
            if ((long)start + length - 1 > int.MaxValue)
                throw new ArgumentException("The specified interval overflows the supported range.");

            _start = start;
            _length = length;
        }

        public int Start
        {
            get
            {
                return _start;
            }
        }

        public int Length
        {
            get
            {
                return _length;
            }
        }

        public long EndExclusive
        {
            get
            {
                return (long)_start + _length;
            }
        }

        public int EndInclusive
        {
            get
            {
                return _start + _length - 1;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return _length < 0;
            }
        }

        public static Interval FromBounds(int start, int endInclusive)
        {
            if (endInclusive < (long)start - 1)
                throw new ArgumentException("The specified interval has negative length.");

            return new Interval(start, endInclusive - start + 1);
        }

        public static bool operator ==(Interval x, Interval y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Interval x, Interval y)
        {
            return !x.Equals(y);
        }

        public bool Contains(int value)
        {
            return value >= Start && value <= EndInclusive;
        }

        public bool Contains(Interval other)
        {
            return other.Start >= Start && other.EndInclusive <= EndInclusive;
        }

        public bool OverlapsWith(Interval other)
        {
            int start = Math.Max(Start, other.Start);
            int end = Math.Max(EndInclusive, other.EndInclusive);
            return start <= end;
        }

        public bool IntersectsWith(Interval other)
        {
            return EndInclusive >= other.Start - 1
                && Start - 1 <= other.EndInclusive;
        }

        public Interval? Intersection(Interval other)
        {
            int start = Math.Max(Start, other.Start);
            int endInclusive = Math.Min(EndInclusive, other.EndInclusive);
            if (endInclusive < (long)start - 1)
                return null;

            return FromBounds(start, endInclusive);
        }

        public Interval? Overlap(Interval other)
        {
            Interval? intersection = Intersection(other);
            if (intersection == null || intersection.Value.IsEmpty)
                return null;

            return intersection;
        }

        public Interval? Union(Interval other)
        {
            if (!IntersectsWith(other))
                return null;

            return Range(other);
        }

        public Interval Range(Interval other)
        {
            int start = Math.Min(Start, other.Start);
            int endInclusive = Math.Max(EndInclusive, other.EndInclusive);
            return FromBounds(start, endInclusive);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Interval))
                return false;

            return this.Equals((Interval)obj);
        }

        public bool Equals(Interval other)
        {
            return _start == other._start
                && _length == other._length;
        }

        public override int GetHashCode()
        {
            long value = ((long)_start << 32) + _length;
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[{0}..{1})", Start, (long)EndInclusive + 1);
        }
    }
}
