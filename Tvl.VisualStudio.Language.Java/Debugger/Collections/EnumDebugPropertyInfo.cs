namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class EnumDebugPropertyInfo : DebugEnumerator<IEnumDebugPropertyInfo2, DEBUG_PROPERTY_INFO>, IEnumDebugPropertyInfo2
    {
        public EnumDebugPropertyInfo(IEnumerable<DEBUG_PROPERTY_INFO> propertyInfo)
            : base(propertyInfo)
        {
            Contract.Requires(propertyInfo != null);
        }
        
        protected EnumDebugPropertyInfo(DEBUG_PROPERTY_INFO[] elements, int currentIndex)
            : base(elements, currentIndex)
        {
        }

        int IEnumDebugPropertyInfo2.Next(uint celt, DEBUG_PROPERTY_INFO[] rgelt, out uint pceltFetched)
        {
            pceltFetched = 0;
            return base.Next(celt, rgelt, ref pceltFetched);
        }

        protected override IEnumDebugPropertyInfo2 CreateClone(DEBUG_PROPERTY_INFO[] elements, int currentIndex)
        {
            return new EnumDebugPropertyInfo(elements, currentIndex);
        }
    }
}
