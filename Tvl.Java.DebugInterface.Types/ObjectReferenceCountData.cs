namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct ObjectReferenceCountData
    {
        [DataMember(IsRequired = true)]
        public ObjectId Object;

        [DataMember(IsRequired = true)]
        public int ReferenceCount;
    }
}
