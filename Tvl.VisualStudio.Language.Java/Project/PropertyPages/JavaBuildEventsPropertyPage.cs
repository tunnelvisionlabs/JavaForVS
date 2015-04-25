namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System;
    using System.Runtime.InteropServices;
    using _PersistStorageType = Microsoft.VisualStudio.Shell.Interop._PersistStorageType;

    [ComVisible(true)]
    [Guid(JavaProjectConstants.JavaBuildEventsPropertyPageGuidString)]
    public class JavaBuildEventsPropertyPage : JavaPropertyPage
    {
        public JavaBuildEventsPropertyPage()
        {
            PageName = JavaConfigConstants.PageNameBuildEvents;
        }

        public new JavaBuildEventsPropertyPagePanel PropertyPagePanel
        {
            get
            {
                return (JavaBuildEventsPropertyPagePanel)base.PropertyPagePanel;
            }
        }

        protected override void BindProperties()
        {
            PropertyPagePanel.PreBuildEvent = GetConfigProperty(JavaConfigConstants.PreBuildEvent, _PersistStorageType.PST_PROJECT_FILE);
            PropertyPagePanel.PostBuildEvent = GetConfigProperty(JavaConfigConstants.PostBuildEvent, _PersistStorageType.PST_PROJECT_FILE);
            PropertyPagePanel.RunPostBuildEvent = GetConfigProperty(JavaConfigConstants.RunPostBuildEvent, _PersistStorageType.PST_PROJECT_FILE);
        }
        protected override bool ApplyChanges()
        {
            SetConfigProperty(JavaConfigConstants.PreBuildEvent, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.PreBuildEvent);
            SetConfigProperty(JavaConfigConstants.PostBuildEvent, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.PostBuildEvent);
            SetConfigProperty(JavaConfigConstants.RunPostBuildEvent, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.RunPostBuildEvent);
            return true;
        }

        protected override JavaPropertyPagePanel CreatePropertyPagePanel()
        {
            return new JavaBuildEventsPropertyPagePanel(this);
        }
    }
}
