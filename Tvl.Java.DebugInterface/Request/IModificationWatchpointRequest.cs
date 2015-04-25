namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IModificationWatchpointRequestContracts))]
    public interface IModificationWatchpointRequest : IWatchpointRequest
    {
    }
}
