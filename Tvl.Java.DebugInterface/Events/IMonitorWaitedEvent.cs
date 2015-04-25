namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMonitorWaitedEventContracts))]
    public interface IMonitorWaitedEvent : IMonitorEvent
    {
        bool GetTimedOut();
    }
}
