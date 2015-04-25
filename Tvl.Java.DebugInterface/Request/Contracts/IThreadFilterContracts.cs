namespace Tvl.Java.DebugInterface.Request.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IThreadFilter))]
    internal abstract class IThreadFilterContracts : IThreadFilter
    {
        #region IThreadFilter Members

        public void AddThreadFilter(IThreadReference thread)
        {
            Contract.Requires<ArgumentNullException>(thread != null, "thread");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(thread.GetVirtualMachine()));

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
