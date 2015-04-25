namespace Tvl.Java.DebugInterface.Client.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class ExceptionEventArgs : ThreadLocationEventArgs
    {
        private readonly IObjectReference _exception;
        private readonly ILocation _catchLocation;

        internal ExceptionEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request, ThreadReference thread, Location location, IObjectReference exception, ILocation catchLocation)
            : base(virtualMachine, suspendPolicy, request, thread, location)
        {
            Contract.Requires<ArgumentNullException>(exception != null, "exception");

            _exception = exception;
            _catchLocation = catchLocation;
        }

        public IObjectReference Exception
        {
            get
            {
                return _exception;
            }
        }

        public ILocation CatchLocation
        {
            get
            {
                return _catchLocation;
            }
        }
    }
}
