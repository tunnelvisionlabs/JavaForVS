namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Project;
    using Tvl.Collections;
    using _PersistStorageType = Microsoft.VisualStudio.Shell.Interop._PersistStorageType;

    [ComVisible(true)]
    [Guid(JavaProjectConstants.JavaApplicationPropertyPageGuidString)]
    public class JavaApplicationPropertyPage : JavaPropertyPage
    {
        private static readonly string NotSetStartupObject = string.Empty;

        private static readonly ImmutableList<string> _defaultAvailableTargetVirtualMachines =
            new ImmutableList<string>(new string[]
            {
                JavaProjectFileConstants.HotspotTargetVirtualMachine,
                JavaProjectFileConstants.JRockitTargetVirtualMachine,
            });
        private static readonly ImmutableList<string> _defaultAvailableOutputTypes =
            new ImmutableList<string>(new string[]
            {
                JavaProjectFileConstants.JavaArchiveOutputType,
            });
        private static readonly ImmutableList<string> _defaultAvailableStartupObjects =
            new ImmutableList<string>(new string[]
            {
                NotSetStartupObject,
            });

        public JavaApplicationPropertyPage()
        {
            PageName = JavaConfigConstants.PageNameApplication;
        }

        public new JavaApplicationPropertyPagePanel PropertyPagePanel
        {
            get
            {
                return (JavaApplicationPropertyPagePanel)base.PropertyPagePanel;
            }
        }

        protected override JavaPropertyPagePanel CreatePropertyPagePanel()
        {
            return new JavaApplicationPropertyPagePanel(this);
        }

        protected override void BindProperties()
        {
            // package name
            PropertyPagePanel.PackageName = GetConfigProperty(ProjectFileConstants.AssemblyName, _PersistStorageType.PST_PROJECT_FILE);

            // available items
            PropertyPagePanel.AvailableTargetVirtualMachines = _defaultAvailableTargetVirtualMachines;
            PropertyPagePanel.AvailableOutputTypes = _defaultAvailableOutputTypes;
            PropertyPagePanel.AvailableStartupObjects = _defaultAvailableStartupObjects;

            // selected items
            PropertyPagePanel.TargetVirtualMachine = GetConfigProperty(JavaConfigConstants.TargetVM, _PersistStorageType.PST_PROJECT_FILE);
            PropertyPagePanel.OutputType = GetConfigProperty(JavaConfigConstants.OutputType, _PersistStorageType.PST_PROJECT_FILE);
            PropertyPagePanel.StartupObject = GetConfigProperty(JavaConfigConstants.StartupObject, _PersistStorageType.PST_PROJECT_FILE);
        }

        protected override bool ApplyChanges()
        {
            SetConfigProperty(ProjectFileConstants.AssemblyName, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.PackageName);
            SetConfigProperty(JavaConfigConstants.TargetVM, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.TargetVirtualMachine);
            SetConfigProperty(JavaConfigConstants.OutputType, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.OutputType);
            SetConfigProperty(JavaConfigConstants.StartupObject, _PersistStorageType.PST_PROJECT_FILE, PropertyPagePanel.StartupObject);
            return true;
        }
    }
}
