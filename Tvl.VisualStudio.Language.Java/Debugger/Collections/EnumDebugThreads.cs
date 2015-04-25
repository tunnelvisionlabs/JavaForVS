namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class EnumDebugThreads : DebugEnumerator<IEnumDebugThreads2, IDebugThread2>, IEnumDebugThreads2
    {
        public EnumDebugThreads(IEnumerable<IDebugThread2> threads)
            : base(threads)
        {
            Contract.Requires(threads != null);
        }

        protected EnumDebugThreads(IDebugThread2[] elements, int currentIndex)
            : base(elements, currentIndex)
        {
        }

        protected override IEnumDebugThreads2 CreateClone(IDebugThread2[] elements, int currentIndex)
        {
            return new EnumDebugThreads(elements, currentIndex);
        }
    }
}
