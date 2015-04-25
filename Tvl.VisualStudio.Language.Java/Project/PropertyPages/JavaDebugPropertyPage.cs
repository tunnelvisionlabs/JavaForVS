namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System;
    using System.Runtime.InteropServices;
    using _PersistStorageType = Microsoft.VisualStudio.Shell.Interop._PersistStorageType;

    [ComVisible(true)]
    [Guid(JavaProjectConstants.JavaDebugPropertyPageGuidString)]
    public class JavaDebugPropertyPage : JavaPropertyPage
    {
        public JavaDebugPropertyPage()
        {
            PageName = JavaConfigConstants.PageNameDebug;
        }

        public new JavaDebugPropertyPagePanel PropertyPagePanel
        {
            get
            {
                return (JavaDebugPropertyPagePanel)base.PropertyPagePanel;
            }
        }

        protected override JavaPropertyPagePanel CreatePropertyPagePanel()
        {
            return new JavaDebugPropertyPagePanel(this);
        }

        protected override void BindProperties()
        {
            if (ProjectManager != null)
                ProjectManager.SharedBuildOptions.Debug = PropertyPagePanel;

            StartAction startAction;
            if (!Enum.TryParse(GetConfigProperty(JavaConfigConstants.DebugStartAction, _PersistStorageType.PST_USER_FILE), out startAction))
                startAction = StartAction.Class;

            PropertyPagePanel.StartAction = startAction;
            PropertyPagePanel.StartClass = GetConfigProperty(JavaConfigConstants.DebugStartClass, _PersistStorageType.PST_USER_FILE);
            PropertyPagePanel.StartProgram = GetConfigProperty(JavaConfigConstants.DebugStartProgram, _PersistStorageType.PST_USER_FILE);
            PropertyPagePanel.StartBrowserUrl = GetConfigProperty(JavaConfigConstants.DebugStartBrowserUrl, _PersistStorageType.PST_USER_FILE);

            PropertyPagePanel.ExtraArguments = GetConfigProperty(JavaConfigConstants.DebugExtraArgs, _PersistStorageType.PST_USER_FILE);
            PropertyPagePanel.WorkingDirectory = GetConfigProperty(JavaConfigConstants.DebugWorkingDirectory, _PersistStorageType.PST_USER_FILE);
            PropertyPagePanel.UseRemoteMachine = GetConfigPropertyBoolean(JavaConfigConstants.DebugUseRemoteMachine, _PersistStorageType.PST_USER_FILE);
            PropertyPagePanel.RemoteMachineName = GetConfigProperty(JavaConfigConstants.DebugRemoteMachineName, _PersistStorageType.PST_USER_FILE);

            string debugAgentName = GetConfigProperty(JavaConfigConstants.DebugAgent, _PersistStorageType.PST_USER_FILE);
            DebugAgent debugAgent;
            if (!Enum.TryParse(debugAgentName, out debugAgent))
                debugAgent = DebugAgent.CustomJvmti;

            PropertyPagePanel.DebugAgent = debugAgent;

            PropertyPagePanel.VirtualMachineArguments = GetConfigProperty(JavaConfigConstants.DebugJvmArguments, _PersistStorageType.PST_USER_FILE);
            PropertyPagePanel.AgentArguments = GetConfigProperty(JavaConfigConstants.DebugAgentArguments, _PersistStorageType.PST_USER_FILE);
        }

        protected override bool ApplyChanges()
        {
            SetConfigProperty(JavaConfigConstants.DebugStartAction, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.StartAction.ToString());
            SetConfigProperty(JavaConfigConstants.DebugStartClass, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.StartClass);
            SetConfigProperty(JavaConfigConstants.DebugStartProgram, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.StartProgram);
            SetConfigProperty(JavaConfigConstants.DebugStartBrowserUrl, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.StartBrowserUrl);

            SetConfigProperty(JavaConfigConstants.DebugExtraArgs, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.ExtraArguments);
            SetConfigProperty(JavaConfigConstants.DebugWorkingDirectory, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.WorkingDirectory);
            SetConfigProperty(JavaConfigConstants.DebugUseRemoteMachine, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.UseRemoteMachine);
            SetConfigProperty(JavaConfigConstants.DebugRemoteMachineName, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.RemoteMachineName);
            SetConfigProperty(JavaConfigConstants.DebugAgent, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.DebugAgent.ToString());

            SetConfigProperty(JavaConfigConstants.DebugJvmArguments, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.VirtualMachineArguments);
            SetConfigProperty(JavaConfigConstants.DebugAgentArguments, _PersistStorageType.PST_USER_FILE, PropertyPagePanel.AgentArguments);

            return true;
        }
    }
}
