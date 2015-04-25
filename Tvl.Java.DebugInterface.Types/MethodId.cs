namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct MethodId : IEquatable<MethodId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public MethodId(long handle)
        {
            Handle = handle;
        }

        public MethodId(IntPtr handle)
        {
            Handle = handle.ToInt64();
        }

        public static bool operator ==(MethodId x, MethodId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(MethodId x, MethodId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(MethodId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MethodId))
                return false;

            return this.Handle == ((MethodId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
