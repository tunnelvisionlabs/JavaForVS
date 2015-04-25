namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Microsoft.VisualStudio.TextManager.Interop;

    public static class DebugDocumentPositionExtensions
    {
        public static IDebugDocument2 GetDocument(this IDebugDocumentPosition2 documentPosition)
        {
            Contract.Requires<ArgumentNullException>(documentPosition != null, "documentPosition");

            IDebugDocument2 document;
            ErrorHandler.ThrowOnFailure(documentPosition.GetDocument(out document));
            return document;
        }

        public static string GetFileName(this IDebugDocumentPosition2 documentPosition)
        {
            Contract.Requires<ArgumentNullException>(documentPosition != null, "documentPosition");

            string fileName;
            ErrorHandler.ThrowOnFailure(documentPosition.GetFileName(out fileName));
            return fileName;
        }

        public static TextSpan GetRange(this IDebugDocumentPosition2 documentPosition)
        {
            Contract.Requires<ArgumentNullException>(documentPosition != null, "documentPosition");
            TEXT_POSITION[] startPosition = new TEXT_POSITION[1];
            TEXT_POSITION[] endPosition = new TEXT_POSITION[1];
            ErrorHandler.ThrowOnFailure(documentPosition.GetRange(startPosition, endPosition));
            return new TextSpan()
            {
                iStartLine = (int)startPosition[0].dwLine,
                iStartIndex = (int)startPosition[0].dwColumn,
                iEndLine = (int)endPosition[0].dwLine,
                iEndIndex = (int)endPosition[0].dwColumn
            };
        }
    }
}
