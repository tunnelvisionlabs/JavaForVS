namespace Tvl.VisualStudio.Shell.OutputWindow.Interfaces
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a single pane in the <strong>Output</strong> window.
    /// </summary>
    /// <threadsafety/>
    [ContractClass(typeof(Contracts.IOutputWindowPaneContracts))]
    public interface IOutputWindowPane
    {
        /// <summary>
        /// Gets or sets the name of the window pane.
        /// </summary>
        /// <value>The name of the output window pane.</value>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <see langword="null"/>.</exception>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Shows and activates the output window pane.
        /// </summary>
        /// <remarks>
        /// Calling this method on an output window pane will not force the <strong>Output</strong>
        /// window itself to become visible if it is not visible already. If this output window pane
        /// was originally created as a hidden pane, calling this method makes it the selected and
        /// visible pane inside the <strong>Output</strong> window.
        /// </remarks>
        void Activate();

        /// <summary>
        /// Hides the output window pane.
        /// </summary>
        /// <remarks>
        /// This method makes the output window pane inaccessible.
        /// </remarks>
        void Hide();

        /// <summary>
        /// Writes text to the output window.
        /// </summary>
        /// <param name="text">The text to write to the output window.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="text"/> is <see langword="null"/>.</exception>
        void Write(string text);

        /// <summary>
        /// Writes a line text to the output window.
        /// </summary>
        /// <param name="text">The line text to write to the output window.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="text"/> is <see langword="null"/>.</exception>
        void WriteLine(string text);
    }
}
