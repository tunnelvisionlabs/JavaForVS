namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IAccessWatchpointRequestContracts))]
    public interface IAccessWatchpointRequest : IWatchpointRequest
    {
    }
}
