namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    public sealed class BreakpointLocationResolution : BreakpointLocation
    {
        private readonly IDebugBreakpointResolution2 _resolution;

        public BreakpointLocationResolution(BP_LOCATION location, bool releaseComObjects)
        {
            Contract.Requires<ArgumentException>((enum_BP_LOCATION_TYPE)location.bpLocationType == enum_BP_LOCATION_TYPE.BPLT_RESOLUTION);

            try
            {
                if (location.unionmember1 != IntPtr.Zero)
                    _resolution = Marshal.GetObjectForIUnknown(location.unionmember1) as IDebugBreakpointResolution2;
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
                return enum_BP_LOCATION_TYPE.BPLT_RESOLUTION;
            }
        }

        public IDebugBreakpointResolution2 Resolution
        {
            get
            {
                return _resolution;
            }
        }
    }
}
