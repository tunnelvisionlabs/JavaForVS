namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugBreakpointUnboundEvent : DebugEvent, IDebugBreakpointUnboundEvent2
    {
        private readonly IDebugBoundBreakpoint2 _breakpoint;
        private readonly enum_BP_UNBOUND_REASON _reason;

        public DebugBreakpointUnboundEvent(enum_EVENTATTRIBUTES attributes, IDebugBoundBreakpoint2 breakpoint, enum_BP_UNBOUND_REASON reason)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(breakpoint != null, "breakpoint");

            _breakpoint = breakpoint;
            _reason = reason;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugBreakpointUnboundEvent2).GUID;
            }
        }

        public int GetBreakpoint(out IDebugBoundBreakpoint2 ppBP)
        {
            ppBP = _breakpoint;
            return VSConstants.S_OK;
        }

        public int GetReason(enum_BP_UNBOUND_REASON[] pdwUnboundReason)
        {
            if (pdwUnboundReason == null)
                throw new ArgumentNullException("pdwUnboundReason");
            if (pdwUnboundReason.Length < 1)
                throw new ArgumentException();

            pdwUnboundReason[0] = _reason;
            return VSConstants.S_OK;
        }
    }
}
