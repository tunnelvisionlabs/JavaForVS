namespace Tvl.Java.DebugHost.Services
{
    using System;
    using System.Runtime.Serialization;
    using Tvl.Java.DebugHost.Interop;

    [DataContract]
    public struct JvmObjectRemoteHandle
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public JvmObjectRemoteHandle(jobject @object)
        {
            Handle = @object.Handle.ToInt64();
        }

        public static implicit operator jobject(JvmObjectRemoteHandle handle)
        {
            return new jobject((IntPtr)handle.Handle);
        }
    }
}
