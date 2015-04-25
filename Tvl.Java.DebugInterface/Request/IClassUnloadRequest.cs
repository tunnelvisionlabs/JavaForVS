namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IClassUnloadRequestContracts))]
    public interface IClassUnloadRequest : IEventRequest, IClassNameFilter
    {
    }
}
