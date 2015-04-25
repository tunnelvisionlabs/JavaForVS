namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    partial class JavaGeneralPropertyPagePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label1;
            this.txtJavacPath = new Tvl.VisualStudio.Language.Java.Project.Controls.FileBrowserTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            groupBox1 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Controls.Add(this.txtJavacPath);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new System.Drawing.Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(386, 58);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Compiler";
            // 
            // txtJavacPath
            // 
            this.txtJavacPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJavacPath.Location = new System.Drawing.Point(156, 21);
            this.txtJavacPath.MakeRelative = true;
            this.txtJavacPath.Margin = new System.Windows.Forms.Padding(0);
            this.txtJavacPath.MinimumSize = new System.Drawing.Size(64, 23);
            this.txtJavacPath.Name = "txtJavacPath";
            this.txtJavacPath.RootFolder = null;
            this.txtJavacPath.Size = new System.Drawing.Size(201, 22);
            this.txtJavacPath.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtJavacPath, "Path to the java compiler (javac.exe).");
            this.txtJavacPath.TextChanged += new System.EventHandler(this.folderBrowserTextBox1_TextChanged);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(31, 25);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(64, 13);
            label1.TabIndex = 1;
            label1.Text = "Javac Path:";
            this.toolTip1.SetToolTip(label1, "Path to the java compiler (javac.exe).");
            // 
            // JavaGeneralPropertyPagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(groupBox1);
            this.MinimumSize = new System.Drawing.Size(392, 237);
            this.Name = "JavaGeneralPropertyPagePanel";
            this.Size = new System.Drawing.Size(392, 237);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Tvl.VisualStudio.Language.Java.Project.Controls.FileBrowserTextBox txtJavacPath;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}
