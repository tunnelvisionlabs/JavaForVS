namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class IntegerType : PrimitiveType, IByteType
    {
        internal IntegerType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public override string GetName()
        {
            return "int";
        }

        public override string GetSignature()
        {
            return "I";
        }
    }
}
