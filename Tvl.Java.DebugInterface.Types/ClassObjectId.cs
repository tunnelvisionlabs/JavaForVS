namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct ClassObjectId : IEquatable<ClassObjectId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public ClassObjectId(long handle)
        {
            Handle = handle;
        }

        public static implicit operator ObjectId(ClassObjectId classObject)
        {
            return new ObjectId(classObject.Handle);
        }

        public static explicit operator ClassObjectId(ObjectId @object)
        {
            return new ClassObjectId(@object.Handle);
        }

        public static explicit operator ClassObjectId(TaggedObjectId @object)
        {
            if (@object == default(TaggedObjectId))
                return default(ClassObjectId);

            if (@object.Tag != Tag.ClassObject)
                throw new ArgumentException();

            return new ClassObjectId(@object.ObjectId.Handle);
        }

        public static bool operator ==(ClassObjectId x, ClassObjectId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(ClassObjectId x, ClassObjectId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(ClassObjectId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ClassObjectId))
                return false;

            return this.Handle == ((ClassObjectId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
