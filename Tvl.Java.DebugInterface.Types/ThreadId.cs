namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct ThreadId : IEquatable<ThreadId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public ThreadId(long handle)
        {
            Handle = handle;
        }

        public static implicit operator ObjectId(ThreadId thread)
        {
            return new ObjectId(thread.Handle);
        }

        public static explicit operator ThreadId(ObjectId @object)
        {
            return new ThreadId(@object.Handle);
        }

        public static explicit operator ThreadId(TaggedObjectId @object)
        {
            if (@object == default(TaggedObjectId))
                return default(ThreadId);

            if (@object.Tag != Tag.Thread)
                throw new ArgumentException();

            return new ThreadId(@object.ObjectId.Handle);
        }

        public static bool operator ==(ThreadId x, ThreadId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(ThreadId x, ThreadId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(ThreadId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ThreadId))
                return false;

            return this.Handle == ((ThreadId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Thread #{0}", Handle);
        }
    }
}
