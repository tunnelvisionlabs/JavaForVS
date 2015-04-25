namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Types;

    internal abstract class JavaType : Mirror, IType
    {
        protected JavaType(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        public virtual string GetName()
        {
            return SignatureHelper.DecodeTypeName(GetSignature());
        }

        public abstract string GetSignature();
    }
}
