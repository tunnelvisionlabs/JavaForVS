namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The mirror for a value in the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IValueContracts))]
    public interface IValue : IMirror
    {
        /// <summary>
        /// Returns the run-time type of this value.
        /// </summary>
        IType GetValueType();
    }
}
