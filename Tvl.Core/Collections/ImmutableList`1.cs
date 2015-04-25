namespace Tvl.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class ImmutableList<T> : ReadOnlyCollection<T>
    {
        public ImmutableList(IEnumerable<T> collection)
            : base(GetImmutableList(collection))
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
        }

        public ImmutableList(ImmutableList<T> collection)
            : base(collection.Items)
        {
            Contract.Requires<ArgumentNullException>(collection != null, "collection");
        }

        private static IList<T> GetImmutableList(IEnumerable<T> collection)
        {
            Contract.Requires(collection != null);

            ImmutableList<T> immutable = collection as ImmutableList<T>;
            if (immutable != null)
                return immutable;

            return collection.ToArray();
        }
    }
}
