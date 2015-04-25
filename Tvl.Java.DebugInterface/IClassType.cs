namespace Tvl.Java.DebugInterface
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A mirror of a class in the target VM. A ClassType is a refinement of ReferenceType that applies to true
    /// classes in the JLS sense of the definition (not an interface, not an array type). Any ObjectReference
    /// that mirrors an instance of such a class will have a ClassType as its type.
    /// </summary>
    [ContractClass(typeof(Contracts.IClassTypeContracts))]
    public interface IClassType : IReferenceType
    {
        /// <summary>
        /// Gets the interfaces implemented by this class.
        /// </summary>
        /// <param name="includeInherited">If true, this method will include interfaces directly and indirectly implemented by this class.</param>
        ReadOnlyCollection<IInterfaceType> GetInterfaces(bool includeInherited);

        /// <summary>
        /// Returns a the single non-abstract Method visible from this class that has the given name and signature.
        /// </summary>
        IMethod GetConcreteMethod(string name, string signature);

        /// <summary>
        /// Invokes the specified static Method in the target VM.
        /// </summary>
        IStrongValueHandle<IValue> InvokeMethod(IThreadReference thread, IMethod method, InvokeOptions options, params IValue[] arguments);

        /// <summary>
        /// Determine if this class was declared as an enum.
        /// </summary>
        bool GetIsEnum();

        /// <summary>
        /// Constructs a new instance of this type, using the given constructor Method in the target VM.
        /// </summary>
        IStrongValueHandle<IObjectReference> CreateInstance(IThreadReference thread, IMethod method, InvokeOptions options, params IValue[] arguments);

        /// <summary>
        /// Assigns a value to a static field.
        /// </summary>
        void SetValue(IField field, IValue value);

        /// <summary>
        /// Gets the currently loaded, direct subclasses of this class.
        /// </summary>
        ReadOnlyCollection<IClassType> GetSubclasses();

        /// <summary>
        /// Gets the superclass of this class.
        /// </summary>
        IClassType GetSuperclass();
    }
}
