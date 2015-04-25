namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugErrorBreakpoint : IDebugErrorBreakpoint2
    {
        private readonly IDebugPendingBreakpoint2 _pendingBreakpoint;
        private readonly IDebugErrorBreakpointResolution2 _resolution;

        public DebugErrorBreakpoint(IDebugPendingBreakpoint2 pendingBreakpoint, IDebugErrorBreakpointResolution2 resolution)
        {
            Contract.Requires<ArgumentNullException>(pendingBreakpoint != null, "pendingBreakpoint");
            Contract.Requires<ArgumentNullException>(resolution != null, "resolution");

            _pendingBreakpoint = pendingBreakpoint;
            _resolution = resolution;
        }

        public int GetBreakpointResolution(out IDebugErrorBreakpointResolution2 ppErrorResolution)
        {
            ppErrorResolution = _resolution;
            return VSConstants.S_OK;
        }

        public int GetPendingBreakpoint(out IDebugPendingBreakpoint2 ppPendingBreakpoint)
        {
            ppPendingBreakpoint = _pendingBreakpoint;
            return VSConstants.S_OK;
        }
    }
}
