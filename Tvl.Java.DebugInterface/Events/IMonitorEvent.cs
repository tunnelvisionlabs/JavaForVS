namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMonitorEventContracts))]
    public interface IMonitorEvent : ILocatableEvent
    {
        IObjectReference GetMonitor();
    }
}
