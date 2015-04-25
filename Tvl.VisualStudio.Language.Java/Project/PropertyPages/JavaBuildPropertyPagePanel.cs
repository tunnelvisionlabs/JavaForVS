namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System;
    using _PersistStorageType = Microsoft.VisualStudio.Shell.Interop._PersistStorageType;
    using CommandLineBuilder = Microsoft.Build.Utilities.CommandLineBuilder;
    using Path = System.IO.Path;

    public partial class JavaBuildPropertyPagePanel : JavaPropertyPagePanel
    {
        public JavaBuildPropertyPagePanel()
            : this(null)
        {
        }

        public JavaBuildPropertyPagePanel(JavaPropertyPage parentPropertyPage)
            : base(parentPropertyPage)
        {
            InitializeComponent();
            UpdateStates();
            RefreshCommandLine();
        }

        public new JavaBuildPropertyPage ParentPropertyPage
        {
            get
            {
                return base.ParentPropertyPage as JavaBuildPropertyPage;
            }
        }

        public string SourceRelease
        {
            get
            {
                return (cmbSourceRelease.SelectedItem ?? "").ToString();
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Default";

                cmbSourceRelease.SelectedItem = value;
            }
        }

        public string TargetRelease
        {
            get
            {
                return (cmbTargetRelease.SelectedItem ?? "").ToString();
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Default";

                cmbTargetRelease.SelectedItem = value;
            }
        }

        public string Encoding
        {
            get
            {
                return txtEncoding.Text;
            }

            set
            {
                txtEncoding.Text = value ?? string.Empty;
            }
        }

        public DebuggingInformation DebuggingInformation
        {
            get
            {
                if (btnDebugInfoAll.Checked)
                    return DebuggingInformation.All;
                else if (btnDebugInfoNone.Checked)
                    return DebuggingInformation.None;
                else if (btnDebugInfoSpecific.Checked)
                    return DebuggingInformation.Specific;
                else
                    return DebuggingInformation.Default;
            }

            set
            {
                switch (value)
                {
                case DebuggingInformation.All:
                    btnDebugInfoAll.Checked = true;
                    break;

                case DebuggingInformation.None:
                    btnDebugInfoNone.Checked = true;
                    break;

                case DebuggingInformation.Specific:
                    btnDebugInfoSpecific.Checked = true;
                    break;

                case DebuggingInformation.Default:
                default:
                    btnDebugInfoDefault.Checked = true;
                    break;
                }
            }
        }

        public string SpecificDebuggingInformation
        {
            get
            {
                return txtSpecificDebugInfo.Text;
            }

            set
            {
                txtSpecificDebugInfo.Text = value ?? string.Empty;
            }
        }

        public bool ShowWarnings
        {
            get
            {
                return chkShowWarnings.Checked;
            }

            set
            {
                chkShowWarnings.Checked = value;
            }
        }

        public bool ShowAllWarnings
        {
            get
            {
                return chkAllWarnings.Checked;
            }

            set
            {
                chkAllWarnings.Checked = value;
            }
        }

        public WarningsAsErrors WarningsAsErrors
        {
            get
            {
                if (btnWarnAsErrorSpecific.Checked)
                    return WarningsAsErrors.Specific;
                else if (btnWarnAsErrorAll.Checked)
                    return WarningsAsErrors.All;
                else
                    return WarningsAsErrors.None;
            }

            set
            {
                switch (value)
                {
                case WarningsAsErrors.All:
                    btnWarnAsErrorAll.Checked = true;
                    break;

                case WarningsAsErrors.Specific:
                    btnWarnAsErrorSpecific.Checked = true;
                    break;

                case WarningsAsErrors.None:
                default:
                    btnWarnAsErrorNone.Checked = true;
                    break;
                }
            }
        }

        public string SpecificWarningsAsErrors
        {
            get
            {
                return txtSpecificWarningsAsErrors.Text;
            }

            set
            {
                txtSpecificWarningsAsErrors.Text = value ?? string.Empty;
            }
        }

        public string OutputPath
        {
            get
            {
                return txtOutputPath.Text;
            }

            set
            {
                txtOutputPath.Text = value;
            }
        }

        public string ExtraArguments
        {
            get
            {
                return txtBuildExtraOptions.Text;
            }

            set
            {
                txtBuildExtraOptions.Text = value ?? string.Empty;
            }
        }

        internal void RefreshCommandLine()
        {
            CommandLineBuilder commandLine = new CommandLineBuilder();

            string javacPath = null;
            if (ParentPropertyPage.ProjectManager != null && ParentPropertyPage.ProjectManager.SharedBuildOptions.General != null)
                javacPath = ParentPropertyPage.ProjectManager.SharedBuildOptions.General.JavacPath;
            if (javacPath == null)
                javacPath = ParentPropertyPage.GetConfigProperty(JavaConfigConstants.JavacPath, _PersistStorageType.PST_PROJECT_FILE);

            string fullucc = javacPath;
            try
            {
                if (!string.IsNullOrEmpty(fullucc) && !Path.IsPathRooted(fullucc) && ParentPropertyPage.ProjectManager != null)
                {
                    fullucc = Path.Combine(ParentPropertyPage.ProjectManager.ProjectFolder, javacPath);
                }
            }
            catch (ArgumentException)
            {
                fullucc = javacPath;
            }

            if (string.IsNullOrEmpty(fullucc))
            {
                var projectConfigs = ParentPropertyPage.Configurations;
                JavaProjectConfig projectConfig = projectConfigs != null && projectConfigs.Count == 1 ? (JavaProjectConfig)projectConfigs[0] : null;
                fullucc = projectConfig != null ? projectConfig.FindJavaBinary("javac.exe", true) : null;
            }

            commandLine.AppendFileNameIfNotNull(fullucc);

            commandLine.AppendSwitchIfNotNullOrEmpty("-encoding ", Encoding);

            switch (DebuggingInformation)
            {
            case DebuggingInformation.All:
                commandLine.AppendSwitch("-g");
                break;

            case DebuggingInformation.Specific:
                if (!string.IsNullOrEmpty(SpecificDebuggingInformation))
                    commandLine.AppendSwitchIfNotNull("-g:", SpecificDebuggingInformation);
                else
                    commandLine.AppendSwitch("-g:none");

                break;

            case DebuggingInformation.None:
                commandLine.AppendSwitch("-g:none");
                break;

            case DebuggingInformation.Default:
            default:
                break;
            }

            if (!string.IsNullOrEmpty(SourceRelease) && !string.Equals(SourceRelease, "Default", StringComparison.OrdinalIgnoreCase))
                commandLine.AppendSwitchIfNotNull("-source ", SourceRelease);
            if (!string.IsNullOrEmpty(TargetRelease) && !string.Equals(TargetRelease, "Default", StringComparison.OrdinalIgnoreCase))
                commandLine.AppendSwitchIfNotNull("-target ", TargetRelease);

            commandLine.AppendSwitchIfNotNullOrEmpty("-d ", OutputPath);

            if (!ShowWarnings)
            {
                commandLine.AppendSwitch("-nowarn");
            }
            else if (ShowAllWarnings)
            {
                commandLine.AppendSwitch("-Xlint");
                commandLine.AppendSwitch("-deprecation");
            }

            if (!string.IsNullOrEmpty(ExtraArguments))
                commandLine.AppendTextUnquoted(" " + ExtraArguments);

            txtBuildCommandLine.Text = commandLine.ToString();
        }

        private void UpdateStates()
        {
            chkAllWarnings.Enabled = chkShowWarnings.Checked;
            txtSpecificDebugInfo.Enabled = btnDebugInfoSpecific.Checked;
            txtSpecificWarningsAsErrors.Enabled = btnWarnAsErrorSpecific.Checked;
        }

        private void HandleStateAffectingChange(object sender, EventArgs e)
        {
            ParentPropertyPage.IsDirty = true;
            UpdateStates();
            RefreshCommandLine();
        }

        private void HandleCommandLineAffectingChange(object sender, EventArgs e)
        {
            ParentPropertyPage.IsDirty = true;
            RefreshCommandLine();
        }
    }
}
