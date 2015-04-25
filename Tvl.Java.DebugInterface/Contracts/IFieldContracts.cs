namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IField))]
    internal abstract class IFieldContracts : IField
    {
        #region IField Members

        public bool GetIsEnumConstant()
        {
            throw new NotImplementedException();
        }

        public bool GetIsTransient()
        {
            throw new NotImplementedException();
        }

        public bool GetIsVolatile()
        {
            throw new NotImplementedException();
        }

        public IType GetFieldType()
        {
            Contract.Ensures(Contract.Result<IType>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IType>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public string GetFieldTypeName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        #endregion

        #region IEquatable<IField> Members

        public bool Equals(IField other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITypeComponent Members

        public IReferenceType GetDeclaringType()
        {
            throw new NotImplementedException();
        }

        public string GetGenericSignature()
        {
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
            throw new NotImplementedException();
        }

        public string GetSignature()
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
    }
}
