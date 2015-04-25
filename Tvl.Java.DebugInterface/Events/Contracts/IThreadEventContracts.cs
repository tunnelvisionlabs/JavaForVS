namespace Tvl.Java.DebugInterface.Events.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IThreadEvent))]
    internal abstract class IThreadEventContracts : IThreadEvent
    {
        #region IThreadEvent Members

        public IThreadReference GetThread()
        {
            Contract.Ensures(Contract.Result<IThreadReference>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IThreadReference>().GetVirtualMachine()));

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
    }
}
