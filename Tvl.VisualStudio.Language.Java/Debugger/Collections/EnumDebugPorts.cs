namespace Tvl.VisualStudio.Language.Java.Debugger.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class EnumDebugPorts : DebugEnumerator<IEnumDebugPorts2, IDebugPort2>, IEnumDebugPorts2
    {
        public EnumDebugPorts(IEnumerable<IDebugPort2> ports)
            : base(ports)
        {
            Contract.Requires(ports != null);
        }

        protected EnumDebugPorts(IDebugPort2[] elements, int currentIndex)
            : base(elements, currentIndex)
        {
        }

        protected override IEnumDebugPorts2 CreateClone(IDebugPort2[] elements, int currentIndex)
        {
            return new EnumDebugPorts(elements, currentIndex);
        }
    }
}
