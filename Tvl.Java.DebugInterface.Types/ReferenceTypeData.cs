namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct ReferenceTypeData
    {
        [DataMember(IsRequired = true)]
        public TypeTag ReferenceTypeTag;

        [DataMember(IsRequired = true)]
        public ReferenceTypeId TypeId;

        [DataMember(IsRequired = true)]
        public string Signature;

        [DataMember(IsRequired = true)]
        public string GenericSignature;

        [DataMember(IsRequired = true)]
        public ClassStatus Status;

        public ReferenceTypeData(TaggedReferenceTypeId type, string signature, string genericSignature, ClassStatus status)
        {
            ReferenceTypeTag = type.TypeTag;
            TypeId = type.TypeId;
            Signature = signature;
            GenericSignature = genericSignature;
            Status = status;
        }
    }
}
