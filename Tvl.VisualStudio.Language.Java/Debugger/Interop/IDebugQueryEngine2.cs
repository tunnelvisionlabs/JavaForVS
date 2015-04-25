namespace Microsoft.VisualStudio.Debugger.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("C989ADC9-F305-4EF5-8CA2-20898E8D0E28")]
    public interface IDebugQueryEngine2
    {
        [PreserveSig]
        int GetEngineInterface([MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }
}
