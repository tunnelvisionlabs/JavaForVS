namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IMirror))]
    internal abstract class IMirrorContracts : IMirror
    {
        #region IMirror Members

        public IVirtualMachine GetVirtualMachine()
        {
            Contract.Ensures(Contract.Result<IVirtualMachine>() != null);

            throw new NotImplementedException();
        }

        #endregion
    }
}
