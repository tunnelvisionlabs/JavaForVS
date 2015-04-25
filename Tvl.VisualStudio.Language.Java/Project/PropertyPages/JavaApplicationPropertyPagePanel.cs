namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Tvl.Collections;

    public partial class JavaApplicationPropertyPagePanel : JavaPropertyPagePanel
    {
        private static readonly string DisplayJavaArchiveOutputType = "Java Archive (jar)";
        private static readonly string DisplayNotSetStartupObject = "(Not Set)";

        private static readonly ImmutableList<string> _emptyList = new ImmutableList<string>(new string[0]);

        private ImmutableList<string> _availableTargetVirtualMachines = _emptyList;
        private ImmutableList<string> _availableOutputTypes = _emptyList;
        private ImmutableList<string> _availableStartupObjects = _emptyList;

        public JavaApplicationPropertyPagePanel()
            : this(null)
        {
        }

        public JavaApplicationPropertyPagePanel(JavaApplicationPropertyPage parentPropertyPage)
            : base(parentPropertyPage)
        {
            InitializeComponent();
        }

        internal new JavaApplicationPropertyPage ParentPropertyPage
        {
            get
            {
                return (JavaApplicationPropertyPage)base.ParentPropertyPage;
            }
        }

        public ImmutableList<string> AvailableTargetVirtualMachines
        {
            get
            {
                Contract.Ensures(Contract.Result<ImmutableList<string>>() != null);

                return _availableTargetVirtualMachines;
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");
                Contract.Requires<ArgumentException>(Contract.ForAll(value, i => !string.IsNullOrEmpty(i)));

                if (_availableTargetVirtualMachines.SequenceEqual(value, StringComparer.CurrentCulture))
                    return;

                _availableTargetVirtualMachines = value;
                cmbTargetVirtualMachine.Items.Clear();
                cmbTargetVirtualMachine.Items.AddRange(value.ToArray());
            }
        }

        public ImmutableList<string> AvailableOutputTypes
        {
            get
            {
                Contract.Ensures(Contract.Result<ImmutableList<string>>() != null);

                return _availableOutputTypes;
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");
                Contract.Requires<ArgumentException>(Contract.ForAll(value, i => !string.IsNullOrEmpty(i)));

                if (_availableOutputTypes.SequenceEqual(value, StringComparer.CurrentCulture))
                    return;

                _availableOutputTypes = value;
                cmbOutputType.Items.Clear();
                cmbOutputType.Items.AddRange(value.Select(i => (i == JavaProjectFileConstants.JavaArchiveOutputType) ? DisplayJavaArchiveOutputType : i).ToArray());
            }
        }

        public ImmutableList<string> AvailableStartupObjects
        {
            get
            {
                Contract.Ensures(Contract.Result<ImmutableList<string>>() != null);

                return _availableStartupObjects;
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");
                Contract.Requires<ArgumentException>(Contract.ForAll(value, i => i != null));

                if (_availableStartupObjects.SequenceEqual(value, StringComparer.CurrentCulture))
                    return;

                _availableStartupObjects = value;
                cmbStartupObject.Items.Clear();
                cmbStartupObject.Items.AddRange(value.Select(i => string.IsNullOrEmpty(i) ? DisplayNotSetStartupObject : i).ToArray());
            }
        }

        public string PackageName
        {
            get
            {
                return txtPackageName.Text;
            }

            set
            {
                txtPackageName.Text = value ?? string.Empty;
            }
        }

        public string TargetVirtualMachine
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                if (cmbTargetVirtualMachine.SelectedItem == null)
                    return string.Empty;

                return cmbTargetVirtualMachine.SelectedItem.ToString();
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");

                cmbTargetVirtualMachine.SelectedItem = value;
            }
        }

        public string OutputType
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                if (cmbOutputType.SelectedItem == null)
                    return string.Empty;

                string outputType = cmbOutputType.SelectedItem.ToString();
                if (outputType == DisplayJavaArchiveOutputType)
                    outputType = JavaProjectFileConstants.JavaArchiveOutputType;

                return outputType;
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");

                string outputType = value;
                if (outputType == JavaProjectFileConstants.JavaArchiveOutputType)
                    outputType = DisplayJavaArchiveOutputType;

                cmbOutputType.SelectedItem = outputType;
            }
        }

        public string StartupObject
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                if (cmbStartupObject.SelectedItem == null)
                    return string.Empty;

                string startupObject = cmbStartupObject.SelectedItem.ToString();
                if (startupObject == DisplayNotSetStartupObject)
                    startupObject = string.Empty;

                return startupObject;
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");

                string startupObject = value;
                if (string.IsNullOrEmpty(startupObject))
                    startupObject = DisplayNotSetStartupObject;

                cmbStartupObject.SelectedItem = startupObject;
            }
        }

        private void HandleBuildSettingChanged(object sender, EventArgs e)
        {
            ParentPropertyPage.IsDirty = true;
        }
    }
}
