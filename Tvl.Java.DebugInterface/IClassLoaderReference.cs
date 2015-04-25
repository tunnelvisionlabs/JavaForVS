namespace Tvl.Java.DebugInterface
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A class loader object from the target VM. A <see cref="IClassLoaderReference"/> is an
    /// <see cref="IObjectReference"/> with additional access to classloader-specific information
    /// from the target VM. Instances of <see cref="IClassLoaderReference"/> are obtained through
    /// calls to <see cref="IReferenceType.GetClassLoader()"/>.
    /// </summary>
    [ContractClass(typeof(Contracts.IClassLoaderReferenceContracts))]
    public interface IClassLoaderReference : IObjectReference
    {
        /// <summary>
        /// Returns a list of all loaded classes that were defined by this class loader.
        /// </summary>
        ReadOnlyCollection<IReferenceType> GetDefinedClasses();

        /// <summary>
        /// Returns a list of all classes for which this class loader has been recorded as the
        /// initiating loader in the target VM.
        /// </summary>
        ReadOnlyCollection<IReferenceType> GetVisibleClasses();
    }
}
