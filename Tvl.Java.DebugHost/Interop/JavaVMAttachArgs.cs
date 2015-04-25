namespace Tvl.Java.DebugHost.Interop
{
    using System;

    public struct JavaVMAttachArgs
    {
        public readonly jniVersion Version;
        public readonly IntPtr Name;
        public readonly jthreadGroup ThreadGroup;

        public JavaVMAttachArgs(jniVersion version, IntPtr name, jthreadGroup threadGroup)
        {
            Version = version;
            Name = name;
            ThreadGroup = threadGroup;
        }
    }
}
