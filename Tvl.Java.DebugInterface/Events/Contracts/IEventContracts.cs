namespace Tvl.Java.DebugInterface.Events.Contracts
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    [ContractClassFor(typeof(IEvent))]
    internal abstract class IEventContracts : IEvent
    {
        #region IEvent Members

        public IEventRequest GetRequest()
        {
            Contract.Ensures(Contract.Result<IEventRequest>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IEventRequest>()));

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
