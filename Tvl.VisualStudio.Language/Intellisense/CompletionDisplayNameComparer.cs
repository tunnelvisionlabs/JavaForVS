namespace Tvl.VisualStudio.Language.Intellisense
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Completion = Microsoft.VisualStudio.Language.Intellisense.Completion;
    using IComparer = System.Collections.IComparer;
    using IEqualityComparer = System.Collections.IEqualityComparer;

    public class CompletionDisplayNameComparer : IComparer<Completion>, IEqualityComparer<Completion>, IComparer, IEqualityComparer
    {
        private static readonly CompletionDisplayNameComparer _currentCulture = new CompletionDisplayNameComparer(StringComparer.CurrentCulture);
        private static readonly CompletionDisplayNameComparer _currentCultureIgnoreCase = new CompletionDisplayNameComparer(StringComparer.CurrentCultureIgnoreCase);
        private static readonly CompletionDisplayNameComparer _invariantCulture = new CompletionDisplayNameComparer(StringComparer.InvariantCulture);
        private static readonly CompletionDisplayNameComparer _invariantCultureIgnoreCase = new CompletionDisplayNameComparer(StringComparer.InvariantCultureIgnoreCase);
        private static readonly CompletionDisplayNameComparer _ordinal = new CompletionDisplayNameComparer(StringComparer.Ordinal);
        private static readonly CompletionDisplayNameComparer _ordinalIgnoreCase = new CompletionDisplayNameComparer(StringComparer.OrdinalIgnoreCase);

        private readonly StringComparer _comparer;

        public CompletionDisplayNameComparer(StringComparer comparer)
        {
            Contract.Requires<ArgumentNullException>(comparer != null, "comparer");

            _comparer = comparer;
        }

        public static CompletionDisplayNameComparer CurrentCulture
        {
            get
            {
                return _currentCulture;
            }
        }

        public static CompletionDisplayNameComparer CurrentCultureIgnoreCase
        {
            get
            {
                return _currentCultureIgnoreCase;
            }
        }

        public static CompletionDisplayNameComparer InvariantCulture
        {
            get
            {
                return _invariantCulture;
            }
        }

        public static CompletionDisplayNameComparer InvariantCultureIgnoreCase
        {
            get
            {
                return _invariantCultureIgnoreCase;
            }
        }

        public static CompletionDisplayNameComparer Ordinal
        {
            get
            {
                return _ordinal;
            }
        }

        public static CompletionDisplayNameComparer OrdinalIgnoreCase
        {
            get
            {
                return _ordinalIgnoreCase;
            }
        }

        public int Compare(Completion x, Completion y)
        {
            string xs = x != null ? x.DisplayText : null;
            string ys = y != null ? y.DisplayText : null;
            return _comparer.Compare(xs, ys);
        }

        public bool Equals(Completion x, Completion y)
        {
            string xs = x != null ? x.DisplayText : null;
            string ys = y != null ? y.DisplayText : null;
            return _comparer.Equals(xs, ys);
        }

        public int GetHashCode(Completion obj)
        {
            string objs = obj != null ? obj.DisplayText : null;
            return _comparer.GetHashCode(objs);
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((Completion)x, (Completion)y);
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            return Equals((Completion)x, (Completion)y);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetHashCode((Completion)obj);
        }
    }
}
