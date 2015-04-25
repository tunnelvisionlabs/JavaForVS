namespace Tvl.VisualStudio.Language.Java.Project.Controls
{
    partial class FileBrowserTextBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FileBrowserTextBox ) );
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.fileBrowserDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // fileTextBox
            // 
            resources.ApplyResources( this.fileTextBox, "fileTextBox" );
            this.fileTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.fileTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.fileTextBox.MinimumSize = new System.Drawing.Size( 4, 21 );
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.TextChanged += new System.EventHandler( this.OnFolderTextBoxTextChanged );
            // 
            // browseButton
            // 
            resources.ApplyResources( this.browseButton, "browseButton" );
            this.browseButton.MinimumSize = new System.Drawing.Size( 29, 23 );
            this.browseButton.Name = "browseButton";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler( this.OnBrowseButtonClick );
            // 
            // FileBrowserTextBox
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.browseButton );
            this.Controls.Add( this.fileTextBox );
            this.MinimumSize = new System.Drawing.Size( 64, 23 );
            this.Name = "FileBrowserTextBox";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fileTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.OpenFileDialog fileBrowserDialog;
    }
}
