namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IArrayType))]
    internal abstract class IArrayTypeContracts : IArrayType
    {
        #region IArrayType Members

        public string GetComponentSignature()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public IType GetComponentType()
        {
            Contract.Ensures(Contract.Result<IType>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IType>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public string GetComponentTypeName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public IStrongValueHandle<IArrayReference> CreateInstance(int length)
        {
            Contract.Requires<ArgumentOutOfRangeException>(length >= 0);
            Contract.Ensures(Contract.Result<IStrongValueHandle<IArrayReference>>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IStrongValueHandle<IArrayReference>>().GetVirtualMachine()));
            Contract.Ensures(Contract.Result<IStrongValueHandle<IArrayReference>>().Value.GetLength() >= 0);

            throw new NotImplementedException();
        }

        #endregion

        #region IReferenceType Members

        public ReadOnlyCollection<IField> GetFields(bool includeInherited)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILocation> GetLineLocations()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILocation> GetLineLocations(string stratum, string sourceName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMethod> GetMethods(bool includeInherited)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<string> GetAvailableStrata()
        {
            throw new NotImplementedException();
        }

        public IClassLoaderReference GetClassLoader()
        {
            throw new NotImplementedException();
        }

        public IClassObjectReference GetClassObject()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<Types.ConstantPoolEntry> GetConstantPool()
        {
            throw new NotImplementedException();
        }

        public int GetConstantPoolCount()
        {
            throw new NotImplementedException();
        }

        public string GetDefaultStratum()
        {
            throw new NotImplementedException();
        }

        public bool GetFailedToInitialize()
        {
            throw new NotImplementedException();
        }

        public IField GetFieldByName(string fieldName)
        {
            throw new NotImplementedException();
        }

        public string GetGenericSignature()
        {
            throw new NotImplementedException();
        }

        public IValue GetValue(IField field)
        {
            throw new NotImplementedException();
        }

        public IDictionary<IField, IValue> GetValues(IEnumerable<IField> fields)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IObjectReference> GetInstances(long maxInstances)
        {
            throw new NotImplementedException();
        }

        public bool GetIsAbstract()
        {
            throw new NotImplementedException();
        }

        public bool GetIsFinal()
        {
            throw new NotImplementedException();
        }

        public bool GetIsInitialized()
        {
            throw new NotImplementedException();
        }

        public bool GetIsPrepared()
        {
            throw new NotImplementedException();
        }

        public bool GetIsStatic()
        {
            throw new NotImplementedException();
        }

        public bool GetIsVerified()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILocation> GetLocationsOfLine(int lineNumber)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILocation> GetLocationsOfLine(string stratum, string sourceName, int lineNumber)
        {
            throw new NotImplementedException();
        }

        public int GetMajorVersion()
        {
            throw new NotImplementedException();
        }

        public int GetMinorVersion()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMethod> GetMethodsByName(string name)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMethod> GetMethodsByName(string name, string signature)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IReferenceType> GetNestedTypes()
        {
            throw new NotImplementedException();
        }

        public string GetSourceDebugExtension()
        {
            throw new NotImplementedException();
        }

        public string GetSourceName()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<string> GetSourceNames(string stratum)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<string> GetSourcePaths(string stratum)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IField> GetVisibleFields()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMethod> GetVisibleMethods()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IType Members

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

        #region IEquatable<IReferenceType> Members

        public bool Equals(IReferenceType other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
