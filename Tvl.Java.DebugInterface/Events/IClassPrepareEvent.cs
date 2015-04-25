namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IClassPrepareEventContracts))]
    public interface IClassPrepareEvent : IThreadEvent
    {
        IReferenceType GetReferenceType();
    }
}
