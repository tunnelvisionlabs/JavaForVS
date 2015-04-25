namespace Tvl.Java.DebugHost.Interop
{
    using System;

    [Flags]
    public enum jvmtiThreadState
    {
        None = 0,

        /// <summary>
        /// Thread is alive. Zero if thread is new (not started) or terminated.
        /// </summary>
        Alive = 0x0001,

        /// <summary>
        /// Thread has completed execution.
        /// </summary>
        Terminated = 0x0002,

        /// <summary>
        /// Thread is runnable.
        /// </summary>
        Runnable = 0x0004,

        /// <summary>
        /// Thread is waiting to enter a synchronization block/method or, after an Object.wait(), waiting to re-enter a synchronization block/method.
        /// </summary>
        BlockedOnMonitorEnter = 0x0400,

        /// <summary>
        /// Thread is waiting.
        /// </summary>
        Waiting = 0x0080,

        /// <summary>
        /// Thread is waiting without a timeout. For example, Object.wait().
        /// </summary>
        WaitingIndefinitely = 0x0010,

        /// <summary>
        /// Thread is waiting with a maximum time to wait specified. For example, Object.wait(long).
        /// </summary>
        WaitingWithTimeout = 0x0020,

        /// <summary>
        /// Thread is sleeping -- Thread.sleep(long).
        /// </summary>
        Sleeping = 0x0040,

        /// <summary>
        /// Thread is waiting on an object monitor -- Object.wait.
        /// </summary>
        InObjectWait = 0x0100,

        /// <summary>
        /// Thread is parked, for example: LockSupport.park, LockSupport.parkUtil and LockSupport.parkNanos.
        /// </summary>
        Parked = 0x0200,

        /// <summary>
        /// Thread suspended. java.lang.Thread.suspend() or a JVM TI suspend function (such as SuspendThread) has been called on the thread. If this bit is set, the other bits refer to the thread state before suspension.
        /// </summary>
        Suspended = 0x100000,

        /// <summary>
        /// Thread has been interrupted.
        /// </summary>
        Interrupted = 0x200000,

        /// <summary>
        /// Thread is in native code--that is, a native method is running which has not called back into the VM or Java programming language code.
        /// </summary>
        /// <remarks>
        /// This flag is not set when running VM compiled Java programming language code nor is it set when running VM code or VM support code. Native VM interface functions, such as JNI and JVM TI functions, may be implemented as VM code.
        /// </remarks>
        InNative = 0x400000,

        /// <summary>
        /// Defined by VM vendor.
        /// </summary>
        Vendor1 = 0x10000000,

        /// <summary>
        /// Defined by VM vendor.
        /// </summary>
        Vendor2 = 0x20000000,

        /// <summary>
        /// Defined by VM vendor.
        /// </summary>
        Vendor3 = 0x40000000,
    }
}
