namespace Tvl.Collections
{
    using System.Collections.Generic;

    public static class ArrayEqualityComparers
    {
        public static Int32ArrayEqualityComparer Int32
        {
            get
            {
                return Int32ArrayEqualityComparer.Default;
            }
        }

        public sealed class Int32ArrayEqualityComparer : IEqualityComparer<int[]>
        {
            private static readonly Int32ArrayEqualityComparer _default = new Int32ArrayEqualityComparer();

            public static Int32ArrayEqualityComparer Default
            {
                get
                {
                    return _default;
                }
            }

            public bool Equals(int[] x, int[] y)
            {
                if (x == y)
                    return true;
                if (x == null || y == null)
                    return false;

                unsafe
                {
                    int length = x.Length;
                    if (length != y.Length)
                        return false;
                    if (length == 0)
                        return true;

                    fixed (int* xbase = x, ybase = y)
                    {
                        int* px = xbase;
                        int* py = ybase;
                        for (int i = 0; i < length; i++)
                        {
                            if (*px++ != *py++)
                                return false;
                        }
                    }
                }

                return true;
            }

            public int GetHashCode(int[] obj)
            {
                if (obj == null)
                    return 0;

                int hashCode = 1;
                unsafe
                {
                    int length = obj.Length;
                    if (length > 0)
                    {
                        fixed (int* objbase = obj)
                        {
                            int* pobj = objbase;
                            for (int i = 0; i < length; i++)
                                hashCode = 31 * hashCode + *pobj++;
                        }
                    }
                }

                return hashCode;
            }
        }
    }
}
