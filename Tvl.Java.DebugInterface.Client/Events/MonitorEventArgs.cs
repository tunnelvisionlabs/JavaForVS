namespace Tvl.Java.DebugInterface.Client.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class MonitorEventArgs : ThreadLocationEventArgs
    {
        private readonly IObjectReference _object;

        internal MonitorEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request, ThreadReference thread, Location location, IObjectReference @object)
            : base(virtualMachine, suspendPolicy, request, thread, location)
        {
            Contract.Requires<ArgumentNullException>(@object != null, "object");

            _object = @object;
        }

        public IObjectReference Object
        {
            get
            {
                return _object;
            }
        }
    }
}
