namespace Tvl.Java.DebugInterface.Client.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class FieldAccessEventArgs : ThreadLocationEventArgs
    {
        private readonly IField _field;
        private readonly IObjectReference _instance;

        internal FieldAccessEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request, ThreadReference thread, Location location, IField field, IObjectReference instance)
            : base(virtualMachine, suspendPolicy, request, thread, location)
        {
            Contract.Requires<ArgumentNullException>(field != null, "field");
            Contract.Requires(field.GetIsStatic() || instance != null);

            _field = field;
            _instance = instance;
        }

        public IField Field
        {
            get
            {
                return _field;
            }
        }

        public IObjectReference Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
