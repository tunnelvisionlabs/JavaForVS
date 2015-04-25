namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class FloatValue : PrimitiveValue, IFloatValue
    {
        private readonly float _value;

        internal FloatValue(VirtualMachine virtualMachine, float value)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            _value = value;
        }

        public float GetValue()
        {
            return _value;
        }

        protected override Types.Value ToNetworkValueImpl()
        {
            return (Types.Value)_value;
        }

        public override IType GetValueType()
        {
            return VirtualMachine.PrimitiveTypes.Float;
        }

        public int CompareTo(IFloatValue other)
        {
            if (other == null)
                return 1;

            return _value.CompareTo(other.GetValue());
        }

        public bool Equals(IFloatValue other)
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
            return this.Equals(obj as FloatValue);
        }

        public override int GetHashCode()
        {
            return this.VirtualMachine.GetHashCode() ^ _value.GetHashCode();
        }
    }
}
