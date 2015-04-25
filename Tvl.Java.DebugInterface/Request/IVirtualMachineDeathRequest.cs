namespace Tvl.Java.DebugInterface.Request
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(Contracts.IVirtualMachineDeathRequestContracts))]
    public interface IVirtualMachineDeathRequest : IEventRequest
    {
    }
}
