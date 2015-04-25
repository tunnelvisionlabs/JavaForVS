namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugStepCompleteEvent : DebugEvent, IDebugStepCompleteEvent2, IDebugStepCompleteEvent90
    {
        private readonly IEnumDebugBoundBreakpoints2 _breakpoints;
        private readonly enum_STEPSTATUS _stepStatus;

        public DebugStepCompleteEvent(enum_EVENTATTRIBUTES attributes)
            : this(attributes, null, enum_STEPSTATUS.STEPSTATUS_UNKNOWN)
        {
        }

        public DebugStepCompleteEvent(enum_EVENTATTRIBUTES attributes, IEnumDebugBoundBreakpoints2 breakpoints, enum_STEPSTATUS stepStatus)
            : base(attributes)
        {
            _breakpoints = breakpoints;
            _stepStatus = stepStatus;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugStepCompleteEvent2).GUID;
            }
        }

        public int EnumBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            ppEnum = _breakpoints;
            return ppEnum != null ? VSConstants.S_OK : VSConstants.E_NOTIMPL;
        }

        public int GetStepStatus(out uint pStatus)
        {
            pStatus = (uint)_stepStatus;
            return VSConstants.S_OK;
        }
    }
}
