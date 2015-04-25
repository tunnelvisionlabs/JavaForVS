namespace Tvl.Java.DebugInterface.Client.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Client.Request;
    using Tvl.Java.DebugInterface.Request;

    public class ClassPrepareEventArgs : ThreadEventArgs
    {
        private readonly string _signature;
        private readonly ReferenceType _type;

        internal ClassPrepareEventArgs(VirtualMachine virtualMachine, SuspendPolicy suspendPolicy, EventRequest request, ThreadReference thread, string signature, ReferenceType type)
            : base(virtualMachine, suspendPolicy, request, thread)
        {
            Contract.Requires<ArgumentNullException>(signature != null, "signature");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(signature));

            _signature = signature;
            _type = type;
        }

        public string Signature
        {
            get
            {
                return _signature;
            }
        }

        public IReferenceType Type
        {
            get
            {
                return _type;
            }
        }
    }
}
