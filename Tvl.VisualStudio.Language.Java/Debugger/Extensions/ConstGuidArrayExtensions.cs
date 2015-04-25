namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Debugger.Interop;

    public static class ConstGuidArrayExtensions
    {
        public static IEnumerable<Guid> AsEnumerable(this CONST_GUID_ARRAY source)
        {
            int guidSize = Marshal.SizeOf(typeof(Guid));
            for (uint i = 0; i < source.dwCount; i++)
            {
                IntPtr ptr = (IntPtr)(source.Members.ToInt64() + guidSize * i);
                yield return (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
            }
        }
    }
}
