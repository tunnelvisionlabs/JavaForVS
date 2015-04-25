namespace Tvl.Java.DebugInterface.Events.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IEventQueue))]
    internal abstract class IEventQueueContracts : IEventQueue
    {
        #region IEventQueue Members

        public IEventSet Remove()
        {
            Contract.Ensures(Contract.Result<IEventSet>() != null);

            throw new NotImplementedException();
        }

        public IEventSet Remove(TimeSpan timeout)
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
