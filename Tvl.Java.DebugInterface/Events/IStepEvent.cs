namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IStepEventContracts))]
    public interface IStepEvent : ILocatableEvent
    {
    }
}
