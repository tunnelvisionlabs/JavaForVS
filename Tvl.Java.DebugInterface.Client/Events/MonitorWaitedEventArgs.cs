namespace Tvl.Java.DebugInterface.Client.Events
{
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class MonitorWaitedEventArgs : MonitorEventArgs
    {
        private readonly bool _timedOut;

        internal MonitorWaitedEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request, ThreadReference thread, Location location, IObjectReference @object, bool timedOut)
            : base(virtualMachine, suspendPolicy, request, thread, location, @object)
        {
            _timedOut = timedOut;
        }

        public bool TimedOut
        {
            get
            {
                return _timedOut;
            }
        }
    }
}
