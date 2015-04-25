namespace Tvl.Java.DebugHost.Interop
{
    using System.Runtime.InteropServices;

    public static class JavaVMUnsafeNativeMethods
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DestroyJavaVM(JavaVMHandle vm);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int AttachCurrentThread(JavaVMHandle vm, out JNIEnvHandle penv, ref JavaVMAttachArgs args);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DetachCurrentThread(JavaVMHandle vm);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int GetEnv(JavaVMHandle vm, out jvmtiEnvHandle penv, jvmtiVersion version);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int AttachCurrentThreadAsDaemon(JavaVMHandle vm, out JNIEnvHandle penv, ref JavaVMAttachArgs args);
    }
}
