namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class LongType : PrimitiveType, ILongType
    {
        internal LongType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public override string GetName()
        {
            return "long";
        }

        public override string GetSignature()
        {
            return "J";
        }
    }
}
