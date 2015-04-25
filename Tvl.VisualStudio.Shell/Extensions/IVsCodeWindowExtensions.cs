namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.TextManager.Interop;

    public static class IVsCodeWindowExtensions
    {
        public static IVsTextLines GetBuffer(this IVsCodeWindow codeWindow)
        {
            Contract.Requires<ArgumentNullException>(codeWindow != null, "codeWindow");

            IVsTextLines buffer;
            ErrorHandler.ThrowOnFailure(codeWindow.GetBuffer(out buffer));
            return buffer;
        }

        public static string GetEditorCaption(this IVsCodeWindow codeWindow, READONLYSTATUS status)
        {
            Contract.Requires<ArgumentNullException>(codeWindow != null, "codeWindow");

            string caption;
            if (ErrorHandler.Failed(codeWindow.GetEditorCaption(status, out caption)))
                return null;

            return caption;
        }

        public static IVsTextView GetLastActiveView(this IVsCodeWindow codeWindow)
        {
            Contract.Requires<ArgumentNullException>(codeWindow != null, "codeWindow");

            IVsTextView view;
            if (ErrorHandler.Failed(codeWindow.GetLastActiveView(out view)))
                return null;

            return view;
        }

        public static IVsTextView GetPrimaryView(this IVsCodeWindow codeWindow)
        {
            Contract.Requires<ArgumentNullException>(codeWindow != null, "codeWindow");

            IVsTextView view;
            if (ErrorHandler.Failed(codeWindow.GetPrimaryView(out view)))
                return null;

            return view;
        }

        public static IVsTextView GetSecondaryView(this IVsCodeWindow codeWindow)
        {
            Contract.Requires<ArgumentNullException>(codeWindow != null, "codeWindow");

            IVsTextView view;
            if (ErrorHandler.Failed(codeWindow.GetSecondaryView(out view)))
                return null;

            return view;
        }

        public static Guid GetViewClassID(this IVsCodeWindow codeWindow)
        {
            Contract.Requires<ArgumentNullException>(codeWindow != null, "codeWindow");

            Guid classID;
            ErrorHandler.ThrowOnFailure(codeWindow.GetViewClassID(out classID));
            return classID;
        }

        public static bool IsReadOnly(this IVsCodeWindow codeWindow)
        {
            Contract.Requires<ArgumentNullException>(codeWindow != null, "codeWindow");

            IVsCodeWindowEx codeWindowEx = codeWindow as IVsCodeWindowEx;
            if (codeWindowEx == null)
                throw new NotSupportedException();

            int result = codeWindowEx.IsReadOnly();
            ErrorHandler.ThrowOnFailure(result);
            return result == VSConstants.S_OK;
        }
    }
}
