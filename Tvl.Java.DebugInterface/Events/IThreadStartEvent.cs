namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IThreadStartEventContracts))]
    public interface IThreadStartEvent : IThreadEvent
    {
    }
}
