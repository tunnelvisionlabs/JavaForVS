namespace Tvl.Java.DebugInterface.Events.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IClassPrepareEvent))]
    internal abstract class IClassPrepareEventContracts : IClassPrepareEvent
    {
        #region IClassPrepareEvent Members

        public IReferenceType GetReferenceType()
        {
            Contract.Ensures(Contract.Result<IReferenceType>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IReferenceType>().GetVirtualMachine()));

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
    }
}
