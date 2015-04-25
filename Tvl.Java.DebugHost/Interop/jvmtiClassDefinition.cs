// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using IntPtr = System.IntPtr;

    internal struct jvmtiClassDefinition
    {
        public readonly jclass _class;
        public readonly int _classByteCount;
        public readonly IntPtr _classBytes;
    }
}
