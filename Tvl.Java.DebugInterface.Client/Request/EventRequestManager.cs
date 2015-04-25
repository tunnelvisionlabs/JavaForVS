namespace Tvl.Java.DebugInterface.Client.Request
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Tvl.Java.DebugInterface.Request;
    using EventKind = Tvl.Java.DebugInterface.Types.EventKind;
    using RequestId = Tvl.Java.DebugInterface.Types.RequestId;

    internal sealed class EventRequestManager : Mirror, IEventRequestManager
    {
        private readonly List<IAccessWatchpointRequest> _accessWatchpointRequests = new List<IAccessWatchpointRequest>();
        private readonly List<IBreakpointRequest> _breakpointRequests = new List<IBreakpointRequest>();
        private readonly List<IClassPrepareRequest> _classPrepareRequests = new List<IClassPrepareRequest>();
        private readonly List<IClassUnloadRequest> _classUnloadRequests = new List<IClassUnloadRequest>();
        private readonly List<IExceptionRequest> _exceptionRequests = new List<IExceptionRequest>();
        private readonly List<IMethodEntryRequest> _methodEntryRequests = new List<IMethodEntryRequest>();
        private readonly List<IMethodExitRequest> _methodExitRequests = new List<IMethodExitRequest>();
        private readonly List<IModificationWatchpointRequest> _modificationWatchpointRequests = new List<IModificationWatchpointRequest>();
        private readonly List<IMonitorContendedEnterRequest> _monitorContendedEnterRequests = new List<IMonitorContendedEnterRequest>();
        private readonly List<IMonitorContendedEnteredRequest> _monitorContendedEnteredRequests = new List<IMonitorContendedEnteredRequest>();
        private readonly List<IMonitorWaitedRequest> _monitorWaitedRequests = new List<IMonitorWaitedRequest>();
        private readonly List<IMonitorWaitRequest> _monitorWaitRequests = new List<IMonitorWaitRequest>();
        private readonly List<IStepRequest> _stepRequests = new List<IStepRequest>();
        private readonly List<IThreadDeathRequest> _threadDeathRequests = new List<IThreadDeathRequest>();
        private readonly List<IThreadStartRequest> _threadStartRequests = new List<IThreadStartRequest>();
        private readonly List<IVirtualMachineDeathRequest> _virtualMachineDeathRequests = new List<IVirtualMachineDeathRequest>();

        public EventRequestManager(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public ReadOnlyCollection<IAccessWatchpointRequest> GetAccessWatchpointRequests()
        {
            return _accessWatchpointRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IBreakpointRequest> GetBreakpointRequests()
        {
            return _breakpointRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IClassPrepareRequest> GetClassPrepareRequests()
        {
            return _classPrepareRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IClassUnloadRequest> GetClassUnloadRequests()
        {
            return _classUnloadRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IExceptionRequest> GetExceptionRequests()
        {
            return _exceptionRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IMethodEntryRequest> GetMethodEntryRequests()
        {
            return _methodEntryRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IMethodExitRequest> GetMethodExitRequest()
        {
            return _methodExitRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IModificationWatchpointRequest> GetModificationWatchpointRequests()
        {
            return _modificationWatchpointRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IMonitorContendedEnteredRequest> GetMonitorContendedEnteredRequests()
        {
            return _monitorContendedEnteredRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IMonitorContendedEnterRequest> GetMonitorContendedEnterRequests()
        {
            return _monitorContendedEnterRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IMonitorWaitedRequest> GetMonitorWaitedRequests()
        {
            return _monitorWaitedRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IMonitorWaitRequest> GetMonitorWaitRequests()
        {
            return _monitorWaitRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IStepRequest> GetStepRequests()
        {
            return _stepRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IThreadDeathRequest> GetThreadDeathRequests()
        {
            return _threadDeathRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IThreadStartRequest> GetThreadStartRequests()
        {
            return _threadStartRequests.AsReadOnly();
        }

        public ReadOnlyCollection<IVirtualMachineDeathRequest> GetVirtualMachineDeathRequests()
        {
            return _virtualMachineDeathRequests.AsReadOnly();
        }

        public IAccessWatchpointRequest CreateAccessWatchpointRequest(IField field)
        {
            Field internalField = field as Field;
            if (internalField == null || !internalField.VirtualMachine.Equals(VirtualMachine))
                throw new VirtualMachineMismatchException();

            var request = new AccessWatchpointRequest(VirtualMachine, internalField);
            _accessWatchpointRequests.Add(request);
            return request;
        }

        public IBreakpointRequest CreateBreakpointRequest(ILocation location)
        {
            Location internalLocation = location as Location;
            if (internalLocation == null || !internalLocation.VirtualMachine.Equals(VirtualMachine))
                throw new VirtualMachineMismatchException();

            var request = new BreakpointRequest(VirtualMachine, internalLocation);
            _breakpointRequests.Add(request);
            return request;
        }

        public IClassPrepareRequest CreateClassPrepareRequest()
        {
            var request = new ClassPrepareRequest(VirtualMachine);
            _classPrepareRequests.Add(request);
            return request;
        }

        public IClassUnloadRequest CreateClassUnloadRequest()
        {
            var request = new ClassUnloadRequest(VirtualMachine);
            _classUnloadRequests.Add(request);
            return request;
        }

        public IExceptionRequest CreateExceptionRequest(IReferenceType referenceType, bool notifyCaught, bool notifyUncaught)
        {
            ReferenceType type = referenceType as ReferenceType;
            if ((type == null || !type.VirtualMachine.Equals(this.VirtualMachine)) && referenceType != null)
                throw new VirtualMachineMismatchException();

            var request = new ExceptionRequest(VirtualMachine, type, notifyCaught, notifyUncaught);
            _exceptionRequests.Add(request);
            return request;
        }

        public IMethodEntryRequest CreateMethodEntryRequest()
        {
            var request = new MethodEntryRequest(VirtualMachine);
            _methodEntryRequests.Add(request);
            return request;
        }

        public IMethodExitRequest CreateMethodExitRequest()
        {
            var request = new MethodExitRequest(VirtualMachine);
            _methodExitRequests.Add(request);
            return request;
        }

        public IModificationWatchpointRequest CreateModificationWatchpointRequest(IField field)
        {
            Field internalField = field as Field;
            if (internalField == null || !internalField.VirtualMachine.Equals(VirtualMachine))
                throw new VirtualMachineMismatchException();

            var request = new ModificationWatchpointRequest(VirtualMachine, internalField);
            _modificationWatchpointRequests.Add(request);
            return request;
        }

        public IMonitorContendedEnteredRequest CreateMonitorContendedEnteredRequest()
        {
            var request = new MonitorContendedEnteredRequest(VirtualMachine);
            _monitorContendedEnteredRequests.Add(request);
            return request;
        }

        public IMonitorContendedEnterRequest CreateMonitorContendedEnterRequest()
        {
            var request = new MonitorContendedEnterRequest(VirtualMachine);
            _monitorContendedEnterRequests.Add(request);
            return request;
        }

        public IMonitorWaitedRequest CreateMonitorWaitedRequest()
        {
            var request = new MonitorWaitedRequest(VirtualMachine);
            _monitorWaitedRequests.Add(request);
            return request;
        }

        public IMonitorWaitRequest CreateMonitorWaitRequest()
        {
            var request = new MonitorWaitRequest(VirtualMachine);
            _monitorWaitRequests.Add(request);
            return request;
        }

        public IStepRequest CreateStepRequest(IThreadReference thread, StepSize size, StepDepth depth)
        {
            ThreadReference threadReference = thread as ThreadReference;
            if ((threadReference == null || !threadReference.VirtualMachine.Equals(this.VirtualMachine)) && thread != null)
                throw new VirtualMachineMismatchException();

            var request = new StepRequest(VirtualMachine, threadReference, size, depth);
            _stepRequests.Add(request);
            return request;
        }

        public IThreadDeathRequest CreateThreadDeathRequest()
        {
            var request = new ThreadDeathRequest(VirtualMachine);
            _threadDeathRequests.Add(request);
            return request;
        }

        public IThreadStartRequest CreateThreadStartRequest()
        {
            var request = new ThreadStartRequest(VirtualMachine);
            _threadStartRequests.Add(request);
            return request;
        }

        public IVirtualMachineDeathRequest CreateVirtualMachineDeathRequest()
        {
            var request = new VirtualMachineDeathRequest(VirtualMachine);
            _virtualMachineDeathRequests.Add(request);
            return request;
        }

        public void DeleteAllBreakpoints()
        {
            throw new NotImplementedException();
        }

        public void DeleteEventRequest(IEventRequest request)
        {
            throw new NotImplementedException();
        }

        public void DeleteEventRequests(IEnumerable<IEventRequest> requests)
        {
            throw new NotImplementedException();
        }

        internal EventRequest GetEventRequest(EventKind eventKind, RequestId requestId)
        {
            if (requestId.Id == 0)
                return null;

            IEnumerable<EventRequest> requests = null;
            switch (eventKind)
            {
            case EventKind.SingleStep:
                requests = GetStepRequests().Cast<EventRequest>();
                break;

            case EventKind.Breakpoint:
                requests = GetBreakpointRequests().Cast<EventRequest>();
                break;

            case EventKind.Exception:
                requests = GetExceptionRequests().Cast<EventRequest>();
                break;

            case EventKind.ThreadStart:
                requests = GetThreadStartRequests().Cast<EventRequest>();
                break;

            case EventKind.ClassPrepare:
                requests = GetClassPrepareRequests().Cast<EventRequest>();
                break;

            case EventKind.ClassUnload:
                requests = GetClassUnloadRequests().Cast<EventRequest>();
                break;

            case EventKind.FieldAccess:
                requests = GetAccessWatchpointRequests().Cast<EventRequest>();
                break;

            case EventKind.FieldModification:
                requests = GetModificationWatchpointRequests().Cast<EventRequest>();
                break;

            case EventKind.MethodEntry:
                requests = GetMethodEntryRequests().Cast<EventRequest>();
                break;

            case EventKind.MethodExit:
                requests = GetMethodExitRequest().Cast<EventRequest>();
                break;

            case EventKind.VirtualMachineDeath:
                requests = GetVirtualMachineDeathRequests().Cast<EventRequest>();
                break;

            case EventKind.ThreadDeath:
            //case EventKind.ThreadEnd:
                requests = GetThreadDeathRequests().Cast<EventRequest>();
                break;

            case EventKind.FramePop:
            case EventKind.UserDefined:
            case EventKind.ClassLoad:
            case EventKind.ExceptionCatch:
            case EventKind.VirtualMachineDisconnected:
            case EventKind.VirtualMachineStart:
            //case EventKind.VirtualMachineInit:
                // these requests don't have a mirror in this API
                break;

            case EventKind.Invalid:
            default:
                break;
            }

            EventRequest request = null;
            if (requests != null)
                request = requests.FirstOrDefault(i => i.RequestId == requestId);

            return request;
        }
    }
}
