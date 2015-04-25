namespace Tvl.Java.DebugInterface.Client.Events
{
    using System;
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class MonitorWaitEventArgs : MonitorEventArgs
    {
        private readonly TimeSpan _timeout;

        internal MonitorWaitEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request, ThreadReference thread, Location location, IObjectReference @object, TimeSpan timeout)
            : base(virtualMachine, suspendPolicy, request, thread, location, @object)
        {
            _timeout = timeout;
        }

        public TimeSpan Timeout
        {
            get
            {
                return _timeout;
            }
        }
    }
}
