namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMonitorContendedEnterEventContracts))]
    public interface IMonitorContendedEnterEvent : IMonitorEvent
    {
    }
}
