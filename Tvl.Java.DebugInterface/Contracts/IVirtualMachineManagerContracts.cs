namespace Tvl.Java.DebugInterface.Contracts
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Connect;

    [ContractClassFor(typeof(IVirtualMachineManager))]
    internal abstract class IVirtualMachineManagerContracts : IVirtualMachineManager
    {
        #region IVirtualMachineManager Members

        public ReadOnlyCollection<IConnector> GetConnectors()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IConnector>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IConnector>>(), connector => connector != null));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IAttachingConnector> GetAttachingConnectors()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IAttachingConnector>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IAttachingConnector>>(), connector => connector != null));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IVirtualMachine> GetConnectedVirtualMachines()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IVirtualMachine>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IVirtualMachine>>(), vm => vm != null));
#endif

            throw new NotImplementedException();
        }

        public ILaunchingConnector GetDefaultConnector()
        {
            Contract.Ensures(Contract.Result<ILaunchingConnector>() != null);

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ILaunchingConnector> GetLaunchingConnectors()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<ILaunchingConnector>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<ILaunchingConnector>>(), connector => connector != null));
#endif

            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IListeningConnector> GetListeningConnectors()
        {
            Contract.Ensures(Contract.Result<ReadOnlyCollection<IListeningConnector>>() != null);
#if CONTRACTS_FORALL
            Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IListeningConnector>>(), connector => connector != null));
#endif

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
