namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.Java.DebugInterface.Request;

    [ComVisible(true)]
    public class JavaDebugBoundBreakpoint : IDebugBoundBreakpoint3, IDebugBoundBreakpoint2
    {
        private readonly IDebugPendingBreakpoint2 _pendingBreakpoint;
        private readonly JavaDebugProgram _program;
        private readonly IBreakpointRequest _eventRequest;
        private readonly DebugBreakpointResolution _resolution;

        private uint _hitcount;
        private bool _disabled;
        private bool _deleted;

        public JavaDebugBoundBreakpoint(IDebugPendingBreakpoint2 pendingBreakpoint, JavaDebugProgram program, IBreakpointRequest eventRequest, DebugBreakpointResolution resolution)
        {
            Contract.Requires<ArgumentNullException>(pendingBreakpoint != null, "pendingBreakpoint");
            Contract.Requires<ArgumentNullException>(program != null, "program");
            Contract.Requires<ArgumentNullException>(eventRequest != null, "eventRequest");
            Contract.Requires<ArgumentNullException>(resolution != null, "resolution");

            _pendingBreakpoint = pendingBreakpoint;
            _program = program;
            _eventRequest = eventRequest;
            _resolution = resolution;
            _disabled = true;
        }

        public JavaDebugProgram Program
        {
            get
            {
                Contract.Ensures(Contract.Result<JavaDebugProgram>() != null);
                return _program;
            }
        }

        internal IBreakpointRequest EventRequest
        {
            get
            {
                return _eventRequest;
            }
        }

        #region IDebugBoundBreakpoint2 Members

        public int Delete()
        {
            if (_deleted)
            {
                return AD7Constants.E_BP_DELETED;
            }

            _eventRequest.IsEnabled = false;
            _deleted = true;
            return VSConstants.S_OK;
        }

        public int Enable(int fEnable)
        {
            if (_deleted)
            {
                return AD7Constants.E_BP_DELETED;
            }

            bool enable = fEnable != 0;
            if (_disabled == !enable)
                return VSConstants.S_OK;

            _eventRequest.IsEnabled = enable;
            _disabled = !enable;
            return VSConstants.S_OK;
        }

        public int GetBreakpointResolution(out IDebugBreakpointResolution2 resolution)
        {
            if (_deleted)
            {
                resolution = null;
                return AD7Constants.E_BP_DELETED;
            }

            resolution = _resolution;
            return VSConstants.S_OK;
        }

        public int GetHitCount(out uint pdwHitCount)
        {
            if (_deleted)
            {
                pdwHitCount = 0;
                return AD7Constants.E_BP_DELETED;
            }

            pdwHitCount = _hitcount;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Sets the hit count for the bound breakpoint.
        /// </summary>
        /// <param name="dwHitCount">The hit count to set.</param>
        /// <returns>
        /// If successful, returns S_OK; otherwise, returns an error code. Returns E_BP_DELETED
        /// if the state of the bound breakpoint object is set to BPS_DELETED (part of the BP_STATE
        /// enumeration).
        /// </returns>
        /// <remarks>
        /// The hit count is the number of times this breakpoint has fired during the current run of the session.
        /// This method is typically called by the debug engine to update the current hit count on this breakpoint.
        /// </remarks>
        public int SetHitCount(uint dwHitCount)
        {
            if (_deleted)
            {
                return AD7Constants.E_BP_DELETED;
            }

            _hitcount = dwHitCount;
            return VSConstants.S_OK;
        }

        public int GetPendingBreakpoint(out IDebugPendingBreakpoint2 pendingBreakpoint)
        {
            if (_deleted)
            {
                pendingBreakpoint = null;
                return AD7Constants.E_BP_DELETED;
            }

            pendingBreakpoint = _pendingBreakpoint;
            return VSConstants.S_OK;
        }

        public int GetState(enum_BP_STATE[] pState)
        {
            if (pState == null)
                throw new ArgumentNullException("pState");
            if (pState.Length == 0)
                throw new ArgumentException();

            if (_deleted)
                pState[0] = enum_BP_STATE.BPS_DELETED;
            else if (_disabled)
                pState[0] = enum_BP_STATE.BPS_DISABLED;
            else
                pState[0] = enum_BP_STATE.BPS_ENABLED;

            return VSConstants.S_OK;
        }

        public int SetCondition(BP_CONDITION bpCondition)
        {
            if (_deleted)
            {
                return AD7Constants.E_BP_DELETED;
            }

            throw new NotImplementedException();
        }

        public int SetPassCount(BP_PASSCOUNT bpPassCount)
        {
            if (_deleted)
            {
                return AD7Constants.E_BP_DELETED;
            }

            throw new NotImplementedException();
        }

        #endregion

        #region IDebugBoundBreakpoint3 Members

        public int SetTracepoint(string bpBstrTracepoint, enum_BP_FLAGS bpFlags)
        {
            if (_deleted)
            {
                return AD7Constants.E_BP_DELETED;
            }

            throw new NotImplementedException();
        }

        #endregion
    }
}
