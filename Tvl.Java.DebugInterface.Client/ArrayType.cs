namespace Tvl.Java.DebugInterface.Client
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Types;

    internal sealed class ArrayType : ReferenceType, IArrayType
    {
        internal ArrayType(VirtualMachine virtualMachine, ArrayTypeId typeId)
            : base(virtualMachine, new TaggedReferenceTypeId(TypeTag.Array, typeId))
        {
            Contract.Requires(virtualMachine != null);
        }

        public string GetComponentSignature()
        {
            return GetSignature().Substring(1);
        }

        public IType GetComponentType()
        {
            return VirtualMachine.FindType(GetComponentSignature());
        }

        public string GetComponentTypeName()
        {
            return SignatureHelper.DecodeTypeName(GetSignature().Substring(1));
        }

        public IStrongValueHandle<IArrayReference> CreateInstance(int length)
        {
            TaggedObjectId newArray;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.CreateArrayInstance(out newArray, (ArrayTypeId)ReferenceTypeId, length));
            return new StrongValueHandle<ArrayReference>((ArrayReference)VirtualMachine.GetMirrorOf(newArray));
        }
    }
}
