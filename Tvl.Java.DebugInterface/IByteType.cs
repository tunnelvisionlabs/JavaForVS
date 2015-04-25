namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The type of all primitive byte values accessed in the target VM. Calls to
    /// <see cref="IValue.GetValueType()"/> will return an implementor of this interface.
    /// </summary>
    [ContractClass(typeof(Contracts.IByteTypeContracts))]
    public interface IByteType : IPrimitiveType
    {
    }
}
