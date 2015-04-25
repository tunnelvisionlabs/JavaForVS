namespace Tvl.Java.DebugInterface.Events.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using IEnumerator = System.Collections.IEnumerator;
    using SuspendPolicy = Tvl.Java.DebugInterface.Request.SuspendPolicy;

    [ContractClassFor(typeof(IEventSet))]
    internal abstract class IEventSetContracts : IEventSet
    {
        #region IEventSet Members

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public SuspendPolicy SuspendPolicy()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICollection<IEvent> Members

        public void Add(IEvent item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IEvent item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IEvent[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Remove(IEvent item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<IEvent> Members

        public IEnumerator<IEvent> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
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
    }
}
