namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides access to the class of an array and the type of its components in the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IArrayTypeContracts))]
    public interface IArrayType : IReferenceType
    {
        /// <summary>
        /// Gets the JNI signature of the components of this array class.
        /// </summary>
        string GetComponentSignature();

        /// <summary>
        /// Returns the component type of this array, as specified in the array declaration.
        /// </summary>
        IType GetComponentType();

        /// <summary>
        /// Returns a text representation of the component type of this array.
        /// </summary>
        string GetComponentTypeName();

        /// <summary>
        /// Creates a new instance of this array class in the target VM.
        /// </summary>
        IStrongValueHandle<IArrayReference> CreateInstance(int length);
    }
}
