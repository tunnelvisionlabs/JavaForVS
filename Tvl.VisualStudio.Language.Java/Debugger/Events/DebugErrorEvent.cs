namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;

    [ComVisible(true)]
    public class DebugErrorEvent : DebugEvent, IDebugErrorEvent2
    {
        private readonly enum_MESSAGETYPE _messageType;
        private readonly string _format;
        private readonly int _reason;
        private readonly MessageBoxIcon _severity;
        private readonly string _helpFileName;
        private readonly uint _helpId;

        public DebugErrorEvent(enum_EVENTATTRIBUTES attributes, enum_MESSAGETYPE messageType, string format, int reason, MessageBoxIcon severity, string helpFileName = null, uint helpId = 0)
            : base(attributes)
        {
            _messageType = messageType;
            _format = format;
            _reason = reason;
            _severity = severity;
            _helpFileName = helpFileName;
            _helpId = helpId;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugErrorEvent2).GUID;
            }
        }

        /// <summary>
        /// Returns information that allows construction of a human-readable error message.
        /// </summary>
        /// <param name="pMessageType">Returns a value from the MESSAGETYPE enumeration, describing the type of message.</param>
        /// <param name="pbstrErrorFormat">The format of the final message to the user (see "Remarks" for details).</param>
        /// <param name="phrErrorReason">The error code the message is about.</param>
        /// <param name="pdwType">Severity of the error.</param>
        /// <param name="pbstrHelpFileName">Path to a help file (set to a null value if there is no help file).</param>
        /// <param name="pdwHelpId">ID of the help topic to display (set to 0 if there is no help topic).</param>
        /// <returns>If successful, returns S_OK; otherwise, returns an error code.</returns>
        /// <remarks>
        /// The error message should be formatted along the lines of "What I was doing. %1". The "%1" would then be replaced by the caller with the error message derived from the error code (which is returned in hrErrorReason). The pMessageType parameter tells the caller how the final error message should be displayed.
        /// </remarks>
        public int GetErrorMessage(enum_MESSAGETYPE[] pMessageType, out string pbstrErrorFormat, out int phrErrorReason, out uint pdwType, out string pbstrHelpFileName, out uint pdwHelpId)
        {
            if (pMessageType == null)
                throw new ArgumentNullException("pMessageType");
            if (pMessageType.Length < 1)
                throw new ArgumentException();

            pMessageType[0] = _messageType;
            pbstrErrorFormat = _format;
            phrErrorReason = _reason;
            pdwType = (uint)_severity;
            pbstrHelpFileName = _helpFileName;
            pdwHelpId = _helpId;
            return VSConstants.S_OK;
        }
    }
}
