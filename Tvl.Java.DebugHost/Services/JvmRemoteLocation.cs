namespace Tvl.Java.DebugHost.Services
{
    using System.Runtime.Serialization;
    using Tvl.Java.DebugHost.Interop;

    [DataContract]
    public struct JvmRemoteLocation
    {
        [DataMember]
        public JvmMethodRemoteHandle Method;

        [DataMember]
        public jlocation Location;

        public JvmRemoteLocation(JvmMethodRemoteHandle method, jlocation location)
        {
            Method = method;
            Location = location;
        }
    }
}
