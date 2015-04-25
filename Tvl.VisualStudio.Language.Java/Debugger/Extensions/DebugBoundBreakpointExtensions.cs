namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    public static class DebugBoundBreakpointExtensions
    {
        public static enum_BP_STATE GetState(this IDebugBoundBreakpoint2 boundBreakpoint)
        {
            Contract.Requires<ArgumentNullException>(boundBreakpoint != null, "boundBreakpoint");

            enum_BP_STATE[] state = new enum_BP_STATE[1];
            ErrorHandler.ThrowOnFailure(boundBreakpoint.GetState(state));
            return state[0];
        }

        public static uint GetHitCount(this IDebugBoundBreakpoint2 boundBreakpoint)
        {
            Contract.Requires<ArgumentNullException>(boundBreakpoint != null, "boundBreakpoint");

            uint hitCount;
            ErrorHandler.ThrowOnFailure(boundBreakpoint.GetHitCount(out hitCount));
            return hitCount;
        }

        public static void SetHitCount(this IDebugBoundBreakpoint2 boundBreakpoint, uint value)
        {
            Contract.Requires<ArgumentNullException>(boundBreakpoint != null, "boundBreakpoint");
            ErrorHandler.ThrowOnFailure(boundBreakpoint.SetHitCount(value));
        }

        public static IDebugBreakpointResolution2 GetBreakpointResolution(this IDebugBoundBreakpoint2 boundBreakpoint)
        {
            Contract.Requires<ArgumentNullException>(boundBreakpoint != null, "boundBreakpoint");

            IDebugBreakpointResolution2 resolution;
            ErrorHandler.ThrowOnFailure(boundBreakpoint.GetBreakpointResolution(out resolution));
            return resolution;
        }
    }
}
