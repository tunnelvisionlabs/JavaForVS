namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class FloatType : PrimitiveType, IFloatType
    {
        internal FloatType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public override string GetName()
        {
            return "float";
        }

        public override string GetSignature()
        {
            return "F";
        }
    }
}
