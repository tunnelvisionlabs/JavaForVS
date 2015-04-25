// The field 'field_name' is never used
#pragma warning disable 169

namespace Tvl.Java.DebugHost.Interop
{
    using System.Runtime.InteropServices;
    using IntPtr = System.IntPtr;

    [StructLayout(LayoutKind.Sequential)]
    public class JniInvokeInterface
    {
        private readonly IntPtr _reserved0;
        private readonly IntPtr _reserved1;
        private readonly IntPtr _reserved2;

        public readonly JavaVMUnsafeNativeMethods.DestroyJavaVM DestroyJavaVM;

        public readonly JavaVMUnsafeNativeMethods.AttachCurrentThread AttachCurrentThread;

        public readonly JavaVMUnsafeNativeMethods.DetachCurrentThread DetachCurrentThread;

        public readonly JavaVMUnsafeNativeMethods.GetEnv GetEnv;

        public readonly JavaVMUnsafeNativeMethods.AttachCurrentThreadAsDaemon AttachCurrentThreadAsDaemon;
    }
}
