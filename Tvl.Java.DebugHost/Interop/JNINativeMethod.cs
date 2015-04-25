// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using IntPtr = System.IntPtr;

    internal struct JNINativeMethod
    {
        public readonly IntPtr _name;
        public readonly IntPtr _signature;
        public readonly IntPtr _functionPointer;
    }
}
