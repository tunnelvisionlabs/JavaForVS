namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using ExceptionTableEntry = Tvl.Java.DebugInterface.Types.Loader.ExceptionTableEntry;

    /// <summary>
    /// A static or instance method in the target VM. See <see cref="ITypeComponent"/> for
    /// general information about Field and Method mirrors.
    /// </summary>
    [ContractClass(typeof(Contracts.IMethodContracts))]
    public interface IMethod : ITypeComponent, ILocatable, IEquatable<IMethod>
    {
        /// <summary>
        /// Returns a list containing a <see cref="ILocation"/> object for each executable source line in this method.
        /// </summary>
        ReadOnlyCollection<ILocation> GetLineLocations();

        /// <summary>
        /// Returns a list containing a <see cref="ILocation"/> object for each executable source line in this method.
        /// </summary>
        ReadOnlyCollection<ILocation> GetLineLocations(string stratum, string sourceName);

        /// <summary>
        /// Returns a list containing each <see cref="ILocalVariable"/> that is declared as an argument of this method.
        /// </summary>
        ReadOnlyCollection<ILocalVariable> GetArguments();

        /// <summary>
        /// Returns a list containing a text representation of the type of each formal parameter of this method.
        /// </summary>
        ReadOnlyCollection<string> GetArgumentTypeNames();

        /// <summary>
        /// Returns a list containing the type of each formal parameter of this method.
        /// </summary>
        ReadOnlyCollection<IType> GetArgumentTypes();

        /// <summary>
        /// Returns an array containing the bytecodes for this method.
        /// </summary>
        byte[] GetBytecodes();

        ReadOnlyCollection<ExceptionTableEntry> GetExceptionTable();

        /// <summary>
        /// Determine if this method is abstract.
        /// </summary>
        bool GetIsAbstract();

        /// <summary>
        /// Determine if this method is a bridge method.
        /// </summary>
        bool GetIsBridge();

        /// <summary>
        /// Determine if this method is a constructor.
        /// </summary>
        bool GetIsConstructor();

        /// <summary>
        /// Determine if this method is native.
        /// </summary>
        bool GetIsNative();

        /// <summary>
        /// Determine if this method is obsolete.
        /// </summary>
        bool GetIsObsolete();

        /// <summary>
        /// Determine if this method is a static initializer.
        /// </summary>
        bool GetIsStaticInitializer();

        /// <summary>
        /// Determine if this method is synchronized.
        /// </summary>
        bool GetIsSynchronized();

        /// <summary>
        /// Determine if this method accepts a variable number of arguments.
        /// </summary>
        bool GetIsVarArgs();

        /// <summary>
        /// Determine if local variable information is available for this method.
        /// </summary>
        bool GetHasVariableInfo();

        /// <summary>
        /// Returns a <see cref="ILocation"/> for the given code index.
        /// </summary>
        ILocation GetLocationOfCodeIndex(long codeIndex);

        /// <summary>
        /// Returns a list containing all <see cref="ILocation"/> objects that map to the given line number.
        /// </summary>
        ReadOnlyCollection<ILocation> GetLocationsOfLine(int lineNumber);

        /// <summary>
        /// Returns a list containing all <see cref="ILocation"/> objects that map to the given line number and source name.
        /// </summary>
        ReadOnlyCollection<ILocation> GetLocationsOfLine(string stratum, string sourceName, int lineNumber);

        /// <summary>
        /// Returns the return type, as specified in the declaration of this method.
        /// </summary>
        IType GetReturnType();

        /// <summary>
        /// Returns a text representation of the return type, as specified in the declaration of this method.
        /// </summary>
        string GetReturnTypeName();

        /// <summary>
        /// Returns a list containing each <see cref="ILocalVariable"/> declared in this method.
        /// </summary>
        ReadOnlyCollection<ILocalVariable> GetVariables();

        /// <summary>
        /// Returns a list containing each <see cref="ILocalVariable"/> of a given name in this method.
        /// </summary>
        ReadOnlyCollection<ILocalVariable> GetVariablesByName(string name);
    }
}
