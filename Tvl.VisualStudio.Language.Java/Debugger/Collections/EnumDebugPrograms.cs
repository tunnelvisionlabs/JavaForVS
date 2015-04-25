namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class EnumDebugPrograms : DebugEnumerator<IEnumDebugPrograms2, IDebugProgram2>, IEnumDebugPrograms2
    {
        public EnumDebugPrograms(IEnumerable<IDebugProgram2> programs)
            : base(programs)
        {
            Contract.Requires(programs != null);
        }

        protected EnumDebugPrograms(IDebugProgram2[] elements, int currentIndex)
            : base(elements, currentIndex)
        {
        }

        protected override IEnumDebugPrograms2 CreateClone(IDebugProgram2[] elements, int currentIndex)
        {
            return new EnumDebugPrograms(elements, currentIndex);
        }
    }
}
