namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class IntegerValue : PrimitiveValue, IIntegerValue
    {
        private readonly int _value;

        internal IntegerValue(VirtualMachine virtualMachine, int value)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            _value = value;
        }

        public int GetValue()
        {
            return _value;
        }

        public override IType GetValueType()
        {
            return VirtualMachine.PrimitiveTypes.Integer;
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return (Types.Value)_value;
        }

        public int CompareTo(IIntegerValue other)
        {
            if (other == null)
                return 1;

            return _value.CompareTo(other.GetValue());
        }

        public bool Equals(IIntegerValue other)
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
            return this.Equals(obj as IntegerValue);
        }

        public override int GetHashCode()
        {
            return this.VirtualMachine.GetHashCode() ^ _value.GetHashCode();
        }
    }
}
