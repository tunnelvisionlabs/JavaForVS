namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IStrongValueHandle<>))]
    internal abstract class IStrongValueHandleContracts<T> : IStrongValueHandle<T>
        where T : IValue
    {
        #region IStrongValueHandle Members

        public T Value
        {
            get
            {
                Contract.Ensures(Contract.Result<T>() != null);
                Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<T>().GetVirtualMachine()));

                throw new NotImplementedException();
            }
        }

        #endregion

        #region IMirror Members

        public IVirtualMachine GetVirtualMachine()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
