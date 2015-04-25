namespace Tvl.Java.DebugInterface.Client.Request
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    internal sealed class MethodExitRequest : EventRequest, IMethodExitRequest
    {
        public MethodExitRequest(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        internal override Types.EventKind EventKind
        {
            get
            {
                return Types.EventKind.MethodExit;
            }
        }

        public void AddClassFilter(IReferenceType referenceType)
        {
            ReferenceType type = referenceType as ReferenceType;
            if (type == null || !type.VirtualMachine.Equals(this.VirtualMachine))
                throw new VirtualMachineMismatchException();

            Modifiers.Add(Types.EventRequestModifier.ClassTypeFilter(type.ReferenceTypeId));
        }

        public void AddClassExclusionFilter(string classPattern)
        {
            Modifiers.Add(Types.EventRequestModifier.ClassExcludeFilter(classPattern));
        }

        public void AddClassFilter(string classPattern)
        {
            Modifiers.Add(Types.EventRequestModifier.ClassMatchFilter(classPattern));
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
