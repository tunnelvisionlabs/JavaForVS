namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System;
    using System.Runtime.InteropServices;
    using _PersistStorageType = Microsoft.VisualStudio.Shell.Interop._PersistStorageType;

    [ComVisible(true)]
    [Guid(JavaProjectConstants.JavaGeneralPropertyPageGuidString)]
    public class JavaGeneralPropertyPage : JavaPropertyPage
    {
        public JavaGeneralPropertyPage()
        {
            PageName = JavaConfigConstants.PageNameGeneral;
        }

        public new JavaGeneralPropertyPagePanel PropertyPagePanel
        {
            get
            {
                return (JavaGeneralPropertyPagePanel)base.PropertyPagePanel;
            }
        }

        protected override void BindProperties()
        {
            if (ProjectManager != null)
                ProjectManager.SharedBuildOptions.General = PropertyPagePanel;

            PropertyPagePanel.ProjectFolder = ProjectManager.ProjectFolder;
            PropertyPagePanel.JavacPath = GetConfigProperty(JavaConfigConstants.JavacPath, _PersistStorageType.PST_PROJECT_FILE);
        }

        protected override bool ApplyChanges()
        {
            SetProperty(JavaConfigConstants.JavacPath, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.JavacPath);
            return true;
        }

        protected override JavaPropertyPagePanel CreatePropertyPagePanel()
        {
            return new JavaGeneralPropertyPagePanel(this);
        }
    }
}
