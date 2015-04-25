namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class VoidType : PrimitiveType, IVoidType
    {
        internal VoidType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public override string GetName()
        {
            return "void";
        }

        public override string GetSignature()
        {
            return "V";
        }
    }
}
