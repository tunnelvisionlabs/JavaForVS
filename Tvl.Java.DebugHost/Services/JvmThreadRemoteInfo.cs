namespace Tvl.Java.DebugHost.Services
{
    using System.Runtime.Serialization;

    [DataContract]
    public struct JvmThreadRemoteInfo
    {
        [DataMember]
        public string Name;

        [DataMember]
        public int Priority;

        [DataMember]
        public bool IsDaemon;

        [DataMember]
        public JvmThreadGroupRemoteHandle ThreadGroup;

        [DataMember]
        public JvmObjectRemoteHandle ContextClassLoader;
    }
}
