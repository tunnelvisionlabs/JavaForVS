namespace Tvl.Java.DebugHost.Services
{
    using System;
    using System.ServiceModel;
    using Tvl.Java.DebugHost.Interop;

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class JvmDebugSessionService : IJvmDebugSessionService
    {
        public void Attach()
        {
            AgentExports.DebuggerAttachComplete.Set();
        }

        public void Detach()
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}
