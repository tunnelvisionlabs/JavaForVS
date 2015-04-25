namespace Tvl.Java.DebugHost.Services
{
    using System.Runtime.Serialization;
    using Tvl.Java.DebugHost.Interop;

    [DataContract]
    public struct JvmThreadGroupRemoteHandle
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public JvmThreadGroupRemoteHandle(jthreadGroup group)
        {
            Handle = group.Handle.ToInt64();
        }
    }
}
