namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.TextManager.Interop;

    public static class IVsExpansionManagerExtensions
    {
        public static VsExpansion[] EnumerateExpansions(this IVsExpansionManager expansionManager, Guid language)
        {
            Contract.Requires(expansionManager != null);
            Contract.Ensures(Contract.Result<VsExpansion[]>() != null);

            return expansionManager.EnumerateExpansions(language, null, false);
        }

        public static VsExpansion[] EnumerateExpansions(this IVsExpansionManager expansionManager, Guid language, string[] snippetTypes, bool shortcutsOnly)
        {
            Contract.Requires<ArgumentNullException>(expansionManager != null, "expansionManager");
            Contract.Ensures(Contract.Result<VsExpansion[]>() != null);

            bool includeNullType = false;
            bool includeDuplicates = false;

            IVsExpansionEnumeration expEnum;
            if (ErrorHandler.Succeeded(expansionManager.EnumerateExpansions(language, shortcutsOnly ? 1 : 0, snippetTypes, snippetTypes.Length, includeNullType ? 1 : 0, includeDuplicates ? 1 : 0, out expEnum)))
            {
                uint count;
                ErrorHandler.ThrowOnFailure(expEnum.GetCount(out count));

                IntPtr[] raw = new IntPtr[count];
                try
                {
                    for (int i = 0; i < raw.Length; i++)
                        raw[i] = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(VsExpansion)));

                    uint fetched;
                    ErrorHandler.ThrowOnFailure(expEnum.Next(count, raw, out fetched));

                    VsExpansion[] results = new VsExpansion[fetched];
                    for (int i = 0; i < results.Length; i++)
                    {
                        if (raw[i] != IntPtr.Zero)
                        {
                            results[i] = (VsExpansion)Marshal.PtrToStructure(raw[i], typeof(VsExpansion));
                        }
                    }

                    return results;
                }
                finally
                {
                    foreach (IntPtr p in raw)
                    {
                        Marshal.FreeCoTaskMem(p);
                    }
                }
            }

            return new VsExpansion[0];
        }
    }
}
