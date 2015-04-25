namespace Tvl.Java.DebugInterface.Client.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class MethodExitEventArgs : ThreadLocationEventArgs
    {
        private readonly IValue _returnValue;

        internal MethodExitEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request, ThreadReference thread, Location location, IValue returnValue)
            : base(virtualMachine, suspendPolicy, request, thread, location)
        {
            Contract.Requires<ArgumentNullException>(returnValue != null, "returnValue");

            _returnValue = returnValue;
        }

        public IValue ReturnValue
        {
            get
            {
                return _returnValue;
            }
        }
    }
}
