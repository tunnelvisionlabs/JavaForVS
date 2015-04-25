namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    public sealed class BreakpointLocationDataString : BreakpointLocation
    {
        private readonly IDebugThread2 _thread;
        private readonly string _context;
        private readonly string _dataExpression;
        private readonly uint _elementCount;

        public BreakpointLocationDataString(BP_LOCATION location, bool releaseComObjects)
        {
            Contract.Requires<ArgumentException>((enum_BP_LOCATION_TYPE)location.bpLocationType == enum_BP_LOCATION_TYPE.BPLT_DATA_STRING);

            try
            {
                if (location.unionmember1 != IntPtr.Zero)
                    _thread = Marshal.GetObjectForIUnknown(location.unionmember1) as IDebugThread2;
                if (location.unionmember2 != IntPtr.Zero)
                    _context = Marshal.PtrToStringBSTR(location.unionmember2);
                if (location.unionmember3 != IntPtr.Zero)
                    _dataExpression = Marshal.PtrToStringBSTR(location.unionmember3);

                _elementCount = (uint)location.unionmember4;
            }
            finally
            {
                if (releaseComObjects)
                {
                    if (location.unionmember1 != IntPtr.Zero)
                        Marshal.Release(location.unionmember1);
                    if (location.unionmember2 != IntPtr.Zero)
                        Marshal.FreeBSTR(location.unionmember2);
                    if (location.unionmember3 != IntPtr.Zero)
                        Marshal.FreeBSTR(location.unionmember3);
                }
            }
        }

        public override enum_BP_LOCATION_TYPE LocationType
        {
            get
            {
                return enum_BP_LOCATION_TYPE.BPLT_DATA_STRING;
            }
        }

        public IDebugThread2 Thread
        {
            get
            {
                return _thread;
            }
        }

        public string Context
        {
            get
            {
                return _context;
            }
        }

        public string DataExpression
        {
            get
            {
                return _dataExpression;
            }
        }

        public uint ElementCount
        {
            get
            {
                return _elementCount;
            }
        }
    }
}
