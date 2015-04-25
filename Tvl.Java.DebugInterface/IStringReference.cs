namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A string object from the target VM. A StringReference is an <see cref="IObjectReference"/>
    /// with additional access to string-specific information from the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IStringReferenceContracts))]
    public interface IStringReference : IObjectReference
    {
        /// <summary>
        /// Returns the IStringReference as a string.
        /// </summary>
        string GetValue();
    }
}
