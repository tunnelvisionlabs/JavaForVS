namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// An entity declared within a user defined type (class or interface).
    /// </summary>
    [ContractClass(typeof(Contracts.ITypeComponentContracts))]
    public interface ITypeComponent : IMirror, IAccessible
    {
        /// <summary>
        /// Returns the type in which this component was declared.
        /// </summary>
        IReferenceType GetDeclaringType();

        /// <summary>
        /// Gets the generic signature for this TypeComponent if there is one.
        /// </summary>
        string GetGenericSignature();

        /// <summary>
        /// Determines if this TypeComponent is final.
        /// </summary>
        [Pure]
        bool GetIsFinal();

        /// <summary>
        /// Determines if this TypeComponent is static.
        /// </summary>
        [Pure]
        bool GetIsStatic();

        /// <summary>
        /// Determines if this TypeComponent is synthetic.
        /// </summary>
        [Pure]
        bool GetIsSynthetic();

        /// <summary>
        /// Gets the name of this type component.
        /// </summary>
        string GetName();

        /// <summary>
        /// Gets the JNI-style signature for this type component.
        /// </summary>
        string GetSignature();
    }
}
