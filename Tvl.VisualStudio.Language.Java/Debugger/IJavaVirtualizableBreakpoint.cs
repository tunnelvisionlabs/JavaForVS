namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System.Collections.Generic;
    using Tvl.Java.DebugInterface;

    public interface IJavaVirtualizableBreakpoint
    {
        void Bind(JavaDebugProgram program, JavaDebugThread thread, IReferenceType type, IEnumerable<string> sourcePaths);
    }
}
