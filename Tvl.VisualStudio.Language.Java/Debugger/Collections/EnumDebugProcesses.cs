namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class EnumDebugProcesses : DebugEnumerator<IEnumDebugProcesses2, IDebugProcess2>, IEnumDebugProcesses2
    {
        public EnumDebugProcesses(IEnumerable<IDebugProcess2> processes)
            : base(processes)
        {
            Contract.Requires(processes != null);
        }

        protected EnumDebugProcesses(IDebugProcess2[] elements, int currentIndex)
            : base(elements, currentIndex)
        {
        }

        protected override IEnumDebugProcesses2 CreateClone(IDebugProcess2[] elements, int currentIndex)
        {
            return new EnumDebugProcesses(elements, currentIndex);
        }
    }
}
