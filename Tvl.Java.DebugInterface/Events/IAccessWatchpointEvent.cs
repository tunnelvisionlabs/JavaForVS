namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IAccessWatchpointEventContracts))]
    public interface IAccessWatchpointEvent : IWatchpointEvent
    {
    }
}
