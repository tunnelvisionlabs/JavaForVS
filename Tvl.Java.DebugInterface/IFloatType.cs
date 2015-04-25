namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The type of all primitive float values accessed in the target VM. Calls to
    /// <see cref="IValue.GetValueType()"/> will return an implementor of this interface.
    /// </summary>
    [ContractClass(typeof(Contracts.IFloatTypeContracts))]
    public interface IFloatType : IPrimitiveType
    {
    }
}
