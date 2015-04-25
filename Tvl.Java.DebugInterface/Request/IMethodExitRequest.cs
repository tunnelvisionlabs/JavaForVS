namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMethodExitRequestContracts))]
    public interface IMethodExitRequest : IEventRequest, IClassFilter, IInstanceFilter, IThreadFilter
    {
    }
}
