namespace Microsoft.VisualStudio.Debugger.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("C99D588F-778C-44FE-8B2E-40124A738891")]
    public interface IDebugProgramHost2
    {
        [PreserveSig]
        int GetHostName(uint dwType, [MarshalAs(UnmanagedType.BStr)] out string pbstrHostName);

        [PreserveSig]
        int GetHostId([Out, ComAliasName("Microsoft.VisualStudio.Debugger.Interop.AD_PROCESS_ID"), MarshalAs(UnmanagedType.LPArray)] AD_PROCESS_ID[] pProcessId);

        [PreserveSig]
        int GetHostMachineName([MarshalAs(UnmanagedType.BStr)] out string pbstrHostMachineName);
    }
}
