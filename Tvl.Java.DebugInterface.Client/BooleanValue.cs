namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class BooleanValue : PrimitiveValue, IBooleanValue
    {
        private readonly bool _value;

        internal BooleanValue(VirtualMachine virtualMachine, bool value)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            _value = value;
        }

        public bool GetValue()
        {
            return _value;
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return (Types.Value)_value;
        }

        public override IType GetValueType()
        {
            return VirtualMachine.PrimitiveTypes.Boolean;
        }

        public bool Equals(IBooleanValue other)
        {
            if (other == null)
                return false;

            if (_value != other.GetValue())
                return false;

            if (!other.GetVirtualMachine().Equals(this.VirtualMachine))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BooleanValue);
        }

        public override int GetHashCode()
        {
            return this.VirtualMachine.GetHashCode() ^ _value.GetHashCode();
        }
    }
}
