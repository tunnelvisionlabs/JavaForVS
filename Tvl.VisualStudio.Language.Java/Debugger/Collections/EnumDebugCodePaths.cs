namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class EnumDebugCodePaths : DebugEnumerator<IEnumDebugCodePaths90, IDebugCodePath90>, IEnumDebugCodePaths90
    {
        public EnumDebugCodePaths(IEnumerable<IDebugCodePath90> codePaths)
            : base(codePaths)
        {
            Contract.Requires(codePaths != null);
        }

        protected EnumDebugCodePaths(IDebugCodePath90[] elements, int currentIndex)
            : base(elements, currentIndex)
        {
        }

        protected override IEnumDebugCodePaths90 CreateClone(IDebugCodePath90[] elements, int currentIndex)
        {
            return new EnumDebugCodePaths(elements, currentIndex);
        }
    }
}
