namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IArrayReference))]
    internal abstract class IArrayReferenceContracts : IArrayReference
    {
        #region IArrayReference Members

        public IValue GetValue(int index)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<ArgumentException>(index < GetLength());
            Contract.Ensures(Contract.Result<IValue>() == null || this.GetVirtualMachine().Equals(Contract.Result<IValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IValue> GetValues()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IValue>>() != null);
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IValue>>().Count == GetLength());
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IValue>>(), value => value == null || this.GetVirtualMachine().Equals(value.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IValue> GetValues(int index, int length)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(length >= 0);
            Contract.Requires<ArgumentException>(index <= GetLength() - length);
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IValue>>() != null);
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IValue>>().Count == length);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IValue>>(), value => value == null || this.GetVirtualMachine().Equals(value.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public int GetLength()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);

            throw new NotImplementedException();
        }

        public void SetValue(int index, IValue value)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<VirtualMachineMismatchException>(value == null || this.GetVirtualMachine().Equals(value.GetVirtualMachine()));
            Contract.Requires<ArgumentException>(index < GetLength());

            throw new NotImplementedException();
        }

        public void SetValues(int index, IValue[] values, int sourceIndex, int length)
        {
            Contract.Requires<ArgumentNullException>(values != null, "values");
#if CONTRACTS_FORALL
            Contract.Requires<VirtualMachineMismatchException>(Contract.ForAll(values, value => value == null || this.GetVirtualMachine().Equals(value.GetVirtualMachine())));
#endif
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(sourceIndex >= 0);
            Contract.Requires<ArgumentException>(index <= GetLength() - length);
            Contract.Requires<ArgumentException>(sourceIndex <= values.Length - length);

            throw new NotImplementedException();
        }

        public void SetValues(IValue[] values)
        {
            Contract.Requires<ArgumentNullException>(values != null, "values");
            Contract.Requires<ArgumentException>(values.Length == GetLength());
#if CONTRACTS_FORALL
            Contract.Requires<VirtualMachineMismatchException>(Contract.ForAll(values, value => value == null || this.GetVirtualMachine().Equals(value.GetVirtualMachine())));
#endif

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

        public System.Collections.Generic.IDictionary<IField, IValue> GetValues(System.Collections.Generic.IEnumerable<IField> fields)
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
