namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IStringReference))]
    internal abstract class IStringReferenceContracts : IStringReference
    {
        #region IStringReference Members

        public string GetValue()
        {
            Contract.Ensures(Contract.Result<string>() != null);

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
