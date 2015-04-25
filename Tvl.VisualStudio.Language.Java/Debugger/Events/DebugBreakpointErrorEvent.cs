namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugBreakpointErrorEvent : DebugEvent, IDebugBreakpointErrorEvent2
    {
        private readonly IDebugErrorBreakpoint2 _breakpoint;

        public DebugBreakpointErrorEvent(enum_EVENTATTRIBUTES attributes, IDebugErrorBreakpoint2 breakpoint)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(breakpoint != null, "breakpoint");
            _breakpoint = breakpoint;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugBreakpointErrorEvent2).GUID;
            }
        }

        public int GetErrorBreakpoint(out IDebugErrorBreakpoint2 ppErrorBP)
        {
            ppErrorBP = _breakpoint;
            return VSConstants.S_OK;
        }
    }
}
