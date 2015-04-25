namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Types;

    internal class StringReference : ObjectReference, IStringReference
    {
        internal StringReference(VirtualMachine virtualMachine, StringId stringId)
            : base(virtualMachine, stringId, null)
        {
            Contract.Requires(virtualMachine != null);
        }

        internal StringReference(VirtualMachine virtualMachine, StringId stringId, IReferenceType stringType)
            : base(virtualMachine, stringId, stringType)
        {
            Contract.Requires(virtualMachine != null);
        }

        public StringId StringId
        {
            get
            {
                return (StringId)base.ObjectId;
            }
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return new Types.Value(Tag.String, ObjectId.Handle);
        }

        public string GetValue()
        {
            string value;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetStringValue(out value, StringId));
            return value;
        }
    }
}
