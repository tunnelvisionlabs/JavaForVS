namespace Tvl.VisualStudio.Shell
{
    using System.Runtime.InteropServices;
    using Convert = System.Convert;
    using IntPtr = System.IntPtr;
    using Message = System.Windows.Forms.Message;
    using Padding = System.Windows.Forms.Padding;
    using Size = System.Drawing.Size;
    using UserControl = System.Windows.Forms.UserControl;
    using VSCOMPONENTSELECTORDATA = Microsoft.VisualStudio.Shell.Interop.VSCOMPONENTSELECTORDATA;
    using VSConstants = Microsoft.VisualStudio.VSConstants;

    public class ComponentSelectorControl : UserControl
    {
        public ComponentSelectorControl()
        {
            Padding = new Padding(12);
        }

        protected virtual bool CanSelectItems
        {
            get
            {
                return true;
            }
        }

        protected virtual void InitializeItems()
        {
        }

        protected virtual void ClearSelection()
        {
        }

        protected virtual void SetSelectionMode(bool multiSelect)
        {
        }

        protected virtual ComponentSelectorData[] GetSelection()
        {
            return new ComponentSelectorData[0];
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
            case VSConstants.CPPM_CLEARSELECTION:
                HandleClearSelection(m);
                break;

            case VSConstants.CPPM_GETSELECTION:
                HandleGetSelection(m);
                break;

            case VSConstants.CPPM_INITIALIZELIST:
                HandleInitializeItems(m);
                break;

            case VSConstants.CPPM_INITIALIZETAB:
                HandleInitializeTab(m);
                break;

            case VSConstants.CPPM_QUERYCANSELECT:
                HandleQueryCanSelect(m);
                break;

            case VSConstants.CPPM_SETMULTISELECT:
                HandleSetMultiSelect(m);
                break;

            case UnsafeNativeMethods.WM_SIZE:
                HandleWmSize(m);
                break;

            default:
                base.WndProc(ref m);
                break;
            }
        }

        private void HandleClearSelection(Message m)
        {
            ClearSelection();
        }

        private void HandleGetSelection(Message m)
        {
            ComponentSelectorData[] items = GetSelection();
            int count = items != null ? items.Length : 0;
            Marshal.WriteInt32(m.WParam, count);
            if (count > 0)
            {
                IntPtr ppItems = Marshal.AllocCoTaskMem(count * Marshal.SizeOf(typeof(IntPtr)));
                for (int i = 0; i < count; i++)
                {
                    IntPtr pItem = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(VSCOMPONENTSELECTORDATA)));
                    Marshal.WriteIntPtr(ppItems, i * IntPtr.Size, pItem);
                    VSCOMPONENTSELECTORDATA data = new VSCOMPONENTSELECTORDATA()
                    {
                        dwSize = (uint)Marshal.SizeOf(typeof(VSCOMPONENTSELECTORDATA)),
                        bstrFile = items[i].File,
                        bstrTitle = items[i].Title,
                        bstrProjRef = items[i].ProjectReference,
                        guidTypeLibrary = items[i].TypeLibrary,
                        lCustom = items[i].CustomInformation,
                        type = items[i].ComponentType,
                        // the following items are handled separately
                        lcidTypeLibrary = 0,
                        wFileBuildNumber = 0,
                        wFileMajorVersion = 0,
                        wFileMinorVersion = 0,
                        wFileRevisionNumber = 0,
                        wTypeLibraryMajorVersion = 0,
                        wTypeLibraryMinorVersion = 0,
                    };

                    if (items[i].TypeLibraryCulture != null)
                    {
                        data.lcidTypeLibrary = (uint)items[i].TypeLibraryCulture.LCID;
                    }

                    if (items[i].FileVersion != null)
                    {
                        data.wFileMajorVersion = (ushort)items[i].FileVersion.Major;
                        data.wFileMinorVersion = (ushort)items[i].FileVersion.Minor;
                        data.wFileBuildNumber = (ushort)items[i].FileVersion.Build;
                        data.wFileRevisionNumber = (ushort)items[i].FileVersion.Revision;
                    }

                    if (items[i].TypeLibraryVersion != null)
                    {
                        data.wTypeLibraryMajorVersion = (ushort)items[i].TypeLibraryVersion.Major;
                        data.wTypeLibraryMinorVersion = (ushort)items[i].TypeLibraryVersion.Minor;
                    }

                    Marshal.StructureToPtr(data, pItem, false);
                }

                Marshal.WriteIntPtr(m.LParam, ppItems);
            }
        }

        private void HandleInitializeItems(Message m)
        {
            InitializeItems();
        }

        private void HandleInitializeTab(Message m)
        {
            int exStyle = UnsafeNativeMethods.GetWindowLong(UnsafeNativeMethods.GetParent(Handle), UnsafeNativeMethods.GWL_EXSTYLE);
            if ((exStyle | UnsafeNativeMethods.WS_EX_CONTROLPARENT) != UnsafeNativeMethods.WS_EX_CONTROLPARENT)
            {
                exStyle ^= UnsafeNativeMethods.WS_EX_CONTROLPARENT;
                UnsafeNativeMethods.SetWindowLong(UnsafeNativeMethods.GetParent(Handle), UnsafeNativeMethods.GWL_EXSTYLE, exStyle);
            }

            Invalidate(true);
        }

        private void HandleQueryCanSelect(Message m)
        {
            if (m.LParam != IntPtr.Zero)
                Marshal.WriteByte(m.LParam, (byte)(CanSelectItems ? 1 : 0));
        }

        private void HandleSetMultiSelect(Message m)
        {
            SetSelectionMode(Convert.ToBoolean((byte)m.LParam));
        }

        private void HandleWmSize(Message m)
        {
            if (m.WParam.ToInt32() == UnsafeNativeMethods.SIZE_RESTORED)
            {
                int newSize = m.LParam.ToInt32();
                short newWidth = (short)newSize;
                short newHeight = (short)(newSize >> 16);
                this.Size = new Size(newWidth, newHeight);
                UnsafeNativeMethods.SetWindowPos(UnsafeNativeMethods.GetParent(Handle), IntPtr.Zero, 0, 0, newWidth, newHeight, 0);
                PerformLayout();
                Invalidate(true);
            }
        }

        private static class UnsafeNativeMethods
        {
            public const int GWL_EXSTYLE = -20;
            public const int SIZE_RESTORED = 0;
            public const int WM_SIZE = 5;
            public const int WS_EX_CONTROLPARENT = 0x10000;

            [DllImport("user32.dll", SetLastError = true)]
            public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern IntPtr GetParent(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        }
    }
}
