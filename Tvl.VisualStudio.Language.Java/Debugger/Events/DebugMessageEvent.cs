namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using DialogResult = System.Windows.Forms.DialogResult;
    using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;

    [ComVisible(true)]
    public class DebugMessageEvent : DebugEvent, IDebugMessageEvent2
    {
        private readonly enum_MESSAGETYPE _messageType;
        private readonly string _message;
        private readonly MessageBoxIcon _severity;
        private readonly string _helpFileName;
        private readonly uint _helpId;

        private DialogResult? _response;

        public DebugMessageEvent(enum_EVENTATTRIBUTES attributes, enum_MESSAGETYPE messageType, string message, MessageBoxIcon severity, string helpFileName = null, uint helpId = 0)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(message != null, "message");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(message));

            _messageType = messageType;
            _message = message;
            _severity = severity;
            _helpFileName = helpFileName;
            _helpId = helpId;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugMessageEvent2).GUID;
            }
        }

        public DialogResult? Response
        {
            get
            {
                return _response;
            }

            set
            {
                _response = value;
            }
        }

        public int GetMessage(enum_MESSAGETYPE[] pMessageType, out string pbstrMessage, out uint pdwType, out string pbstrHelpFileName, out uint pdwHelpId)
        {
            if (pMessageType == null)
                throw new ArgumentNullException("pMessageType");
            if (pMessageType.Length == 0)
                throw new ArgumentException();

            pMessageType[0] = _messageType;
            pbstrMessage = _message;
            pdwType = (uint)_severity;
            pbstrHelpFileName = _helpFileName;
            pdwHelpId = _helpId;
            return VSConstants.S_OK;
        }

        public int SetResponse(uint dwResponse)
        {
            _response = (DialogResult)dwResponse;
            return VSConstants.S_OK;
        }
    }
}
