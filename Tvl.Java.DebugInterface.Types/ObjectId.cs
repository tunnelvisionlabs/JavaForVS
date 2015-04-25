namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct ObjectId : IEquatable<ObjectId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public ObjectId(long handle)
        {
            Handle = handle;
        }

        public static bool operator ==(ObjectId x, ObjectId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(ObjectId x, ObjectId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(ObjectId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ObjectId))
                return false;

            return this.Handle == ((ObjectId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
