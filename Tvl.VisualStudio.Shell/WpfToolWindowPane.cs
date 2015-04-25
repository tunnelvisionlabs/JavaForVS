namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
    using VsPackage = Microsoft.VisualStudio.Shell.Package;

    public abstract class WpfToolWindowPane : ToolWindowPane
    {
        public const int TabImageHeight = 0x10;
        public const int TabImageWidth = 0x10;

        private Control _toolWindowControl;

        public WpfToolWindowPane()
            : base(null)
        {
            GlobalServiceProvider = (IServiceProvider)VsPackage.GetGlobalService(typeof(IServiceProvider));
            if (GlobalServiceProvider == null)
                GlobalServiceProvider = new ServiceProvider((IOleServiceProvider)VsPackage.GetGlobalService(typeof(IOleServiceProvider)));

            this._toolWindowControl = CreateToolWindowControl();
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        protected IServiceProvider GlobalServiceProvider
        {
            get;
            private set;
        }

        public override object Content
        {
            get
            {
                return _toolWindowControl;
            }
        }

        public virtual BitmapSource Icon
        {
            get
            {
                return null;
            }
        }

        public static void ProvideToolWindowCommand<TToolWindowPane>(VsPackage package, Guid menuGroup, int commandId)
            where TToolWindowPane : WpfToolWindowPane
        {
            EventHandler handler =
                (object sender, EventArgs e) =>
                {
                    try
                    {
                        ToolWindowPane pane = package.FindToolWindow(typeof(TToolWindowPane), 0, true);
                        if (pane != null)
                            ErrorHandler.ThrowOnFailure(((IVsWindowFrame)pane.Frame).Show());
                    }
                    catch (Exception ex)
                    {
                        if (ErrorHandler.IsCriticalException(ex))
                            throw;
                    }
                };

            OleMenuCommandService commandService = (OleMenuCommandService)((IServiceProvider)package).GetService(typeof(IMenuCommandService));
            if (commandService != null)
            {
                CommandID toolWindowCommandId = new CommandID(menuGroup, commandId);
                MenuCommand command = new MenuCommand(handler, toolWindowCommandId);
                commandService.AddCommand(command);
            }
        }

        protected abstract Control CreateToolWindowControl();

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    IVsWindowFrame frame = (IVsWindowFrame)base.Frame;
                    if (frame != null)
                    {
                        frame.CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_SaveIfDirty);
                    }
                }
            }

            base.Dispose(disposing);
            this.IsDisposed = true;
        }

        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();

            IVsWindowFrame windowFrame = base.Frame as IVsWindowFrame;
            if (Icon != null && windowFrame != null)
            {
                var icon = Icon;

                if (icon.PixelWidth == TabImageWidth && icon.PixelHeight == TabImageHeight)
                {
                    int stride = icon.Format.BitsPerPixel * icon.PixelWidth;
                    byte[] pixels = new byte[stride * icon.PixelHeight];
                    icon.CopyPixels(pixels, stride, 0);
                    icon = BitmapSource.Create(16, 16, 96.0, 96.0, icon.Format, null, pixels, stride);
                    windowFrame.SetProperty((int)__VSFPROPID4.VSFPROPID_TabImage, icon);
                }
                else
                {
                    Trace.WriteLine(string.Format("The icon for the {0} window could not be used because it was not {1}x{2}px.", Caption, TabImageWidth, TabImageHeight));
                }
            }
        }

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}
