namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The mirror for a type in the target VM. This interface is the root of a type hierarchy
    /// encompassing primitive types and reference types.
    /// </summary>
    [ContractClass(typeof(Contracts.ITypeContracts))]
    public interface IType : IMirror
    {
        /// <summary>
        /// Returns a text representation of this type.
        /// </summary>
        string GetName();

        /// <summary>
        /// Returns the JNI-style signature for this type.
        /// </summary>
        string GetSignature();
    }
}
