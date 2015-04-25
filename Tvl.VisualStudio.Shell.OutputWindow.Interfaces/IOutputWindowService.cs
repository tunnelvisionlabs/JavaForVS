namespace Tvl.VisualStudio.Shell.OutputWindow.Interfaces
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// This MEF service provides access to named panes in the <strong>Output</strong> window.
    /// </summary>
    [ContractClass(typeof(Contracts.IOutputWindowServiceContracts))]
    public interface IOutputWindowService
    {
        /// <summary>
        /// Gets the output window pane with the specified name.
        /// </summary>
        /// <param name="name">The name of the output window pane.</param>
        /// <returns>
        /// The <see cref="IOutputWindowPane"/> instance providing access to the
        /// window pane, or <see langword="null"/> if no pane exists with the
        /// specified name.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name"/> is empty.</exception>
        IOutputWindowPane TryGetPane(string name);
    }
}
