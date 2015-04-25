namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The type of all primitive boolean values accessed in the target VM. Calls
    /// to IValue.GetValueType() will return an implementor of this interface.
    /// </summary>
    [ContractClass(typeof(Contracts.IBooleanTypeContracts))]
    public interface IBooleanType : IPrimitiveType
    {
    }
}
