namespace Tvl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public static class ListExtensions
    {
        public static int FindIndex<T>(this IList<T> collection, Predicate<T> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");
            Contract.Ensures(Contract.Result<int>() >= -1 && Contract.Result<int>() < collection.Count);

            return FindIndex(collection, 0, collection.Count, predicate);
        }

        public static int FindIndex<T>(this IList<T> collection, int startIndex, Predicate<T> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
            Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0);
            Contract.Requires<ArgumentException>(startIndex <= collection.Count);
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");
            Contract.Ensures(Contract.Result<int>() >= -1 && Contract.Result<int>() < collection.Count);

            return FindIndex(collection, startIndex, collection.Count - startIndex, predicate);
        }

        public static int FindIndex<T>(this IList<T> collection, int startIndex, int count, Predicate<T> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
            Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0);
            Contract.Requires<ArgumentException>(startIndex <= collection.Count);
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");
            Contract.Ensures(Contract.Result<int>() >= -1 && Contract.Result<int>() < collection.Count);

            for (int i = 0; i < collection.Count; i++)
            {
                if (predicate(collection[i]))
                    return i;
            }

            return -1;
        }

        public static int FindLastIndex<T>(this IList<T> collection, Predicate<T> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");
            Contract.Ensures(Contract.Result<int>() >= -1 && Contract.Result<int>() < collection.Count);

            return FindLastIndex(collection, 0, collection.Count, predicate);
        }

        public static int FindLastIndex<T>(this IList<T> collection, int startIndex, Predicate<T> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
            Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0);
            Contract.Requires<ArgumentException>(startIndex <= collection.Count);
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");
            Contract.Ensures(Contract.Result<int>() >= -1 && Contract.Result<int>() < collection.Count);

            return FindLastIndex(collection, startIndex, collection.Count - startIndex, predicate);
        }

        public static int FindLastIndex<T>(this IList<T> collection, int startIndex, int count, Predicate<T> predicate)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
            Contract.Requires<ArgumentOutOfRangeException>(startIndex >= 0);
            Contract.Requires<ArgumentException>(startIndex <= collection.Count);
            Contract.Requires<ArgumentNullException>(predicate != null, "predicate");
            Contract.Ensures(Contract.Result<int>() >= -1 && Contract.Result<int>() < collection.Count);

            for (int i = collection.Count - 1; i >= 0; i--)
            {
                if (predicate(collection[i]))
                    return i;
            }

            return -1;
        }

        public static T Find<T>(this IList<T> collection, Predicate<T> match)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
            Contract.Requires<ArgumentNullException>(match != null, "match");

            for (int i = 0; i < collection.Count; i++)
            {
                if (match(collection[i]))
                    return collection[i];
            }

            return default(T);
        }

        public static T FindLast<T>(this IList<T> collection, Predicate<T> match)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
            Contract.Requires<ArgumentNullException>(match != null, "match");

            for (int i = collection.Count - 1; i >= 0; i--)
            {
                if (match(collection[i]))
                    return collection[i];
            }

            return default(T);
        }

        public static List<T> FindAll<T>(this IList<T> collection, Predicate<T> match)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
            Contract.Requires<ArgumentNullException>(match != null, "match");
            Contract.Ensures(Contract.Result<List<T>>() != null);

            List<T> result = new List<T>();
            for (int i = 0; i < collection.Count; i++)
            {
                if (match(collection[i]))
                    result.Add(collection[i]);
            }

            return result;
        }
    }
}
