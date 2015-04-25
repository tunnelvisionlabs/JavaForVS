namespace Tvl.Java.DebugInterface
{
    public enum ThreadStatus
    {
        /// <summary>
        /// Thread status is unknown.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Thread has completed execution.
        /// </summary>
        Zombie = 0,

        /// <summary>
        /// Thread is runnable.
        /// </summary>
        Running = 1,

        /// <summary>
        /// Thread is sleeping - Thread.sleep() or JVM_Sleep() was called.
        /// </summary>
        Sleeping = 2,

        /// <summary>
        /// Thread is waiting on a Java monitor.
        /// </summary>
        Monitor = 3,

        /// <summary>
        /// Thread is waiting - Object.wait() or JVM_MonitorWait() was called.
        /// </summary>
        Wait = 4,

        /// <summary>
        /// Thread has not yet been started.
        /// </summary>
        NotStarted = 5,
    }
}
