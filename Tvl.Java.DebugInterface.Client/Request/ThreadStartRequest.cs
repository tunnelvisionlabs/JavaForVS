namespace Tvl.Java.DebugInterface.Client.Request
{
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Request;

    internal sealed class ThreadStartRequest : EventRequest, IThreadStartRequest
    {
        public ThreadStartRequest(VirtualMachine virtualMachine)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
        }

        internal override Types.EventKind EventKind
        {
            get
            {
                return Types.EventKind.ThreadStart;
            }
        }

        public void AddThreadFilter(IThreadReference thread)
        {
            ThreadReference threadReference = thread as ThreadReference;
            if (threadReference == null || !threadReference.VirtualMachine.Equals(this.VirtualMachine))
                throw new VirtualMachineMismatchException();

            Modifiers.Add(Types.EventRequestModifier.ThreadFilter(threadReference.ThreadId));
        }
    }
}
