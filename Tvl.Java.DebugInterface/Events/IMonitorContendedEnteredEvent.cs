namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMonitorContendedEnteredEventContracts))]
    public interface IMonitorContendedEnteredEvent : IMonitorEvent
    {
    }
}
