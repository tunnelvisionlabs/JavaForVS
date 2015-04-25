namespace Tvl.VisualStudio.Shell.OutputWindow.Interfaces
{
    /// <summary>
    /// This class defines constants for the names of some of the predefined output window panes.
    /// </summary>
    public static class PredefinedOutputWindowPanes
    {
        /// <summary>
        /// Gets the name of the <strong>General</strong> output window pane.
        /// </summary>
        public static readonly string General = "General";

        /// <summary>
        /// Gets the name of the <strong>Debug</strong> output window pane.
        /// </summary>
        public static readonly string Debug = "Debug";

        /// <summary>
        /// Gets the name of the <strong>Build</strong> output window pane.
        /// </summary>
        public static readonly string Build = "Build";

        /// <summary>
        /// Gets the name of the <strong>TVL IntelliSense Engine</strong> output window pane.
        /// </summary>
        /// <remarks>
        /// This output window pane is used for shared output from several language services
        /// created by Tunnel Vision Laboratories, LLC.
        /// </remarks>
        public const string TvlIntellisense = "TVL IntelliSense Engine";

        /// <summary>
        /// Gets the name of the <strong>TVL Diagnostics</strong> output window pane.
        /// </summary>
        /// <remarks>
        /// This output window pane is used for shared diagnostics output from several extensions
        /// created by Tunnel Vision Laboratories, LLC.
        /// </remarks>
        public const string TvlDiagnostics = "TVL Diagnostics";
    }
}
