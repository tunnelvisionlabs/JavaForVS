namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using ConstantPoolEntry = Tvl.Java.DebugInterface.Types.ConstantPoolEntry;

    [ContractClassFor(typeof(IReferenceType))]
    internal abstract class IReferenceTypeContracts : IReferenceType
    {
        #region IReferenceType Members

        public ReadOnlyCollection<IField> GetFields(bool includeInherited)
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IField>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IField>>(), field => field != null && this.GetVirtualMachine().Equals(field.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILocation> GetLineLocations()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ILocation>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<ILocation>>(), location => location != null && this.GetVirtualMachine().Equals(location.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILocation> GetLineLocations(string stratum, string sourceName)
        {
            Contract.Requires<ArgumentException>(stratum == null || !string.IsNullOrEmpty(stratum));
            Contract.Requires<ArgumentException>(sourceName == null || !string.IsNullOrEmpty(sourceName));
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ILocation>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<ILocation>>(), location => location != null && this.GetVirtualMachine().Equals(location.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMethod> GetMethods(bool includeInherited)
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMethod>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMethod>>(), method => method != null && this.GetVirtualMachine().Equals(method.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<string> GetAvailableStrata()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<string>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<string>>(), strata => !string.IsNullOrEmpty(strata)));
#endif

            throw new NotImplementedException();
        }

        public IClassLoaderReference GetClassLoader()
        {
            Contract.Ensures(Contract.Result<IClassLoaderReference>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IClassLoaderReference>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IClassObjectReference GetClassObject()
        {
            Contract.Ensures(Contract.Result<IClassObjectReference>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IClassObjectReference>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ConstantPoolEntry> GetConstantPool()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ConstantPoolEntry>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<ConstantPoolEntry>>(), entry => entry != null));
#endif

            throw new NotImplementedException();
        }

        public int GetConstantPoolCount()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);

            throw new NotImplementedException();
        }

        public string GetDefaultStratum()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public bool GetFailedToInitialize()
        {
            throw new NotImplementedException();
        }

        public IField GetFieldByName(string fieldName)
        {
            Contract.Requires<ArgumentNullException>(fieldName != null, "fieldName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fieldName));
            Contract.Ensures(Contract.Result<IField>() == null || this.GetVirtualMachine().Equals(Contract.Result<IField>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public string GetGenericSignature()
        {
            throw new NotImplementedException();
        }

        public IValue GetValue(IField field)
        {
            Contract.Requires<ArgumentNullException>(field != null, "field");
            Contract.Requires<VirtualMachineMismatchException>(this.GetVirtualMachine().Equals(field.GetVirtualMachine()));
            Contract.Ensures(Contract.Result<IValue>() == null || this.GetVirtualMachine().Equals(Contract.Result<IValue>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public IDictionary<IField, IValue> GetValues(IEnumerable<IField> fields)
        {
            Contract.Requires<ArgumentNullException>(fields != null, "fields");
#if CONTRACTS_FORALL
            Contract.Requires<VirtualMachineMismatchException>(Contract.ForAll(fields, field => this.GetVirtualMachine().Equals(field.GetVirtualMachine())));
#endif
            Contract.Ensures(Contract.Result<IDictionary<IField, IValue>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<IDictionary<IField, IValue>>(), pair => pair.Key != null));
            Contract.Ensures(Contract.ForAll(Contract.Result<IDictionary<IField, IValue>>(), pair => this.GetVirtualMachine().Equals(pair.Key.GetVirtualMachine()) && (pair.Value == null || this.GetVirtualMachine().Equals(pair.Value.GetVirtualMachine()))));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IObjectReference> GetInstances(long maxInstances)
        {
            Contract.Requires<ArgumentOutOfRangeException>(maxInstances >= 0);
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IObjectReference>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IObjectReference>>(), value => this.GetVirtualMachine().Equals(value.GetVirtualMachine())));
#endif

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
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ILocation>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<ILocation>>(), location => this.GetVirtualMachine().Equals(location.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILocation> GetLocationsOfLine(string stratum, string sourceName, int lineNumber)
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ILocation>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<ILocation>>(), location => this.GetVirtualMachine().Equals(location.GetVirtualMachine())));
#endif

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
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMethod>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMethod>>(), method => method != null && this.GetVirtualMachine().Equals(method.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMethod> GetMethodsByName(string name, string signature)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(signature != null, "signature");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(signature));
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMethod>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMethod>>(), method => method != null && this.GetVirtualMachine().Equals(method.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IReferenceType> GetNestedTypes()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IReferenceType>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IReferenceType>>(), type => type != null && this.GetVirtualMachine().Equals(type.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public string GetSourceDebugExtension()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public string GetSourceName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<string> GetSourceNames(string stratum)
        {
            Contract.Requires<ArgumentException>(stratum == null || stratum.Length > 0);
            Contract.Ensures(Contract.Result<ReadOnlyCollection<string>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<string>>(), name => !string.IsNullOrEmpty(name)));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<string> GetSourcePaths(string stratum)
        {
            Contract.Requires<ArgumentException>(stratum == null || stratum.Length > 0);
            Contract.Ensures(Contract.Result<ReadOnlyCollection<string>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<string>>(), name => !string.IsNullOrEmpty(name)));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IField> GetVisibleFields()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IField>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IField>>(), field => field != null && this.GetVirtualMachine().Equals(field.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMethod> GetVisibleMethods()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IMethod>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IMethod>>(), method => method != null && this.GetVirtualMachine().Equals(method.GetVirtualMachine())));
#endif

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
    }
}
