namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.Java.DebugInterface;
    using Tvl.Java.DebugInterface.Request;
    using Tvl.VisualStudio.Language.Java.Debugger.Collections;
    using Tvl.VisualStudio.Language.Java.Debugger.Events;

    using File = System.IO.File;
    using Path = System.IO.Path;
    using TextSpan = Microsoft.VisualStudio.TextManager.Interop.TextSpan;

    internal class JavaDebugLocationPendingBreakpoint : IJavaVirtualizableBreakpoint, IDebugPendingBreakpoint2, IDebugPendingBreakpoint3, IDebugQueryEngine2
    {
        private readonly JavaDebugEngine _engine;
        private readonly BreakpointRequestInfo _requestInfo;

        private BP_CONDITION? _condition;
        private BP_PASSCOUNT? _passCount;

        private readonly List<IDebugErrorBreakpoint2> _errorBreakpoints = new List<IDebugErrorBreakpoint2>();
        private readonly List<JavaDebugBoundBreakpoint> _boundBreakpoints = new List<JavaDebugBoundBreakpoint>();

        private bool _virtualized;
        private bool _disabled;
        private bool _deleted;

        private bool? _firstOnLine;

        public JavaDebugLocationPendingBreakpoint(JavaDebugEngine engine, BreakpointRequestInfo requestInfo)
        {
            Contract.Requires<ArgumentNullException>(engine != null, "engine");
            Contract.Requires<ArgumentNullException>(requestInfo != null, "requestInfo");
            Contract.Requires<ArgumentException>(requestInfo.Location != null);
            Contract.Requires<ArgumentException>(requestInfo.Location.LocationType == enum_BP_LOCATION_TYPE.BPLT_CODE_FILE_LINE);

            _engine = engine;
            _requestInfo = requestInfo;

            _condition = requestInfo.Condition;
            _passCount = requestInfo.PassCount;
        }

        internal JavaDebugEngine DebugEngine
        {
            get
            {
                return _engine;
            }
        }

        private BreakpointLocationCodeFileLine RequestLocation
        {
            get
            {
                return (BreakpointLocationCodeFileLine)_requestInfo.Location;
            }
        }

        private bool IsFirstOnLine()
        {
            if (_firstOnLine != null)
                return _firstOnLine.Value;

            try
            {
                string fileName = RequestLocation.DocumentPosition.GetFileName();
                string text = File.ReadAllText(fileName);

                IList<IParseTree> statementTrees;
                IList<IToken> tokens;
                if (!LineStatementAnalyzer.TryGetLineStatements(text, RequestLocation.DocumentPosition.GetRange().iStartLine, out statementTrees, out tokens))
                {
                    _firstOnLine = false;
                    return _firstOnLine.Value;
                }

                if (statementTrees.Count <= 1)
                {
                    _firstOnLine = true;
                    return _firstOnLine.Value;
                }

                IToken endToken = tokens[statementTrees[0].SourceInterval.b];
                _firstOnLine = RequestLocation.DocumentPosition.GetRange().iStartIndex <= endToken.Column + 1;
                return _firstOnLine.Value;
            }
            catch (Exception ex)
            {
                if (ErrorHandler.IsCriticalException(ex))
                    throw;

                _firstOnLine = false;
                return _firstOnLine.Value;
            }
        }

        #region IVirtualizableBreakpoint Members

        public void Bind(JavaDebugProgram program, JavaDebugThread thread, IReferenceType type, IEnumerable<string> sourcePaths)
        {
            IVirtualMachine virtualMachine = program.VirtualMachine;

            IEnumerable<string> validPaths = sourcePaths.Where(i => string.Equals(Path.GetFileName(RequestLocation.DocumentPosition.GetFileName()), Path.GetFileName(i), StringComparison.OrdinalIgnoreCase));

            List<JavaDebugBoundBreakpoint> boundBreakpoints = new List<JavaDebugBoundBreakpoint>();
            List<IDebugErrorBreakpoint2> errorBreakpoints = new List<IDebugErrorBreakpoint2>();
            foreach (var path in validPaths)
            {
                TextSpan range = RequestLocation.DocumentPosition.GetRange();
                try
                {
                    ReadOnlyCollection<ILocation> locations = type.GetLocationsOfLine(range.iStartLine + 1);
                    ILocation bindLocation = locations.OrderBy(i => i.GetCodeIndex()).FirstOrDefault();
                    if (bindLocation != null && IsFirstOnLine())
                    {
                        IEventRequestManager eventRequestManager = virtualMachine.GetEventRequestManager();

                        IBreakpointRequest eventRequest = eventRequestManager.CreateBreakpointRequest(bindLocation);
                        eventRequest.SuspendPolicy = SuspendPolicy.All;

                        JavaDebugCodeContext codeContext = new JavaDebugCodeContext(program, bindLocation);
                        BreakpointResolutionLocationCode location = new BreakpointResolutionLocationCode(codeContext);
                        DebugBreakpointResolution resolution = new DebugBreakpointResolution(program, thread, enum_BP_TYPE.BPT_CODE, location);
                        JavaDebugBoundBreakpoint boundBreakpoint = new JavaDebugBoundBreakpoint(this, program, eventRequest, resolution);
                        if (!_disabled)
                            boundBreakpoint.Enable(1);

                        boundBreakpoints.Add(boundBreakpoint);
                    }
                }
                catch (MissingInformationException)
                {
                }
            }

            _boundBreakpoints.AddRange(boundBreakpoints);
            if (boundBreakpoints.Count > 0)
            {
                _errorBreakpoints.Clear();
            }

            _errorBreakpoints.AddRange(errorBreakpoints);

            if (boundBreakpoints.Count > 0)
            {
                DebugEvent debugEvent = new DebugBreakpointBoundEvent(enum_EVENTATTRIBUTES.EVENT_SYNCHRONOUS, this, new EnumDebugBoundBreakpoints(boundBreakpoints));
                program.Callback.Event(DebugEngine, program.Process, program, null, debugEvent);
            }

            foreach (var errorBreakpoint in errorBreakpoints)
            {
                DebugEvent debugEvent = new DebugBreakpointErrorEvent(enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS, errorBreakpoint);
                program.Callback.Event(DebugEngine, program.Process, program, null, debugEvent);
            }
        }

        #endregion

        #region IDebugPendingBreakpoint2 Members

        public int Bind()
        {
            if (_deleted)
            {
                return AD7Constants.E_BP_DELETED;
            }

            //Task.Factory.StartNew(AsyncBindImpl).HandleNonCriticalExceptions();
            AsyncBindImpl();
            return VSConstants.S_OK;
        }

        private void AsyncBindImpl()
        {
            UnbindAllBreakpoints(enum_BP_UNBOUND_REASON.BPUR_BREAKPOINT_REBIND);

            string fileName = RequestLocation.DocumentPosition.GetFileName();
            int lineNumber = RequestLocation.DocumentPosition.GetRange().iStartLine + 1;
            bool errorNotFirstOnLine = false;

            List<JavaDebugBoundBreakpoint> boundBreakpoints = new List<JavaDebugBoundBreakpoint>();
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
                    {
                        if (!IsFirstOnLine())
                        {
                            errorNotFirstOnLine = true;
                            break;
                        }

                        IEventRequestManager eventRequestManager = virtualMachine.GetEventRequestManager();
                        IBreakpointRequest eventRequest = eventRequestManager.CreateBreakpointRequest(bindLocation);
                        eventRequest.SuspendPolicy = SuspendPolicy.All;

                        JavaDebugCodeContext codeContext = new JavaDebugCodeContext(program, bindLocation);
                        BreakpointResolutionLocationCode location = new BreakpointResolutionLocationCode(codeContext);
                        DebugBreakpointResolution resolution = new DebugBreakpointResolution(program, null, enum_BP_TYPE.BPT_CODE, location);
                        JavaDebugBoundBreakpoint boundBreakpoint = new JavaDebugBoundBreakpoint(this, program, eventRequest, resolution);
                        if (!_disabled)
                            boundBreakpoint.Enable(1);

                        boundBreakpoints.Add(boundBreakpoint);
                    }
                }

                if (errorNotFirstOnLine)
                    break;
            }

            _boundBreakpoints.AddRange(boundBreakpoints);

            if (_boundBreakpoints.Count == 0)
            {
                foreach (var program in programs)
                {
                    JavaDebugThread thread = null;
                    IDebugCodeContext2 codeContext = new DebugDocumentCodeContext(RequestLocation.DocumentPosition);
                    BreakpointResolutionLocation location = new BreakpointResolutionLocationCode(codeContext);
                    string message = "The class is not yet loaded, or the location is not present in the debug symbols for this document.";
                    if (errorNotFirstOnLine)
                        message = "Only breakpoints on the first statement on a line can be bound at this time.";

                    DebugErrorBreakpointResolution resolution = new DebugErrorBreakpointResolution(program, thread, enum_BP_TYPE.BPT_CODE, location, enum_BP_ERROR_TYPE.BPET_GENERAL_WARNING, message);
                    DebugErrorBreakpoint errorBreakpoint = new DebugErrorBreakpoint(this, resolution);
                    _errorBreakpoints.Add(errorBreakpoint);

                    DebugEvent debugEvent = new DebugBreakpointErrorEvent(enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS, errorBreakpoint);
                    program.Callback.Event(DebugEngine, program.Process, program, null, debugEvent);
                }
            }

            foreach (var group in boundBreakpoints.GroupBy(i => i.Program))
            {
                DebugEvent debugEvent = new DebugBreakpointBoundEvent(enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS, this, new EnumDebugBoundBreakpoints(group));
                group.Key.Callback.Event(DebugEngine, group.Key.Process, group.Key, null, debugEvent);
            }
        }

        private void UnbindAllBreakpoints(enum_BP_UNBOUND_REASON reason)
        {
            foreach (var breakpoint in _boundBreakpoints)
            {
                DebugEvent debugEvent = new DebugBreakpointUnboundEvent(enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS, breakpoint, reason);
                breakpoint.Program.Callback.Event(DebugEngine, breakpoint.Program.Process, breakpoint.Program, null, debugEvent);
                breakpoint.Delete();
            }

            _boundBreakpoints.Clear();
        }

        /// <summary>
        /// Determines whether this pending breakpoint can bind to a code location.
        /// </summary>
        /// <param name="ppErrorEnum">
        /// [out] Returns an IEnumDebugErrorBreakpoints2 object that contains a list of IDebugErrorBreakpoint2
        /// objects if there could be errors.
        /// </param>
        /// <returns>
        /// If successful, returns S_OK. Returns S_FALSE if the breakpoint cannot bind, in which case the errors
        /// are returned by the ppErrorEnum parameter. Otherwise, returns an error code. Returns E_BP_DELETED if
        /// the breakpoint has been deleted.
        /// </returns>
        /// <remarks>
        /// This method is called to determine what would happen if this pending breakpoint was bound. Call the
        /// IDebugPendingBreakpoint2::Bind method to actually bind the pending breakpoint.
        /// </remarks>
        public int CanBind(out IEnumDebugErrorBreakpoints2 ppErrorEnum)
        {
            if (_deleted)
            {
                ppErrorEnum = null;
                return AD7Constants.E_BP_DELETED;
            }

            string fileName = RequestLocation.DocumentPosition.GetFileName();
            int lineNumber = RequestLocation.DocumentPosition.GetRange().iStartLine + 1;
            bool errorNotFirstOnLine = false;

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
                    {
                        if (IsFirstOnLine())
                        {
                            ppErrorEnum = null;
                            return VSConstants.S_OK;
                        }
                        else
                        {
                            errorNotFirstOnLine = true;
                            break;
                        }
                    }
                }

                if (errorNotFirstOnLine)
                    break;
            }

            foreach (var program in programs)
            {
                JavaDebugThread thread = null;
                IDebugCodeContext2 codeContext = new DebugDocumentCodeContext(RequestLocation.DocumentPosition);
                BreakpointResolutionLocation location = new BreakpointResolutionLocationCode(codeContext);
                string message = "The class is not yet loaded, or the location is not present in the debug symbols for this document.";
                if (errorNotFirstOnLine)
                    message = "Only breakpoints on the first statement on a line can be bound at this time.";

                DebugErrorBreakpointResolution resolution = new DebugErrorBreakpointResolution(program, thread, enum_BP_TYPE.BPT_CODE, location, enum_BP_ERROR_TYPE.BPET_GENERAL_WARNING, message);
                DebugErrorBreakpoint errorBreakpoint = new DebugErrorBreakpoint(this, resolution);
                _errorBreakpoints.Add(errorBreakpoint);

                DebugEvent debugEvent = new DebugBreakpointErrorEvent(enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS, errorBreakpoint);
                program.Callback.Event(DebugEngine, program.Process, program, null, debugEvent);
            }

            if (_errorBreakpoints.Count == 0)
            {
                JavaDebugProgram program = null;
                JavaDebugThread thread = null;
                IDebugCodeContext2 codeContext = new DebugDocumentCodeContext(RequestLocation.DocumentPosition);
                BreakpointResolutionLocation location = new BreakpointResolutionLocationCode(codeContext);
                string message = "The binding process is not yet implemented.";

                DebugErrorBreakpointResolution resolution = new DebugErrorBreakpointResolution(program, thread, enum_BP_TYPE.BPT_CODE, location, enum_BP_ERROR_TYPE.BPET_GENERAL_ERROR, message);
                DebugErrorBreakpoint errorBreakpoint = new DebugErrorBreakpoint(this, resolution);
                _errorBreakpoints.Add(errorBreakpoint);
            }

            ppErrorEnum = new EnumDebugErrorBreakpoints(_errorBreakpoints);
            return VSConstants.S_FALSE;
        }

        public int Delete()
        {
            if (_deleted)
            {
                return AD7Constants.E_BP_DELETED;
            }

            foreach (var breakpoint in _boundBreakpoints)
                breakpoint.Delete();

            _boundBreakpoints.Clear();
            _errorBreakpoints.Clear();
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

            _disabled = !enable;
            foreach (var breakpoint in _boundBreakpoints)
                breakpoint.Enable(fEnable);

            return VSConstants.S_OK;
        }

        public int EnumBoundBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            if (_deleted)
            {
                ppEnum = null;
                return AD7Constants.E_BP_DELETED;
            }

            ppEnum = new EnumDebugBoundBreakpoints(_boundBreakpoints);
            return VSConstants.S_OK;
        }

        public int EnumErrorBreakpoints(enum_BP_ERROR_TYPE bpErrorType, out IEnumDebugErrorBreakpoints2 ppEnum)
        {
            if (_deleted)
            {
                ppEnum = null;
                return AD7Constants.E_BP_DELETED;
            }

            ppEnum = new EnumDebugErrorBreakpoints(_errorBreakpoints);
            return VSConstants.S_OK;
        }

        public int GetBreakpointRequest(out IDebugBreakpointRequest2 ppBPRequest)
        {
            if (_deleted)
            {
                ppBPRequest = null;
                return AD7Constants.E_BP_DELETED;
            }

            ppBPRequest = _requestInfo.Request;
            return VSConstants.S_OK;
        }

        public int GetState(PENDING_BP_STATE_INFO[] pState)
        {
            if (pState == null)
                throw new ArgumentNullException("pState");
            if (pState.Length == 0)
                throw new ArgumentException();

            if (_virtualized)
                pState[0].Flags = enum_PENDING_BP_STATE_FLAGS.PBPSF_VIRTUALIZED;
            else
                pState[0].Flags = enum_PENDING_BP_STATE_FLAGS.PBPSF_NONE;

            if (_deleted)
                pState[0].state = enum_PENDING_BP_STATE.PBPS_DELETED;
            else if (_disabled)
                pState[0].state = enum_PENDING_BP_STATE.PBPS_DISABLED;
            else
                pState[0].state = enum_PENDING_BP_STATE.PBPS_ENABLED;

            return VSConstants.S_OK;
        }

        public int SetCondition(BP_CONDITION bpCondition)
        {
            _condition = bpCondition;
            return VSConstants.S_OK;
        }

        public int SetPassCount(BP_PASSCOUNT bpPassCount)
        {
            _passCount = bpPassCount;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Toggles the virtualized state of this pending breakpoint. When a pending breakpoint is virtualized, the
        /// debug engine will attempt to bind it every time new code loads into the program.
        /// </summary>
        /// <param name="fVirtualize">[in] Set to nonzero (TRUE) to virtualize the pending breakpoint, or to zero (FALSE) to turn off virtualization.</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code. Returns E_BP_DELETED if the breakpoint has been deleted.</returns>
        /// <remarks>A virtualized breakpoint is bound every time code is loaded.</remarks>
        public int Virtualize(int fVirtualize)
        {
            bool virtualize = fVirtualize != 0;
            if (_virtualized == virtualize)
                return VSConstants.S_OK;

            _virtualized = virtualize;

            if (virtualize)
                DebugEngine.VirtualizedBreakpoints.Add(this);
            else
                DebugEngine.VirtualizedBreakpoints.Remove(this);

            return VSConstants.S_OK;
        }

        #endregion

        #region IDebugPendingBreakpoint3 Members

        public int GetErrorResolutionInfo(enum_BPERESI_FIELDS dwFields, BP_ERROR_RESOLUTION_INFO[] pErrorResolutionInfo)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDebugQueryEngine2 Members

        public int GetEngineInterface(out object ppUnk)
        {
            ppUnk = _engine;
            return VSConstants.S_OK;
        }

        #endregion
    }
}
