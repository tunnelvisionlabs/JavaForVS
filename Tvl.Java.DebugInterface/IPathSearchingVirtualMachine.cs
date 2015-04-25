namespace Tvl.Java.DebugInterface
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A virtual machine which searches for classes through paths.
    /// </summary>
    [ContractClass(typeof(Contracts.IPathSearchingVirtualMachineContracts))]
    public interface IPathSearchingVirtualMachine : IVirtualMachine
    {
        /// <summary>
        /// Get the base directory used for path searching.
        /// </summary>
        string GetBaseDirectory();

        /// <summary>
        /// Get the boot class path for this virtual machine.
        /// </summary>
        ReadOnlyCollection<string> GetBootClassPath();

        /// <summary>
        /// Get the class path for this virtual machine.
        /// </summary>
        ReadOnlyCollection<string> GetClassPath();
    }
}
