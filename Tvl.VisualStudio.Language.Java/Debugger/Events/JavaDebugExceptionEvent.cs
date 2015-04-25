namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.Java.DebugInterface;

    [ComVisible(true)]
    public class JavaDebugExceptionEvent : DebugEvent, IDebugExceptionEvent2
    {
        private readonly JavaDebugProgram _program;
        private readonly IThreadReference _thread;
        private readonly IObjectReference _exceptionObject;

        private readonly ILocation _location;
        private readonly ILocation _catchLocation;


        public JavaDebugExceptionEvent(enum_EVENTATTRIBUTES attributes, JavaDebugProgram program, IThreadReference thread, IObjectReference exceptionObject, ILocation location, ILocation catchLocation)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(program != null, "program");
            Contract.Requires<ArgumentNullException>(thread != null, "thread");
            Contract.Requires<ArgumentNullException>(exceptionObject != null, "exceptionObject");

            _program = program;
            _thread = thread;
            _exceptionObject = exceptionObject;
            _location = location;
            _catchLocation = catchLocation;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugExceptionEvent2).GUID;
            }
        }

        public string GetDescription()
        {
            string messageFormat = "{0} exception of type '{1}' occurred in {2}.{3}";
            string classification = _catchLocation != null ? "A first chance" : "An unhandled";
            string type = _exceptionObject.GetReferenceType().GetName();
            IMethod method = _thread.GetFrame(0).GetLocation().GetMethod();
            string locationClass = method.GetDeclaringType().GetName();
            string locationMethod = method.GetName();
            return string.Format(messageFormat, classification, type, locationClass, locationMethod);
        }

        /// <summary>
        /// Determines whether or not the debug engine (DE) supports the option of passing this exception
        /// to the program being debugged when execution resumes.
        /// </summary>
        /// <returns>
        /// Returns either S_OK (the exception can be passed to the program) or S_FALSE (the exception cannot be passed on).
        /// </returns>
        /// <remarks>
        /// The DE must have a default action for passing to the debuggee. The IDE may receive the IDebugExceptionEvent2 event
        /// and call the IDebugProcess3.Continue method without calling the CanPassToDebuggee method. Therefore, the DE should
        /// have a default case for passing the exception on or not.
        /// </remarks>
        public int CanPassToDebuggee()
        {
            return VSConstants.S_OK;
        }

        public int GetException(EXCEPTION_INFO[] pExceptionInfo)
        {
            if (pExceptionInfo == null)
                throw new ArgumentNullException("pExceptionInfo");
            if (pExceptionInfo.Length == 0)
                throw new ArgumentException();

            pExceptionInfo[0].bstrExceptionName = _exceptionObject.GetReferenceType().GetName();
            pExceptionInfo[0].bstrProgramName = _program.GetName();
            pExceptionInfo[0].dwCode = 0;
            pExceptionInfo[0].dwState = _catchLocation != null ? enum_EXCEPTION_STATE.EXCEPTION_STOP_FIRST_CHANCE : enum_EXCEPTION_STATE.EXCEPTION_STOP_SECOND_CHANCE;
            pExceptionInfo[0].guidType = JavaDebuggerConstants.JavaDebugEngineGuid;
            pExceptionInfo[0].pProgram = _program;
            return VSConstants.S_OK;
        }

        public int GetExceptionDescription(out string pbstrDescription)
        {
            pbstrDescription = GetDescription();
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Specifies whether the exception should be passed on to the program being debugged when execution
        /// resumes, or if the exception should be discarded.
        /// </summary>
        /// <param name="fPass">[in] Nonzero (TRUE) if the exception should be passed on to the program being debugged when execution resumes, or zero (FALSE) if the exception should be discarded.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// Calling this method does not actually cause any code to be executed in the program being debugged.
        /// The call is merely to set the state for the next code execution. For example, calls to the
        /// IDebugExceptionEvent2.CanPassToDebuggee method may return S_OK with the EXCEPTION_INFO.dwState field
        /// set to EXCEPTION_STOP_SECOND_CHANCE.
        ///
        /// The IDE may receive the IDebugExceptionEvent2 event and call the IDebugProgram2.Continue method.
        /// The debug engine (DE) should have a default behavior to handle the case if the PassToDebuggee
        /// method is not called.
        /// </remarks>
        public int PassToDebuggee(int fPass)
        {
            if (fPass == 0)
                throw new NotImplementedException();

            return VSConstants.S_OK;
        }
    }
}
