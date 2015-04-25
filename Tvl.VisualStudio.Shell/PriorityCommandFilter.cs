namespace Tvl.VisualStudio.Shell
{
    using System;
    using Microsoft.VisualStudio.Shell.Interop;
    using IOleCommandTarget = Microsoft.VisualStudio.OLE.Interop.IOleCommandTarget;

    public abstract class PriorityCommandFilter : CommandFilter
    {
        private uint _cookie;

        protected PriorityCommandFilter(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        protected IServiceProvider ServiceProvider
        {
            get;
            private set;
        }

        protected override IOleCommandTarget Connect()
        {
            IVsRegisterPriorityCommandTarget registerPct = (IVsRegisterPriorityCommandTarget)ServiceProvider.GetService(typeof(SVsRegisterPriorityCommandTarget));
            registerPct.RegisterPriorityCommandTarget(0, this, out _cookie);
            return null;
        }

        protected override void Disconnect()
        {
            try
            {
                if (_cookie != 0)
                {
                    IVsRegisterPriorityCommandTarget registerPct = (IVsRegisterPriorityCommandTarget)ServiceProvider.GetService(typeof(SVsRegisterPriorityCommandTarget));
                    registerPct.UnregisterPriorityCommandTarget(_cookie);
                }
            }
            finally
            {
                _cookie = 0;
            }
        }
    }
}
