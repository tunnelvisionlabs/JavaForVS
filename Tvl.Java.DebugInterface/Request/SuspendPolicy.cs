namespace Tvl.Java.DebugInterface.Request
{
    public enum SuspendPolicy
    {
        /// <summary>
        /// Suspend no threads when the event occurs.
        /// </summary>
        None = 0,

        /// <summary>
        /// Suspend only the thread which generated the event when the event occurs.
        /// </summary>
        EventThread = 1,

        /// <summary>
        /// Suspend all threads when the event occurs.
        /// </summary>
        All = 2,
    }
}
