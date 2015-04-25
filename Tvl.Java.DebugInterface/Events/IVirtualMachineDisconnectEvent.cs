namespace Tvl.Java.DebugInterface.Events
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IVirtualMachineDisconnectEventContracts))]
    public interface IVirtualMachineDisconnectEvent : IEvent
    {
    }
}
