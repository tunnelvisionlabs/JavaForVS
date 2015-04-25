namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// An instance of java.lang.Class from the target VM. Use this interface to access type
    /// information for the class, array, or interface that this object reflects.
    /// </summary>
    [ContractClass(typeof(Contracts.IClassObjectReferenceContracts))]
    public interface IClassObjectReference : IObjectReference
    {
        /// <summary>
        /// Gets the <see cref="IReferenceType"/> corresponding to this class object.
        /// </summary>
        IReferenceType GetReflectedType();
    }
}
