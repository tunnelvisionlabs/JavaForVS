namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using IVsTextBuffer = Microsoft.VisualStudio.TextManager.Interop.IVsTextBuffer;

    public static class IVsTextBufferExtensions
    {
        public static Guid? GetLanguageServiceID(this IVsTextBuffer textBuffer)
        {
            Contract.Requires<ArgumentNullException>(textBuffer != null, "textBuffer");

            Guid id;
            int hr = textBuffer.GetLanguageServiceID(out id);
            if (hr != VSConstants.S_OK)
                return null;

            return id;
        }
    }
}
