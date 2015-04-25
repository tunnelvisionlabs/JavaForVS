namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.ObjectModel;
    using Tvl.Java.DebugInterface.Connect;

    internal sealed class VirtualMachineManager : IVirtualMachineManager
    {
        #region IVirtualMachineManager Members

        public ReadOnlyCollection<IConnector> GetConnectors()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IAttachingConnector> GetAttachingConnectors()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IVirtualMachine> GetConnectedVirtualMachines()
        {
            throw new NotImplementedException();
        }

        public ILaunchingConnector GetDefaultConnector()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILaunchingConnector> GetLaunchingConnectors()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IListeningConnector> GetListeningConnectors()
        {
            throw new NotImplementedException();
        }

        public int GetMajorInterfaceVersion()
        {
            throw new NotImplementedException();
        }

        public int GetMinorInterfaceVersion()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
