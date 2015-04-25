namespace Tvl.Java.DebugHost.Services
{
    using System.Runtime.Serialization;
    using JavaVMHandle = Tvl.Java.DebugHost.Interop.JavaVMHandle;

    [DataContract]
    public struct JvmVirtualMachineRemoteHandle
    {
        [DataMember(IsRequired = true)]
        public long Handle;

        public JvmVirtualMachineRemoteHandle(JavaVMHandle handle)
        {
            Handle = handle.Handle.ToInt64();
        }
    }
}
