namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugCurrentThreadChangedEvent : DebugEvent, IDebugCurrentThreadChangedEvent100
    {
        public DebugCurrentThreadChangedEvent(enum_EVENTATTRIBUTES attributes)
            : base(attributes)
        {
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugCurrentThreadChangedEvent100).GUID;
            }
        }
    }
}
