namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct TaggedObjectId : IEquatable<TaggedObjectId>
    {
        [DataMember(IsRequired = true)]
        public Tag Tag;

        [DataMember(IsRequired = true)]
        public ObjectId ObjectId;

        [DataMember(IsRequired = true)]
        public TaggedReferenceTypeId Type;

        public TaggedObjectId(Tag tag, ObjectId objectId, TaggedReferenceTypeId type)
        {
            Tag = tag;
            ObjectId = objectId;
            Type = type;
        }

        public static bool operator ==(TaggedObjectId x, TaggedObjectId y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(TaggedObjectId x, TaggedObjectId y)
        {
            return !x.Equals(y);
        }

        public bool Equals(TaggedObjectId other)
        {
            return this.Tag == other.Tag
                && this.ObjectId == other.ObjectId
                && this.Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TaggedObjectId))
                return false;

            return this.Equals((TaggedObjectId)obj);
        }

        public override int GetHashCode()
        {
            int hashCode = 1;
            hashCode = hashCode * 7 ^ Tag.GetHashCode();
            hashCode = hashCode * 7 ^ ObjectId.GetHashCode();
            hashCode = hashCode * 7 ^ Type.GetHashCode();
            return hashCode;
        }
    }
}
