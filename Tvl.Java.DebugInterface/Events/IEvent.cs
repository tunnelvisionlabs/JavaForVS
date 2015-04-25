namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    [ContractClass(typeof(Contracts.IEventContracts))]
    public interface IEvent : IMirror
    {
        IEventRequest GetRequest();
    }
}
