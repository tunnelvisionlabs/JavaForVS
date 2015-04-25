namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    public sealed class BreakpointLocationCodeAddress : BreakpointLocation
    {
        private readonly string _context;
        private readonly string _moduleUrl;
        private readonly string _function;
        private readonly string _address;

        public BreakpointLocationCodeAddress(BP_LOCATION location, bool releaseComObjects)
        {
            Contract.Requires<ArgumentException>((enum_BP_LOCATION_TYPE)location.bpLocationType == enum_BP_LOCATION_TYPE.BPLT_CODE_ADDRESS);

            try
            {
                if (location.unionmember1 != IntPtr.Zero)
                    _context = Marshal.PtrToStringBSTR(location.unionmember1);
                if (location.unionmember2 != IntPtr.Zero)
                    _moduleUrl = Marshal.PtrToStringBSTR(location.unionmember2);
                if (location.unionmember3 != IntPtr.Zero)
                    _function = Marshal.PtrToStringBSTR(location.unionmember3);
                if (location.unionmember4 != IntPtr.Zero)
                    _address = Marshal.PtrToStringBSTR(location.unionmember4);
            }
            finally
            {
                if (releaseComObjects)
                {
                    if (location.unionmember1 != IntPtr.Zero)
                        Marshal.FreeBSTR(location.unionmember1);
                    if (location.unionmember2 != IntPtr.Zero)
                        Marshal.FreeBSTR(location.unionmember2);
                    if (location.unionmember3 != IntPtr.Zero)
                        Marshal.FreeBSTR(location.unionmember3);
                    if (location.unionmember4 != IntPtr.Zero)
                        Marshal.FreeBSTR(location.unionmember4);
                }
            }
        }

        public override enum_BP_LOCATION_TYPE LocationType
        {
            get
            {
                return enum_BP_LOCATION_TYPE.BPLT_CODE_ADDRESS;
            }
        }

        public string Context
        {
            get
            {
                return _context;
            }
        }

        public string ModuleUrl
        {
            get
            {
                return _moduleUrl;
            }
        }

        public string Function
        {
            get
            {
                return _function;
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
        }
    }
}
