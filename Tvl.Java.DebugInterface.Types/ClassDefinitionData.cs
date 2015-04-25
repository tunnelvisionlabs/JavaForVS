namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct ClassDefinitionData
    {
        [DataMember(IsRequired = true)]
        public ReferenceTypeId ReferenceType;

        [DataMember(IsRequired = true)]
        public byte[] ClassFileData;

        public ClassDefinitionData(ReferenceTypeId referenceType, byte[] classFileData)
        {
            ReferenceType = referenceType;
            ClassFileData = classFileData;
        }
    }
}
