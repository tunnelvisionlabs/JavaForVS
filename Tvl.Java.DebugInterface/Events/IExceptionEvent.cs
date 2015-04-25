namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IExceptionEventContracts))]
    public interface IExceptionEvent : ILocatableEvent
    {
        ILocation GetCatchLocation();

        IObjectReference GetException();
    }
}
