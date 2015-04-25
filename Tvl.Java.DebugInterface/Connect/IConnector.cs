namespace Tvl.Java.DebugInterface.Connect
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A method of connection between a debugger and a target VM.
    /// </summary>
    /// <remarks>
    /// A method of connection between a debugger and a target VM. A connector encapsulates exactly one
    /// <see cref="ITransport"/> used to establish the connection. Each connector has a set of arguments
    /// which controls its operation. The arguments are stored as a dictionary, keyed by a string. Each
    /// implementation defines the string argument keys it accepts.
    /// </remarks>
    [ContractClass(typeof(Contracts.IConnectorContracts))]
    public interface IConnector
    {
        /// <summary>
        /// Returns the arguments accepted by this connector and their default values.
        /// </summary>
        IDictionary<string, IConnectorArgument> DefaultArguments
        {
            get;
        }

        /// <summary>
        /// Returns a human-readable description of this connector and its purpose.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// Returns a short identifier for the connector.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Returns the transport mechanism used by this connector to establish connections with a target VM.
        /// </summary>
        ITransport Transport
        {
            get;
        }
    }
}
