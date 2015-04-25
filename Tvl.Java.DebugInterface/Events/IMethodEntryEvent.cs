namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMethodEntryEventContracts))]
    public interface IMethodEntryEvent : ILocatableEvent
    {
        IMethod GetMethod();
    }
}
