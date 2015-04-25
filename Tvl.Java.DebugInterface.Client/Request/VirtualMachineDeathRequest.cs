namespace Tvl.Java.DebugInterface.Client.Request
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    internal sealed class VirtualMachineDeathRequest : EventRequest, IVirtualMachineDeathRequest
    {
        public VirtualMachineDeathRequest(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        internal override Types.EventKind EventKind
        {
            get
            {
                return Types.EventKind.VirtualMachineDeath;
            }
        }
    }
}
