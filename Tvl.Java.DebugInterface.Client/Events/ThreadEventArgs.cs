namespace Tvl.Java.DebugInterface.Client.Events
{
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class ThreadEventArgs : VirtualMachineEventArgs
    {
        private readonly ThreadReference _thread;

        internal ThreadEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request, ThreadReference thread)
            : base(virtualMachine, suspendPolicy, request)
        {
            _thread = thread;
        }

        public IThreadReference Thread
        {
            get
            {
                return _thread;
            }
        }
    }
}
