namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugThreadNameChangedEvent : DebugEvent, IDebugThreadNameChangedEvent2
    {
        public DebugThreadNameChangedEvent(enum_EVENTATTRIBUTES attributes)
            : base(attributes)
        {
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugThreadNameChangedEvent2).GUID;
            }
        }
    }
}
