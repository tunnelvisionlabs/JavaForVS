namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    public static class DebugProgramExtensions
    {
        public static IDebugProcess2 GetProcess(this IDebugProgram2 program)
        {
            Contract.Requires<ArgumentNullException>(program != null, "program");

            IDebugProcess2 process;
            ErrorHandler.ThrowOnFailure(program.GetProcess(out process));
            return process;
        }

        public static string GetName(this IDebugProgram2 program)
        {
            Contract.Requires<ArgumentNullException>(program != null, "program");

            string name;
            ErrorHandler.ThrowOnFailure(program.GetName(out name));
            return name;
        }
    }
}
