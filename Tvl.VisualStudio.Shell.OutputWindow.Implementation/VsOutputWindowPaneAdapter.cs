namespace Tvl.VisualStudio.Shell.OutputWindow.Implementation
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using Tvl.VisualStudio.Shell.OutputWindow.Interfaces;

    internal sealed class VsOutputWindowPaneAdapter : IOutputWindowPane
    {
        private readonly IVsOutputWindowPane _pane;

        public VsOutputWindowPaneAdapter(IVsOutputWindowPane pane)
        {
            Contract.Requires<ArgumentNullException>(pane != null, "pane");

            this._pane = pane;
        }

        public string Name
        {
            get
            {
                string name = null;
                ErrorHandler.ThrowOnFailure(this._pane.GetName(ref name));
                return name;
            }
            set
            {
                ErrorHandler.ThrowOnFailure(this._pane.SetName(value));
            }
        }

        public void Activate()
        {
            ErrorHandler.ThrowOnFailure(this._pane.Activate());
        }

        public void Hide()
        {
            ErrorHandler.ThrowOnFailure(this._pane.Hide());
        }

        public void Write(string text)
        {
            ErrorHandler.ThrowOnFailure(this._pane.OutputStringThreadSafe(text));
        }

        public void WriteLine(string text)
        {
            if (!text.EndsWith(Environment.NewLine))
                text += Environment.NewLine;

            Write(text);
        }
    }
}
