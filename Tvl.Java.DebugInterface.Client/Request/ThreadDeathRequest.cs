namespace Tvl.Java.DebugInterface.Client.Request
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    internal sealed class ThreadDeathRequest : EventRequest, IThreadDeathRequest
    {
        public ThreadDeathRequest(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        internal override Types.EventKind EventKind
        {
            get
            {
                return Types.EventKind.ThreadDeath;
            }
        }

        public void AddThreadFilter(IThreadReference thread)
        {
            ThreadReference threadReference = thread as ThreadReference;
            if (threadReference == null)
                throw new VirtualMachineMismatchException();

            Modifiers.Add(Types.EventRequestModifier.ThreadFilter(threadReference.ThreadId));
        }
    }
}
