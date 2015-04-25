namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.VisualStudio.Language.Java.Debugger.Events;

    public static class DebugEventCallbackExtensions
    {
        public static int Event(this IDebugEventCallback2 callback, IDebugEngine2 engine, IDebugProcess2 process, IDebugProgram2 program, IDebugThread2 thread, DebugEvent debugEvent)
        {
            Contract.Requires<ArgumentNullException>(callback != null, "callback");
            Contract.Requires<ArgumentNullException>(debugEvent != null, "debugEvent");
            Contract.Requires<ArgumentNullException>(engine != null, "engine");

            Guid guid = debugEvent.EventGuid;
            enum_EVENTATTRIBUTES attributes = debugEvent.Attributes;
            return callback.Event(engine, process, program, thread, debugEvent, ref guid, (uint)attributes);
        }
    }
}
