namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IThreadDeathRequestContracts))]
    public interface IThreadDeathRequest : IEventRequest, IThreadFilter
    {
    }
}
