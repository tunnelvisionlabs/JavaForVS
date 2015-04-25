namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System;
    using System.Runtime.InteropServices;
    using _PersistStorageType = Microsoft.VisualStudio.Shell.Interop._PersistStorageType;

    [ComVisible(true)]
    [Guid(JavaProjectConstants.JavaBuildPropertyPageGuidString)]
    public class JavaBuildPropertyPage : JavaPropertyPage
    {
        public JavaBuildPropertyPage()
        {
            PageName = JavaConfigConstants.PageNameBuild;
        }

        public new JavaBuildPropertyPagePanel PropertyPagePanel
        {
            get
            {
                return (JavaBuildPropertyPagePanel)base.PropertyPagePanel;
            }
        }

        protected override void BindProperties()
        {
            if (ProjectManager != null)
                ProjectManager.SharedBuildOptions.Build = PropertyPagePanel;

            // general
            PropertyPagePanel.SourceRelease = GetConfigProperty(JavaConfigConstants.SourceRelease, _PersistStorageType.PST_PROJECT_FILE);
            PropertyPagePanel.TargetRelease = GetConfigProperty(JavaConfigConstants.TargetRelease, _PersistStorageType.PST_PROJECT_FILE);
            PropertyPagePanel.Encoding = GetConfigProperty(JavaConfigConstants.SourceEncoding, _PersistStorageType.PST_PROJECT_FILE);

            // debugging
            DebuggingInformation info;
            if (!Enum.TryParse(GetConfigProperty(JavaConfigConstants.DebugSymbols, _PersistStorageType.PST_PROJECT_FILE), out info))
                info = DebuggingInformation.Default;

            PropertyPagePanel.DebuggingInformation = info;
            PropertyPagePanel.SpecificDebuggingInformation = GetConfigProperty(JavaConfigConstants.SpecificDebugSymbols, _PersistStorageType.PST_PROJECT_FILE);

            // warnings
            PropertyPagePanel.ShowWarnings = GetConfigPropertyBoolean(JavaConfigConstants.ShowWarnings, _PersistStorageType.PST_PROJECT_FILE);
            PropertyPagePanel.ShowAllWarnings = GetConfigPropertyBoolean(JavaConfigConstants.ShowAllWarnings, _PersistStorageType.PST_PROJECT_FILE);

            // warnings as errors
            WarningsAsErrors warnAsError;
            if (!Enum.TryParse(GetConfigProperty(JavaConfigConstants.TreatWarningsAsErrors, _PersistStorageType.PST_PROJECT_FILE), out warnAsError))
                warnAsError = WarningsAsErrors.None;

            PropertyPagePanel.WarningsAsErrors = warnAsError;
            PropertyPagePanel.SpecificWarningsAsErrors = GetConfigProperty(JavaConfigConstants.WarningsAsErrors, _PersistStorageType.PST_PROJECT_FILE);

            // output
            PropertyPagePanel.OutputPath = GetConfigProperty(JavaConfigConstants.OutputPath, _PersistStorageType.PST_PROJECT_FILE);

            // extra arguments
            PropertyPagePanel.ExtraArguments = GetConfigProperty(JavaConfigConstants.BuildArgs, _PersistStorageType.PST_PROJECT_FILE);
        }

        protected override bool ApplyChanges()
        {
            // general
            SetConfigProperty(JavaConfigConstants.SourceRelease, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.SourceRelease);
            SetConfigProperty(JavaConfigConstants.TargetRelease, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.TargetRelease);
            SetConfigProperty(JavaConfigConstants.SourceEncoding, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.Encoding);

            // debugging
            SetConfigProperty(JavaConfigConstants.DebugSymbols, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.DebuggingInformation.ToString());
            SetConfigProperty(JavaConfigConstants.SpecificDebugSymbols, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.SpecificDebuggingInformation);

            // warnings
            SetConfigProperty(JavaConfigConstants.ShowWarnings, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.ShowWarnings);
            SetConfigProperty(JavaConfigConstants.ShowAllWarnings, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.ShowAllWarnings);

            // warnings as errors
            SetConfigProperty(JavaConfigConstants.TreatWarningsAsErrors, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.WarningsAsErrors.ToString());
            SetConfigProperty(JavaConfigConstants.WarningsAsErrors, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.SpecificWarningsAsErrors);

            // output
            SetConfigProperty(JavaConfigConstants.OutputPath, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.OutputPath);

            // extra arguments
            SetConfigProperty(JavaConfigConstants.BuildArgs, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.ExtraArguments);

            return true;
        }

        protected override JavaPropertyPagePanel CreatePropertyPagePanel()
        {
            return new JavaBuildPropertyPagePanel(this);
        }
    }
}
