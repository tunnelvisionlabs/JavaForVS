namespace Tvl.Java.DebugHost.Services
{
    using System.Runtime.Serialization;
    using Tvl.Java.DebugHost.Interop;

    [DataContract]
    public struct JvmMethodRemoteHandle
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        internal JvmMethodRemoteHandle(jmethodID method)
        {
            Handle = method.Handle.ToInt64();
        }
    }
}
