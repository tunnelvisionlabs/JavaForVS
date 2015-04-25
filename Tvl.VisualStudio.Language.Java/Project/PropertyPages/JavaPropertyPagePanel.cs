namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;
    using IServiceProvider = System.IServiceProvider;
    using OLEMSGBUTTON = Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON;
    using OLEMSGDEFBUTTON = Microsoft.VisualStudio.Shell.Interop.OLEMSGDEFBUTTON;
    using OLEMSGICON = Microsoft.VisualStudio.Shell.Interop.OLEMSGICON;
    using VsShellUtilities = Microsoft.VisualStudio.Shell.VsShellUtilities;

    public partial class JavaPropertyPagePanel : UserControl
    {
        private readonly JavaPropertyPage _parentPropertyPage;

        public JavaPropertyPagePanel()
            : this(null)
        {
        }

        public JavaPropertyPagePanel(JavaPropertyPage parentPropertyPage)
        {
            _parentPropertyPage = parentPropertyPage;
            InitializeComponent();
        }

        internal JavaPropertyPage ParentPropertyPage
        {
            get
            {
                return _parentPropertyPage;
            }
        }

        protected bool IsDirty
        {
            get
            {
                Debug.Assert(ParentPropertyPage != null, "ParentPropertyPage has not been set.");
                if (ParentPropertyPage != null)
                    return ParentPropertyPage.IsDirty;

                return true;
            }

            set
            {
                Debug.Assert(ParentPropertyPage != null, "ParentPropertyPage has not been set.");
                if (ParentPropertyPage != null)
                    ParentPropertyPage.IsDirty = value;
            }
        }

        public virtual void ValidateProperties()
        {
        }

        protected virtual void OnControlValidating(object sender, CancelEventArgs e)
        {
            ValidateControl(sender, e);
            if (!e.Cancel)
                ParentPropertyPage.UpdateStatus();
        }

        protected void ValidateControl(object sender, CancelEventArgs e)
        {
            try
            {
                ValidateProperties();
            }
            catch (PropertyPageArgumentException ex)
            {
                IServiceProvider serviceProvider = ParentPropertyPage.Site;
                string message = ex.Message;
                string title = "Java Project Error";
                OLEMSGICON icon = OLEMSGICON.OLEMSGICON_CRITICAL;
                OLEMSGBUTTON button = OLEMSGBUTTON.OLEMSGBUTTON_OK;
                OLEMSGDEFBUTTON defaultButton = OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST;
                if (serviceProvider != null)
                    VsShellUtilities.ShowMessageBox(serviceProvider, message, title, icon, button, defaultButton);

                e.Cancel = true;
            }
        }
    }
}
