namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class EnumDebugCodeContexts : DebugEnumerator<IEnumDebugCodeContexts2, IDebugCodeContext2>, IEnumDebugCodeContexts2
    {
        public EnumDebugCodeContexts(IEnumerable<IDebugCodeContext2> codeContexts)
            : base(codeContexts)
        {
            Contract.Requires(codeContexts != null);
        }

        protected EnumDebugCodeContexts(IDebugCodeContext2[] elements, int currentIndex)
            : base(elements, currentIndex)
        {
        }

        protected override IEnumDebugCodeContexts2 CreateClone(IDebugCodeContext2[] elements, int currentIndex)
        {
            return new EnumDebugCodeContexts(elements, currentIndex);
        }
    }
}
