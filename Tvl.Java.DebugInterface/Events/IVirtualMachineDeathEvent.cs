namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IVirtualMachineDeathEventContracts))]
    public interface IVirtualMachineDeathEvent : IEvent
    {
    }
}
