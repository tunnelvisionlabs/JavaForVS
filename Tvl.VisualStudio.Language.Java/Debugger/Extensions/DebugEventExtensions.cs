namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    public static class DebugEventExtensions
    {
        public static enum_EVENTATTRIBUTES GetAttributes(this IDebugEvent2 debugEvent)
        {
            Contract.Requires<ArgumentNullException>(debugEvent != null, "debugEvent");

            uint attributes;
            ErrorHandler.ThrowOnFailure(debugEvent.GetAttributes(out attributes));
            return (enum_EVENTATTRIBUTES)attributes;
        }
    }
}
