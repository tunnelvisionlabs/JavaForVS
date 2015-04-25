namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMonitorWaitRequestContracts))]
    public interface IMonitorWaitRequest : IEventRequest, IClassFilter, IInstanceFilter, IThreadFilter
    {
    }
}
