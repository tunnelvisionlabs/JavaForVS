namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;

    internal abstract class PrimitiveType : JavaType, IPrimitiveType
    {
        protected PrimitiveType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }
    }
}
