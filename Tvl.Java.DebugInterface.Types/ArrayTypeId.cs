namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct ArrayTypeId : IEquatable<ArrayTypeId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public ArrayTypeId(long handle)
        {
            Handle = handle;
        }

        public static implicit operator ReferenceTypeId(ArrayTypeId @array)
        {
            return new ReferenceTypeId(@array.Handle);
        }

        public static explicit operator ArrayTypeId(ReferenceTypeId referenceType)
        {
            return new ArrayTypeId(referenceType.Handle);
        }

        public static explicit operator ArrayTypeId(TaggedReferenceTypeId @object)
        {
            if (@object == default(TaggedReferenceTypeId))
                return default(ArrayTypeId);

            if (@object.TypeTag != TypeTag.Array)
                throw new ArgumentException();

            return new ArrayTypeId(@object.TypeId.Handle);
        }

        public static bool operator ==(ArrayTypeId x, ArrayTypeId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(ArrayTypeId x, ArrayTypeId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(ArrayTypeId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ArrayTypeId))
                return false;

            return this.Handle == ((ArrayTypeId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
