namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class EnumDebugReferenceInfo : DebugEnumerator<IEnumDebugReferenceInfo2, DEBUG_REFERENCE_INFO>, IEnumDebugReferenceInfo2
    {
        public EnumDebugReferenceInfo(IEnumerable<DEBUG_REFERENCE_INFO> referenceInfo)
            :  base(referenceInfo)
        {
            Contract.Requires(referenceInfo != null);
        }
        
        protected EnumDebugReferenceInfo(DEBUG_REFERENCE_INFO[] elements, int currentIndex)
            : base(elements, currentIndex)
        {
        }

        int IEnumDebugReferenceInfo2.Next(uint celt, DEBUG_REFERENCE_INFO[] rgelt, out uint pceltFetched)
        {
            pceltFetched = 0;
            return base.Next(celt, rgelt, ref pceltFetched);
        }

        protected override IEnumDebugReferenceInfo2 CreateClone(DEBUG_REFERENCE_INFO[] elements, int currentIndex)
        {
            return new EnumDebugReferenceInfo(elements, currentIndex);
        }
    }
}
