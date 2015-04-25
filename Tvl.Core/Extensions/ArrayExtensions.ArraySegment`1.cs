namespace Tvl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using ICollection = System.Collections.ICollection;
    using IEnumerable = System.Collections.IEnumerable;
    using IEnumerator = System.Collections.IEnumerator;
    using IList = System.Collections.IList;

    partial class ArrayExtensions
    {
        private sealed class ArraySegment<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
        {
            private readonly T[] _array;
            private readonly int _offset;
            private readonly int _count;

            public ArraySegment(T[] array, int offset, int count)
            {
                Contract.Requires<ArgumentNullException>(array != null, "array");
                Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
                Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
                Contract.Requires<ArgumentException>(offset <= array.Length);
                Contract.Requires<ArgumentException>(checked(offset + count) <= array.Length);

                _array = array;
                _offset = offset;
                _count = count;
            }

            public int Count
            {
                get
                {
                    return _count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public bool IsFixedSize
            {
                get
                {
                    return true;
                }
            }

            bool ICollection.IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            object ICollection.SyncRoot
            {
                get
                {
                    return this;
                }
            }

            public T this[int index]
            {
                get
                {
                    Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
                    Contract.Requires<ArgumentException>(index < Count);

                    return _array[_offset + index];
                }

                set
                {
                    Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
                    Contract.Requires<ArgumentException>(index < Count);

                    _array[_offset + index] = value;
                }
            }

            T IList<T>.this[int index]
            {
                get
                {
                    return this[index];
                }

                set
                {
                    this[index] = value;
                }
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }

                set
                {
                    this[index] = (T)value;
                }
            }

            public int IndexOf(T item)
            {
                return Array.IndexOf(_array, item, _offset, _count) - _offset;
            }

            void IList<T>.Insert(int index, T item)
            {
                throw new NotSupportedException();
            }

            void IList<T>.RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public bool Contains(T item)
            {
                return IndexOf(item) >= 0;
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                Array.Copy(_array, _offset, array, arrayIndex, _count);
            }

            public IEnumerator<T> GetEnumerator()
            {
                Contract.Ensures(Contract.Result<IEnumerator<T>>() != null);

                for (int i = 0; i < _count; i++)
                    yield return _array[_offset + i];
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                Contract.Ensures(Contract.Result<IEnumerator>() != null);

                return GetEnumerator();
            }

            bool IList.Contains(object value)
            {
                return Array.IndexOf(_array, value, _offset, _count) >= 0;
            }

            int IList.IndexOf(object value)
            {
                return Array.IndexOf(_array, value, _offset, _count) - _offset;
            }

            void ICollection.CopyTo(Array array, int index)
            {
                Array.Copy(_array, _offset, array, index, _count);
            }

            void ICollection<T>.Add(T item)
            {
                throw new NotSupportedException();
            }

            void ICollection<T>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<T>.Remove(T item)
            {
                throw new NotSupportedException();
            }

            int IList.Add(object value)
            {
                throw new NotSupportedException();
            }

            void IList.Clear()
            {
                throw new NotSupportedException();
            }

            void IList.Insert(int index, object value)
            {
                throw new NotSupportedException();
            }

            void IList.Remove(object value)
            {
                throw new NotSupportedException();
            }

            void IList.RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_array != null);
                Contract.Invariant(_offset >= 0 && _offset <= _array.Length);
                Contract.Invariant(_count >= 0);
                Contract.Invariant(_offset + _count <= _array.Length);
            }
        }
    }
}
