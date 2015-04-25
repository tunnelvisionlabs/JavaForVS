// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using IntPtr = System.IntPtr;

    internal struct jvmtiAddressLocationMap
    {
        public readonly IntPtr _startAddress;
        public readonly jlocation _location;
    }
}
