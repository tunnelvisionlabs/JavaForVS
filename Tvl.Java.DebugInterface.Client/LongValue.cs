namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class LongValue : PrimitiveValue, ILongValue
    {
        private readonly long _value;

        internal LongValue(VirtualMachine virtualMachine, long value)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            _value = value;
        }

        public long GetValue()
        {
            return _value;
        }

        public override IType GetValueType()
        {
            return VirtualMachine.PrimitiveTypes.Long;
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return (Types.Value)_value;
        }

        public int CompareTo(ILongValue other)
        {
            if (other == null)
                return 1;

            return _value.CompareTo(other.GetValue());
        }

        public bool Equals(ILongValue other)
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
            return this.Equals(obj as LongValue);
        }

        public override int GetHashCode()
        {
            return this.VirtualMachine.GetHashCode() ^ _value.GetHashCode();
        }
    }
}
