namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugNoSymbolsEvent : DebugEvent, IDebugNoSymbolsEvent2
    {
        public DebugNoSymbolsEvent(enum_EVENTATTRIBUTES attributes)
            : base(attributes)
        {
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugNoSymbolsEvent2).GUID;
            }
        }
    }
}
