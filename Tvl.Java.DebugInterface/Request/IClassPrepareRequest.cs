namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IClassPrepareRequestContracts))]
    public interface IClassPrepareRequest : IEventRequest, IClassFilter
    {
        void AddSourceNameFilter(string sourceNamePattern);
    }
}
