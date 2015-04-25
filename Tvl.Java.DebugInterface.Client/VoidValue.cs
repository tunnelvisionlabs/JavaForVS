namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class VoidValue : Value, IVoidValue
    {
        public VoidValue(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public override IType GetValueType()
        {
            return VirtualMachine.PrimitiveTypes.Void;
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return new Types.Value(Types.Tag.Void, 0);
        }

        public bool Equals(IVoidValue other)
        {
            if (other == null)
                return false;

            return this.VirtualMachine.Equals(other.GetVirtualMachine());
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as VoidValue);
        }

        public override int GetHashCode()
        {
            return VirtualMachine.GetHashCode();
        }
    }
}
