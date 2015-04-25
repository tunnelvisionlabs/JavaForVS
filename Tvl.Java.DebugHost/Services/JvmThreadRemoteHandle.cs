namespace Tvl.Java.DebugHost.Services
{
    using System.Runtime.Serialization;
    using IntPtr = System.IntPtr;
    using jthread = Tvl.Java.DebugHost.Interop.jthread;

    [DataContract]
    public struct JvmThreadRemoteHandle
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public JvmThreadRemoteHandle(jthread thread)
        {
            Handle = thread.Handle.ToInt64();
        }

        public static implicit operator jthread(JvmThreadRemoteHandle thread)
        {
            return new jthread((IntPtr)thread.Handle);
        }
    }
}
