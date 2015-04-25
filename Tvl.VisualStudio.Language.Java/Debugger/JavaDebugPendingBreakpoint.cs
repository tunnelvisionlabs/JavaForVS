namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class JavaDebugPendingBreakpoint
        : IDebugPendingBreakpoint3
        , IDebugPendingBreakpoint2
    {
        private readonly IDebugBreakpointRequest2 _request;
        private readonly List<IDebugBoundBreakpoint2> _boundBreakpoints = new List<IDebugBoundBreakpoint2>();
        private readonly List<IDebugErrorBreakpoint2> _errorBreakpoints = new List<IDebugErrorBreakpoint2>();

        public JavaDebugPendingBreakpoint(IDebugBreakpointRequest2 request)
        {
            Contract.Requires<ArgumentNullException>(request != null, "request");

            _request = request;
        }

        #region IDebugPendingBreakpoint3 Members

        public int Bind()
        {
            throw new NotImplementedException();
        }

        public int CanBind(out IEnumDebugErrorBreakpoints2 ppErrorEnum)
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public int Enable(int fEnable)
        {
            throw new NotImplementedException();
        }

        public int EnumBoundBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            throw new NotImplementedException();
        }

        public int EnumErrorBreakpoints(enum_BP_ERROR_TYPE bpErrorType, out IEnumDebugErrorBreakpoints2 ppEnum)
        {
            throw new NotImplementedException();
        }

        public int GetBreakpointRequest(out IDebugBreakpointRequest2 ppBPRequest)
        {
            throw new NotImplementedException();
        }

        public int GetErrorResolutionInfo(enum_BPERESI_FIELDS dwFields, BP_ERROR_RESOLUTION_INFO[] pErrorResolutionInfo)
        {
            throw new NotImplementedException();
        }

        public int GetState(PENDING_BP_STATE_INFO[] pState)
        {
            throw new NotImplementedException();
        }

        public int SetCondition(BP_CONDITION bpCondition)
        {
            throw new NotImplementedException();
        }

        public int SetPassCount(BP_PASSCOUNT bpPassCount)
        {
            throw new NotImplementedException();
        }

        public int Virtualize(int fVirtualize)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
