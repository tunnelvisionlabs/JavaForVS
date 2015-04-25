namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IMethodExitEventContracts))]
    public interface IMethodExitEvent : ILocatableEvent
    {
        IMethod GetMethod();

        IValue GetReturnValue();
    }
}
