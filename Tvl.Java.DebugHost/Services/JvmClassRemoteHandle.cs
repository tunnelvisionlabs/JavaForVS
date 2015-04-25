namespace Tvl.Java.DebugHost.Services
{
    using System.Runtime.Serialization;
    using Tvl.Java.DebugHost.Interop;
    using IntPtr = System.IntPtr;

    [DataContract]
    public struct JvmClassRemoteHandle
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public JvmClassRemoteHandle(jclass @class)
        {
            Handle = @class.Handle.ToInt64();
        }

        public static implicit operator jclass(JvmClassRemoteHandle @class)
        {
            return new jclass((IntPtr)@class.Handle);
        }
    }
}
