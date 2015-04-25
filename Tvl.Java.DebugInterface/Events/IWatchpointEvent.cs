namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IWatchpointEventContracts))]
    public interface IWatchpointEvent : ILocatableEvent
    {
        IField GetField();

        IObjectReference GetObject();

        IValue GetCurrentValue();
    }
}
