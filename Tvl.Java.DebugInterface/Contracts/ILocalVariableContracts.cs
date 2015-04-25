namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(ILocalVariable))]
    internal abstract class ILocalVariableContracts : ILocalVariable
    {
        #region ILocalVariable Members

        public string GetGenericSignature()
        {
            Contract.Ensures(Contract.Result<string>() == null || !string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public bool GetIsArgument()
        {
            throw new NotImplementedException();
        }

        public bool GetIsVisible(IStackFrame frame)
        {
            Contract.Requires<ArgumentNullException>(frame != null, "frame");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(frame.GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public bool GetIsVisible(ILocation location)
        {
            Contract.Requires<ArgumentNullException>(location != null, "location");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(location.GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public int GetSlot()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);

            throw new NotImplementedException();
        }

        public string GetName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public string GetSignature()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public IType GetLocalType()
        {
            Contract.Ensures(Contract.Result<IType>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IType>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public string GetLocalTypeName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        #endregion

        #region IEquatable<ILocalVariable> Members

        public bool Equals(ILocalVariable other)
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
