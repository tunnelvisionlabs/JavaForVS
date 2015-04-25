namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMethodEntryRequestContracts))]
    public interface IMethodEntryRequest : IEventRequest, IClassFilter, IInstanceFilter, IThreadFilter
    {
    }
}
