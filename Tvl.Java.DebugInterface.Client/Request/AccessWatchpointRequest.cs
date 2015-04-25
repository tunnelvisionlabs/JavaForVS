namespace Tvl.Java.DebugInterface.Client.Request
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    internal sealed class AccessWatchpointRequest : WatchpointRequest, IAccessWatchpointRequest
    {
        public AccessWatchpointRequest(VirtualMachine virtualMachine, Field field)
            : base(virtualMachine, field)
        {
            Contract.Requires(virtualMachine != null);
            Contract.Requires(field != null);
        }

        internal override Types.EventKind EventKind
        {
            get
            {
                return Types.EventKind.FieldAccess;
            }
        }
    }
}
