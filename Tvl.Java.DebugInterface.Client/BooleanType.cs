namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class BooleanType : PrimitiveType, IBooleanType
    {
        internal BooleanType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public override string GetName()
        {
            return "boolean";
        }

        public override string GetSignature()
        {
            return "Z";
        }
    }
}
