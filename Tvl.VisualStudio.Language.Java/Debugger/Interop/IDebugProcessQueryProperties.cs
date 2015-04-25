namespace Microsoft.VisualStudio.Debugger.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("230A0071-62EF-4CAE-AAC0-8988C37024BF")]
    public interface IDebugProcessQueryProperties
    {
        [PreserveSig]
        int QueryProperty([In, ComAliasName("Microsoft.VisualStudio.Debugger.Interop.PROCESS_PROPERTY_TYPE")] uint dwPropType, [MarshalAs(UnmanagedType.Struct)] out object pvarPropValue);

        [PreserveSig]
        int QueryProperties([In] uint celt, [In, ComAliasName("Microsoft.VisualStudio.Debugger.Interop.PROCESS_PROPERTY_TYPE"), MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] rgdwPropTypes, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)] object[] rgtPropValues);
    }
}
