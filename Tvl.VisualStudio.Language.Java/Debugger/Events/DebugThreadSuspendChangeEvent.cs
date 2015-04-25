namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugThreadSuspendChangeEvent : DebugEvent, IDebugThreadSuspendChangeEvent100
    {
        private readonly IDebugThread100 _thread;

        public DebugThreadSuspendChangeEvent(enum_EVENTATTRIBUTES attributes, IDebugThread100 thread)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(thread != null, "thread");
            _thread = thread;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugThreadSuspendChangeEvent100).GUID;
            }
        }

        public int GetThread(out IDebugThread100 ppThread)
        {
            ppThread = _thread;
            return VSConstants.S_OK;
        }
    }
}
