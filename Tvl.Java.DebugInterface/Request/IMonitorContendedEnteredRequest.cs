namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMonitorContendedEnteredRequestContracts))]
    public interface IMonitorContendedEnteredRequest : IEventRequest, IClassFilter, IInstanceFilter, IThreadFilter
    {
    }
}
