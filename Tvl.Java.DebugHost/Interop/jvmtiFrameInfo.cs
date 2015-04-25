// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    internal struct jvmtiFrameInfo
    {
        public readonly jmethodID _method;
        public readonly jlocation _location;
    }
}
