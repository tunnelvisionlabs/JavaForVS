namespace Tvl.Java.DebugInterface.Request.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IEventRequestManager))]
    internal abstract class IEventRequestManagerContracts : IEventRequestManager
    {
        #region IEventRequestManager Members

        public ReadOnlyCollection<IAccessWatchpointRequest> GetAccessWatchpointRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IAccessWatchpointRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IAccessWatchpointRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IBreakpointRequest> GetBreakpointRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IBreakpointRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IBreakpointRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IClassPrepareRequest> GetClassPrepareRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IClassPrepareRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IClassPrepareRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IClassUnloadRequest> GetClassUnloadRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IClassUnloadRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IClassUnloadRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IExceptionRequest> GetExceptionRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IExceptionRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IExceptionRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMethodEntryRequest> GetMethodEntryRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMethodEntryRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMethodEntryRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMethodExitRequest> GetMethodExitRequest()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMethodExitRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMethodExitRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IModificationWatchpointRequest> GetModificationWatchpointRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IModificationWatchpointRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IModificationWatchpointRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMonitorContendedEnteredRequest> GetMonitorContendedEnteredRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMonitorContendedEnteredRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMonitorContendedEnteredRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMonitorContendedEnterRequest> GetMonitorContendedEnterRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMonitorContendedEnterRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMonitorContendedEnterRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMonitorWaitedRequest> GetMonitorWaitedRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMonitorWaitedRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMonitorWaitedRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMonitorWaitRequest> GetMonitorWaitRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMonitorWaitRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMonitorWaitRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IStepRequest> GetStepRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IStepRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IStepRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IThreadDeathRequest> GetThreadDeathRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IThreadDeathRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IThreadDeathRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IThreadStartRequest> GetThreadStartRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IThreadStartRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IThreadStartRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IVirtualMachineDeathRequest> GetVirtualMachineDeathRequests()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IVirtualMachineDeathRequest>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IVirtualMachineDeathRequest>>(), request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public IAccessWatchpointRequest CreateAccessWatchpointRequest(IField field)
        {
            Contract.Requires<ArgumentNullException>(field != null, "field");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(field.GetVirtualMachine()));
            Contract.Ensures(Contract.Result<IAccessWatchpointRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IAccessWatchpointRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IBreakpointRequest CreateBreakpointRequest(ILocation location)
        {
            Contract.Requires<ArgumentNullException>(location != null, "location");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(location.GetVirtualMachine()));
            Contract.Ensures(Contract.Result<IBreakpointRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IBreakpointRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IClassPrepareRequest CreateClassPrepareRequest()
        {
            Contract.Ensures(Contract.Result<IClassPrepareRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IClassPrepareRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IClassUnloadRequest CreateClassUnloadRequest()
        {
            Contract.Ensures(Contract.Result<IClassUnloadRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IClassUnloadRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IExceptionRequest CreateExceptionRequest(IReferenceType referenceType, bool notifyCaught, bool notifyUncaught)
        {
            Contract.Requires<VirtualMachineMismatchException>(referenceType == null || this.GetVirtualMachine().Equals(referenceType.GetVirtualMachine()));
            Contract.Ensures(Contract.Result<IExceptionRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IExceptionRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IMethodEntryRequest CreateMethodEntryRequest()
        {
            Contract.Ensures(Contract.Result<IMethodEntryRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IMethodEntryRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IMethodExitRequest CreateMethodExitRequest()
        {
            Contract.Ensures(Contract.Result<IMethodExitRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IMethodExitRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IModificationWatchpointRequest CreateModificationWatchpointRequest(IField field)
        {
            Contract.Requires<ArgumentNullException>(field != null, "field");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(field.GetVirtualMachine()));
            Contract.Ensures(Contract.Result<IModificationWatchpointRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IModificationWatchpointRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IMonitorContendedEnteredRequest CreateMonitorContendedEnteredRequest()
        {
            Contract.Ensures(Contract.Result<IMonitorContendedEnteredRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IMonitorContendedEnteredRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IMonitorContendedEnterRequest CreateMonitorContendedEnterRequest()
        {
            Contract.Ensures(Contract.Result<IMonitorContendedEnterRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IMonitorContendedEnterRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IMonitorWaitedRequest CreateMonitorWaitedRequest()
        {
            Contract.Ensures(Contract.Result<IMonitorWaitedRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IMonitorWaitedRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IMonitorWaitRequest CreateMonitorWaitRequest()
        {
            Contract.Ensures(Contract.Result<IMonitorWaitRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IMonitorWaitRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IStepRequest CreateStepRequest(IThreadReference thread, StepSize size, StepDepth depth)
        {
            Contract.Requires<VirtualMachineMismatchException>(thread == null || this.GetVirtualMachine().Equals(thread.GetVirtualMachine()));
            Contract.Ensures(Contract.Result<IStepRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IStepRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IThreadDeathRequest CreateThreadDeathRequest()
        {
            Contract.Ensures(Contract.Result<IThreadDeathRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IThreadDeathRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IThreadStartRequest CreateThreadStartRequest()
        {
            Contract.Ensures(Contract.Result<IThreadStartRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IThreadStartRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IVirtualMachineDeathRequest CreateVirtualMachineDeathRequest()
        {
            Contract.Ensures(Contract.Result<IVirtualMachineDeathRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IVirtualMachineDeathRequest>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public void DeleteAllBreakpoints()
        {
            throw new NotImplementedException();
        }

        public void DeleteEventRequest(IEventRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null, "request");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(request.GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public void DeleteEventRequests(IEnumerable<IEventRequest> requests)
        {
            Contract.Requires<ArgumentNullException>(requests != null, "request");
            Contract.Requires<ArgumentException>(Contract.ForAll(requests, request => request != null));
            Contract.Requires<ArgumentException>(Contract.ForAll(requests, request => this.GetVirtualMachine().Equals(request.GetVirtualMachine())));

            throw new NotImplementedException();
        }

        #endregion

        #region IMirror Members

        public IVirtualMachine GetVirtualMachine()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
