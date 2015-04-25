namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    public sealed class BreakpointLocationCodeContext : BreakpointLocation
    {
        private readonly IDebugCodeContext2 _codeContext;

        public BreakpointLocationCodeContext(BP_LOCATION location, bool releaseComObjects)
        {
            Contract.Requires<ArgumentException>((enum_BP_LOCATION_TYPE)location.bpLocationType == enum_BP_LOCATION_TYPE.BPLT_CODE_CONTEXT);

            try
            {
                if (location.unionmember1 != IntPtr.Zero)
                    _codeContext = Marshal.GetObjectForIUnknown(location.unionmember2) as IDebugCodeContext2;
            }
            finally
            {
                if (releaseComObjects)
                {
                    if (location.unionmember1 != IntPtr.Zero)
                        Marshal.Release(location.unionmember1);
                }
            }
        }

        public override enum_BP_LOCATION_TYPE LocationType
        {
            get
            {
                return enum_BP_LOCATION_TYPE.BPLT_CODE_CONTEXT;
            }
        }

        public IDebugCodeContext2 CodeContext
        {
            get
            {
                return _codeContext;
            }
        }
    }
}
