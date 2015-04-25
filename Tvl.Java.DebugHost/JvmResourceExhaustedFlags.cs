namespace Tvl.Java.DebugHost
{
    using System;

    [Flags]
    public enum JvmResourceExhaustedFlags
    {
        None = 0,

        /// <summary>
        /// After this event returns, the VM will throw a java.lang.OutOfMemoryError.
        /// </summary>
        OutOfMemory = 0x0001,

        /// <summary>
        /// The VM was unable to allocate memory from the JavaTM platform heap. The heap is the runtime
        /// data area from which memory for all class instances and arrays are allocated.
        /// </summary>
        JavaHeap = 0x0001,

        /// <summary>
        /// The VM was unable to create a thread.
        /// </summary>
        Threads = 0x0001,
    }
}
