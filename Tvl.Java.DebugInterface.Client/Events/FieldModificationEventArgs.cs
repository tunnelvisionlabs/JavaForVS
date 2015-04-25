namespace Tvl.Java.DebugInterface.Client.Events
{
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class FieldModificationEventArgs : FieldAccessEventArgs
    {
        private readonly IValue _newValue;

        internal FieldModificationEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request, ThreadReference thread, Location location, IField field, IObjectReference instance, IValue newValue)
            : base(virtualMachine, suspendPolicy, request, thread, location, field, instance)
        {
            _newValue = newValue;
        }

        public IValue NewValue
        {
            get
            {
                return _newValue;
            }
        }
    }
}
