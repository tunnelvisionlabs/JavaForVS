namespace Tvl.Java.DebugInterface
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A mirror of an interface in the target VM. An InterfaceType is a refinement of <see cref="IReferenceType"/>
    /// that applies to true interfaces in the JLS sense of the definition (not a class, not an array type). An
    /// interface type will never be returned by <see cref="IObjectReference.GetReferenceType()"/>, but it may be
    /// in the list of implemented interfaces for a <see cref="IClassType"/> that is returned by that method.
    /// </summary>
    [ContractClass(typeof(Contracts.IInterfaceTypeContracts))]
    public interface IInterfaceType : IReferenceType
    {
        /// <summary>
        /// Gets the currently prepared classes which directly implement this interface.
        /// </summary>
        ReadOnlyCollection<IClassType> GetImplementors();

        /// <summary>
        /// Gets the currently prepared interfaces which directly extend this interface.
        /// </summary>
        ReadOnlyCollection<IInterfaceType> GetSubInterfaces();

        /// <summary>
        /// Gets the interfaces directly extended by this interface.
        /// </summary>
        ReadOnlyCollection<IInterfaceType> GetSuperInterfaces();
    }
}
