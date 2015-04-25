namespace Tvl.Java.DebugInterface.Connect
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A connector which listens for a connection initiated by a target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IListeningConnectorContracts))]
    public interface IListeningConnector : IConnector
    {
        /// <summary>
        /// Waits for a target VM to attach to this connector.
        /// </summary>
        IVirtualMachine Accept(IEnumerable<KeyValuePair<string, IConnectorArgument>> arguments);

        /// <summary>
        /// Listens for one or more connections initiated by target VMs.
        /// </summary>
        string StartListening(IEnumerable<KeyValuePair<string, IConnectorArgument>> arguments);

        /// <summary>
        /// Cancels listening for connections.
        /// </summary>
        void StopListening(IEnumerable<KeyValuePair<string, IConnectorArgument>> arguments);

        /// <summary>
        /// Indicates whether this listening connector supports multiple connections for a single argument map.
        /// </summary>
        bool GetSupportsMultipleConnections();
    }
}
