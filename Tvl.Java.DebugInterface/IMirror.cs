namespace Tvl.Java.DebugInterface
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A proxy used by a debugger to examine or manipulate some entity in another virtual machine.
    /// </summary>
    /// <remarks>
    /// IMirror is the root of the interface hierarchy for this package. Mirrors can be proxies for objects in the
    /// target VM (<see cref="IObjectReference"/>), primitive values (for example, <see cref="IIntegerValue"/>),
    /// types (for example, <see cref="IReferenceType"/>), dynamic application state (for example,
    /// <see cref="IStackFrame"/>), and even debugger-specific constructs (for example,
    /// <see cref="IBreakpointRequest"/>). The <see cref="IVirtualMachine"/> itself is also considered a mirror,
    /// representing the composite state of the target VM.
    /// 
    /// There is no guarantee that a particular entity in the target VM will map to a single instance of IMirror.
    /// Implementors are free to decide whether a single mirror will be used for some or all mirrors. Clients of
    /// this interface should always use <see cref="Equals"/> to compare two mirrors for equality.
    /// 
    /// Any method on a IMirror that takes a IMirror as an parameter directly or indirectly (e.g., as a element
    /// in a List) will throw <see cref="VirtualMachineMismatchException"/> if the mirrors are from different
    /// virtual machines.
    /// </remarks>
    [ContractClass(typeof(Contracts.IMirrorContracts))]
    public interface IMirror
    {
        /// <summary>
        /// Gets the <see cref="IVirtualMachine"/> to which this mirror belongs.
        /// </summary>
        [Pure]
        IVirtualMachine GetVirtualMachine();

        /// <summary>
        /// Returns a string describing this mirror.
        /// </summary>
        [Pure]
        string ToString();
    }
}
