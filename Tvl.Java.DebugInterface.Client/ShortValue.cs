namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class ShortValue : PrimitiveValue, IShortValue
    {
        private readonly short _value;

        internal ShortValue(VirtualMachine virtualMachine, short value)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            _value = value;
        }

        public short GetValue()
        {
            return _value;
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return (Types.Value)_value;
        }

        public override IType GetValueType()
        {
            return VirtualMachine.PrimitiveTypes.Short;
        }

        public int CompareTo(IShortValue other)
        {
            if (other == null)
                return 1;

            return _value.CompareTo(other.GetValue());
        }

        public bool Equals(IShortValue other)
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
            return this.Equals(obj as ShortValue);
        }

        public override int GetHashCode()
        {
            return this.VirtualMachine.GetHashCode() ^ _value.GetHashCode();
        }
    }
}
