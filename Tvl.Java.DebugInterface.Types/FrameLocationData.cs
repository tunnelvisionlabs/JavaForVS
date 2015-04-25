namespace Tvl.Java.DebugInterface.Types
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct FrameLocationData
    {
        [DataMember(IsRequired = true)]
        public FrameId FrameId;

        [DataMember(IsRequired = true)]
        public Location Location;

        public FrameLocationData(FrameId frameId, Location location)
        {
            FrameId = frameId;
            Location = location;
        }
    }
}
