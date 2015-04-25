namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugModuleLoadEvent : DebugEvent, IDebugModuleLoadEvent2
    {
        private readonly IDebugModule2 _module;
        private readonly string _debugMessage;
        private readonly bool _isLoading;

        public DebugModuleLoadEvent(enum_EVENTATTRIBUTES attributes, IDebugModule2 module, string debugMessage, bool isLoading)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(module != null, "module");

            _module = module;
            _debugMessage = debugMessage;
            _isLoading = isLoading;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugModuleLoadEvent2).GUID;
            }
        }

        public int GetModule(out IDebugModule2 pModule, ref string pbstrDebugMessage, ref int pbLoad)
        {
            pModule = _module;
            pbstrDebugMessage = _debugMessage;
            pbLoad = _isLoading ? 1 : 0;
            return VSConstants.S_OK;
        }
    }
}
