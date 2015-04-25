namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IMonitorInfo))]
    internal abstract class IMonitorInfoContracts : IMonitorInfo
    {
        #region IMonitorInfo Members

        public IObjectReference GetMonitor()
        {
            Contract.Ensures(Contract.Result<IObjectReference>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IObjectReference>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public int GetStackDepth()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);

            throw new NotImplementedException();
        }

        public IThreadReference GetThread()
        {
            Contract.Ensures(Contract.Result<IThreadReference>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IThreadReference>().GetVirtualMachine()));

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
