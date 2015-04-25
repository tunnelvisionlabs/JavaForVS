namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugProcessContinueEvent : DebugEvent, IDebugProcessContinueEvent100
    {
        private readonly IDebugProcess2 _process;

        public DebugProcessContinueEvent(enum_EVENTATTRIBUTES attributes, IDebugProcess2 process)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(process != null, "process");
            _process = process;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugProcessContinueEvent100).GUID;
            }
        }

        public int GetProcess(out IDebugProcess2 ppProcess)
        {
            ppProcess = _process;
            return VSConstants.S_OK;
        }
    }
}
