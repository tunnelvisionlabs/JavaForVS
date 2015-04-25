namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct ClassLoaderId : IEquatable<ClassLoaderId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public ClassLoaderId(long handle)
        {
            Handle = handle;
        }

        public static implicit operator ObjectId(ClassLoaderId classLoader)
        {
            return new ObjectId(classLoader.Handle);
        }

        public static explicit operator ClassLoaderId(ObjectId @object)
        {
            return new ClassLoaderId(@object.Handle);
        }

        public static explicit operator ClassLoaderId(TaggedObjectId @object)
        {
            if (@object == default(TaggedObjectId))
                return default(ClassLoaderId);

            if (@object.Tag != Tag.ClassLoader)
                throw new ArgumentException();

            return new ClassLoaderId(@object.ObjectId.Handle);
        }

        public static bool operator ==(ClassLoaderId x, ClassLoaderId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(ClassLoaderId x, ClassLoaderId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(ClassLoaderId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ClassLoaderId))
                return false;

            return this.Handle == ((ClassLoaderId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
