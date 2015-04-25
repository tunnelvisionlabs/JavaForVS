namespace Tvl.Java.DebugInterface.Client
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using ConstantPoolEntry = Tvl.Java.DebugInterface.Types.ConstantPoolEntry;

    internal class UnloadedReferenceType : JavaType, IReferenceType
    {
        private readonly string _signature;

        internal UnloadedReferenceType(VirtualMachine virtualMachine, string signature)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            Contract.Requires(!string.IsNullOrEmpty(signature));
            _signature = signature;
        }

        public string Signature
        {
            get
            {
                return _signature;
            }
        }

        public override string GetSignature()
        {
            return _signature;
        }

        #region IReferenceType Members

        public ReadOnlyCollection<IField> GetFields(bool includeInherited)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<ILocation> GetLineLocations()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<ILocation> GetLineLocations(string stratum, string sourceName)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<IMethod> GetMethods(bool includeInherited)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<string> GetAvailableStrata()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public IClassLoaderReference GetClassLoader()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public IClassObjectReference GetClassObject()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<ConstantPoolEntry> GetConstantPool()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public int GetConstantPoolCount()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public string GetDefaultStratum()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public bool GetFailedToInitialize()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public IField GetFieldByName(string fieldName)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public string GetGenericSignature()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public IValue GetValue(IField field)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public IDictionary<IField, IValue> GetValues(IEnumerable<IField> fields)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<IObjectReference> GetInstances(long maxInstances)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public bool GetIsAbstract()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public bool GetIsFinal()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public bool GetIsInitialized()
        {
            return false;
        }

        public bool GetIsPrepared()
        {
            return false;
        }

        public bool GetIsStatic()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public bool GetIsVerified()
        {
            return false;
        }

        public ReadOnlyCollection<ILocation> GetLocationsOfLine(int lineNumber)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<ILocation> GetLocationsOfLine(string stratum, string sourceName, int lineNumber)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public int GetMajorVersion()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public int GetMinorVersion()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<IMethod> GetMethodsByName(string name)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<IMethod> GetMethodsByName(string name, string signature)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<IReferenceType> GetNestedTypes()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public string GetSourceDebugExtension()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public string GetSourceName()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<string> GetSourceNames(string stratum)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<string> GetSourcePaths(string stratum)
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<IField> GetVisibleFields()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public ReadOnlyCollection<IMethod> GetVisibleMethods()
        {
            throw new ClassNotLoadedException(GetName());
        }

        #endregion

        #region IAccessible Members

        public bool GetIsPackagePrivate()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public bool GetIsPrivate()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public bool GetIsProtected()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public bool GetIsPublic()
        {
            throw new ClassNotLoadedException(GetName());
        }

        public AccessModifiers GetModifiers()
        {
            throw new ClassNotLoadedException(GetName());
        }

        #endregion

        #region IEquatable<IReferenceType> Members

        public bool Equals(IReferenceType other)
        {
            UnloadedReferenceType otherType = other as UnloadedReferenceType;
            if (otherType == null)
                return false;

            return this.VirtualMachine.Equals(otherType.VirtualMachine)
                && this.Signature == otherType.Signature;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as UnloadedReferenceType);
        }

        public override int GetHashCode()
        {
            return VirtualMachine.GetHashCode() ^ _signature.GetHashCode();
        }

        #endregion
    }
}
