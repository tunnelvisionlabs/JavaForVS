namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct DeclaredFieldData
    {
        [DataMember(IsRequired = true)]
        public FieldId FieldId;

        [DataMember(IsRequired = true)]
        public string Name;

        [DataMember(IsRequired = true)]
        public string Signature;

        [DataMember(IsRequired = true)]
        public string GenericSignature;

        [DataMember(IsRequired = true)]
        public AccessModifiers Modifiers;

        public DeclaredFieldData(FieldId fieldId, string name, string signature, string genericSignature, AccessModifiers modifiers)
        {
            FieldId = fieldId;
            Name = name;
            Signature = signature;
            GenericSignature = genericSignature;
            Modifiers = modifiers;
        }
    }
}
