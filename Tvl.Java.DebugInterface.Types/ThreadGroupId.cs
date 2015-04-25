namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct ThreadGroupId : IEquatable<ThreadGroupId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public ThreadGroupId(long handle)
        {
            Handle = handle;
        }

        public static implicit operator ObjectId(ThreadGroupId threadGroup)
        {
            return new ObjectId(threadGroup.Handle);
        }

        public static explicit operator ThreadGroupId(ObjectId @object)
        {
            return new ThreadGroupId(@object.Handle);
        }

        public static explicit operator ThreadGroupId(TaggedObjectId @object)
        {
            if (@object == default(TaggedObjectId))
                return default(ThreadGroupId);

            if (@object.Tag != Tag.ThreadGroup)
                throw new ArgumentException();

            return new ThreadGroupId(@object.ObjectId.Handle);
        }

        public static bool operator ==(ThreadGroupId x, ThreadGroupId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(ThreadGroupId x, ThreadGroupId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(ThreadGroupId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ThreadGroupId))
                return false;

            return this.Handle == ((ThreadGroupId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
