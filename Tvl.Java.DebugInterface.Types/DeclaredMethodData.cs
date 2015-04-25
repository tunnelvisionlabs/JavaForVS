namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct DeclaredMethodData
    {
        [DataMember(IsRequired = true)]
        public MethodId MethodId;

        [DataMember(IsRequired = true)]
        public string Name;

        [DataMember(IsRequired = true)]
        public string Signature;

        [DataMember(IsRequired = true)]
        public string GenericSignature;

        [DataMember(IsRequired = true)]
        public AccessModifiers Modifiers;

        public DeclaredMethodData(MethodId methodId, string name, string signature, string genericSignature, AccessModifiers modifiers)
        {
            MethodId = methodId;
            Name = name;
            Signature = signature;
            GenericSignature = genericSignature;
            Modifiers = modifiers;
        }
    }
}
