namespace Tvl.Java.DebugInterface.Events.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IWatchpointEvent))]
    internal abstract class IWatchpointEventContracts : IWatchpointEvent
    {
        #region IWatchpointEvent Members

        public IField GetField()
        {
            Contract.Ensures(Contract.Result<IField>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IField>()));

            throw new NotImplementedException();
        }

        public IObjectReference GetObject()
        {
            // will be null if Field is static
            Contract.Ensures(Contract.Result<IObjectReference>() == null || this.GetVirtualMachine().Equals(Contract.Result<IObjectReference>()));

            throw new NotImplementedException();
        }

        public IValue GetCurrentValue()
        {
            Contract.Ensures(Contract.Result<IValue>() == null || this.GetVirtualMachine().Equals(Contract.Result<IValue>()));

            throw new NotImplementedException();
        }

        #endregion

        #region IThreadEvent Members

        public IThreadReference GetThread()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEvent Members

        public Request.IEventRequest GetRequest()
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

        #region ILocatable Members

        public ILocation GetLocation()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
