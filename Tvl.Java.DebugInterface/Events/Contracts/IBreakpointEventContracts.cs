namespace Tvl.Java.DebugInterface.Events.Contracts
{
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IBreakpointEvent))]
    internal abstract class IBreakpointEventContracts : IBreakpointEvent
    {
        #region IThreadEvent Members

        public IThreadReference GetThread()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IEvent Members

        public Request.IEventRequest GetRequest()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IMirror Members

        public IVirtualMachine GetVirtualMachine()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region ILocatable Members

        public ILocation GetLocation()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
