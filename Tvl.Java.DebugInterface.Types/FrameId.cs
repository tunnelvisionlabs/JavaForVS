namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct FrameId : IEquatable<FrameId>
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public FrameId(long handle)
        {
            Handle = handle;
        }

        public static bool operator ==(FrameId x, FrameId y)
        {
            return x.Handle == y.Handle;
        }

        public static bool operator !=(FrameId x, FrameId y)
        {
            return x.Handle != y.Handle;
        }

        public bool Equals(FrameId other)
        {
            return this.Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FrameId))
                return false;

            return this.Handle == ((FrameId)obj).Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
