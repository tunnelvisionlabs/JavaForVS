namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugErrorBreakpointResolution : IDebugErrorBreakpointResolution2
    {
        private readonly IDebugProgram2 _program;
        private readonly IDebugThread2 _thread;
        private readonly enum_BP_TYPE _breakpointType;
        private readonly enum_BP_ERROR_TYPE _errorType;
        private readonly BreakpointResolutionLocation _location;
        private readonly string _message;

        public DebugErrorBreakpointResolution(IDebugProgram2 program, IDebugThread2 thread, enum_BP_TYPE breakpointType, BreakpointResolutionLocation location, enum_BP_ERROR_TYPE errorType, string message)
        {
            //Contract.Requires<ArgumentNullException>(program != null, "program");
            //Contract.Requires<ArgumentNullException>(thread != null, "thread");
            Contract.Requires<ArgumentNullException>(message != null, "message");
            Contract.Requires<ArgumentNullException>(location != null, "location");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(message));

            _program = program;
            _thread = thread;
            _breakpointType = breakpointType;
            _errorType = errorType;
            _location = location;
            _message = message;
        }

        public int GetBreakpointType(enum_BP_TYPE[] pBPType)
        {
            if (pBPType == null)
                throw new ArgumentNullException("pBPType");
            if (pBPType.Length == 0)
                throw new ArgumentException();

            pBPType[0] = _breakpointType;
            return VSConstants.S_OK;
        }

        public int GetResolutionInfo(enum_BPERESI_FIELDS dwFields, BP_ERROR_RESOLUTION_INFO[] pErrorResolutionInfo)
        {
            if (pErrorResolutionInfo == null)
                throw new ArgumentNullException("pErrorResolutionInfo");
            if (pErrorResolutionInfo.Length == 0)
                throw new ArgumentException();

            pErrorResolutionInfo[0].dwFields = 0;

            if ((dwFields & enum_BPERESI_FIELDS.BPERESI_MESSAGE) != 0 && _message != null)
            {
                pErrorResolutionInfo[0].dwFields |= enum_BPERESI_FIELDS.BPERESI_MESSAGE;
                pErrorResolutionInfo[0].bstrMessage = _message;
            }

            if ((dwFields & enum_BPERESI_FIELDS.BPERESI_PROGRAM) != 0 && _program != null)
            {
                pErrorResolutionInfo[0].dwFields |= enum_BPERESI_FIELDS.BPERESI_PROGRAM;
                pErrorResolutionInfo[0].pProgram = _program;
            }

            if ((dwFields & enum_BPERESI_FIELDS.BPERESI_THREAD) != 0 && _thread != null)
            {
                pErrorResolutionInfo[0].dwFields |= enum_BPERESI_FIELDS.BPERESI_THREAD;
                pErrorResolutionInfo[0].pThread = _thread;
            }

            if ((dwFields & enum_BPERESI_FIELDS.BPERESI_TYPE) != 0)
            {
                pErrorResolutionInfo[0].dwFields |= enum_BPERESI_FIELDS.BPERESI_TYPE;
                pErrorResolutionInfo[0].dwType = _errorType;
            }

            if ((dwFields & enum_BPERESI_FIELDS.BPERESI_BPRESLOCATION) != 0 && _location != null)
            {
                pErrorResolutionInfo[0].dwFields |= enum_BPERESI_FIELDS.BPERESI_BPRESLOCATION;
                _location.ToNativeForm(out pErrorResolutionInfo[0].bpResLocation);
            }

            return VSConstants.S_OK;
        }
    }
}
