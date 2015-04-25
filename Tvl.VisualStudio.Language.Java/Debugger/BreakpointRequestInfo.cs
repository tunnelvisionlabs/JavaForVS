namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    public sealed class BreakpointRequestInfo
    {
        private readonly IDebugBreakpointRequest2 _request;

        private readonly IDebugProgram2 _program;
        private readonly IDebugThread2 _thread;
        private readonly Guid? _languageGuid;
        private readonly Guid? _vendorGuid;
        private readonly string _constraint;
        private readonly string _programName;
        private readonly string _threadName;
        private readonly string _tracepoint;
        private readonly enum_BP_FLAGS? _flags;
        private readonly BP_PASSCOUNT? _passCount;
        private readonly BP_CONDITION? _condition;
        private readonly BreakpointLocation _location;

        public BreakpointRequestInfo(IDebugBreakpointRequest2 request)
        {
            Contract.Requires<ArgumentNullException>(request != null, "request");

            _request = request;

            IDebugBreakpointRequest3 request3 = request as IDebugBreakpointRequest3;
            BP_REQUEST_INFO2[] requestInfo2 = new BP_REQUEST_INFO2[1];
            if (request3 != null && ErrorHandler.Succeeded(request3.GetRequestInfo2(enum_BPREQI_FIELDS.BPREQI_ALLFIELDS, requestInfo2)))
            {
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_PROGRAM) != 0)
                    _program = requestInfo2[0].pProgram;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_THREAD) != 0)
                    _thread = requestInfo2[0].pThread;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_LANGUAGE) != 0)
                    _languageGuid = requestInfo2[0].guidLanguage;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_VENDOR) != 0)
                    _vendorGuid = requestInfo2[0].guidVendor;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_CONSTRAINT) != 0)
                    _constraint = requestInfo2[0].bstrConstraint;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_PROGRAMNAME) != 0)
                    _programName = requestInfo2[0].bstrProgramName;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_THREADNAME) != 0)
                    _threadName = requestInfo2[0].bstrThreadName;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_TRACEPOINT) != 0)
                    _tracepoint = requestInfo2[0].bstrTracepoint;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_FLAGS) != 0)
                    _flags = requestInfo2[0].dwFlags;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_PASSCOUNT) != 0)
                    _passCount = requestInfo2[0].bpPassCount;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_CONDITION) != 0)
                    _condition = requestInfo2[0].bpCondition;
                if ((requestInfo2[0].dwFields & enum_BPREQI_FIELDS.BPREQI_BPLOCATION) != 0)
                    _location = BreakpointLocation.FromNativeForm(requestInfo2[0].bpLocation, true);
            }
            else
            {
                BP_REQUEST_INFO[] requestInfo = new BP_REQUEST_INFO[1];
                ErrorHandler.ThrowOnFailure(request.GetRequestInfo(enum_BPREQI_FIELDS.BPREQI_ALLFIELDS, requestInfo));
                if ((requestInfo[0].dwFields & enum_BPREQI_FIELDS.BPREQI_PROGRAM) != 0)
                    _program = requestInfo[0].pProgram;
                if ((requestInfo[0].dwFields & enum_BPREQI_FIELDS.BPREQI_THREAD) != 0)
                    _thread = requestInfo[0].pThread;
                if ((requestInfo[0].dwFields & enum_BPREQI_FIELDS.BPREQI_LANGUAGE) != 0)
                    _languageGuid = requestInfo[0].guidLanguage;
                if ((requestInfo[0].dwFields & enum_BPREQI_FIELDS.BPREQI_PROGRAMNAME) != 0)
                    _programName = requestInfo[0].bstrProgramName;
                if ((requestInfo[0].dwFields & enum_BPREQI_FIELDS.BPREQI_THREADNAME) != 0)
                    _threadName = requestInfo[0].bstrThreadName;
                if ((requestInfo[0].dwFields & enum_BPREQI_FIELDS.BPREQI_FLAGS) != 0)
                    _flags = requestInfo[0].dwFlags;
                if ((requestInfo[0].dwFields & enum_BPREQI_FIELDS.BPREQI_PASSCOUNT) != 0)
                    _passCount = requestInfo[0].bpPassCount;
                if ((requestInfo[0].dwFields & enum_BPREQI_FIELDS.BPREQI_CONDITION) != 0)
                    _condition = requestInfo[0].bpCondition;
                if ((requestInfo[0].dwFields & enum_BPREQI_FIELDS.BPREQI_BPLOCATION) != 0)
                    _location = BreakpointLocation.FromNativeForm(requestInfo[0].bpLocation, true);
            }
        }

        public IDebugBreakpointRequest2 Request
        {
            get
            {
                return _request;
            }
        }

        public IDebugProgram2 Program
        {
            get
            {
                return _program;
            }
        }

        public IDebugThread2 Thread
        {
            get
            {
                return _thread;
            }
        }

        public Guid? LanguageGuid
        {
            get
            {
                return _languageGuid;
            }
        }

        public Guid? VendorGuid
        {
            get
            {
                return _vendorGuid;
            }
        }

        public string Constraint
        {
            get
            {
                return _constraint;
            }
        }

        public string ProgramName
        {
            get
            {
                return _programName;
            }
        }

        public string ThreadName
        {
            get
            {
                return _threadName;
            }
        }

        public string Tracepoint
        {
            get
            {
                return _tracepoint;
            }
        }

        public enum_BP_FLAGS? Flags
        {
            get
            {
                return _flags;
            }
        }

        public BP_PASSCOUNT? PassCount
        {
            get
            {
                return _passCount;
            }
        }

        public BP_CONDITION? Condition
        {
            get
            {
                return _condition;
            }
        }

        public BreakpointLocation Location
        {
            get
            {
                return _location;
            }
        }
    }
}
