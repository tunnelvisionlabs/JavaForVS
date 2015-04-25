namespace Tvl.Java.DebugInterface.Client.Request
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    internal sealed class BreakpointRequest : EventRequest, IBreakpointRequest
    {
        private readonly Location _location;

        public BreakpointRequest(VirtualMachine virtualMachine, Location location)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            Contract.Requires<ArgumentNullException>(location != null, "location");

            _location = location;

            Types.Location protocolLocation = new Types.Location(_location.Method.DeclaringType.TaggedReferenceTypeId, _location.Method.MethodId, checked((ulong)_location.CodeIndex));
            Modifiers.Add(Types.EventRequestModifier.LocationFilter(protocolLocation));
        }

        internal override Types.EventKind EventKind
        {
            get
            {
                return Types.EventKind.Breakpoint;
            }
        }

        public ILocation GetLocation()
        {
            return _location;
        }

        public void AddInstanceFilter(IObjectReference instance)
        {
            ObjectReference objectReference = instance as ObjectReference;
            if (objectReference == null || !objectReference.VirtualMachine.Equals(this.VirtualMachine))
                throw new VirtualMachineMismatchException();

            Modifiers.Add(Types.EventRequestModifier.InstanceFilter(objectReference.ObjectId));
        }

        public void AddThreadFilter(IThreadReference thread)
        {
            ThreadReference threadReference = thread as ThreadReference;
            if (threadReference == null || !threadReference.VirtualMachine.Equals(this.VirtualMachine))
                throw new VirtualMachineMismatchException();

            Modifiers.Add(Types.EventRequestModifier.ThreadFilter(threadReference.ThreadId));
        }
    }
}
