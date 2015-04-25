namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The type of all primitive void values accessed in the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IVoidTypeContracts))]
    public interface IVoidType : IType
    {
    }
}
