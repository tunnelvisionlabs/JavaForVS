namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct InterfaceId : IEquatable<InterfaceId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public InterfaceId(long handle)
        {
            Handle = handle;
        }

        public static implicit operator ReferenceTypeId(InterfaceId @interface)
        {
            return new ReferenceTypeId(@interface.Handle);
        }

        public static explicit operator InterfaceId(ReferenceTypeId referenceType)
        {
            return new InterfaceId(referenceType.Handle);
        }

        public static explicit operator InterfaceId(TaggedReferenceTypeId @object)
        {
            if (@object == default(TaggedReferenceTypeId))
                return default(InterfaceId);

            if (@object.TypeTag != TypeTag.Interface)
                throw new ArgumentException();

            return new InterfaceId(@object.TypeId.Handle);
        }

        public static bool operator ==(InterfaceId x, InterfaceId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(InterfaceId x, InterfaceId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(InterfaceId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is InterfaceId))
                return false;

            return this.Handle == ((InterfaceId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
