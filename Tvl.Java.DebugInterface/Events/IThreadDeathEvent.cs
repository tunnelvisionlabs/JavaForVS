namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IThreadDeathEventContracts))]
    public interface IThreadDeathEvent : IThreadEvent
    {
    }
}
