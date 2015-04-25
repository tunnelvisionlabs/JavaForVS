namespace Tvl.Java.DebugHost.Interop
{
    using System;

    public struct JavaVMHandle
    {
        public IntPtr Handle;

        public JavaVMHandle(IntPtr handle)
        {
            Handle = handle;
        }
    }
}
