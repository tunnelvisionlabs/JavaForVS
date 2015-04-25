namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IWatchpointRequestContracts))]
    public interface IWatchpointRequest : IEventRequest, IClassFilter, IInstanceFilter, IThreadFilter
    {
        IField Field
        {
            get;
        }
    }
}
