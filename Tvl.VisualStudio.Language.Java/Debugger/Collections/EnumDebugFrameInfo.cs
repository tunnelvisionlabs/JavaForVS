namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class EnumDebugFrameInfo : DebugEnumerator<IEnumDebugFrameInfo2, FRAMEINFO>, IEnumDebugFrameInfo2
    {
        public EnumDebugFrameInfo(IEnumerable<FRAMEINFO> frameInfo)
            : base(frameInfo)
        {
            Contract.Requires(frameInfo != null);
        }

        protected EnumDebugFrameInfo(FRAMEINFO[] elements, int currentIndex)
            : base(elements, currentIndex)
        {
        }

        protected override IEnumDebugFrameInfo2 CreateClone(FRAMEINFO[] elements, int currentIndex)
        {
            return new EnumDebugFrameInfo(elements, currentIndex);
        }
    }
}
