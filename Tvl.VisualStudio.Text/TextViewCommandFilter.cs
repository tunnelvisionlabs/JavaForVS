namespace Tvl.VisualStudio.Text
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Tvl.VisualStudio.Shell;

    using IOleCommandTarget = Microsoft.VisualStudio.OLE.Interop.IOleCommandTarget;

    [ComVisible(true)]
    public abstract class TextViewCommandFilter : CommandFilter
    {
        protected TextViewCommandFilter(IVsTextView textViewAdapter)
        {
            Contract.Requires<ArgumentNullException>(textViewAdapter != null, "textViewAdapter");

            this.TextViewAdapter = textViewAdapter;
        }

        protected IVsTextView TextViewAdapter
        {
            get;
            private set;
        }

        protected override IOleCommandTarget Connect()
        {
            IOleCommandTarget next;
            ErrorHandler.ThrowOnFailure(TextViewAdapter.AddCommandFilter(this, out next));
            return next;
        }

        protected override void Disconnect()
        {
            int hr = TextViewAdapter.RemoveCommandFilter(this);
            if (!IsDisposing)
                ErrorHandler.ThrowOnFailure(hr);
        }
    }
}
