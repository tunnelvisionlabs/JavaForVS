namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Microsoft.VisualStudio.Utilities;
    using Tvl.Java.DebugInterface;
    using Tvl.Java.DebugInterface.Request;
    using Tvl.VisualStudio.Language.Java.Debugger.Events;

    using EnumDebugPrograms = Tvl.VisualStudio.Language.Java.Debugger.Collections.EnumDebugPrograms;
    using Trace = System.Diagnostics.Trace;

    [ComVisible(true)]
    [Guid(JavaDebuggerConstants.JavaDebugEngineGuidString)]
    public class JavaDebugEngine
        : IDebugEngine3
        , IDebugEngine2
        //, IDebugEngineLaunch2
    {
        private CultureInfo _culture;
        private string _registryRoot;
        private string[] _symbolSearchPath;
        private string[] _symbolCachePath;

        private readonly List<JavaDebugProgram> _programs = new List<JavaDebugProgram>();
        private readonly List<IDebugPendingBreakpoint2> _pendingBreakpoints = new List<IDebugPendingBreakpoint2>();
        private readonly HashSet<IJavaVirtualizableBreakpoint> _virtualizedBreakpoints = new HashSet<IJavaVirtualizableBreakpoint>();

        private readonly Dictionary<string, EXCEPTION_INFO> _exceptions =
            new Dictionary<string, EXCEPTION_INFO>();

        public JavaDebugEngine()
        {
        }

        internal IEnumerable<JavaDebugProgram> Programs
        {
            get
            {
                return _programs;
            }
        }

        internal ReadOnlyCollection<IDebugPendingBreakpoint2> PendingBreakpoints
        {
            get
            {
                return _pendingBreakpoints.AsReadOnly();
            }
        }

        internal ISet<IJavaVirtualizableBreakpoint> VirtualizedBreakpoints
        {
            get
            {
                return _virtualizedBreakpoints;
            }
        }

        #region IDebugEngine2 Members

        public int Attach(IDebugProgram2[] rgpPrograms, IDebugProgramNode2[] rgpProgramNodes, uint celtPrograms, IDebugEventCallback2 pCallback, enum_ATTACH_REASON dwReason)
        {
            if (celtPrograms == 0)
                return VSConstants.S_OK;

            if (pCallback == null)
                throw new ArgumentNullException("pCallback");
            if (rgpPrograms == null || rgpPrograms.Length < celtPrograms)
                throw new ArgumentException();
            if (rgpProgramNodes == null || rgpProgramNodes.Length < celtPrograms)
                throw new ArgumentException();

            if (celtPrograms > 1)
                throw new NotImplementedException();

            if (dwReason != enum_ATTACH_REASON.ATTACH_REASON_LAUNCH)
                throw new NotImplementedException();

            JavaDebugProgram program = rgpProgramNodes[0] as JavaDebugProgram;
            if (program == null)
                throw new NotSupportedException();

            lock (_programs)
            {
                _programs.Add(program);
            }

            DebugEvent @event = new DebugEngineCreateEvent(enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS, this);
            pCallback.Event(this, program.GetProcess(), program, null, @event);

            program.InitializeDebuggerChannel(this, pCallback);
            return VSConstants.S_OK;
        }

        public int CauseBreak()
        {
            foreach (var program in Programs)
            {
                ErrorHandler.ThrowOnFailure(program.CauseBreak());
            }

            return VSConstants.S_OK;
        }

        public int ContinueFromSynchronousEvent(IDebugEvent2 pEvent)
        {
            if (pEvent == null)
                throw new ArgumentNullException("pEvent");
            if (!(pEvent is DebugEvent))
                return VSConstants.E_INVALIDARG;

            if (pEvent is IDebugEngineCreateEvent2)
                return VSConstants.S_OK;

            IPropertyOwner propertyOwner = pEvent as IPropertyOwner;
            if (propertyOwner != null)
            {
                bool manualResume;
                propertyOwner.Properties.TryGetProperty("ManualResume", out manualResume);

                SuspendPolicy suspendPolicy;
                if (!manualResume && propertyOwner.Properties.TryGetProperty(typeof(SuspendPolicy), out suspendPolicy))
                {
                    IThreadReference thread = propertyOwner.Properties.GetProperty<IThreadReference>(typeof(IThreadReference));

                    switch (suspendPolicy)
                    {
                    case SuspendPolicy.All:
                        JavaDebugProgram program = propertyOwner.Properties.GetProperty<JavaDebugProgram>(typeof(JavaDebugProgram));
                        JavaDebugThread debugThread;
                        program.Threads.TryGetValue(thread.GetUniqueId(), out debugThread);
                        program.Continue(debugThread);
                        break;

                    case SuspendPolicy.EventThread:
                        Task.Factory.StartNew(thread.Resume).HandleNonCriticalExceptions();
                        break;

                    case SuspendPolicy.None:
                        break;
                    }
                }
            }

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Creates a pending breakpoint in the debug engine (DE).
        /// </summary>
        /// <param name="breakpointRequest">An IDebugBreakpointRequest2 object that describes the pending breakpoint to create.</param>
        /// <param name="pendingBreakpoint">Returns an IDebugPendingBreakpoint2 object that represents the pending breakpoint.</param>
        /// <returns>
        /// If successful, returns S_OK; otherwise, returns an error code. Typically returns E_FAIL if the pBPRequest parameter
        /// does not match any language supported by the DE of if the pBPRequest parameter is invalid or incomplete.
        /// </returns>
        /// <remarks>
        /// A pending breakpoint is essentially a collection of all the information needed to bind a breakpoint to code. The
        /// pending breakpoint returned from this method is not bound to code until the IDebugPendingBreakpoint2.Bind method
        /// is called.
        /// 
        /// For each pending breakpoint the user sets, the session debug manager (SDM) calls this method in each attached DE.
        /// It is up to the DE to verify that the breakpoint is valid for programs running in that DE.
        /// 
        /// When the user sets a breakpoint on a line of code, the DE is free to bind the breakpoint to the closest line in
        /// the document that corresponds to this code. This makes it possible for the user to set a breakpoint on the first
        /// line of a multi-line statement, but bind it on the last line (where all the code is attributed in the debug
        /// information).
        /// </remarks>
        public int CreatePendingBreakpoint(IDebugBreakpointRequest2 breakpointRequest, out IDebugPendingBreakpoint2 pendingBreakpoint)
        {
            pendingBreakpoint = null;

            BreakpointRequestInfo requestInfo = new BreakpointRequestInfo(breakpointRequest);
            if (requestInfo.LanguageGuid != Constants.JavaLanguageGuid && requestInfo.LanguageGuid != Guid.Empty)
                return VSConstants.E_FAIL;

            if (requestInfo.Location.LocationType == enum_BP_LOCATION_TYPE.BPLT_CODE_FILE_LINE)
            {
                pendingBreakpoint = new JavaDebugLocationPendingBreakpoint(this, requestInfo);
                _pendingBreakpoints.Add(pendingBreakpoint);
                return VSConstants.S_OK;
            }

            throw new NotImplementedException();
        }

        public int DestroyProgram(IDebugProgram2 program)
        {
            JavaDebugProgram javaProgram = program as JavaDebugProgram;
            if (javaProgram == null)
                return VSConstants.E_INVALIDARG;

            lock (_programs)
            {
                _programs.Remove(javaProgram);
            }

            return VSConstants.S_OK;
        }

        public int EnumPrograms(out IEnumDebugPrograms2 programs)
        {
            lock (_programs)
            {
                programs = new EnumDebugPrograms(_programs.ToArray());
            }

            return VSConstants.S_OK;
        }

        public int GetEngineId(out Guid pguidEngine)
        {
            pguidEngine = JavaDebuggerConstants.JavaDebugEngineGuid;
            return VSConstants.S_OK;
        }

        public int RemoveAllSetExceptions(ref Guid guidType)
        {
            if (guidType != JavaDebuggerConstants.JavaDebugEngineGuid && guidType != Constants.JavaLanguageGuid)
                return VSConstants.E_INVALIDARG;

            _exceptions.Clear();
            return VSConstants.S_OK;
        }

        public int RemoveSetException(EXCEPTION_INFO[] pException)
        {
            if (pException == null)
                throw new ArgumentNullException("pException");
            if (pException.Length != 1)
                throw new ArgumentException();

            if (pException[0].guidType != JavaDebuggerConstants.JavaDebugEngineGuid)
                return VSConstants.E_INVALIDARG;

            _exceptions.Remove(pException[0].bstrExceptionName);
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Specifies how the debug engine (DE) should handle a given exception.
        /// </summary>
        /// <param name="pException">[in] An EXCEPTION_INFO structure that describes the exception and how to debug it.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>A DE could be instructed to stop the program generating an exception at first chance, second chance, or not at all.</remarks>
        public int SetException(EXCEPTION_INFO[] pException)
        {
            if (pException == null)
                throw new ArgumentNullException("pException");
            if (pException.Length != 1)
                throw new ArgumentException();

            if (pException[0].guidType != JavaDebuggerConstants.JavaDebugEngineGuid)
                return VSConstants.E_INVALIDARG;

            _exceptions[pException[0].bstrExceptionName] = pException[0];
            return VSConstants.S_OK;
        }

        public bool TryGetException(string exceptionName, out EXCEPTION_INFO exceptionInfo)
        {
            return _exceptions.TryGetValue(exceptionName, out exceptionInfo);
        }

        public int SetLocale(ushort wLangID)
        {
            _culture = CultureInfo.GetCultureInfo(wLangID);
            return VSConstants.S_OK;
        }

        public int SetMetric(string pszMetric, object varValue)
        {
            Trace.WriteLine(string.Format("  {0}.SetMetric: {1}={2}", GetType().Name, pszMetric, varValue));
            return VSConstants.S_OK;
        }

        public int SetRegistryRoot(string pszRegistryRoot)
        {
            _registryRoot = pszRegistryRoot;
            return VSConstants.S_OK;
        }

        #endregion

        #region IDebugEngine3 Members

        public int LoadSymbols()
        {
            return VSConstants.S_OK;
        }

        public int SetAllExceptions(enum_EXCEPTION_STATE dwState)
        {
            throw new NotImplementedException();
        }

        public int SetEngineGuid(ref Guid guidEngine)
        {
            Contract.Assert(guidEngine == JavaDebuggerConstants.JavaDebugEngineGuid);
            return VSConstants.S_OK;
        }

        public int SetJustMyCodeState(int fUpdate, uint dwModules, JMC_CODE_SPEC[] rgJMCSpec)
        {
            throw new NotImplementedException();
        }

        public int SetSymbolPath(string szSymbolSearchPath, string szSymbolCachePath, uint flags)
        {
            if (szSymbolSearchPath == null)
                throw new ArgumentNullException("szSymbolSearchPath");
            if (szSymbolCachePath == null)
                throw new ArgumentNullException("szSymbolCachePath");

            _symbolSearchPath = szSymbolSearchPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            _symbolCachePath = szSymbolCachePath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //_symbolPathFlags = flags; // flags is always 0
            return VSConstants.S_OK;
        }

        #endregion

        #region IDebugEngineLaunch2 Members

        public int CanTerminateProcess(IDebugProcess2 pProcess)
        {
            throw new NotImplementedException();
        }

        public int LaunchSuspended(string pszServer, IDebugPort2 pPort, string pszExe, string pszArgs, string pszDir, string bstrEnv, string pszOptions, enum_LAUNCH_FLAGS dwLaunchFlags, uint hStdInput, uint hStdOutput, uint hStdError, IDebugEventCallback2 pCallback, out IDebugProcess2 ppProcess)
        {
            throw new NotImplementedException();
        }

        public int ResumeProcess(IDebugProcess2 pProcess)
        {
            throw new NotImplementedException();
        }

        public int TerminateProcess(IDebugProcess2 pProcess)
        {
            throw new NotImplementedException();
        }

        #endregion

        internal void BindVirtualizedBreakpoints(JavaDebugProgram program, JavaDebugThread thread, IReferenceType type, IEnumerable<string> sourcePaths)
        {
            Contract.Requires<ArgumentNullException>(program != null, "program");
            Contract.Requires<ArgumentNullException>(sourcePaths != null, "sourcePaths");

            var breakpoints = VirtualizedBreakpoints.ToArray();
            foreach (var breakpoint in breakpoints)
            {
                breakpoint.Bind(program, thread, type, sourcePaths);
            }
        }
    }
}
