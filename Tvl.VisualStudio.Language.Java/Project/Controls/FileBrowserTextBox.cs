namespace Tvl.VisualStudio.Language.Java.Project.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Url = Microsoft.VisualStudio.Shell.Url;

    /// <summary>
    /// Extends a simple text box specialized for browsing to folders. Supports auto-complete and
    /// a browse button that brings up the folder browse dialog.
    /// </summary>
    internal partial class FileBrowserTextBox : UserControl
    {
        private string _rootFolder;

        // =========================================================================================
        // Constructors
        // =========================================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderBrowserTextBox"/> class.
        /// </summary>
        public FileBrowserTextBox()
        {
            this.InitializeComponent();

            fileTextBox.Enabled = Enabled;
            browseButton.Enabled = Enabled;
        }

        // =========================================================================================
        // Events
        // =========================================================================================

        /// <summary>
        /// Occurs when the text has changed.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new event EventHandler TextChanged
        {
            add { base.TextChanged += value; }
            remove { base.TextChanged -= value; }
        }

        // =========================================================================================
        // Properties
        // =========================================================================================

        /// <summary>
        /// Gets or sets the path of the selected file.
        /// </summary>
        [Bindable(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get { return this.fileTextBox.Text; }
            set { this.fileTextBox.Text = value; }
        }

        [Bindable( true )]
        [Browsable( true )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
        [EditorBrowsable( EditorBrowsableState.Always )]
        public string RootFolder
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(_rootFolder))
                        Path.IsPathRooted(_rootFolder);
                }
                catch (ArgumentException)
                {
                    return string.Empty;
                }

                return _rootFolder;
            }

            set
            {
                _rootFolder = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        [Description("When this property is 'true', the file path will be made relative to RootFolder, when possible.")]
        public bool MakeRelative
        {
            get;
            set;
        }

        private string FullPath
        {
            get
            {
                try
                {
                    string text = Text ?? string.Empty;
                    if (!string.IsNullOrEmpty(RootFolder))
                        text = Path.Combine(RootFolder, text);

                    return Path.GetFullPath(text);
                }
                catch (ArgumentException)
                {
                    return string.Empty;
                }
            }
        }

        // =========================================================================================
        // Methods
        // =========================================================================================

        /// <summary>
        /// Sets the bounds of the control. In this case, we fix the height to the text box's height.
        /// </summary>
        /// <param name="x">The new x value.</param>
        /// <param name="y">The new y value.</param>
        /// <param name="width">The new width value.</param>
        /// <param name="height">The height value.</param>
        /// <param name="specified">A set of flags indicating which bounds to set.</param>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if ((specified & BoundsSpecified.Height) == BoundsSpecified.Height)
            {
                height = this.fileTextBox.Height + 1;
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        /// <summary>
        /// Brings up the browse folder dialog.
        /// </summary>
        /// <param name="sender">The browse button.</param>
        /// <param name="e">The <see cref="EventArgs"/> object that contains the event data.</param>
        private void OnBrowseButtonClick(object sender, EventArgs e)
        {
            // initialize the dialog to the current file (if it exists)
            string fullPath = this.FullPath;
            if (File.Exists(fullPath))
            {
                this.fileBrowserDialog.InitialDirectory = Path.GetDirectoryName(fullPath);
                this.fileBrowserDialog.FileName = Path.GetFileName(fullPath);
            }

            // show the dialog
            if (this.fileBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                fullPath = this.fileBrowserDialog.FileName;
                string path = fullPath;
                if (MakeRelative && !string.IsNullOrEmpty(RootFolder))
                {
                    string rootFolder = Path.GetFullPath(RootFolder);
                    if (Directory.Exists(rootFolder))
                    {
                        if (!rootFolder.EndsWith(Path.DirectorySeparatorChar.ToString()) && !rootFolder.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                            rootFolder = rootFolder + Path.DirectorySeparatorChar;

                        path = new Url(rootFolder).MakeRelative(new Url(fullPath));
                    }
                }

                this.fileTextBox.Text = path;
            }
        }

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event.
        /// </summary>
        /// <param name="sender">The folder text box.</param>
        /// <param name="e">The <see cref="EventArgs"/> object that contains the event data.</param>
        private void OnFolderTextBoxTextChanged(object sender, EventArgs e)
        {
            UpdateColor();
            this.OnTextChanged(EventArgs.Empty);
        }

        protected override void OnEnabledChanged( EventArgs e )
        {
            fileTextBox.Enabled = Enabled;
            browseButton.Enabled = Enabled;
            UpdateColor();
            browseButton.Invalidate();
            base.OnEnabledChanged( e );
        }

        private void UpdateColor()
        {
            if ( !Enabled )
            {
                fileTextBox.BackColor = SystemColors.ControlLight;
                fileTextBox.ForeColor = SystemColors.GrayText;
                return;
            }

            fileTextBox.ForeColor = SystemColors.ControlText;
            fileTextBox.BackColor = File.Exists(FullPath) ? SystemColors.ControlLightLight : Color.LightSalmon;
        }
    }
}
