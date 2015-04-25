namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    using COMException = System.Runtime.InteropServices.COMException;
    using Directory = System.IO.Directory;
    using Marshal = System.Runtime.InteropServices.Marshal;
    using Path = System.IO.Path;

    public static class IVsUIShellExtensions
    {
        private static readonly Dictionary<Guid, string> _persistedDirectories = new Dictionary<Guid, string>();

        private static string DefaultDirectory
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
        }

        public static string GetErrorInfo(this IVsUIShell shell)
        {
            string message;
            ErrorHandler.ThrowOnFailure(shell.GetErrorInfo(out message));
            return message;
        }

        public static IEnumerable<IVsWindowFrame> GetToolWindows(this IVsUIShell shell)
        {
            Contract.Requires<ArgumentNullException>(shell != null, "shell");
            Contract.Ensures(Contract.Result<IEnumerable<IVsWindowFrame>>() != null);

            IEnumWindowFrames frames;
            ErrorHandler.ThrowOnFailure(shell.GetToolWindowEnum(out frames));

            IVsWindowFrame[] array = new IVsWindowFrame[1];
            while (true)
            {
                uint count;
                int hr = frames.Next((uint)array.Length, array, out count);
                ErrorHandler.ThrowOnFailure(hr);
                if (hr == VSConstants.S_FALSE || count == 0)
                    break;

                for (uint i = 0; i < count; i++)
                    yield return array[i];
            }
        }

        public static string GetDirectoryViaBrowseDialog(this IVsUIShell2 shell, IntPtr parentWindow, Guid persistenceSlot, string title, string initialDirectory, bool overridePersistedInitialDirectory)
        {
            if (shell == null)
                throw new ArgumentNullException("shell");
            if (title == null)
                throw new ArgumentNullException("title");

            const int MaxDirName = 10000;
            IntPtr dirNameBuffer = Marshal.AllocCoTaskMem((MaxDirName + 1) * sizeof(char));
            try
            {
                VSBROWSEINFOW[] browse = new VSBROWSEINFOW[]
                    {
                        new VSBROWSEINFOW
                        {
                            pwzDlgTitle = title,
                            dwFlags = (uint)BrowseInfoFlags.ReturnOnlyFileSystemDirectories,
                            hwndOwner = parentWindow,
                            pwzInitialDir = GetInitialDirectoryToUse(initialDirectory, overridePersistedInitialDirectory, persistenceSlot),
                            nMaxDirName = MaxDirName,
                            lStructSize = (uint)Marshal.SizeOf(typeof(VSBROWSEINFOW)),
                            pwzDirName = dirNameBuffer,
                            dwHelpTopic = 0
                        }
                    };

                string helpTopic = string.Empty;
                string openButtonLabel = null;
                string ceilingDir = null;
                VSNSEBROWSEINFOW[] nseBrowseInfo = null;

                ErrorHandler.ThrowOnFailure(shell.GetDirectoryViaBrowseDlgEx(browse, helpTopic, openButtonLabel, ceilingDir, nseBrowseInfo));
                string folder = Marshal.PtrToStringUni(browse[0].pwzDirName);
                if (!string.IsNullOrEmpty(folder))
                    PersistLastUseDirectory(persistenceSlot, folder);

                return folder;
            }
            catch (COMException ex)
            {
                if (ex.ErrorCode != VSConstants.OLE_E_PROMPTSAVECANCELLED)
                    throw;
            }
            finally
            {
                if (dirNameBuffer != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(dirNameBuffer);
                    dirNameBuffer = IntPtr.Zero;
                }
            }

            return null;
        }

        private static void PersistLastUseDirectory(Guid persistenceSlot, string folder)
        {
            if (persistenceSlot != Guid.Empty && !string.IsNullOrEmpty(folder))
                _persistedDirectories[persistenceSlot] = folder;
        }

        private static string GetInitialDirectoryToUse(string initialDirectory, bool overridePersistedInitialDirectory, Guid persistenceSlot)
        {
            string path = initialDirectory;
            if (persistenceSlot != Guid.Empty && !overridePersistedInitialDirectory)
            {
                if (!_persistedDirectories.TryGetValue(persistenceSlot, out path))
                    path = initialDirectory;
            }

            try
            {
                path = Path.GetFullPath(path);
                if (!Directory.Exists(path))
                    path = DefaultDirectory;
            }
            catch (ArgumentException)
            {
                path = DefaultDirectory;
            }

            return path;
        }

        [Flags]
        private enum BrowseInfoFlags
        {
            None,
            ReturnOnlyFileSystemDirectories = 0x0001,
            DoNotGoBelowDomain = 0x0002,
            StatusText = 0x0004,
            ReturnFileSystemAncestors = 0x0008,
            EditBox = 0x0010,
            Validate = 0x0020,
            NewDialogStyle = 0x0040,
            BrowseIncludeUrls = 0x0080,
            UseNewInterface = EditBox | NewDialogStyle,
            UAHint = 0x0100,
            NoNewFolderButton = 0x0200,
            NoTranslateTargets = 0x0400,
            BrowseForComputer = 0x1000,
            BrowseForPrinter = 0x2000,
            BrowseIncludeFiles = 0x4000,
            Shareable = 0x8000,
            BrowseFileJunctions = 0x10000
        }
    }
}
