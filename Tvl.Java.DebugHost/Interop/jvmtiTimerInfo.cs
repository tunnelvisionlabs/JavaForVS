// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using jboolean = System.Byte;

    internal struct jvmtiTimerInfo
    {
        public readonly long _maxValue;
        public readonly jboolean _maySkipForward;
        public readonly jboolean _maySkipBackward;
        public readonly jvmtiTimerKind _kind;
        public readonly long _reserved1;
        public readonly long _reserved2;
    }
}
