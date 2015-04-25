namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class EnumDebugModules : DebugEnumerator<IEnumDebugModules2, IDebugModule2>, IEnumDebugModules2
    {
        public EnumDebugModules(IEnumerable<IDebugModule2> modules)
            : base(modules)
        {
            Contract.Requires(modules != null);
        }

        protected EnumDebugModules(IDebugModule2[] elements, int currentIndex)
            : base(elements, currentIndex)
        {
        }

        protected override IEnumDebugModules2 CreateClone(IDebugModule2[] elements, int currentIndex)
        {
            return new EnumDebugModules(elements, currentIndex);
        }
    }
}
