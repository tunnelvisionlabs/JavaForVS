namespace Tvl.Java.DebugInterface.Events.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IExceptionEvent))]
    internal abstract class IExceptionEventContracts : IExceptionEvent
    {
        #region IExceptionEvent Members

        public ILocation GetCatchLocation()
        {
            Contract.Ensures(Contract.Result<ILocation>() == null || this.GetVirtualMachine().Equals(Contract.Result<ILocation>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IObjectReference GetException()
        {
            Contract.Ensures(Contract.Result<IObjectReference>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IObjectReference>().GetVirtualMachine()));

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
