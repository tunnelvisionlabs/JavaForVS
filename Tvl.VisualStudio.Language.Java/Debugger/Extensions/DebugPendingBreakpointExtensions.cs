namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    public static class DebugPendingBreakpointExtensions
    {
        public static enum_PENDING_BP_STATE GetState(this IDebugPendingBreakpoint2 breakpoint)
        {
            Contract.Requires<ArgumentNullException>(breakpoint != null, "breakpoint");

            PENDING_BP_STATE_INFO[] state = new PENDING_BP_STATE_INFO[1];
            ErrorHandler.ThrowOnFailure(breakpoint.GetState(state));
            return state[0].state;
        }

        public static bool IsVirtualized(this IDebugPendingBreakpoint2 breakpoint)
        {
            Contract.Requires<ArgumentNullException>(breakpoint != null, "breakpoint");

            PENDING_BP_STATE_INFO[] state = new PENDING_BP_STATE_INFO[1];
            ErrorHandler.ThrowOnFailure(breakpoint.GetState(state));
            return (state[0].Flags & enum_PENDING_BP_STATE_FLAGS.PBPSF_VIRTUALIZED) != 0;
        }

        public static IEnumerable<IDebugBoundBreakpoint2> EnumBoundBreakpoints(this IDebugPendingBreakpoint2 breakpoint)
        {
            Contract.Requires<ArgumentNullException>(breakpoint != null, "breakpoint");

            IEnumDebugBoundBreakpoints2 boundBreakpoints;
            ErrorHandler.ThrowOnFailure(breakpoint.EnumBoundBreakpoints(out boundBreakpoints));

            uint count;
            ErrorHandler.ThrowOnFailure(boundBreakpoints.GetCount(out count));

            IDebugBoundBreakpoint2[] breakpoints = new IDebugBoundBreakpoint2[count];
            uint fetched = 0;
            ErrorHandler.ThrowOnFailure(boundBreakpoints.Next(count, breakpoints, ref fetched));

            return breakpoints.Take((int)fetched);
        }
    }
}
