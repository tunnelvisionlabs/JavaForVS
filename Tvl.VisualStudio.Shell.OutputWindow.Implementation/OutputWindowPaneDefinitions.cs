#pragma warning disable 169 // The field 'fieldname' is never used

namespace Tvl.VisualStudio.Shell.OutputWindow
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Utilities;
    using Tvl.VisualStudio.Shell.OutputWindow.Interfaces;

    public static class OutputWindowPaneDefinitions
    {
        [Export]
        [Name(PredefinedOutputWindowPanes.TvlIntellisense)]
        private static readonly OutputWindowDefinition TvlIntellisenseOutputWindowDefinition;

        [Export]
        [Name(PredefinedOutputWindowPanes.TvlDiagnostics)]
        private static readonly OutputWindowDefinition TvlDiagnosticsOutputWindowDefinition;
    }
}
