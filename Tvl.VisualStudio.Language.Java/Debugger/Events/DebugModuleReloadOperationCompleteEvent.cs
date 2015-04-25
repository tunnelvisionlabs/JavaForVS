namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugModuleReloadOperationCompleteEvent : DebugEvent, IDebugModuleReloadOperationCompleteEvent100
    {
        public DebugModuleReloadOperationCompleteEvent(enum_EVENTATTRIBUTES attributes)
            : base(attributes)
        {
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugModuleReloadOperationCompleteEvent100).GUID;
            }
        }
    }
}
