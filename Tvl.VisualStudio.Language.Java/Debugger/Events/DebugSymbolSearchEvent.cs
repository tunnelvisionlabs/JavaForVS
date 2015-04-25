namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugSymbolSearchEvent : DebugEvent, IDebugSymbolSearchEvent2
    {
        private readonly IDebugModule3 _module;
        private readonly string _debugMessage;
        private readonly enum_MODULE_INFO_FLAGS _moduleInfoFlags;

        public DebugSymbolSearchEvent(enum_EVENTATTRIBUTES attributes, IDebugModule3 module, string debugMessage, enum_MODULE_INFO_FLAGS moduleInfoFlags)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(module != null, "module");
            Contract.Requires<ArgumentNullException>(debugMessage != null, "debugMessage");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(debugMessage));

            _module = module;
            _debugMessage = debugMessage;
            _moduleInfoFlags = moduleInfoFlags;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugSymbolSearchEvent2).GUID;
            }
        }

        public int GetSymbolSearchInfo(out IDebugModule3 pModule, ref string pbstrDebugMessage, enum_MODULE_INFO_FLAGS[] pdwModuleInfoFlags)
        {
            if (pdwModuleInfoFlags == null)
                throw new ArgumentNullException("pdwModuleInfoFlags");
            if (pdwModuleInfoFlags.Length < 1)
                throw new ArgumentException();

            pModule = _module;
            pbstrDebugMessage = _debugMessage;
            pdwModuleInfoFlags[0] = _moduleInfoFlags;
            return VSConstants.S_OK;
        }
    }
}
