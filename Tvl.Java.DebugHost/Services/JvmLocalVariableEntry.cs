namespace Tvl.Java.DebugHost.Services
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct JvmLocalVariableEntry
    {
        [DataMember(IsRequired = true)]
        public JvmRemoteLocation StartLocation;

        [DataMember(IsRequired = true)]
        public int Length;

        [DataMember(IsRequired = true)]
        public string Name;

        [DataMember(IsRequired = true)]
        public string Signature;

        [DataMember(IsRequired = true)]
        public string GenericSignature;

        [DataMember(IsRequired = true)]
        public int Slot;
    }
}
