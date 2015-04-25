namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public struct RequestId : IEquatable<RequestId>
    {
        [DataMember(IsRequired = true)]
        public int Id;

        public RequestId(int id)
        {
            Id = id;
        }

        public static bool operator ==(RequestId x, RequestId y)
        {
            return x.Id == y.Id;
        }

        public static bool operator !=(RequestId x, RequestId y)
        {
            return x.Id != y.Id;
        }

        public bool Equals(RequestId other)
        {
            return this.Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RequestId))
                return false;

            return this.Id == ((RequestId)obj).Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
