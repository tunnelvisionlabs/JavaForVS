namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMonitorContendedEnterRequestContracts))]
    public interface IMonitorContendedEnterRequest : IEventRequest, IClassFilter, IInstanceFilter, IThreadFilter
    {
    }
}
