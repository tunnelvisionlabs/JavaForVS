namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugEngineCreateEvent : DebugEvent, IDebugEngineCreateEvent2
    {
        private readonly IDebugEngine2 _engine;

        public DebugEngineCreateEvent(enum_EVENTATTRIBUTES attributes, IDebugEngine2 engine)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(engine != null, "engine");
            _engine = engine;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugEngineCreateEvent2).GUID;
            }
        }

        public int GetEngine(out IDebugEngine2 pEngine)
        {
            pEngine = _engine;
            return VSConstants.S_OK;
        }
    }
}
