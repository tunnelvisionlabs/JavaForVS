namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugActivateDocumentEvent : DebugEvent, IDebugActivateDocumentEvent2
    {
        private readonly IDebugDocument2 _document;
        private readonly IDebugDocumentContext2 _documentContext;

        public DebugActivateDocumentEvent(enum_EVENTATTRIBUTES attributes, IDebugDocument2 document, IDebugDocumentContext2 documentContext)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(document != null, "document");
            Contract.Requires<ArgumentNullException>(documentContext != null, "documentContext");

            _document = document;
            _documentContext = documentContext;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugActivateDocumentEvent2).GUID;
            }
        }

        public int GetDocument(out IDebugDocument2 ppDoc)
        {
            ppDoc = _document;
            return VSConstants.S_OK;
        }

        public int GetDocumentContext(out IDebugDocumentContext2 ppDocContext)
        {
            ppDocContext = _documentContext;
            return VSConstants.S_OK;
        }
    }
}
