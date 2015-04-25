namespace Tvl.Java.DebugInterface.Client.Request
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    internal sealed class ClassPrepareRequest : EventRequest, IClassPrepareRequest
    {
        public ClassPrepareRequest(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        internal override Types.EventKind EventKind
        {
            get
            {
                return Types.EventKind.ClassPrepare;
            }
        }

        public void AddSourceNameFilter(string sourceNamePattern)
        {
            Modifiers.Add(Types.EventRequestModifier.SourceNameMatch(sourceNamePattern));
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
    }
}
