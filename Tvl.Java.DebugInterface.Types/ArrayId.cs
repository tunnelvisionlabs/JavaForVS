namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct ArrayId : IEquatable<ArrayId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public ArrayId(long handle)
        {
            Handle = handle;
        }

        public static implicit operator ObjectId(ArrayId array)
        {
            return new ObjectId(array.Handle);
        }

        public static explicit operator ArrayId(ObjectId @object)
        {
            return new ArrayId(@object.Handle);
        }

        public static explicit operator ArrayId(TaggedObjectId @object)
        {
            if (@object == default(TaggedObjectId))
                return default(ArrayId);

            if (@object.Tag != Tag.Array)
                throw new ArgumentException();

            return new ArrayId(@object.ObjectId.Handle);
        }

        public static bool operator ==(ArrayId x, ArrayId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(ArrayId x, ArrayId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(ArrayId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ArrayId))
                return false;

            return this.Handle == ((ArrayId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
