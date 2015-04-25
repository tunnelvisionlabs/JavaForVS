namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugThreadDestroyEvent : DebugEvent, IDebugThreadDestroyEvent2
    {
        private readonly uint _exitCode;

        public DebugThreadDestroyEvent(enum_EVENTATTRIBUTES attributes, uint exitCode)
            : base(attributes)
        {
            _exitCode = exitCode;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugThreadDestroyEvent2).GUID;
            }
        }

        public int GetExitCode(out uint pdwExit)
        {
            pdwExit = _exitCode;
            return VSConstants.S_OK;
        }
    }
}
