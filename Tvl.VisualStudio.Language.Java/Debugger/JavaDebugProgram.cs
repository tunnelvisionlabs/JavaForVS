namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Tvl.Java.DebugInterface;
    using Tvl.Java.DebugInterface.Client.Connect;
    using Tvl.Java.DebugInterface.Client.Events;
    using Tvl.Java.DebugInterface.Connect;
    using Tvl.Java.DebugInterface.Request;
    using Tvl.VisualStudio.Language.Java.Debugger.Collections;
    using Tvl.VisualStudio.Language.Java.Debugger.Events;
    using Tvl.VisualStudio.Language.Java.Project;
    using Tvl.VisualStudio.Shell;

    using Directory = System.IO.Directory;
    using Interlocked = System.Threading.Interlocked;
    using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
    using MSBuild = Microsoft.Build.Evaluation;
    using OAProject = Microsoft.VisualStudio.Project.Automation.OAProject;
    using Path = System.IO.Path;
    using Task = System.Threading.Tasks.Task;

    [ComVisible(true)]
    public partial class JavaDebugProgram
        : IDebugProgram3
        , IDebugProgram2
        , IDebugProgramEx2
        , IDebugProgramHost2
        , IDebugEngineProgram2
        , IDebugProgramNode2
        , IDebugProviderProgramNode2
        , IDebugProgramNodeAttach2
        , IDebugProgramEngines2
        , IDebugProgramEnhancedStep90
        , IDebugQueryEngine2
    {
        private readonly IDebugProcess2 _process;
        private Guid? _programId;
        private JavaDebugEngine _debugEngine;
        private IDebugEventCallback2 _callback;

        private IVirtualMachine _virtualMachine;
        private IStepRequest _causeBreakRequest;
        private bool _isLoaded;

        private int _suspended;
        private bool _inGetHostName;

        private readonly Dictionary<long, JavaDebugThread> _threads = new Dictionary<long, JavaDebugThread>();

        public JavaDebugProgram(IDebugProcess2 process)
        {
            Contract.Requires<ArgumentNullException>(process != null, "process");

            _process = process;
        }

        public JavaDebugEngine DebugEngine
        {
            get
            {
                return _debugEngine;
            }
        }

        public IDebugProcess2 Process
        {
            get
            {
                return _process;
            }
        }

        public IDebugEventCallback2 Callback
        {
            get
            {
                return _callback;
            }
        }

        internal IVirtualMachine VirtualMachine
        {
            get
            {
                return _virtualMachine;
            }
        }

        internal bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }
        }

        internal IDictionary<long, JavaDebugThread> Threads
        {
            get
            {
                return _threads;
            }
        }

        internal void InitializeDebuggerChannel(JavaDebugEngine debugEngine, IDebugEventCallback2 callback)
        {
            Contract.Requires<ArgumentNullException>(debugEngine != null, "debugEngine");
            Contract.Requires<ArgumentNullException>(callback != null, "callback");

            _debugEngine = debugEngine;
            _callback = callback;

            var connector = new LocalDebuggingAttachingConnector();

            Dictionary<string, IConnectorArgument> arguments = new Dictionary<string, IConnectorArgument>();

            string argumentName = "pid";
            IConnectorIntegerArgument defaultArgument = (IConnectorIntegerArgument)connector.DefaultArguments[argumentName];
            IConnectorIntegerArgument argument = new ConnectorIntegerArgument(defaultArgument, (int)_process.GetPhysicalProcessId().dwProcessId);
            arguments.Add(argumentName, argument);

            argumentName = "sourcePaths";
            IConnectorStringArgument defaultPathsArgument = (IConnectorStringArgument)connector.DefaultArguments[argumentName];
            List<string> sourcePaths = GetSourcePaths();
            IConnectorStringArgument stringArgument = new ConnectorStringArgument(defaultPathsArgument, string.Join(";", sourcePaths));
            arguments.Add(argumentName, stringArgument);

            connector.AttachComplete += HandleAttachComplete;
            _virtualMachine = connector.Attach(arguments);

            IVirtualMachineEvents events = _virtualMachine.GetEventQueue() as IVirtualMachineEvents;
            if (events != null)
            {
                events.VirtualMachineStart += HandleVirtualMachineStart;
                events.VirtualMachineDeath += HandleVirtualMachineDeath;
                events.SingleStep += HandleSingleStep;
                events.Breakpoint += HandleBreakpoint;
                events.MethodEntry += HandleMethodEntry;
                events.MethodExit += HandleMethodExit;
                events.MonitorContendedEnter += HandleMonitorContendedEnter;
                events.MonitorContendedEntered += HandleMonitorContendedEntered;
                events.MonitorContendedWait += HandleMonitorContendedWait;
                events.MonitorContendedWaited += HandleMonitorContendedWaited;
                events.Exception += HandleException;
                events.ThreadStart += HandleThreadStart;
                events.ThreadDeath += HandleThreadDeath;
                events.ClassPrepare += HandleClassPrepare;
                events.ClassUnload += HandleClassUnload;
                events.FieldAccess += HandleFieldAccess;
                events.FieldModification += HandleFieldModification;
            }
        }

        private static List<string> GetSourcePaths()
        {
            List<string> sourcePaths = new List<string>();
            //sourcePaths.Add(@"C:\dev\jrockitsrc");

            var serviceProvider = (IServiceProvider)Package.GetGlobalService(typeof(IServiceProvider));
            if (serviceProvider == null)
                serviceProvider = new ServiceProvider((IOleServiceProvider)Package.GetGlobalService(typeof(IOleServiceProvider)));

            var vsServiceProvider = serviceProvider.AsVsServiceProvider();

            // TODO: make the JRE source directory part of the user project configuration since it varies with the VM in use
            JavaLanguagePackage package = JavaLanguagePackage.Instance ?? vsServiceProvider.GetShell().LoadPackage<JavaLanguagePackage>();
            if (package.IntellisenseOptions.ParseJreSource && Directory.Exists(package.IntellisenseOptions.JreSourcePath))
                sourcePaths.Add(package.IntellisenseOptions.JreSourcePath);

            IVsSolution solution = vsServiceProvider.GetSolution();

            IVsHierarchy[] projectHierarchies = solution.GetProjectHierarchies(__VSENUMPROJFLAGS.EPF_ALLPROJECTS).ToArray();
            object[] automationObjects = projectHierarchies.Select(i => i.GetExtensibilityObjectOrDefault(VSConstants.VSITEMID_ROOT)).ToArray();
            JavaProjectNode[] projects = automationObjects.OfType<OAProject>().Select(i => i.Project).OfType<JavaProjectNode>().ToArray();
            foreach (var project in projects)
            {
                List<MSBuild.ProjectItem> sourceFolders = new List<MSBuild.ProjectItem>();
                sourceFolders.AddRange(project.BuildProject.GetItems(JavaProjectFileConstants.SourceFolder));
                sourceFolders.AddRange(project.BuildProject.GetItems(JavaProjectFileConstants.TestSourceFolder));

                foreach (var folder in sourceFolders)
                {
                    sourcePaths.Add(Path.Combine(project.ProjectFolder, folder.EvaluatedInclude));
                }
            }

            return sourcePaths;
        }

        private void HandleAttachComplete(object virtualMachine, EventArgs e)
        {
            _causeBreakRequest = VirtualMachine.GetEventRequestManager().CreateStepRequest(null, StepSize.Instruction, StepDepth.Into);
            _causeBreakRequest.SuspendPolicy = SuspendPolicy.All;

            DebugEvent @event = new DebugProgramCreateEvent(enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS);
            Callback.Event(DebugEngine, Process, this, null, @event);
        }

        #region IDebugProgram2 Members

        int IDebugProgram2.Attach(IDebugEventCallback2 pCallback)
        {
            throw new NotSupportedException("Per MSDN, a debug engine never calls this method to attach to a program.");
        }

        int IDebugProgram3.Attach(IDebugEventCallback2 pCallback)
        {
            throw new NotSupportedException("Per MSDN, a debug engine never calls this method to attach to a program.");
        }

        public int CanDetach()
        {
            return VSConstants.S_FALSE;
        }

        public int CauseBreak()
        {
            if (_suspended == 0)
                Task.Factory.StartNew(() => _causeBreakRequest.IsEnabled = true).HandleNonCriticalExceptions();

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Continues running this program from a stopped state. Any previous execution state (such
        /// as a step) is preserved, and the program starts executing again.
        /// </summary>
        /// <param name="pThread">An IDebugThread2 object that represents the thread.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// This method is called on this program regardless of how many programs are being debugged,
        /// or which program generated the stopping event. The implementation must retain the previous
        /// execution state (such as a step) and continue execution as though it had never stopped
        /// before completing its prior execution. That is, if a thread in this program was doing a
        /// step-over operation and was stopped because some other program stopped, and then this
        /// method was called, the program must complete the original step-over operation.
        /// 
        /// Do not send a stopping event or an immediate (synchronous) event to IDebugEventCallback2.Event
        /// while handling this call; otherwise the debugger might stop responding.
        /// </remarks>
        public int Continue(IDebugThread2 pThread)
        {
#if true
            if (_suspended != 0)
            {
                Interlocked.Decrement(ref _suspended);
                Task.Factory.StartNew(VirtualMachine.Resume).HandleNonCriticalExceptions();
            }

            return VSConstants.S_OK;
#else
            if (pThread == null)
            {
                Task.Factory.StartNew(VirtualMachine.Resume).HandleNonCriticalExceptions();
                return VSConstants.S_OK;
            }

            JavaDebugThread javaThread = pThread as JavaDebugThread;
            if (javaThread == null)
                return VSConstants.E_INVALIDARG;

            Task.Factory.StartNew(() => javaThread.Resume()).HandleNonCriticalExceptions();
            return VSConstants.S_OK;
#endif
        }

        public int Detach()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a list of the code contexts for a given position in a source file.
        /// </summary>
        /// <param name="pDocPos">An IDebugDocumentPosition2 object representing an abstract position in a source file known to the IDE.</param>
        /// <param name="ppEnum">Returns an IEnumDebugCodeContexts2 object that contains a list of the code contexts.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// This method allows the session debug manager (SDM) or IDE to map a source file position into a code
        /// position. More than one code context is returned if the source generates multiple blocks of code (for
        /// example, C++ templates).
        /// </remarks>
        public int EnumCodeContexts(IDebugDocumentPosition2 pDocPos, out IEnumDebugCodeContexts2 ppEnum)
        {
            if (pDocPos == null)
                throw new ArgumentNullException("pDocPos");

            string fileName = pDocPos.GetFileName();
            int lineNumber = pDocPos.GetRange().iStartLine + 1;

            List<IDebugCodeContext2> codeContexts = new List<IDebugCodeContext2>();
            IEnumerable<JavaDebugProgram> programs = DebugEngine.Programs.ToArray();
            foreach (var program in programs)
            {
                if (!program.IsLoaded)
                    continue;

                IVirtualMachine virtualMachine = program.VirtualMachine;
                ReadOnlyCollection<IReferenceType> classes = virtualMachine.GetAllClasses();
                foreach (var @class in classes)
                {
                    if (!@class.GetIsPrepared())
                        continue;

                    ReadOnlyCollection<ILocation> locations = @class.GetLocationsOfLine(@class.GetDefaultStratum(), Path.GetFileName(fileName), lineNumber);
                    ILocation bindLocation = locations.OrderBy(i => i.GetCodeIndex()).FirstOrDefault();
                    if (bindLocation != null)
                        codeContexts.Add(new JavaDebugCodeContext(this, bindLocation));
                }
            }

            if (codeContexts.Count == 0)
            {
                ppEnum = null;
                return VSConstants.E_FAIL;
            }

            ppEnum = new EnumDebugCodeContexts(codeContexts);
            return VSConstants.S_OK;
        }

        int IDebugProgram2.EnumCodePaths(string pszHint, IDebugCodeContext2 pStart, IDebugStackFrame2 pFrame, int fSource, out IEnumCodePaths2 ppEnum, out IDebugCodeContext2 ppSafety)
        {
            throw new NotSupportedException("This method has been replaced by IDebugProgramEnhancedStep90.EnumCodePaths.");
        }

        int IDebugProgram3.EnumCodePaths(string pszHint, IDebugCodeContext2 pStart, IDebugStackFrame2 pFrame, int fSource, out IEnumCodePaths2 ppEnum, out IDebugCodeContext2 ppSafety)
        {
            throw new NotSupportedException("This method has been replaced by IDebugProgramEnhancedStep90.EnumCodePaths.");
        }

        public int EnumCodePaths(IDebugThread2 pThread, IDebugCodeContext2 pStart, /*enum_STEPUNIT*/uint stepUnit, out IEnumDebugCodePaths90 ppEnum)
        {
            throw new NotImplementedException();
        }

        public int EnumModules(out IEnumDebugModules2 ppEnum)
        {
            // TODO: implement modules?
            ppEnum = new EnumDebugModules(Enumerable.Empty<IDebugModule2>());
            return VSConstants.S_OK;
        }

        public int EnumThreads(out IEnumDebugThreads2 ppEnum)
        {
            lock (_threads)
            {
                ppEnum = new EnumDebugThreads(_threads.Values);
            }

            return VSConstants.S_OK;
        }

        public int Execute()
        {
            throw new NotImplementedException();
        }

        public int GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            throw new NotImplementedException();
        }

        public int GetDisassemblyStream(enum_DISASSEMBLY_STREAM_SCOPE dwScope, IDebugCodeContext2 pCodeContext, out IDebugDisassemblyStream2 ppDisassemblyStream)
        {
            ppDisassemblyStream = null;

            if (pCodeContext == null)
                throw new ArgumentNullException("pCodeContext");

            JavaDebugCodeContext codeContext = pCodeContext as JavaDebugCodeContext;
            if (codeContext == null)
                return VSConstants.E_INVALIDARG;

            ppDisassemblyStream = new JavaDebugDisassemblyStream(codeContext);
            return VSConstants.S_OK;
        }

        public int GetENCUpdate(out object ppUpdate)
        {
            throw new NotImplementedException();
        }

        public int GetEngineInfo(out string pbstrEngine, out Guid pguidEngine)
        {
            pguidEngine = JavaDebuggerConstants.JavaDebugEngineGuid;
            IVsDebugger2 shellDebugger = (IVsDebugger2)Package.GetGlobalService(typeof(SVsShellDebugger));
            return shellDebugger.GetEngineName(ref pguidEngine, out pbstrEngine);
        }

        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            throw new NotImplementedException();
        }

        public int GetName(out string pbstrName)
        {
            return GetProgramName(out pbstrName);
        }

        public int GetProcess(out IDebugProcess2 ppProcess)
        {
            ppProcess = _process;
            return VSConstants.S_OK;
        }

        public int GetProgramId(out Guid pguidProgramId)
        {
            pguidProgramId = _programId.Value;
            return VSConstants.S_OK;
        }

        public int Step(IDebugThread2 pThread, enum_STEPKIND sk, enum_STEPUNIT Step)
        {
            JavaDebugThread thread = pThread as JavaDebugThread;
            if (thread == null)
                return VSConstants.E_INVALIDARG;

            StepSize size;
            StepDepth depth;
            switch (Step)
            {
            case enum_STEPUNIT.STEP_INSTRUCTION:
                size = StepSize.Instruction;
                break;

            case enum_STEPUNIT.STEP_LINE:
                size = StepSize.Line;
                break;

            case enum_STEPUNIT.STEP_STATEMENT:
                size = VirtualMachine.GetCanStepByStatement() ? StepSize.Statement : StepSize.Line;
                break;

            default:
                throw new NotSupportedException();
            }

            switch (sk)
            {
            case enum_STEPKIND.STEP_INTO:
                depth = StepDepth.Into;
                break;

            case enum_STEPKIND.STEP_OUT:
                depth = StepDepth.Out;
                break;

            case enum_STEPKIND.STEP_OVER:
                depth = StepDepth.Over;
                break;

            case enum_STEPKIND.STEP_BACKWARDS:
            default:
                throw new NotSupportedException();
            }

            IStepRequest stepRequest = thread.GetStepRequest(size, depth);
            if (stepRequest == null)
                throw new InvalidOperationException();

            Task.Factory.StartNew(() =>
                {
                    // make sure the global "Break All" step request is disabled
                    this._causeBreakRequest.IsEnabled = false;
                    stepRequest.IsEnabled = true;
                    VirtualMachine.Resume();
                    Interlocked.Decrement(ref _suspended);
                }).HandleNonCriticalExceptions();

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Terminates the program.
        /// </summary>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// If possible, the program will be terminated and unloaded from the process; otherwise,
        /// the debug engine (DE) will perform any necessary cleanup.
        ///
        /// This method or the IDebugProcess2.Terminate method is called by the IDE, typically in
        /// response to the user halting all debugging. The implementation of this method should,
        /// ideally, terminate the program within the process. If this is not possible, the DE
        /// should prevent the program from running any more in this process (and do any necessary
        /// cleanup). If the IDebugProcess2.Terminate method was called by the IDE, the entire
        /// process will be terminated sometime after the IDebugProgram2.Terminate method is called.
        /// </remarks>
        public int Terminate()
        {
            Task.Factory.StartNew(() => VirtualMachine.Exit(1)).HandleNonCriticalExceptions();
            return VSConstants.S_OK;
        }

        public int WriteDump(enum_DUMPTYPE DUMPTYPE, string pszDumpUrl)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugProgram3 Members

        /// <summary>
        /// Executes the debugger program. The thread is returned to give the debugger information on which
        /// thread the user is viewing when executing the program.
        /// </summary>
        /// <param name="pThread">An IDebugThread2 object.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// There are three different ways that a debugger can resume execution after stopping:
        ///
        /// <list type="bullet">
        /// <item>Execute: Cancel any previous step, and run until the next breakpoint and so on.</item>
        /// <item>Step: Cancel any old step, and run until the new step completes.</item>
        /// <item>Continue: Run again, and leave any old step active.</item>
        /// </list>
        ///
        /// The thread passed to ExecuteOnThread is useful when deciding which step to cancel. If you do not
        /// know the thread, running execute cancels all steps. With knowledge of the thread, you only need
        /// to cancel the step on the active thread.
        /// </remarks>
        public int ExecuteOnThread(IDebugThread2 pThread)
        {
            Task.Factory.StartNew(() => _causeBreakRequest.IsEnabled = false);
            return Continue(pThread);
        }

        #endregion

        #region IDebugProgramEx2 Members

        public int Attach(IDebugEventCallback2 pCallback, uint dwReason, IDebugSession2 pSession)
        {
            throw new NotImplementedException();
        }

        public int GetProgramNode(out IDebugProgramNode2 ppProgramNode)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugProgramHost2 Members

        public int GetHostId(AD_PROCESS_ID[] pProcessId)
        {
            throw new NotImplementedException();
        }

        public int GetHostMachineName(out string pbstrHostMachineName)
        {
            throw new NotImplementedException();
        }

        int IDebugProgramHost2.GetHostName(uint dwType, out string pbstrHostName)
        {
            return GetHostName((enum_GETHOSTNAME_TYPE)dwType, out pbstrHostName);
        }

        #endregion

        #region IDebugEngineProgram2 Members

        public int Stop()
        {
            // this message is coming from the managed code debugger, which halts this process anyway.
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Allows (or disallows) expression evaluation to occur on the given thread, even if the program has stopped.
        /// </summary>
        /// <param name="pOriginatingProgram">An IDebugProgram2 object representing the program that is evaluating an expression.</param>
        /// <param name="dwTid">Specifies the identifier of the thread.</param>
        /// <param name="dwEvalFlags">A combination of flags from the EVALFLAGS enumeration that specify how the evaluation is to be performed.</param>
        /// <param name="pExprCallback">An IDebugEventCallback2 object to be used to send debug events that occur during expression evaluation.</param>
        /// <param name="fWatch">If non-zero (TRUE), allows expression evaluation on the thread identified by dwTid; otherwise, zero (FALSE) disallows expression evaluation on that thread.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// When the session debug manager (SDM) asks a program, identified by the <paramref name="pOriginatingProgram"/>
        /// parameter, to evaluate an expression, it notifies all other attached programs by calling this method.
        /// 
        /// Expression evaluation in one program may cause code to run in another, due to function evaluation or
        /// evaluation of any IDispatch properties. Because of this, this method allows expression evaluation to
        /// run and complete even though the thread may be stopped in this program.
        /// </remarks>
        public int WatchForExpressionEvaluationOnThread(IDebugProgram2 pOriginatingProgram, uint dwTid, uint dwEvalFlags, IDebugEventCallback2 pExprCallback, int fWatch)
        {
            JavaDebugProgram javaProgram = pOriginatingProgram as JavaDebugProgram;
            if (javaProgram == null)
                return VSConstants.S_OK;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Watches for execution (or stops watching for execution) to occur on the given thread.
        /// </summary>
        /// <param name="pOriginatingProgram">An IDebugProgram2 object representing the program being stepped.</param>
        /// <param name="dwTid">Specifies the identifier of the thread to watch.</param>
        /// <param name="fWatch">Non-zero (TRUE) means start watching for execution on the thread identified by dwTid; otherwise, zero (FALSE) means stop watching for execution on dwTid.</param>
        /// <param name="dwFrame">
        /// Specifies a frame index that controls the step type. When this is value is zero (0), the step type is
        /// "step into" and the program should stop whenever the thread identified by dwTid executes. When dwFrame
        /// is non-zero, the step type is "step over" and the program should stop only if the thread identified by
        /// dwTid is running in a frame whose index is equal to or higher on the stack than dwFrame.
        /// </param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// When the session debug manager (SDM) steps a program, identified by the pOriginatingProgram parameter,
        /// it notifies all other attached programs by calling this method.
        /// 
        /// This method is applicable only to same-thread stepping.
        /// </remarks>
        public int WatchForThreadStep(IDebugProgram2 pOriginatingProgram, uint dwTid, int fWatch, uint dwFrame)
        {
            JavaDebugProgram javaProgram = pOriginatingProgram as JavaDebugProgram;
            if (javaProgram == null)
                return VSConstants.S_OK;

            throw new NotImplementedException();
        }

        #endregion

        #region IDebugProgramNode2 Members

        [Obsolete]
        int IDebugProgramNode2.Attach_V7(IDebugProgram2 pMDMProgram, IDebugEventCallback2 pCallback, uint dwReason)
        {
            throw new NotSupportedException();
        }

        [Obsolete]
        int IDebugProgramNode2.DetachDebugger_V7()
        {
            throw new NotSupportedException();
        }

        [Obsolete]
        int IDebugProgramNode2.GetHostMachineName_V7(out string pbstrHostMachineName)
        {
            throw new NotSupportedException();
        }

        public int GetHostName(enum_GETHOSTNAME_TYPE dwHostNameType, out string pbstrHostName)
        {
            if (_inGetHostName)
            {
                pbstrHostName = null;
                return VSConstants.E_FAIL;
            }

            try
            {
                _inGetHostName = true;
                return _process.GetName((enum_GETNAME_TYPE)dwHostNameType, out pbstrHostName);
            }
            finally
            {
                _inGetHostName = false;
            }
        }

        public int GetHostPid(AD_PROCESS_ID[] pHostProcessId)
        {
            return _process.GetPhysicalProcessId(pHostProcessId);
        }

        public int GetProgramName(out string pbstrProgramName)
        {
            // in the future, this should return the name of the jar or startup class
            int result = GetHostName(enum_GETHOSTNAME_TYPE.GHN_FILE_NAME, out pbstrProgramName);
            if (result == VSConstants.S_OK)
                pbstrProgramName = Path.GetFileName(pbstrProgramName);

            return result;
        }

        #endregion

        #region IDebugProviderProgramNode2 Members

        public int UnmarshalDebuggeeInterface(ref Guid riid, out IntPtr ppvObject)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugProgramNodeAttach2 Members

        /// <summary>
        /// Attaches to the associated program or defers the attach process to the <see cref="IDebugEngine2.Attach"/> method.
        /// </summary>
        /// <param name="guidProgramId">GUID to assign to the associated program.</param>
        /// <returns>
        /// If successful, returns S_OK. Returns S_FALSE if the <see cref="IDebugEngine2.Attach"/> method should
        /// not be called. Otherwise, returns an error code.
        /// </returns>
        /// <remarks>
        /// This method is called during the attach process, before the IDebugEngine2::Attach method is called.
        /// The OnAttach method can perform the attach process itself (in which case, this method returns S_FALSE)
        /// or defer the attach process to the IDebugEngine2::Attach method (the OnAttach method returns S_OK).
        /// In either event, the OnAttach method can set the GUID of the program being debugged to the given GUID.
        /// </remarks>
        public int OnAttach(ref Guid guidProgramId)
        {
            _programId = guidProgramId;
            return VSConstants.S_OK;
        }

        #endregion

        #region IDebugProgramEngines2 Members

        public int EnumPossibleEngines(uint celtBuffer, Guid[] rgguidEngines, ref uint pceltEngines)
        {
            pceltEngines = 1;
            if (celtBuffer < pceltEngines)
                return JavaDebuggerConstants.E_INSUFFICIENT_BUFFER;

            if (rgguidEngines == null || rgguidEngines.Length < 1)
                throw new ArgumentException("rgguidEngines");

            rgguidEngines[0] = JavaDebuggerConstants.JavaDebugEngineGuid;
            return VSConstants.S_OK;
        }

        public int SetEngine(ref Guid guidEngine)
        {
            Contract.Assert(guidEngine == JavaDebuggerConstants.JavaDebugEngineGuid);
            return VSConstants.S_OK;
        }

        #endregion

        #region IDebugQueryEngine2 Members

        public int GetEngineInterface(out object ppUnk)
        {
            ppUnk = _debugEngine;
            return ppUnk != null ? VSConstants.S_OK : VSConstants.E_FAIL;
        }

        #endregion

        private void HandleVirtualMachineStart(object sender, ThreadEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            var requestManager = _virtualMachine.GetEventRequestManager();

            var threadStartRequest = requestManager.CreateThreadStartRequest();
            threadStartRequest.SuspendPolicy = SuspendPolicy.EventThread;
            threadStartRequest.IsEnabled = true;

            var threadDeathRequest = requestManager.CreateThreadDeathRequest();
            threadDeathRequest.SuspendPolicy = SuspendPolicy.EventThread;
            threadDeathRequest.IsEnabled = true;

            var classPrepareRequest = requestManager.CreateClassPrepareRequest();
            classPrepareRequest.SuspendPolicy = SuspendPolicy.EventThread;
            classPrepareRequest.IsEnabled = true;

            var exceptionRequest = requestManager.CreateExceptionRequest(null, true, true);
            exceptionRequest.SuspendPolicy = SuspendPolicy.All;
            exceptionRequest.IsEnabled = true;

            var virtualMachineDeathRequest = requestManager.CreateVirtualMachineDeathRequest();
            virtualMachineDeathRequest.SuspendPolicy = SuspendPolicy.All;
            virtualMachineDeathRequest.IsEnabled = true;

            DebugEvent debugEvent = new DebugLoadCompleteEvent(enum_EVENTATTRIBUTES.EVENT_ASYNC_STOP);
            SetEventProperties(debugEvent, e, false);
            Callback.Event(DebugEngine, Process, this, null, debugEvent);

            _isLoaded = true;

            JavaDebugThread mainThread = null;
            ReadOnlyCollection<IThreadReference> threads = VirtualMachine.GetAllThreads();
            for (int i = 0; i < threads.Count; i++)
            {
                bool isMainThread = threads[i].Equals(e.Thread);
                JavaDebugThread thread = new JavaDebugThread(this, threads[i], isMainThread ? ThreadCategory.Main : ThreadCategory.Worker);
                if (isMainThread)
                    mainThread = thread;

                lock (this._threads)
                {
                    this._threads.Add(threads[i].GetUniqueId(), thread);
                }

                debugEvent = new DebugThreadCreateEvent(enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS);
                Callback.Event(DebugEngine, Process, this, thread, debugEvent);
            }

            if (DebugEngine.VirtualizedBreakpoints.Count > 0)
            {
                ReadOnlyCollection<IReferenceType> classes = VirtualMachine.GetAllClasses();
                foreach (var type in classes)
                {
                    if (!type.GetIsPrepared())
                        continue;

                    ReadOnlyCollection<string> sourceFiles = type.GetSourcePaths(type.GetDefaultStratum());
                    DebugEngine.BindVirtualizedBreakpoints(this, mainThread, type, sourceFiles);
                }
            }

            JavaDebugThread thread2;
            lock (_threads)
            {
                this._threads.TryGetValue(e.Thread.GetUniqueId(), out thread2);
            }

            debugEvent = new DebugEntryPointEvent(GetAttributesForEvent(e));
            SetEventProperties(debugEvent, e, false);
            Callback.Event(DebugEngine, Process, this, thread2, debugEvent);
        }

        private void HandleVirtualMachineDeath(object sender, VirtualMachineEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            DebugEvent debugEvent = new DebugProgramDestroyEvent(GetAttributesForEvent(e), 0);
            SetEventProperties(debugEvent, e, false);
            Callback.Event(DebugEngine, Process, this, null, debugEvent);
        }

        private void HandleSingleStep(object sender, ThreadLocationEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            IStepRequest request = e.Request as IStepRequest;
            if (request == null)
                throw new ArgumentException();

            JavaDebugThread thread;
            lock (_threads)
            {
                this._threads.TryGetValue(e.Thread.GetUniqueId(), out thread);
            }

            if (e.Request == _causeBreakRequest)
            {
                _causeBreakRequest.IsEnabled = false;

                DebugEvent debugEvent = new DebugBreakEvent(GetAttributesForEvent(e));
                SetEventProperties(debugEvent, e, false);
                Callback.Event(DebugEngine, Process, this, thread, debugEvent);
                return;
            }
            else if (thread != null)
            {
                bool wasThreadStepRequest = thread.StepRequests.Contains(request);

                if (wasThreadStepRequest)
                {
                    e.Request.IsEnabled = false;
                    DebugEvent debugEvent = new DebugStepCompleteEvent(GetAttributesForEvent(e));
                    SetEventProperties(debugEvent, e, false);
                    Callback.Event(DebugEngine, Process, this, thread, debugEvent);
                    return;
                }
            }
        }

        private void HandleBreakpoint(object sender, ThreadLocationEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            List<IDebugBoundBreakpoint2> breakpoints = new List<IDebugBoundBreakpoint2>();

            foreach (var pending in DebugEngine.PendingBreakpoints)
            {
                if (pending.GetState() != enum_PENDING_BP_STATE.PBPS_ENABLED)
                    continue;

                foreach (var breakpoint in pending.EnumBoundBreakpoints().OfType<JavaDebugBoundBreakpoint>())
                {
                    if (breakpoint.EventRequest.Equals(e.Request) && breakpoint.GetState() == enum_BP_STATE.BPS_ENABLED)
                        breakpoints.Add(breakpoint);
                }
            }

            if (breakpoints.Count == 0)
            {
                ManualContinueFromEvent(e);
                return;
            }

            JavaDebugThread thread;
            lock (_threads)
            {
                this._threads.TryGetValue(e.Thread.GetUniqueId(), out thread);
            }

            DebugEvent debugEvent = new DebugBreakpointEvent(GetAttributesForEvent(e), new EnumDebugBoundBreakpoints(breakpoints));
            SetEventProperties(debugEvent, e, false);
            Callback.Event(DebugEngine, Process, this, thread, debugEvent);
        }

        private void HandleMethodEntry(object sender, ThreadLocationEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            throw new NotImplementedException();
        }

        private void HandleMethodExit(object sender, MethodExitEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            throw new NotImplementedException();
        }

        private void HandleMonitorContendedEnter(object sender, MonitorEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            throw new NotImplementedException();
        }

        private void HandleMonitorContendedEntered(object sender, MonitorEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            throw new NotImplementedException();
        }

        private void HandleMonitorContendedWait(object sender, MonitorWaitEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            throw new NotImplementedException();
        }

        private void HandleMonitorContendedWaited(object sender, MonitorWaitedEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            throw new NotImplementedException();
        }

        private void HandleException(object sender, ExceptionEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            JavaDebugThread thread;

            lock (_threads)
            {
                this._threads.TryGetValue(e.Thread.GetUniqueId(), out thread);
            }

            bool stop;
            bool firstChance = e.CatchLocation != null;
            EXCEPTION_INFO exceptionInfo;
            if (DebugEngine.TryGetException(e.Exception.GetReferenceType().GetName(), out exceptionInfo))
            {
                if (firstChance && (exceptionInfo.dwState & enum_EXCEPTION_STATE.EXCEPTION_STOP_FIRST_CHANCE) != 0)
                    stop = true;
                else if (!firstChance && (exceptionInfo.dwState & enum_EXCEPTION_STATE.EXCEPTION_STOP_SECOND_CHANCE) != 0)
                    stop = true;
                else
                    stop = !firstChance;
            }
            else
            {
                stop = !firstChance;
            }

            JavaDebugExceptionEvent exceptionEvent = new JavaDebugExceptionEvent(GetAttributesForEvent(e), this, e.Thread, e.Exception, e.Location, e.CatchLocation);

            if (stop)
            {
                SetEventProperties(exceptionEvent, e, false);
                Callback.Event(DebugEngine, Process, this, thread, exceptionEvent);
            }
            else
            {
                string message = exceptionEvent.GetDescription() + Environment.NewLine;
                DebugEvent debugEvent = new DebugOutputStringEvent(message);
                SetEventProperties(debugEvent, e, true);
                Callback.Event(DebugEngine, Process, this, thread, debugEvent);

                ManualContinueFromEvent(e);
            }
        }

        private void HandleThreadStart(object sender, ThreadEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            // nothing to do if this thread is already started
            JavaDebugThread thread;
            if (_threads.TryGetValue(e.Thread.GetUniqueId(), out thread))
            {
                switch (e.SuspendPolicy)
                {
                case SuspendPolicy.All:
                    Continue(thread);
                    break;

                case SuspendPolicy.EventThread:
                    Task.Factory.StartNew(e.Thread.Resume).HandleNonCriticalExceptions();
                    break;

                case SuspendPolicy.None:
                    break;
                }

                return;
            }

            thread = new JavaDebugThread(this, e.Thread, ThreadCategory.Worker);
            lock (this._threads)
            {
                this._threads.Add(e.Thread.GetUniqueId(), thread);
            }

            DebugEvent debugEvent = new DebugThreadCreateEvent(GetAttributesForEvent(e));
            SetEventProperties(debugEvent, e, true);
            Callback.Event(DebugEngine, Process, this, thread, debugEvent);

            ManualContinueFromEvent(e);
        }

        private void HandleThreadDeath(object sender, ThreadEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            JavaDebugThread thread;

            lock (_threads)
            {
                this._threads.TryGetValue(e.Thread.GetUniqueId(), out thread);
            }

            //string name = thread.GetName();
            DebugEvent debugEvent = new DebugThreadDestroyEvent(GetAttributesForEvent(e), 0);
            SetEventProperties(debugEvent, e, false);
            Callback.Event(DebugEngine, Process, this, thread, debugEvent);

            lock (_threads)
            {
                this._threads.Remove(e.Thread.GetUniqueId());
            }
        }

        private void HandleClassPrepare(object sender, ClassPrepareEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            JavaDebugThread thread;

            lock (_threads)
            {
                this._threads.TryGetValue(e.Thread.GetUniqueId(), out thread);
            }

            try
            {
                ReadOnlyCollection<string> sourceFiles = e.Type.GetSourcePaths(e.Type.GetDefaultStratum());
                DebugEngine.BindVirtualizedBreakpoints(this, thread, e.Type, sourceFiles);
            }
            catch (MissingInformationException)
            {
                // Can't bind debug information for classes that don't contain debug information
            }

            // The format of the message created by the .NET debugger is this:
            // 'devenv.exe' (Managed (v4.0.30319)): Loaded 'C:\Windows\Microsoft.Net\assembly\GAC_MSIL\Microsoft.VisualStudio.Windows.Forms\v4.0_10.0.0.0__b03f5f7f11d50a3a\Microsoft.VisualStudio.Windows.Forms.dll'
            string message = string.Format("'{0}' ({1}): Loaded '{2}'\n", Process.GetName(enum_GETNAME_TYPE.GN_BASENAME), Java.Constants.JavaLanguageName, e.Type.GetName());
            DebugEvent outputEvent = new DebugOutputStringEvent(message);
            SetEventProperties(outputEvent, e, true);
            Callback.Event(DebugEngine, Process, this, thread, outputEvent);

            ManualContinueFromEvent(e);
        }

        private void HandleClassUnload(object sender, ClassUnloadEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            throw new NotImplementedException();
        }

        private void HandleFieldAccess(object sender, FieldAccessEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            throw new NotImplementedException();
        }

        private void HandleFieldModification(object sender, FieldModificationEventArgs e)
        {
            if (e.SuspendPolicy == SuspendPolicy.All)
                Interlocked.Increment(ref _suspended);

            throw new NotImplementedException();
        }

        private void ManualContinueFromEvent(ThreadEventArgs e)
        {
            switch (e.SuspendPolicy)
            {
            case SuspendPolicy.All:
                JavaDebugThread thread;
                _threads.TryGetValue(e.Thread.GetUniqueId(), out thread);
                Continue(thread);
                break;

            case SuspendPolicy.EventThread:
                IThreadReference threadReference = e.Thread;
                Task.Factory.StartNew(threadReference.Resume).HandleNonCriticalExceptions();
                break;

            case SuspendPolicy.None:
                break;
            }
        }

        private static enum_EVENTATTRIBUTES GetAttributesForEvent(VirtualMachineEventArgs e)
        {
            enum_EVENTATTRIBUTES attributes = 0;
            if (e.SuspendPolicy != SuspendPolicy.None)
                attributes |= enum_EVENTATTRIBUTES.EVENT_SYNCHRONOUS;
            else
                attributes |= enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS;

            if (e.SuspendPolicy == SuspendPolicy.All)
                attributes |= enum_EVENTATTRIBUTES.EVENT_STOPPING;

            return attributes;
        }

        private static void SetEventProperties(DebugEvent debugEvent, VirtualMachineEventArgs e, bool manualResume)
        {
            SetEventProperties(debugEvent, e.Request, e.SuspendPolicy, e.VirtualMachine, default(IThreadReference), manualResume);
        }

        private static void SetEventProperties(DebugEvent debugEvent, ThreadEventArgs e, bool manualResume)
        {
            SetEventProperties(debugEvent, e.Request, e.SuspendPolicy, e.VirtualMachine, e.Thread, manualResume);
        }

        private static void SetEventProperties(DebugEvent debugEvent, IEventRequest request, SuspendPolicy suspendPolicy, IVirtualMachine virtualMachine, IThreadReference thread, bool manualResume)
        {
            Contract.Requires<ArgumentNullException>(debugEvent != null, "debugEvent");
            debugEvent.Properties.AddProperty(typeof(IEventRequest), request);
            debugEvent.Properties.AddProperty(typeof(SuspendPolicy), suspendPolicy);
            debugEvent.Properties.AddProperty(typeof(IVirtualMachine), virtualMachine);
            debugEvent.Properties.AddProperty(typeof(IThreadReference), thread);
            debugEvent.Properties.AddProperty("ManualResume", manualResume);
        }
    }
}
