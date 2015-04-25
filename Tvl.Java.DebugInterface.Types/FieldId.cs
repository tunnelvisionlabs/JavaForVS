namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct FieldId : IEquatable<FieldId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public FieldId(long handle)
        {
            Handle = handle;
        }

        public FieldId(IntPtr handle)
        {
            Handle = handle.ToInt64();
        }

        public static bool operator ==(FieldId x, FieldId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(FieldId x, FieldId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(FieldId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FieldId))
                return false;

            return this.Handle == ((FieldId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
