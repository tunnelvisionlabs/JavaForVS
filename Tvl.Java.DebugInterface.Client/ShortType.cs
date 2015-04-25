namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal sealed class ShortType : PrimitiveType, IShortType
    {
        internal ShortType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public override string GetName()
        {
            return "short";
        }

        public override string GetSignature()
        {
            return "S";
        }
    }
}
