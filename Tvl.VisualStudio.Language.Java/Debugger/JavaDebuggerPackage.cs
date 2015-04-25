namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using Tvl.VisualStudio.Shell;

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(JavaDebuggerConstants.JavaDebuggerPackageGuidString)]
    [ProvideDebugEngine(typeof(JavaDebugEngine), Constants.JavaLanguageName,
        PortSuppliers = new string[] { "{708C1ECA-FF48-11D2-904F-00C04FA302A1}" },
        ProgramProvider = "{" + JavaDebuggerConstants.JavaProgramProviderGuidString + "}",
        // basic configuration
        AlwaysLoadProgramProviderLocal = true,
        AlwaysLoadLocal = true,
        AutoSelectPriority = 0,
        ExcludeManualSelect = true,
        EngineCanWatchProcess = false,
        // feature support (supported)
        Disassembly = true,
        SuspendThread = true,
        // feature support (not supported by DE / "easy")
        Exceptions = true,
        HitCountBreakpoints = false,
        FunctionBreakpoints = false,
        DataBreakpoints = false,
        ConditionalBreakpoints = false,
        // feature support (not supported by DE / "harder")
        Attach = false,
        JustInTimeDebugging = false,
        EditAndContinue = false,
        DumpWriting = false,
        RemoteDebugging = false,
        // feature support (not supported / VM)
        SetNextStatement = false
        // other
        //AddressBreakpoints = false,
        //CallStackBreakpoints = false,
        //AutoSelectIncompatibleList = new string[] { },
        //DisableJitOptimization = false,
        //EditAndContinueUseNativeBuilder = false,
        //IncompatibleList = new string[] { },
        //LoadProgramProviderUnderWOW64 = false,
        //LoadUnderWOW64 = false,
        //LoadedByDebuggee = false,
        //StopOnExceptionCrossingManagedBoundary = false,
        //WarnIfNoSymbols = false
        )]
    [ProvideObject(typeof(JavaDebugEngine))]
    [ProvideObject(typeof(JavaDebugProgramProvider))]
    public partial class JavaDebuggerPackage : Package
    {
    }
}
