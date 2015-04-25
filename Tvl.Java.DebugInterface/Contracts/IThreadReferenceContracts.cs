namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IThreadReference))]
    internal abstract class IThreadReferenceContracts : IThreadReference
    {
        #region IThreadReference Members

        public IObjectReference GetCurrentContendedMonitor()
        {
            Contract.Ensures(Contract.Result<IObjectReference>() == null || this.GetVirtualMachine().Equals(Contract.Result<IObjectReference>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public void ForceEarlyReturn(IValue returnValue)
        {
            Contract.Requires<ArgumentNullException>(returnValue != null, "returnValue");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(returnValue.GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IStackFrame GetFrame(int index)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Ensures(Contract.Result<IStackFrame>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IStackFrame>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public int GetFrameCount()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IStackFrame> GetFrames()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IStackFrame>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IStackFrame>>(), frame => frame != null && this.GetVirtualMachine().Equals(frame.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IStackFrame> GetFrames(int start, int length)
        {
            Contract.Requires<ArgumentOutOfRangeException>(start >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(length >= 0);
            Contract.Requires<ArgumentException>(start <= GetFrameCount() - length);
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IStackFrame>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IStackFrame>>(), frame => frame != null && this.GetVirtualMachine().Equals(frame.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public void Interrupt()
        {
            throw new NotImplementedException();
        }

        public bool GetIsAtBreakpoint()
        {
            throw new NotImplementedException();
        }

        public bool GetIsSuspended()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IObjectReference> GetOwnedMonitors()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IObjectReference>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IObjectReference>>(), type => type != null && this.GetVirtualMachine().Equals(type.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMonitorInfo> GetOwnedMonitorsAndFrames()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMonitorInfo>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMonitorInfo>>(), type => type != null && this.GetVirtualMachine().Equals(type.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public void PopFrames(IStackFrame frame)
        {
            Contract.Requires<ArgumentNullException>(frame != null, "frame");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(frame.GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public ThreadStatus GetStatus()
        {
            throw new NotImplementedException();
        }

        public void Stop(IObjectReference throwable)
        {
            Contract.Requires<ArgumentNullException>(throwable != null, "throwable");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(throwable.GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public void Suspend()
        {
            throw new NotImplementedException();
        }

        public int GetSuspendCount()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);

            throw new NotImplementedException();
        }

        public IThreadGroupReference GetThreadGroup()
        {
            Contract.Ensures(Contract.Result<IThreadGroupReference>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IThreadGroupReference>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        #endregion

        #region IObjectReference Members

        public void DisableCollection()
        {
            throw new NotImplementedException();
        }

        public void EnableCollection()
        {
            throw new NotImplementedException();
        }

        public int GetEntryCount()
        {
            throw new NotImplementedException();
        }

        public IValue GetValue(IField field)
        {
            throw new NotImplementedException();
        }

        public IDictionary<IField, IValue> GetValues(IEnumerable<IField> fields)
        {
            throw new NotImplementedException();
        }

        public IStrongValueHandle<IValue> InvokeMethod(IThreadReference thread, IMethod method, InvokeOptions options, params IValue[] arguments)
        {
            throw new NotImplementedException();
        }

        public bool GetIsCollected()
        {
            throw new NotImplementedException();
        }

        public IThreadReference GetOwningThread()
        {
            throw new NotImplementedException();
        }

        public IReferenceType GetReferenceType()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IObjectReference> GetReferringObjects(long maxReferrers)
        {
            throw new NotImplementedException();
        }

        public void SetValue(IField field, IValue value)
        {
            throw new NotImplementedException();
        }

        public long GetUniqueId()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IThreadReference> GetWaitingThreads()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IValue Members

        public IType GetValueType()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMirror Members

        public IVirtualMachine GetVirtualMachine()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEquatable<IObjectReference> Members

        public bool Equals(IObjectReference other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
