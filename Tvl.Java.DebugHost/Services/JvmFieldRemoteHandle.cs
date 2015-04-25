namespace Tvl.Java.DebugHost.Services
{
    using System.Runtime.Serialization;
    using Tvl.Java.DebugHost.Interop;

    [DataContract]
    public struct JvmFieldRemoteHandle
    {
        [DataMember(IsRequired = true)]
        public JvmClassRemoteHandle Class;

        [DataMember(IsRequired = true)]
        public long Handle;

        internal JvmFieldRemoteHandle(JvmClassRemoteHandle @class, jfieldID field)
        {
            Class = @class;
            Handle = field.Handle.ToInt64();
        }

    }
}
