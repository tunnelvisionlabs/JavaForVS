namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal abstract class Value : Mirror, IValue
    {
        protected Value(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public static Types.Value ToNetworkValue(Value value)
        {
            if (value == null)
                return default(Types.Value);

            return value.ToNetworkValueImpl();
        }

        public abstract IType GetValueType();

        protected abstract Types.Value ToNetworkValueImpl();
    }
}
