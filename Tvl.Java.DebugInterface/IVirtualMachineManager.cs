namespace Tvl.Java.DebugInterface
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface.Connect;

    /// <summary>
    /// A manager of connections to target virtual machines.
    /// </summary>
    [ContractClass(typeof(Contracts.IVirtualMachineManagerContracts))]
    public interface IVirtualMachineManager
    {
        /// <summary>
        /// Returns the list of all known <see cref="IConnector"/> objects.
        /// </summary>
        ReadOnlyCollection<IConnector> GetConnectors();

        /// <summary>
        /// Returns the list of known <see cref="IAttachingConnector"/> objects.
        /// </summary>
        ReadOnlyCollection<IAttachingConnector> GetAttachingConnectors();

        /// <summary>
        /// Lists all target VMs which are connected to the debugger.
        /// </summary>
        ReadOnlyCollection<IVirtualMachine> GetConnectedVirtualMachines();

#if false
        /// <summary>
        /// Creates a new virtual machine.
        /// </summary>
        IVirtualMachine CreateVirtualMachine(Tvl.Java.DebugInterface.Connect.Spi.Connection connection);

        /// <summary>
        /// Create a virtual machine mirror for a target VM.
        /// </summary>
        IVirtualMachine CreateVirtualMachine(Tvl.Java.DebugInterface.Connect.Spi.Connection connection, System.Diagnostics.Process process);
#endif

        /// <summary>
        /// Identifies the default connector.
        /// </summary>
        ILaunchingConnector GetDefaultConnector();

        /// <summary>
        /// Returns the list of known <see cref="ILaunchingConnector"/> objects.
        /// </summary>
        ReadOnlyCollection<ILaunchingConnector> GetLaunchingConnectors();

        /// <summary>
        /// Returns the list of known <see cref="IListeningConnector"/> objects.
        /// </summary>
        ReadOnlyCollection<IListeningConnector> GetListeningConnectors();

        /// <summary>
        /// Returns the major version number of the JDI interface.
        /// </summary>
        int GetMajorInterfaceVersion();

        /// <summary>
        /// Returns the minor version number of the JDI interface.
        /// </summary>
        int GetMinorInterfaceVersion();
    }
}
