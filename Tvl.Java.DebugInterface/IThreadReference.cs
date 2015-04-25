namespace Tvl.Java.DebugInterface
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A thread object from the target VM. A ThreadReference is an <see cref="IObjectReference"/>
    /// with additional access to thread-specific information from the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IThreadReferenceContracts))]
    public interface IThreadReference : IObjectReference
    {
        /// <summary>
        /// Returns an ObjectReference for the monitor, if any, for which this thread is currently waiting.
        /// </summary>
        IObjectReference GetCurrentContendedMonitor();

        /// <summary>
        /// Force a method to return before it reaches a return statement.
        /// </summary>
        void ForceEarlyReturn(IValue returnValue);

        /// <summary>
        /// Returns the <see cref="IStackFrame"/> at the given index in the thread's current call stack.
        /// </summary>
        IStackFrame GetFrame(int index);

        /// <summary>
        /// Returns the number of stack frames in the thread's current call stack.
        /// </summary>
        [Pure]
        int GetFrameCount();

        /// <summary>
        /// Returns a list containing each <see cref="IStackFrame"/> in the thread's current call stack.
        /// </summary>
        ReadOnlyCollection<IStackFrame> GetFrames();

        /// <summary>
        /// Returns a list containing a range of <see cref="IStackFrame"/> mirrors from the thread's current call stack.
        /// </summary>
        ReadOnlyCollection<IStackFrame> GetFrames(int start, int length);

        /// <summary>
        /// Interrupts this thread unless the thread has been suspended by the debugger.
        /// </summary>
        void Interrupt();

        /// <summary>
        /// Determines whether the thread is suspended at a breakpoint.
        /// </summary>
        bool GetIsAtBreakpoint();

        /// <summary>
        /// Determines whether the thread has been suspended by the the debugger.
        /// </summary>
        bool GetIsSuspended();

        /// <summary>
        /// Returns the name of this thread.
        /// </summary>
        string GetName();

        /// <summary>
        /// Returns a list containing an <see cref="IObjectReference"/> for each monitor owned by the thread.
        /// </summary>
        ReadOnlyCollection<IObjectReference> GetOwnedMonitors();

        /// <summary>
        /// Returns a list containing a <see cref="IMonitorInfo"/> object for each monitor owned by the thread.
        /// </summary>
        ReadOnlyCollection<IMonitorInfo> GetOwnedMonitorsAndFrames();

        /// <summary>
        /// Pop stack frames.
        /// </summary>
        void PopFrames(IStackFrame frame);

        /// <summary>
        /// Resumes this thread.
        /// </summary>
        void Resume();

        /// <summary>
        /// Returns the thread's status.
        /// </summary>
        ThreadStatus GetStatus();

        /// <summary>
        /// Stops this thread with an asynchronous exception.
        /// </summary>
        void Stop(IObjectReference throwable);

        /// <summary>
        /// Suspends this thread.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Returns the number of pending suspends for this thread.
        /// </summary>
        int GetSuspendCount();

        /// <summary>
        /// Returns this thread's thread group.
        /// </summary>
        IThreadGroupReference GetThreadGroup();
    }
}
