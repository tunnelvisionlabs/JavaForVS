namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;

    public static class JavaDebuggerConstants
    {
        public const string JavaDebuggerPackageGuidString = "EFAA479F-65EE-4BE9-AB5B-F972429B95C9";
        public static readonly Guid JavaDebuggerPackageGuid = new Guid("{" + JavaDebuggerPackageGuidString + "}");

        public const string JavaDebugEngineGuidString = "7A65141D-1452-41E2-9B0D-2314A02CFF71";
        public static readonly Guid JavaDebugEngineGuid = new Guid("{" + JavaDebugEngineGuidString + "}");

        public const string JavaProgramProviderGuidString = "8E49DC63-B8ED-4025-A3EA-DD8A96E4EA97";
        public static readonly Guid JavaProgramProviderGuid = new Guid("{" + JavaProgramProviderGuidString + "}");

        public const int E_INSUFFICIENT_BUFFER = unchecked((int)0x8007007A);

        public const string JvmExceptionKind = "Java Runtime Exceptions";
    }
}
