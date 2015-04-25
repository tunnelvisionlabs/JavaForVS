namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using IEnumString = Microsoft.VisualStudio.OLE.Interop.IEnumString;

    public static class IVsCmdNameMappingExtensions
    {
        public static IEnumerable<string> GetMacroNames(this IVsCmdNameMapping commandNameMapping)
        {
            Contract.Requires<ArgumentNullException>(commandNameMapping != null, "commandNameMapping");
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

            IEnumString enumString;
            if (ErrorHandler.Succeeded(commandNameMapping.EnumMacroNames(VSCMDNAMEOPTS.CNO_GETENU, out enumString)))
            {
                string[] array = new string[1];
                while (true)
                {
                    uint count;
                    int hr = enumString.Next((uint)array.Length, array, out count);
                    ErrorHandler.ThrowOnFailure(hr);
                    if (hr == VSConstants.S_FALSE || count == 0)
                        break;

                    for (uint i = 0; i < count; i++)
                        yield return array[i];
                }
            }
        }

        public static IEnumerable<string> GetCommandNames(this IVsCmdNameMapping commandNameMapping)
        {
            Contract.Requires<ArgumentNullException>(commandNameMapping != null, "commandNameMapping");
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

            IEnumString enumString;
            if (ErrorHandler.Succeeded(commandNameMapping.EnumNames(VSCMDNAMEOPTS.CNO_GETENU, out enumString)))
            {
                string[] array = new string[1];
                while (true)
                {
                    uint count;
                    int hr = enumString.Next((uint)array.Length, array, out count);
                    ErrorHandler.ThrowOnFailure(hr);
                    if (hr == VSConstants.S_FALSE || count == 0)
                        break;

                    for (uint i = 0; i < count; i++)
                        yield return array[i];
                }
            }
        }
    }
}
