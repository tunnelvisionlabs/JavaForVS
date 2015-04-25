namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IThreadEventContracts))]
    public interface IThreadEvent : IEvent
    {
        IThreadReference GetThread();
    }
}
