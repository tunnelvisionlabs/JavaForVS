namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The type associated with non-object values in a target VM. Instances of one of the sub-interfaces
    /// of this interface will be returned from <see cref="IValue.GetValueType()"/> for all
    /// <see cref="IPrimitiveValue"/> objects.
    /// </summary>
    [ContractClass(typeof(Contracts.IPrimitiveTypeContracts))]
    public interface IPrimitiveType : IType
    {
    }
}
