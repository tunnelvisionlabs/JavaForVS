namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugBreakpointEvent : DebugEvent, IDebugBreakpointEvent2
    {
        private readonly IEnumDebugBoundBreakpoints2 _breakpoints;

        public DebugBreakpointEvent(enum_EVENTATTRIBUTES attributes, IEnumDebugBoundBreakpoints2 breakpoints)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(breakpoints != null, "breakpoints");
            _breakpoints = breakpoints;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugBreakpointEvent2).GUID;
            }
        }

        public int EnumBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            ppEnum = _breakpoints;
            return VSConstants.S_OK;
        }
    }
}
