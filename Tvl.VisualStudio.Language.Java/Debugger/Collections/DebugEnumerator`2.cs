namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.VisualStudio;

    public abstract class DebugEnumerator<TEnum, TElement>
    {
        private readonly TElement[] _elements;
        private int _currentIndex;

        public DebugEnumerator(IEnumerable<TElement> elements)
        {
            Contract.Requires<ArgumentNullException>(elements != null, "elements");
            _elements = elements.ToArray();
        }

        protected DebugEnumerator(TElement[] elements, int currentIndex)
        {
            _elements = elements;
            _currentIndex = currentIndex;
        }

        public int Clone(out TEnum ppEnum)
        {
            ppEnum = CreateClone(_elements, _currentIndex);
            return ppEnum != null ? VSConstants.S_OK : VSConstants.E_FAIL;
        }

        public int GetCount(out uint pcelt)
        {
            pcelt = (uint)_elements.Length;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns the next set of elements from the enumeration.
        /// </summary>
        /// <param name="celt">The number of elements to retrieve. Also specifies the maximum size of the rgelt array.</param>
        /// <param name="rgelt">Array of TElement elements to be filled in.</param>
        /// <param name="pceltFetched">Returns the number of elements actually returned in rgelt.</param>
        /// <returns>If successful, returns S_OK. Returns S_FALSE if fewer than the requested number of elements could be returned; otherwise, returns an error code.</returns>
        public int Next(uint celt, TElement[] rgelt, ref uint pceltFetched)
        {
            pceltFetched = 0;

            if (rgelt == null || rgelt.Length < celt)
                return VSConstants.E_INVALIDARG;

            int remaining = _elements.Length - _currentIndex;
            pceltFetched = checked((uint)Math.Min(celt, remaining));
            Array.Copy(_elements, _currentIndex, rgelt, 0, pceltFetched);
            _currentIndex += (int)pceltFetched;
            return pceltFetched == celt ? VSConstants.S_OK : VSConstants.S_FALSE;
        }

        public int Reset()
        {
            _currentIndex = 0;
            return VSConstants.S_OK;
        }

        public int Skip(uint celt)
        {
            int remaining = _elements.Length - _currentIndex;
            if (remaining < celt)
            {
                _currentIndex = _elements.Length;
                return VSConstants.S_FALSE;
            }

            _currentIndex += (int)celt;
            return VSConstants.S_OK;
        }

        protected abstract TEnum CreateClone(TElement[] elements, int currentIndex);
    }
}
