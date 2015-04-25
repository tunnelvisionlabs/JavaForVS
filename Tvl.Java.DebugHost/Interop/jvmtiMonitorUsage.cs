// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using IntPtr = System.IntPtr;

    internal struct jvmtiMonitorUsage
    {
        public readonly jthread _owner;
        public readonly int _entryCount;
        public readonly int _waiterCount;
        public readonly IntPtr _waiters;
        public readonly int _notifyWaiterCount;
        public readonly IntPtr _notifyWaiters;
    }
}
