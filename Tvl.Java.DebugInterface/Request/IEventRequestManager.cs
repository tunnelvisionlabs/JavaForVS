namespace Tvl.Java.DebugInterface.Request
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IEventRequestManagerContracts))]
    public interface IEventRequestManager : IMirror
    {
        ReadOnlyCollection<IAccessWatchpointRequest> GetAccessWatchpointRequests();

        ReadOnlyCollection<IBreakpointRequest> GetBreakpointRequests();

        ReadOnlyCollection<IClassPrepareRequest> GetClassPrepareRequests();

        ReadOnlyCollection<IClassUnloadRequest> GetClassUnloadRequests();

        ReadOnlyCollection<IExceptionRequest> GetExceptionRequests();

        ReadOnlyCollection<IMethodEntryRequest> GetMethodEntryRequests();

        ReadOnlyCollection<IMethodExitRequest> GetMethodExitRequest();

        ReadOnlyCollection<IModificationWatchpointRequest> GetModificationWatchpointRequests();

        ReadOnlyCollection<IMonitorContendedEnteredRequest> GetMonitorContendedEnteredRequests();

        ReadOnlyCollection<IMonitorContendedEnterRequest> GetMonitorContendedEnterRequests();

        ReadOnlyCollection<IMonitorWaitedRequest> GetMonitorWaitedRequests();

        ReadOnlyCollection<IMonitorWaitRequest> GetMonitorWaitRequests();

        ReadOnlyCollection<IStepRequest> GetStepRequests();

        ReadOnlyCollection<IThreadDeathRequest> GetThreadDeathRequests();

        ReadOnlyCollection<IThreadStartRequest> GetThreadStartRequests();

        ReadOnlyCollection<IVirtualMachineDeathRequest> GetVirtualMachineDeathRequests();

        /// <summary>
        /// Creates a new disabled <see cref="IAccessWatchpointRequest"/>.
        /// </summary>
        IAccessWatchpointRequest CreateAccessWatchpointRequest(IField field);

        /// <summary>
        /// Creates a new disabled <see cref="IBreakpointRequest"/>.
        /// </summary>
        IBreakpointRequest CreateBreakpointRequest(ILocation location);

        /// <summary>
        /// Creates a new disabled <see cref="IClassPrepareRequest"/>.
        /// </summary>
        IClassPrepareRequest CreateClassPrepareRequest();

        /// <summary>
        /// Creates a new disabled <see cref="IClassUnloadRequest"/>.
        /// </summary>
        IClassUnloadRequest CreateClassUnloadRequest();

        /// <summary>
        /// Creates a new disabled <see cref="IExceptionRequest"/>.
        /// </summary>
        IExceptionRequest CreateExceptionRequest(IReferenceType referenceType, bool notifyCaught, bool notifyUncaught);

        /// <summary>
        /// Creates a new disabled <see cref="IMethodEntryRequest"/>.
        /// </summary>
        IMethodEntryRequest CreateMethodEntryRequest();

        /// <summary>
        /// Creates a new disabled <see cref="IMethodExitRequest"/>.
        /// </summary>
        IMethodExitRequest CreateMethodExitRequest();

        /// <summary>
        /// Creates a new disabled <see cref="IModificationWatchpointRequest"/>.
        /// </summary>
        IModificationWatchpointRequest CreateModificationWatchpointRequest(IField field);

        /// <summary>
        /// Creates a new disabled <see cref="IMonitorContendedEnteredRequest"/>.
        /// </summary>
        IMonitorContendedEnteredRequest CreateMonitorContendedEnteredRequest();

        /// <summary>
        /// Creates a new disabled <see cref="IMonitorContendedEnterRequest"/>.
        /// </summary>
        IMonitorContendedEnterRequest CreateMonitorContendedEnterRequest();

        /// <summary>
        /// Creates a new disabled <see cref="IMonitorWaitedRequest"/>.
        /// </summary>
        IMonitorWaitedRequest CreateMonitorWaitedRequest();

        /// <summary>
        /// Creates a new disabled <see cref="IMonitorWaitRequest"/>.
        /// </summary>
        IMonitorWaitRequest CreateMonitorWaitRequest();

        /// <summary>
        /// Creates a new disabled <see cref="IStepRequest"/>.
        /// </summary>
        IStepRequest CreateStepRequest(IThreadReference thread, StepSize size, StepDepth depth);

        /// <summary>
        /// Creates a new disabled <see cref="IThreadDeathRequest"/>.
        /// </summary>
        IThreadDeathRequest CreateThreadDeathRequest();

        /// <summary>
        /// Creates a new disabled <see cref="IThreadStartRequest"/>.
        /// </summary>
        IThreadStartRequest CreateThreadStartRequest();

        /// <summary>
        /// Creates a new disabled <see cref="IVirtualMachineDeathRequest"/>.
        /// </summary>
        IVirtualMachineDeathRequest CreateVirtualMachineDeathRequest();

        /// <summary>
        /// Remove all breakpoints managed by this EventRequestManager.
        /// </summary>
        void DeleteAllBreakpoints();

        /// <summary>
        /// Removes an event request.
        /// </summary>
        void DeleteEventRequest(IEventRequest request);

        /// <summary>
        /// Removes a list of event requests.
        /// </summary>
        void DeleteEventRequests(IEnumerable<IEventRequest> requests);
    }
}
