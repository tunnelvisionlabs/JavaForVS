namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct StringId : IEquatable<StringId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public StringId(long handle)
        {
            Handle = handle;
        }

        public static implicit operator ObjectId(StringId classObject)
        {
            return new ObjectId(classObject.Handle);
        }

        public static explicit operator StringId(ObjectId @object)
        {
            return new StringId(@object.Handle);
        }

        public static explicit operator StringId(TaggedObjectId @object)
        {
            if (@object == default(TaggedObjectId))
                return default(StringId);

            if (@object.Tag != Tag.String)
                throw new ArgumentException();

            return new StringId(@object.ObjectId.Handle);
        }

        public static bool operator ==(StringId x, StringId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(StringId x, StringId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(StringId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StringId))
                return false;

            return this.Handle == ((StringId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
