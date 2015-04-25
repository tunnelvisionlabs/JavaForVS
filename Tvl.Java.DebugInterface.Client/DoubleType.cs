namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class DoubleType : PrimitiveType, IDoubleType
    {
        internal DoubleType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public override string GetName()
        {
            return "double";
        }

        public override string GetSignature()
        {
            return "D";
        }
    }
}
