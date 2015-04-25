namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IClassUnloadEventContracts))]
    public interface IClassUnloadEvent : IEvent
    {
        string GetClassName();

        string GetClassSignature();
    }
}
