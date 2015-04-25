namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    partial class JavaBuildPropertyPagePanel
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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.GroupBox groupBox4;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.GroupBox groupBox5;
            System.Windows.Forms.GroupBox groupBox6;
            this.cmbTargetRelease = new System.Windows.Forms.ComboBox();
            this.cmbSourceRelease = new System.Windows.Forms.ComboBox();
            this.txtEncoding = new System.Windows.Forms.TextBox();
            this.chkAllWarnings = new System.Windows.Forms.CheckBox();
            this.chkShowWarnings = new System.Windows.Forms.CheckBox();
            this.txtSpecificWarningsAsErrors = new System.Windows.Forms.TextBox();
            this.btnWarnAsErrorSpecific = new System.Windows.Forms.RadioButton();
            this.btnWarnAsErrorAll = new System.Windows.Forms.RadioButton();
            this.btnWarnAsErrorNone = new System.Windows.Forms.RadioButton();
            this.btnBrowseOutputPath = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.txtSpecificDebugInfo = new System.Windows.Forms.TextBox();
            this.btnDebugInfoSpecific = new System.Windows.Forms.RadioButton();
            this.btnDebugInfoNone = new System.Windows.Forms.RadioButton();
            this.btnDebugInfoAll = new System.Windows.Forms.RadioButton();
            this.btnDebugInfoDefault = new System.Windows.Forms.RadioButton();
            this.txtBuildExtraOptions = new System.Windows.Forms.TextBox();
            this.txtBuildCommandLine = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            groupBox4 = new System.Windows.Forms.GroupBox();
            label2 = new System.Windows.Forms.Label();
            groupBox5 = new System.Windows.Forms.GroupBox();
            groupBox6 = new System.Windows.Forms.GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(28, 16);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(76, 13);
            label3.TabIndex = 0;
            label3.Text = "Command line:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(28, 133);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(93, 13);
            label1.TabIndex = 2;
            label1.Text = "Additional options:";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Controls.Add(this.cmbTargetRelease);
            groupBox1.Controls.Add(this.cmbSourceRelease);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(this.txtEncoding);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Location = new System.Drawing.Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(588, 105);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "General";
            // 
            // cmbTargetRelease
            // 
            this.cmbTargetRelease.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetRelease.FormattingEnabled = true;
            this.cmbTargetRelease.Items.AddRange(new object[] {
            "Default",
            "1.3",
            "1.4",
            "1.5",
            "1.6"});
            this.cmbTargetRelease.Location = new System.Drawing.Point(186, 47);
            this.cmbTargetRelease.Name = "cmbTargetRelease";
            this.cmbTargetRelease.Size = new System.Drawing.Size(137, 21);
            this.cmbTargetRelease.TabIndex = 3;
            this.cmbTargetRelease.SelectedIndexChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // cmbSourceRelease
            // 
            this.cmbSourceRelease.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceRelease.Items.AddRange(new object[] {
            "Default",
            "1.3",
            "1.4",
            "1.5",
            "1.6"});
            this.cmbSourceRelease.Location = new System.Drawing.Point(186, 21);
            this.cmbSourceRelease.Name = "cmbSourceRelease";
            this.cmbSourceRelease.Size = new System.Drawing.Size(137, 21);
            this.cmbSourceRelease.TabIndex = 1;
            this.cmbSourceRelease.SelectedIndexChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(28, 77);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(55, 13);
            label6.TabIndex = 4;
            label6.Text = "Encoding:";
            // 
            // txtEncoding
            // 
            this.txtEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEncoding.Location = new System.Drawing.Point(185, 74);
            this.txtEncoding.Name = "txtEncoding";
            this.txtEncoding.Size = new System.Drawing.Size(258, 20);
            this.txtEncoding.TabIndex = 5;
            this.txtEncoding.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(28, 50);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(78, 13);
            label5.TabIndex = 2;
            label5.Text = "Target release:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(28, 24);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(81, 13);
            label4.TabIndex = 0;
            label4.Text = "Source release:";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox2.Controls.Add(this.chkAllWarnings);
            groupBox2.Controls.Add(this.chkShowWarnings);
            groupBox2.Location = new System.Drawing.Point(3, 240);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(588, 67);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Errors and warnings";
            // 
            // chkAllWarnings
            // 
            this.chkAllWarnings.AutoSize = true;
            this.chkAllWarnings.Location = new System.Drawing.Point(50, 42);
            this.chkAllWarnings.Name = "chkAllWarnings";
            this.chkAllWarnings.Size = new System.Drawing.Size(169, 17);
            this.chkAllWarnings.TabIndex = 1;
            this.chkAllWarnings.Text = "Enable non standard warnings";
            this.chkAllWarnings.UseVisualStyleBackColor = true;
            this.chkAllWarnings.CheckedChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // chkShowWarnings
            // 
            this.chkShowWarnings.AutoSize = true;
            this.chkShowWarnings.Location = new System.Drawing.Point(31, 19);
            this.chkShowWarnings.Name = "chkShowWarnings";
            this.chkShowWarnings.Size = new System.Drawing.Size(143, 17);
            this.chkShowWarnings.TabIndex = 0;
            this.chkShowWarnings.Text = "Show warning messages";
            this.chkShowWarnings.UseVisualStyleBackColor = true;
            this.chkShowWarnings.CheckedChanged += new System.EventHandler(this.HandleStateAffectingChange);
            // 
            // groupBox3
            // 
            groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox3.Controls.Add(this.txtSpecificWarningsAsErrors);
            groupBox3.Controls.Add(this.btnWarnAsErrorSpecific);
            groupBox3.Controls.Add(this.btnWarnAsErrorAll);
            groupBox3.Controls.Add(this.btnWarnAsErrorNone);
            groupBox3.Location = new System.Drawing.Point(3, 313);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(588, 100);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Treat warnings as errors";
            // 
            // txtSpecificWarningsAsErrors
            // 
            this.txtSpecificWarningsAsErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpecificWarningsAsErrors.Location = new System.Drawing.Point(186, 65);
            this.txtSpecificWarningsAsErrors.Name = "txtSpecificWarningsAsErrors";
            this.txtSpecificWarningsAsErrors.Size = new System.Drawing.Size(257, 20);
            this.txtSpecificWarningsAsErrors.TabIndex = 3;
            this.txtSpecificWarningsAsErrors.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // btnWarnAsErrorSpecific
            // 
            this.btnWarnAsErrorSpecific.AutoSize = true;
            this.btnWarnAsErrorSpecific.Location = new System.Drawing.Point(31, 66);
            this.btnWarnAsErrorSpecific.Name = "btnWarnAsErrorSpecific";
            this.btnWarnAsErrorSpecific.Size = new System.Drawing.Size(111, 17);
            this.btnWarnAsErrorSpecific.TabIndex = 2;
            this.btnWarnAsErrorSpecific.TabStop = true;
            this.btnWarnAsErrorSpecific.Text = "Specific warnings:";
            this.btnWarnAsErrorSpecific.UseVisualStyleBackColor = true;
            this.btnWarnAsErrorSpecific.CheckedChanged += new System.EventHandler(this.HandleStateAffectingChange);
            // 
            // btnWarnAsErrorAll
            // 
            this.btnWarnAsErrorAll.AutoSize = true;
            this.btnWarnAsErrorAll.Location = new System.Drawing.Point(31, 43);
            this.btnWarnAsErrorAll.Name = "btnWarnAsErrorAll";
            this.btnWarnAsErrorAll.Size = new System.Drawing.Size(36, 17);
            this.btnWarnAsErrorAll.TabIndex = 1;
            this.btnWarnAsErrorAll.TabStop = true;
            this.btnWarnAsErrorAll.Text = "All";
            this.btnWarnAsErrorAll.UseVisualStyleBackColor = true;
            this.btnWarnAsErrorAll.CheckedChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // btnWarnAsErrorNone
            // 
            this.btnWarnAsErrorNone.AutoSize = true;
            this.btnWarnAsErrorNone.Location = new System.Drawing.Point(31, 20);
            this.btnWarnAsErrorNone.Name = "btnWarnAsErrorNone";
            this.btnWarnAsErrorNone.Size = new System.Drawing.Size(51, 17);
            this.btnWarnAsErrorNone.TabIndex = 0;
            this.btnWarnAsErrorNone.TabStop = true;
            this.btnWarnAsErrorNone.Text = "None";
            this.btnWarnAsErrorNone.UseVisualStyleBackColor = true;
            this.btnWarnAsErrorNone.CheckedChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // groupBox4
            // 
            groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox4.Controls.Add(this.btnBrowseOutputPath);
            groupBox4.Controls.Add(this.txtOutputPath);
            groupBox4.Controls.Add(label2);
            groupBox4.Location = new System.Drawing.Point(3, 419);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(588, 58);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "Output";
            // 
            // btnBrowseOutputPath
            // 
            this.btnBrowseOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseOutputPath.Enabled = false;
            this.btnBrowseOutputPath.Location = new System.Drawing.Point(449, 21);
            this.btnBrowseOutputPath.Name = "btnBrowseOutputPath";
            this.btnBrowseOutputPath.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseOutputPath.TabIndex = 2;
            this.btnBrowseOutputPath.Text = "Browse...";
            this.btnBrowseOutputPath.UseVisualStyleBackColor = true;
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputPath.Location = new System.Drawing.Point(186, 23);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(257, 20);
            this.txtOutputPath.TabIndex = 1;
            this.txtOutputPath.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(28, 26);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(66, 13);
            label2.TabIndex = 0;
            label2.Text = "Output path:";
            // 
            // groupBox5
            // 
            groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox5.Controls.Add(this.txtSpecificDebugInfo);
            groupBox5.Controls.Add(this.btnDebugInfoSpecific);
            groupBox5.Controls.Add(this.btnDebugInfoNone);
            groupBox5.Controls.Add(this.btnDebugInfoAll);
            groupBox5.Controls.Add(this.btnDebugInfoDefault);
            groupBox5.Location = new System.Drawing.Point(3, 114);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new System.Drawing.Size(588, 120);
            groupBox5.TabIndex = 1;
            groupBox5.TabStop = false;
            groupBox5.Text = "Debugging information";
            // 
            // txtSpecificDebugInfo
            // 
            this.txtSpecificDebugInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpecificDebugInfo.Location = new System.Drawing.Point(186, 87);
            this.txtSpecificDebugInfo.Name = "txtSpecificDebugInfo";
            this.txtSpecificDebugInfo.Size = new System.Drawing.Size(257, 20);
            this.txtSpecificDebugInfo.TabIndex = 4;
            this.txtSpecificDebugInfo.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // btnDebugInfoSpecific
            // 
            this.btnDebugInfoSpecific.AutoSize = true;
            this.btnDebugInfoSpecific.Location = new System.Drawing.Point(31, 88);
            this.btnDebugInfoSpecific.Name = "btnDebugInfoSpecific";
            this.btnDebugInfoSpecific.Size = new System.Drawing.Size(120, 17);
            this.btnDebugInfoSpecific.TabIndex = 3;
            this.btnDebugInfoSpecific.TabStop = true;
            this.btnDebugInfoSpecific.Text = "Specific information:";
            this.btnDebugInfoSpecific.UseVisualStyleBackColor = true;
            this.btnDebugInfoSpecific.CheckedChanged += new System.EventHandler(this.HandleStateAffectingChange);
            // 
            // btnDebugInfoNone
            // 
            this.btnDebugInfoNone.AutoSize = true;
            this.btnDebugInfoNone.Location = new System.Drawing.Point(31, 65);
            this.btnDebugInfoNone.Name = "btnDebugInfoNone";
            this.btnDebugInfoNone.Size = new System.Drawing.Size(51, 17);
            this.btnDebugInfoNone.TabIndex = 2;
            this.btnDebugInfoNone.TabStop = true;
            this.btnDebugInfoNone.Text = "None";
            this.btnDebugInfoNone.UseVisualStyleBackColor = true;
            this.btnDebugInfoNone.CheckedChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // btnDebugInfoAll
            // 
            this.btnDebugInfoAll.AutoSize = true;
            this.btnDebugInfoAll.Location = new System.Drawing.Point(31, 42);
            this.btnDebugInfoAll.Name = "btnDebugInfoAll";
            this.btnDebugInfoAll.Size = new System.Drawing.Size(36, 17);
            this.btnDebugInfoAll.TabIndex = 1;
            this.btnDebugInfoAll.TabStop = true;
            this.btnDebugInfoAll.Text = "All";
            this.btnDebugInfoAll.UseVisualStyleBackColor = true;
            this.btnDebugInfoAll.CheckedChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // btnDebugInfoDefault
            // 
            this.btnDebugInfoDefault.AutoSize = true;
            this.btnDebugInfoDefault.Location = new System.Drawing.Point(31, 19);
            this.btnDebugInfoDefault.Name = "btnDebugInfoDefault";
            this.btnDebugInfoDefault.Size = new System.Drawing.Size(59, 17);
            this.btnDebugInfoDefault.TabIndex = 0;
            this.btnDebugInfoDefault.TabStop = true;
            this.btnDebugInfoDefault.Text = "Default";
            this.btnDebugInfoDefault.UseVisualStyleBackColor = true;
            this.btnDebugInfoDefault.CheckedChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // groupBox6
            // 
            groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox6.Controls.Add(label3);
            groupBox6.Controls.Add(label1);
            groupBox6.Controls.Add(this.txtBuildExtraOptions);
            groupBox6.Controls.Add(this.txtBuildCommandLine);
            groupBox6.Location = new System.Drawing.Point(3, 483);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new System.Drawing.Size(588, 242);
            groupBox6.TabIndex = 5;
            groupBox6.TabStop = false;
            groupBox6.Text = "Command line";
            // 
            // txtBuildExtraOptions
            // 
            this.txtBuildExtraOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBuildExtraOptions.Location = new System.Drawing.Point(31, 149);
            this.txtBuildExtraOptions.Multiline = true;
            this.txtBuildExtraOptions.Name = "txtBuildExtraOptions";
            this.txtBuildExtraOptions.Size = new System.Drawing.Size(493, 80);
            this.txtBuildExtraOptions.TabIndex = 3;
            this.txtBuildExtraOptions.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // txtBuildCommandLine
            // 
            this.txtBuildCommandLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBuildCommandLine.Location = new System.Drawing.Point(31, 32);
            this.txtBuildCommandLine.Multiline = true;
            this.txtBuildCommandLine.Name = "txtBuildCommandLine";
            this.txtBuildCommandLine.ReadOnly = true;
            this.txtBuildCommandLine.Size = new System.Drawing.Size(493, 98);
            this.txtBuildCommandLine.TabIndex = 1;
            // 
            // JavaBuildPropertyPagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(groupBox6);
            this.Controls.Add(groupBox5);
            this.Controls.Add(groupBox4);
            this.Controls.Add(groupBox3);
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox1);
            this.MinimumSize = new System.Drawing.Size(447, 326);
            this.Name = "JavaBuildPropertyPagePanel";
            this.Size = new System.Drawing.Size(594, 728);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtBuildExtraOptions;
        private System.Windows.Forms.TextBox txtBuildCommandLine;
        private System.Windows.Forms.TextBox txtSpecificWarningsAsErrors;
        private System.Windows.Forms.RadioButton btnWarnAsErrorSpecific;
        private System.Windows.Forms.RadioButton btnWarnAsErrorAll;
        private System.Windows.Forms.RadioButton btnWarnAsErrorNone;
        private System.Windows.Forms.TextBox txtEncoding;
        private System.Windows.Forms.CheckBox chkAllWarnings;
        private System.Windows.Forms.CheckBox chkShowWarnings;
        private System.Windows.Forms.Button btnBrowseOutputPath;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.TextBox txtSpecificDebugInfo;
        private System.Windows.Forms.RadioButton btnDebugInfoSpecific;
        private System.Windows.Forms.RadioButton btnDebugInfoNone;
        private System.Windows.Forms.RadioButton btnDebugInfoAll;
        private System.Windows.Forms.RadioButton btnDebugInfoDefault;
        private System.Windows.Forms.ComboBox cmbTargetRelease;
        private System.Windows.Forms.ComboBox cmbSourceRelease;

    }
}
