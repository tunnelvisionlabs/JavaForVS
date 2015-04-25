namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Diagnostics.Contracts;

    internal abstract class Mirror : IMirror
    {
        private readonly VirtualMachine _virtualMachine;

        protected Mirror(VirtualMachine virtualMachine)
        {
            Contract.Requires<ArgumentNullException>(virtualMachine != null, "virtualMachine");
            _virtualMachine = virtualMachine;
        }

        internal VirtualMachine VirtualMachine
        {
            get
            {
                Contract.Ensures(Contract.Result<VirtualMachine>() != null);
                return _virtualMachine;
            }
        }

        IVirtualMachine IMirror.GetVirtualMachine()
        {
            return VirtualMachine;
        }
    }
}
