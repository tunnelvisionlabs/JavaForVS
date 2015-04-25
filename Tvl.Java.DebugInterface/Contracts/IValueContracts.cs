namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IValue))]
    internal abstract class IValueContracts : IValue
    {
        #region IValue Members

        public IType GetValueType()
        {
            Contract.Ensures(Contract.Result<IType>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IType>().GetVirtualMachine()));

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
