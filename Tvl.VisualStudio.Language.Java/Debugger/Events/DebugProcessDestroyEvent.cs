namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugProcessDestroyEvent : DebugEvent, IDebugProcessDestroyEvent2
    {
        public DebugProcessDestroyEvent(enum_EVENTATTRIBUTES attributes)
            : base(attributes)
        {
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugProcessDestroyEvent2).GUID;
            }
        }
    }
}
