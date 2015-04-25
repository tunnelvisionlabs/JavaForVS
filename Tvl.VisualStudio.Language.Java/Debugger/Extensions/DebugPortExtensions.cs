namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    public static class DebugPortExtensions
    {
        public static Guid GetPortId(this IDebugPort2 port)
        {
            Contract.Requires<ArgumentNullException>(port != null, "port");

            Guid id;
            ErrorHandler.ThrowOnFailure(port.GetPortId(out id));
            return id;
        }

        public static IDebugProcess2 GetProcess(this IDebugPort2 port, AD_PROCESS_ID processId)
        {
            Contract.Requires<ArgumentNullException>(port != null, "port");

            IDebugProcess2 process;
            ErrorHandler.ThrowOnFailure(port.GetProcess(processId, out process));
            return process;
        }
    }
}
