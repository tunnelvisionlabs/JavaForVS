namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugEntryPointEvent : DebugEvent, IDebugEntryPointEvent2
    {
        public DebugEntryPointEvent(enum_EVENTATTRIBUTES attributes)
            : base(attributes)
        {
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugEntryPointEvent2).GUID;
            }
        }
    }
}
