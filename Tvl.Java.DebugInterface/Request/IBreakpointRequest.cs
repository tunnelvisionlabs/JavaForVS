namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IBreakpointRequestContracts))]
    public interface IBreakpointRequest : IEventRequest, ILocatable, IInstanceFilter, IThreadFilter
    {
    }
}
