namespace Tvl.Java.DebugInterface.Connect
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A connector which attaches to a previously running target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IAttachingConnectorContracts))]
    public interface IAttachingConnector : IConnector
    {
        /// <summary>
        /// Attaches to a running application and and returns a mirror of its VM.
        /// </summary>
        IVirtualMachine Attach(IEnumerable<KeyValuePair<string, IConnectorArgument>> arguments);
    }
}
