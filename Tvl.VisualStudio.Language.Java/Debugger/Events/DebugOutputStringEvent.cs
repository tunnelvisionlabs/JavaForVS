namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugOutputStringEvent : DebugEvent, IDebugOutputStringEvent2
    {
        private readonly string _message;

        public DebugOutputStringEvent(string message)
            : base(enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS)
        {
            Contract.Requires<ArgumentNullException>(message != null, "message");

            _message = message;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugOutputStringEvent2).GUID;
            }
        }

        public int GetString(out string pbstrString)
        {
            pbstrString = _message;
            return VSConstants.S_OK;
        }
    }
}
