namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(ITypeComponent))]
    internal abstract class ITypeComponentContracts : ITypeComponent
    {
        #region ITypeComponent Members

        public IReferenceType GetDeclaringType()
        {
            Contract.Ensures(Contract.Result<IReferenceType>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IReferenceType>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public string GetGenericSignature()
        {
            Contract.Ensures(Contract.Result<string>() == null || !string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public bool GetIsFinal()
        {
            throw new NotImplementedException();
        }

        public bool GetIsStatic()
        {
            throw new NotImplementedException();
        }

        public bool GetIsSynthetic()
        {
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

        #endregion

        #region IAccessible Members

        public bool GetIsPackagePrivate()
        {
            throw new NotImplementedException();
        }

        public bool GetIsPrivate()
        {
            throw new NotImplementedException();
        }

        public bool GetIsProtected()
        {
            throw new NotImplementedException();
        }

        public bool GetIsPublic()
        {
            throw new NotImplementedException();
        }

        public AccessModifiers GetModifiers()
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
