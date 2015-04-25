namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMonitorWaitedRequestContracts))]
    public interface IMonitorWaitedRequest : IEventRequest, IClassFilter, IInstanceFilter, IThreadFilter
    {
    }
}
