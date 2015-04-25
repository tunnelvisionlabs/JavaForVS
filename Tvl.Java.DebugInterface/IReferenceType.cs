namespace Tvl.Java.DebugInterface
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using ConstantPoolEntry = Tvl.Java.DebugInterface.Types.ConstantPoolEntry;

    /// <summary>
    /// The type of an object in a target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IReferenceTypeContracts))]
    public interface IReferenceType : IType, IAccessible, IEquatable<IReferenceType>
    {
        /// <summary>
        /// Returns a list containing each <see cref="IField"/> declared in this type.
        /// </summary>
        /// <param name="includeInherited">Specify <c>true</c> to include fields declared in this type's superclasses, implemented interfaces, and/or superinterfaces.</param>
        ReadOnlyCollection<IField> GetFields(bool includeInherited);

        /// <summary>
        /// Returns a list containing a <see cref="ILocation"/> object for each executable source line in this reference type.
        /// </summary>
        ReadOnlyCollection<ILocation> GetLineLocations();

        /// <summary>
        /// Returns a list containing a <see cref="ILocation"/> object for each executable source line in this reference type.
        /// </summary>
        ReadOnlyCollection<ILocation> GetLineLocations(string stratum, string sourceName);

        /// <summary>
        /// Returns a list containing each <see cref="IMethod"/> declared in this type.
        /// </summary>
        /// <param name="includeInherited">Specify <c>true</c> to include methods declared in this type's superclasses, implemented interfaces, and/or superinterfaces.</param>
        ReadOnlyCollection<IMethod> GetMethods(bool includeInherited);

        /// <summary>
        /// Return the available strata for this reference type.
        /// </summary>
        ReadOnlyCollection<string> GetAvailableStrata();

        /// <summary>
        /// Gets the classloader object which loaded the class corresponding to this type.
        /// </summary>
        IClassLoaderReference GetClassLoader();

        /// <summary>
        /// Returns the class object that corresponds to this type in the target VM.
        /// </summary>
        IClassObjectReference GetClassObject();

        /// <summary>
        /// Returns the raw bytes of the constant pool in the format of the constant_pool item
        /// of the Class File Format in the Java Virtual Machine Specification.
        /// </summary>
        ReadOnlyCollection<ConstantPoolEntry> GetConstantPool();

        /// <summary>
        /// Returns the number of entries in the constant pool plus one.
        /// </summary>
        int GetConstantPoolCount();

        /// <summary>
        /// Returns the default stratum for this reference type.
        /// </summary>
        string GetDefaultStratum();

        /// <summary>
        /// Determines if initialization failed for this class.
        /// </summary>
        bool GetFailedToInitialize();

        /// <summary>
        /// Finds the visible <see cref="IField"/> with the given non-ambiguous name.
        /// </summary>
        IField GetFieldByName(string fieldName);

        /// <summary>
        /// Gets the generic signature for this type if there is one.
        /// </summary>
        string GetGenericSignature();

        /// <summary>
        /// Gets the <see cref="IValue"/> of a given static <see cref="IField"/> in this type.
        /// </summary>
        IValue GetValue(IField field);

        /// <summary>
        /// Returns a dictionary containing the <see cref="IValue"/> of each static <see cref="IField"/>
        /// specified in <param name="fields"/>.
        /// </summary>
        IDictionary<IField, IValue> GetValues(IEnumerable<IField> fields);

        /// <summary>
        /// Returns instances of this ReferenceType.
        /// </summary>
        ReadOnlyCollection<IObjectReference> GetInstances(long maxInstances);

        /// <summary>
        /// Determines if this type was declared abstract.
        /// </summary>
        bool GetIsAbstract();

        /// <summary>
        /// Determines if this type was declared final.
        /// </summary>
        bool GetIsFinal();

        /// <summary>
        /// Determines if this type has been initialized.
        /// </summary>
        bool GetIsInitialized();

        /// <summary>
        /// Determines if this type has been prepared.
        /// </summary>
        bool GetIsPrepared();

        /// <summary>
        /// Determines if this type was declared static.
        /// </summary>
        bool GetIsStatic();

        /// <summary>
        /// Determines if this type has been verified.
        /// </summary>
        bool GetIsVerified();

        /// <summary>
        /// Returns a list containing all <see cref="ILocation"/> objects that map to the given line number.
        /// </summary>
        ReadOnlyCollection<ILocation> GetLocationsOfLine(int lineNumber);

        /// <summary>
        /// Returns a list containing all <see cref="ILocation"/> objects that map to the given line number.
        /// </summary>
        ReadOnlyCollection<ILocation> GetLocationsOfLine(string stratum, string sourceName, int lineNumber);

        /// <summary>
        /// Returns the class major version number, as defined in the class file format of the Java Virtual Machine Specification.
        /// </summary>
        int GetMajorVersion();

        /// <summary>
        /// Returns the class minor version number, as defined in the class file format of the Java Virtual Machine Specification.
        /// </summary>
        int GetMinorVersion();

        /// <summary>
        /// Returns a list containing each visible <see cref="IMethod"/> that has the given name.
        /// </summary>
        ReadOnlyCollection<IMethod> GetMethodsByName(string name);

        /// <summary>
        /// Returns a list containing each visible <see cref="IMethod"/> that has the given name and signature.
        /// </summary>
        ReadOnlyCollection<IMethod> GetMethodsByName(string name, string signature);

        /// <summary>
        /// Returns a list containing <see cref="IReferenceType"/> objects that are declared within this
        /// type and are currently loaded into the Virtual Machine.
        /// </summary>
        ReadOnlyCollection<IReferenceType> GetNestedTypes();

        /// <summary>
        /// Get the source debug extension of this type.
        /// </summary>
        string GetSourceDebugExtension();

        /// <summary>
        /// Gets an identifying name for the source corresponding to the declaration of this type.
        /// </summary>
        string GetSourceName();

        /// <summary>
        /// Gets the identifying names for all the source corresponding to the declaration of this type.
        /// </summary>
        ReadOnlyCollection<string> GetSourceNames(string stratum);

        /// <summary>
        /// Gets the paths to the source corresponding to the declaration of this type.
        /// </summary>
        ReadOnlyCollection<string> GetSourcePaths(string stratum);

        /// <summary>
        /// Returns a list containing each unhidden and unambiguous <see cref="IField"/> in this type.
        /// </summary>
        ReadOnlyCollection<IField> GetVisibleFields();

        /// <summary>
        /// Returns a list containing each <see cref="IMethod"/> declared or inherited by this type.
        /// </summary>
        ReadOnlyCollection<IMethod> GetVisibleMethods();
    }
}
