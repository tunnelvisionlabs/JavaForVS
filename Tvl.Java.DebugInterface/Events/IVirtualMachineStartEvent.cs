namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IVirtualMachineStartEventContracts))]
    public interface IVirtualMachineStartEvent : IThreadEvent
    {
    }
}
