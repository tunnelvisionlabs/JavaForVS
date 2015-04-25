namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class CharType : PrimitiveType, IByteType
    {
        internal CharType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public override string GetName()
        {
            return "char";
        }

        public override string GetSignature()
        {
            return "C";
        }
    }
}
