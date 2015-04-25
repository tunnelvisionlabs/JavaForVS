namespace Tvl.Java.DebugInterface.Connect
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A method of communication between a debugger and a target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.ITransportContracts))]
    public interface ITransport
    {
        /// <summary>
        /// Returns a short identifier for the transport.
        /// </summary>
        string Name
        {
            get;
        }
    }
}
