// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    internal struct jvmtiLineNumberEntry
    {
        public readonly jlocation StartLocation;

        public readonly int LineNumber;
    }
}
