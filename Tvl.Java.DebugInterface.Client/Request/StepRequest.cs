namespace Tvl.Java.DebugInterface.Client.Request
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;
    using ThreadId = Tvl.Java.DebugInterface.Types.ThreadId;

    internal sealed class StepRequest : EventRequest, IStepRequest
    {
        private readonly ThreadReference _thread;
        private readonly StepSize _size;
        private readonly StepDepth _depth;

        public StepRequest(VirtualMachine virtualMachine, ThreadReference thread, StepSize size, StepDepth depth)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);

            _thread = thread;
            _size = size;
            _depth = depth;

            ThreadId threadId = default(ThreadId);
            if (thread != null)
                threadId = thread.ThreadId;

            Modifiers.Add(Types.EventRequestModifier.Step(threadId, (Types.StepSize)size, (Types.StepDepth)depth));
        }

        internal override Types.EventKind EventKind
        {
            get
            {
                return Types.EventKind.SingleStep;
            }
        }

        public StepDepth Depth
        {
            get
            {
                return _depth;
            }
        }

        public StepSize Size
        {
            get
            {
                return _size;
            }
        }

        public IThreadReference Thread
        {
            get
            {
                return _thread;
            }
        }

        public void AddClassFilter(IReferenceType referenceType)
        {
            ReferenceType type = referenceType as ReferenceType;
            if (type == null)
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
            if (objectReference == null)
                throw new VirtualMachineMismatchException();

            Modifiers.Add(Types.EventRequestModifier.InstanceFilter(objectReference.ObjectId));
        }
    }
}
