namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    public static class DebugThreadExtensions
    {
        public static string GetName(this IDebugThread2 thread)
        {
            Contract.Requires<ArgumentNullException>(thread != null, "thread");

            string name;
            ErrorHandler.ThrowOnFailure(thread.GetName(out name));
            return name;
        }

        public static IDebugProgram2 GetProgram(this IDebugThread2 thread)
        {
            Contract.Requires<ArgumentNullException>(thread != null, "thread");

            IDebugProgram2 program;
            ErrorHandler.ThrowOnFailure(thread.GetProgram(out program));
            return program;
        }

        public static uint GetThreadId(this IDebugThread2 thread)
        {
            Contract.Requires<ArgumentNullException>(thread != null, "thread");

            uint threadId;
            ErrorHandler.ThrowOnFailure(thread.GetThreadId(out threadId));
            return threadId;
        }

        public static uint Resume(this IDebugThread2 thread)
        {
            Contract.Requires<ArgumentNullException>(thread != null, "thread");

            uint suspendCount;
            ErrorHandler.ThrowOnFailure(thread.Resume(out suspendCount));
            return suspendCount;
        }

        public static uint Suspend(this IDebugThread2 thread)
        {
            Contract.Requires<ArgumentNullException>(thread != null, "thread");

            uint suspendCount;
            ErrorHandler.ThrowOnFailure(thread.Suspend(out suspendCount));
            return suspendCount;
        }

        public static IEnumerable<FRAMEINFO> EnumFrameInfo(enum_FRAMEINFO_FLAGS fieldSpec, uint radix)
        {
            throw new NotImplementedException();
        }
    }
}
