namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    public sealed class BreakpointLocationCodeFunctionOffset : BreakpointLocation
    {
        private readonly string _context;
        private readonly IDebugFunctionPosition2 _functionPosition;

        public BreakpointLocationCodeFunctionOffset(BP_LOCATION location, bool releaseComObjects)
        {
            Contract.Requires<ArgumentException>((enum_BP_LOCATION_TYPE)location.bpLocationType == enum_BP_LOCATION_TYPE.BPLT_CODE_FUNC_OFFSET);

            try
            {
                if (location.unionmember1 != IntPtr.Zero)
                    _context = Marshal.PtrToStringBSTR(location.unionmember1);
                if (location.unionmember2 != IntPtr.Zero)
                    _functionPosition = Marshal.GetObjectForIUnknown(location.unionmember2) as IDebugFunctionPosition2;
            }
            finally
            {
                if (releaseComObjects)
                {
                    if (location.unionmember1 != IntPtr.Zero)
                        Marshal.FreeBSTR(location.unionmember1);
                    if (location.unionmember2 != IntPtr.Zero)
                        Marshal.Release(location.unionmember2);
                }
            }
        }

        public override enum_BP_LOCATION_TYPE LocationType
        {
            get
            {
                return enum_BP_LOCATION_TYPE.BPLT_CODE_FUNC_OFFSET;
            }
        }

        public string Context
        {
            get
            {
                return _context;
            }
        }

        public IDebugFunctionPosition2 FunctionPosition
        {
            get
            {
                return _functionPosition;
            }
        }
    }
}
