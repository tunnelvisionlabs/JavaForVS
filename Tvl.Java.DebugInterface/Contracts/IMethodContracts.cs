namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using ExceptionTableEntry = Tvl.Java.DebugInterface.Types.Loader.ExceptionTableEntry;

    [ContractClassFor(typeof(IMethod))]
    internal abstract class IMethodContracts : IMethod
    {
        #region IMethod Members

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

        public ReadOnlyCollection<ILocalVariable> GetArguments()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ILocalVariable>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<ILocalVariable>>(), variable => variable != null && this.GetVirtualMachine().Equals(variable.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<string> GetArgumentTypeNames()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<string>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<string>>(), name => !string.IsNullOrEmpty(name)));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IType> GetArgumentTypes()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IType>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IType>>(), type => type != null && this.GetVirtualMachine().Equals(type.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public byte[] GetBytecodes()
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ExceptionTableEntry> GetExceptionTable()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ExceptionTableEntry>>() != null);

            throw new NotImplementedException();
        }

        public bool GetIsAbstract()
        {
            throw new NotImplementedException();
        }

        public bool GetIsBridge()
        {
            throw new NotImplementedException();
        }

        public bool GetIsConstructor()
        {
            throw new NotImplementedException();
        }

        public bool GetIsNative()
        {
            throw new NotImplementedException();
        }

        public bool GetIsObsolete()
        {
            throw new NotImplementedException();
        }

        public bool GetIsStaticInitializer()
        {
            throw new NotImplementedException();
        }

        public bool GetIsSynchronized()
        {
            throw new NotImplementedException();
        }

        public bool GetIsVarArgs()
        {
            throw new NotImplementedException();
        }

        public bool GetHasVariableInfo()
        {
            throw new NotImplementedException();
        }

        public ILocation GetLocationOfCodeIndex(long codeIndex)
        {
            Contract.Requires<ArgumentOutOfRangeException>(codeIndex >= 0);
            Contract.Ensures(Contract.Result<ILocation>() == null || this.GetVirtualMachine().Equals(Contract.Result<ILocation>().GetVirtualMachine()));

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

        public IType GetReturnType()
        {
            Contract.Ensures(Contract.Result<IType>() != null);
            Contract.Ensures(this.GetVirtualMachine().Equals(Contract.Result<IType>().GetVirtualMachine()));

            throw new NotImplementedException();
        }

        public string GetReturnTypeName()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILocalVariable> GetVariables()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ILocalVariable>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<ILocalVariable>>(), variable => this.GetVirtualMachine().Equals(variable.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILocalVariable> GetVariablesByName(string name)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ILocalVariable>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<ILocalVariable>>(), variable => this.GetVirtualMachine().Equals(variable.GetVirtualMachine())));
#endif

            throw new NotImplementedException();
        }

        #endregion

        #region ILocatable Members

        public ILocation GetLocation()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEquatable<IMethod> Members

        public bool Equals(IMethod other)
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
