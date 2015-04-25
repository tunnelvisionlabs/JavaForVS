namespace Tvl.Java.DebugInterface
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A thread group object from the target VM. A IThreadGroupReference is an <see cref="IObjectReference"/>
    /// with additional access to threadgroup-specific information from the target VM.
    /// </summary>
    [ContractClass(typeof(Contracts.IThreadGroupReferenceContracts))]
    public interface IThreadGroupReference : IObjectReference
    {
        /// <summary>
        /// Returns the name of this thread group.
        /// </summary>
        string GetName();

        /// <summary>
        /// Returns the parent of this thread group.
        /// </summary>
        IThreadGroupReference GetParent();

        /// <summary>
        /// Resumes all threads in this thread group.
        /// </summary>
        void Resume();

        /// <summary>
        /// Suspends all threads in this thread group.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Returns a list containing each active <see cref="IThreadGroupReference"/> in this thread group.
        /// </summary>
        ReadOnlyCollection<IThreadGroupReference> GetThreadGroups();

        /// <summary>
        /// Returns a list containing a <see cref="IThreadReference"/> for each live thread in this thread group.
        /// </summary>
        ReadOnlyCollection<IThreadReference> GetThreads();
    }
}
