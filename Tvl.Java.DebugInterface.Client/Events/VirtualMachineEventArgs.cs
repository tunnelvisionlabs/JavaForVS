namespace Tvl.Java.DebugInterface.Client.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class VirtualMachineEventArgs : EventArgs
    {
        private readonly VirtualMachine _virtualMachine;
        private readonly SuspendPolicy _suspendPolicy;
        private readonly EventRequest _request;

        internal VirtualMachineEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request)
        {
            Contract.Requires<ArgumentNullException>(virtualMachine != null, "virtualMachine");

            _virtualMachine = virtualMachine;
            _suspendPolicy = suspendPolicy;
            _request = request;
        }

        public IVirtualMachine VirtualMachine
        {
            get
            {
                return _virtualMachine;
            }
        }

        public SuspendPolicy SuspendPolicy
        {
            get
            {
                return _suspendPolicy;
            }
        }

        public IEventRequest Request
        {
            get
            {
                return _request;
            }
        }
    }
}
