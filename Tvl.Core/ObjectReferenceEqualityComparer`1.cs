namespace Tvl
{
    using System.Collections.Generic;
    using IEqualityComparer = System.Collections.IEqualityComparer;
    using RuntimeHelpers = System.Runtime.CompilerServices.RuntimeHelpers;

    public sealed class ObjectReferenceEqualityComparer<T> : IEqualityComparer, IEqualityComparer<T>
        where T : class
    {
        private static readonly ObjectReferenceEqualityComparer<T> _default = new ObjectReferenceEqualityComparer<T>();

        public static ObjectReferenceEqualityComparer<T> Default
        {
            get
            {
                return _default;
            }
        }

        public bool Equals(T x, T y)
        {
            return object.ReferenceEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            return object.ReferenceEquals(x, y);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
