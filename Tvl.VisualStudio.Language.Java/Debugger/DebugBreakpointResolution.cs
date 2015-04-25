namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugBreakpointResolution : IDebugBreakpointResolution2
    {
        private readonly IDebugProgram2 _program;
        private readonly IDebugThread2 _thread;
        private readonly enum_BP_TYPE _breakpointType;
        private readonly BreakpointResolutionLocation _location;

        public DebugBreakpointResolution(IDebugProgram2 program, IDebugThread2 thread, enum_BP_TYPE breakpointType, BreakpointResolutionLocation location)
        {
            Contract.Requires<ArgumentNullException>(program != null, "program");
            Contract.Requires<ArgumentNullException>(location != null, "location");

            _program = program;
            _thread = thread;
            _breakpointType = breakpointType;
            _location = location;
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

        public int GetResolutionInfo(enum_BPRESI_FIELDS dwFields, BP_RESOLUTION_INFO[] pBPResolutionInfo)
        {
            if (pBPResolutionInfo == null)
                throw new ArgumentNullException("pBPResolutionInfo");
            if (pBPResolutionInfo.Length == 0)
                throw new ArgumentException();

            pBPResolutionInfo[0].dwFields = 0;

            if ((dwFields & enum_BPRESI_FIELDS.BPRESI_PROGRAM) != 0)
            {
                pBPResolutionInfo[0].dwFields |= enum_BPRESI_FIELDS.BPRESI_PROGRAM;
                pBPResolutionInfo[0].pProgram = _program;
            }

            if ((dwFields & enum_BPRESI_FIELDS.BPRESI_THREAD) != 0 && _thread != null)
            {
                pBPResolutionInfo[0].dwFields |= enum_BPRESI_FIELDS.BPRESI_THREAD;
                pBPResolutionInfo[0].pThread = _thread;
            }

            if ((dwFields & enum_BPRESI_FIELDS.BPRESI_BPRESLOCATION) != 0 && false)
            {
                pBPResolutionInfo[0].dwFields |= enum_BPRESI_FIELDS.BPRESI_BPRESLOCATION;
                _location.ToNativeForm(out pBPResolutionInfo[0].bpResLocation);
            }

            return VSConstants.S_OK;
        }
    }
}
