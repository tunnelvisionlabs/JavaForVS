namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.ILocatableEventContracts))]
    public interface ILocatableEvent : IThreadEvent, ILocatable
    {
    }
}
