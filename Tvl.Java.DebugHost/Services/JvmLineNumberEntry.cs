namespace Tvl.Java.DebugHost.Services
{
    using System.Runtime.Serialization;
    using Tvl.Java.DebugHost.Interop;

    [DataContract]
    public struct JvmLineNumberEntry
    {
        [DataMember(IsRequired = true)]
        public JvmRemoteLocation StartLocation;

        [DataMember(IsRequired = true)]
        public int LineNumber;

        internal JvmLineNumberEntry(JvmMethodRemoteHandle method, jvmtiLineNumberEntry entry)
        {
            StartLocation = new JvmRemoteLocation(method, entry.StartLocation);
            LineNumber = entry.LineNumber;
        }
    }
}
