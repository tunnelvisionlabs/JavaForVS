namespace Tvl.Java.DebugInterface.Client.Events
{
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class ThreadLocationEventArgs : ThreadEventArgs
    {
        private readonly Location _location;

        internal ThreadLocationEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request, ThreadReference thread, Location location)
            : base(virtualMachine, suspendPolicy, request, thread)
        {
            _location = location;
        }

        public ILocation Location
        {
            get
            {
                return _location;
            }
        }
    }
}
