namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class ByteType : PrimitiveType, IByteType
    {
        internal ByteType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public override string GetName()
        {
            return "byte";
        }

        public override string GetSignature()
        {
            return "B";
        }
    }
}
