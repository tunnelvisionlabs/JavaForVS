namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Marshal = System.Runtime.InteropServices.Marshal;

    public sealed class BreakpointResolutionLocationCode : BreakpointResolutionLocation
    {
        private readonly IDebugCodeContext2 _codeContext;

        public BreakpointResolutionLocationCode(IDebugCodeContext2 codeContext)
        {
            Contract.Requires<ArgumentNullException>(codeContext != null, "codeContext");
            _codeContext = codeContext;
        }

        public BreakpointResolutionLocationCode(BP_RESOLUTION_LOCATION location, bool releaseComObjects)
        {
            if (location.bpType != (uint)enum_BP_TYPE.BPT_CODE)
                throw new ArgumentException();

            try
            {
                if (location.unionmember1 != IntPtr.Zero)
                    _codeContext = Marshal.GetObjectForIUnknown(location.unionmember1) as IDebugCodeContext2;
            }
            finally
            {
                if (releaseComObjects && location.unionmember1 != IntPtr.Zero)
                    Marshal.Release(location.unionmember1);
            }
        }

        public override enum_BP_TYPE Type
        {
            get
            {
                return enum_BP_TYPE.BPT_CODE;
            }
        }

        public override void ToNativeForm(out BP_RESOLUTION_LOCATION location)
        {
            location.bpType = (uint)Type;
            location.unionmember1 = Marshal.GetIUnknownForObject(_codeContext);
            location.unionmember2 = IntPtr.Zero;
            location.unionmember3 = IntPtr.Zero;
            location.unionmember4 = 0;
        }
    }
}
