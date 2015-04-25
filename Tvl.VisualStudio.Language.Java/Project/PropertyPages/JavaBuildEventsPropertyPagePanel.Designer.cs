namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    partial class JavaBuildEventsPropertyPagePanel
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.txtPreBuildCommandLine = new System.Windows.Forms.TextBox();
            this.btnEditPreBuild = new System.Windows.Forms.Button();
            this.txtPostBuildCommandLine = new System.Windows.Forms.TextBox();
            this.btnEditPostBuild = new System.Windows.Forms.Button();
            this.cmbRunPostBuildWhen = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(41, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(149, 13);
            label1.TabIndex = 0;
            label1.Text = "Pre-build event command line:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(41, 170);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(154, 13);
            label2.TabIndex = 0;
            label2.Text = "Post-build event command line:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(41, 301);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(126, 13);
            label3.TabIndex = 0;
            label3.Text = "Run the post-build event:";
            // 
            // txtPreBuildCommandLine
            // 
            this.txtPreBuildCommandLine.Location = new System.Drawing.Point(44, 30);
            this.txtPreBuildCommandLine.Multiline = true;
            this.txtPreBuildCommandLine.Name = "txtPreBuildCommandLine";
            this.txtPreBuildCommandLine.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPreBuildCommandLine.Size = new System.Drawing.Size(395, 84);
            this.txtPreBuildCommandLine.TabIndex = 1;
            this.txtPreBuildCommandLine.WordWrap = false;
            this.txtPreBuildCommandLine.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnEditPreBuild
            // 
            this.btnEditPreBuild.Enabled = false;
            this.btnEditPreBuild.Location = new System.Drawing.Point(324, 120);
            this.btnEditPreBuild.Name = "btnEditPreBuild";
            this.btnEditPreBuild.Size = new System.Drawing.Size(115, 23);
            this.btnEditPreBuild.TabIndex = 2;
            this.btnEditPreBuild.Text = "Edit Pre-build ...";
            this.btnEditPreBuild.UseVisualStyleBackColor = true;
            // 
            // txtPostBuildCommandLine
            // 
            this.txtPostBuildCommandLine.Location = new System.Drawing.Point(44, 186);
            this.txtPostBuildCommandLine.Multiline = true;
            this.txtPostBuildCommandLine.Name = "txtPostBuildCommandLine";
            this.txtPostBuildCommandLine.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPostBuildCommandLine.Size = new System.Drawing.Size(395, 84);
            this.txtPostBuildCommandLine.TabIndex = 1;
            this.txtPostBuildCommandLine.WordWrap = false;
            this.txtPostBuildCommandLine.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btnEditPostBuild
            // 
            this.btnEditPostBuild.Enabled = false;
            this.btnEditPostBuild.Location = new System.Drawing.Point(324, 276);
            this.btnEditPostBuild.Name = "btnEditPostBuild";
            this.btnEditPostBuild.Size = new System.Drawing.Size(115, 23);
            this.btnEditPostBuild.TabIndex = 2;
            this.btnEditPostBuild.Text = "Edit Post-build ...";
            this.btnEditPostBuild.UseVisualStyleBackColor = true;
            // 
            // cmbRunPostBuildWhen
            // 
            this.cmbRunPostBuildWhen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRunPostBuildWhen.FormattingEnabled = true;
            this.cmbRunPostBuildWhen.Items.AddRange(new object[] {
            "Always",
            "On successful build",
            "When the build updates the project output"});
            this.cmbRunPostBuildWhen.Location = new System.Drawing.Point(44, 317);
            this.cmbRunPostBuildWhen.Name = "cmbRunPostBuildWhen";
            this.cmbRunPostBuildWhen.Size = new System.Drawing.Size(395, 21);
            this.cmbRunPostBuildWhen.TabIndex = 3;
            this.cmbRunPostBuildWhen.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // JavaBuildEventsPropertyPagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbRunPostBuildWhen);
            this.Controls.Add(this.btnEditPostBuild);
            this.Controls.Add(this.btnEditPreBuild);
            this.Controls.Add(this.txtPostBuildCommandLine);
            this.Controls.Add(this.txtPreBuildCommandLine);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.MinimumSize = new System.Drawing.Size(448, 346);
            this.Name = "JavaBuildEventsPropertyPagePanel";
            this.Size = new System.Drawing.Size(448, 346);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPreBuildCommandLine;
        private System.Windows.Forms.Button btnEditPreBuild;
        private System.Windows.Forms.TextBox txtPostBuildCommandLine;
        private System.Windows.Forms.Button btnEditPostBuild;
        private System.Windows.Forms.ComboBox cmbRunPostBuildWhen;
    }
}
