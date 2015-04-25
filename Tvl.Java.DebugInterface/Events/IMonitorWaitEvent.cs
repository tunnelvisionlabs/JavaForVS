namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;
    using TimeSpan = System.TimeSpan;

    [ContractClass(typeof(Contracts.IMonitorWaitEventContracts))]
    public interface IMonitorWaitEvent : IMonitorEvent
    {
        TimeSpan GetTimeout();
    }
}
