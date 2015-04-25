namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System;

    public partial class JavaGeneralPropertyPagePanel : JavaPropertyPagePanel
    {
        public JavaGeneralPropertyPagePanel()
            : this(null)
        {
        }

        public JavaGeneralPropertyPagePanel(JavaPropertyPage parentPropertyPage)
            : base(parentPropertyPage)
        {
            InitializeComponent();
        }

        public new JavaGeneralPropertyPage ParentPropertyPage
        {
            get
            {
                return base.ParentPropertyPage as JavaGeneralPropertyPage;
            }
        }

        public string ProjectFolder
        {
            get
            {
                return txtJavacPath.RootFolder;
            }

            set
            {
                txtJavacPath.RootFolder = value;
            }
        }

        public string JavacPath
        {
            get
            {
                return txtJavacPath.Text;
            }

            set
            {
                txtJavacPath.Text = value;
            }
        }

        // Javac Path
        private void folderBrowserTextBox1_TextChanged(object sender, EventArgs e)
        {
            ParentPropertyPage.IsDirty = true;
            if (ParentPropertyPage.ProjectManager != null && ParentPropertyPage.ProjectManager.SharedBuildOptions.Build != null)
                ParentPropertyPage.ProjectManager.SharedBuildOptions.Build.RefreshCommandLine();
        }
    }
}
