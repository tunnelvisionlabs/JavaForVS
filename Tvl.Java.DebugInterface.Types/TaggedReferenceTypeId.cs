namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct TaggedReferenceTypeId : IEquatable<TaggedReferenceTypeId>
    {
        [DataMember(IsRequired = true)]
        public TypeTag TypeTag;

        [DataMember(IsRequired = true)]
        public ReferenceTypeId TypeId;

        public TaggedReferenceTypeId(TypeTag typeTag, ReferenceTypeId typeId)
        {
            TypeTag = typeTag;
            TypeId = typeId;
        }

        public static bool operator ==(TaggedReferenceTypeId x, TaggedReferenceTypeId y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(TaggedReferenceTypeId x, TaggedReferenceTypeId y)
        {
            return !x.Equals(y);
        }

        public bool Equals(TaggedReferenceTypeId other)
        {
            return this.TypeTag == other.TypeTag
                && this.TypeId == other.TypeId;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TaggedReferenceTypeId))
                return false;

            return this.Equals((TaggedReferenceTypeId)obj);
        }

        public override int GetHashCode()
        {
            return TypeTag.GetHashCode() ^ TypeId.GetHashCode();
        }
    }
}
