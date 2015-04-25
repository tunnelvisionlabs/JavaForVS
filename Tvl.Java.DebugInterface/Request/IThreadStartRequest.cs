namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IThreadStartRequestContracts))]
    public interface IThreadStartRequest : IEventRequest, IThreadFilter
    {
    }
}
