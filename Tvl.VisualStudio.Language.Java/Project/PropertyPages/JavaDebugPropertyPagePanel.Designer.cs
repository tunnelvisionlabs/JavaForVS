namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    partial class JavaDebugPropertyPagePanel
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label5;
            this.cmdDebugAgent = new System.Windows.Forms.ComboBox();
            this.txtRemoteMachine = new System.Windows.Forms.TextBox();
            this.chkUseRemoteMachine = new System.Windows.Forms.CheckBox();
            this.btnBrowseWorkingDirectory = new System.Windows.Forms.Button();
            this.txtWorkingDirectory = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtExtraOptions = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStartBrowser = new System.Windows.Forms.RadioButton();
            this.btnStartProgram = new System.Windows.Forms.RadioButton();
            this.btnStartClass = new System.Windows.Forms.RadioButton();
            this.txtStartBrowser = new System.Windows.Forms.TextBox();
            this.txtStartProgram = new System.Windows.Forms.TextBox();
            this.txtStartClass = new System.Windows.Forms.TextBox();
            this.txtCommandLine = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtAgentArguments = new System.Windows.Forms.TextBox();
            this.txtJvmArguments = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(39, 192);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(77, 13);
            label3.TabIndex = 2;
            label3.Text = "Command Line";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(39, 160);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(72, 13);
            label2.TabIndex = 0;
            label2.Text = "Debug agent:";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox2.Controls.Add(this.cmdDebugAgent);
            groupBox2.Controls.Add(this.txtRemoteMachine);
            groupBox2.Controls.Add(this.chkUseRemoteMachine);
            groupBox2.Controls.Add(this.btnBrowseWorkingDirectory);
            groupBox2.Controls.Add(this.txtWorkingDirectory);
            groupBox2.Controls.Add(this.label4);
            groupBox2.Controls.Add(this.txtExtraOptions);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new System.Drawing.Point(3, 112);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(553, 193);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Start Options";
            // 
            // cmdDebugAgent
            // 
            this.cmdDebugAgent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdDebugAgent.FormattingEnabled = true;
            this.cmdDebugAgent.Items.AddRange(new object[] {
            "High-performance debug agent (default)",
            "JDWP debugging (compatibility mode)"});
            this.cmdDebugAgent.Location = new System.Drawing.Point(190, 157);
            this.cmdDebugAgent.Name = "cmdDebugAgent";
            this.cmdDebugAgent.Size = new System.Drawing.Size(318, 21);
            this.cmdDebugAgent.TabIndex = 7;
            this.cmdDebugAgent.SelectedValueChanged += new System.EventHandler(this.HandleStateAffectingChange);
            // 
            // txtRemoteMachine
            // 
            this.txtRemoteMachine.Location = new System.Drawing.Point(190, 131);
            this.txtRemoteMachine.Name = "txtRemoteMachine";
            this.txtRemoteMachine.Size = new System.Drawing.Size(318, 20);
            this.txtRemoteMachine.TabIndex = 6;
            this.txtRemoteMachine.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // chkUseRemoteMachine
            // 
            this.chkUseRemoteMachine.AutoSize = true;
            this.chkUseRemoteMachine.Location = new System.Drawing.Point(42, 133);
            this.chkUseRemoteMachine.Name = "chkUseRemoteMachine";
            this.chkUseRemoteMachine.Size = new System.Drawing.Size(126, 17);
            this.chkUseRemoteMachine.TabIndex = 5;
            this.chkUseRemoteMachine.Text = "Use remote machine:";
            this.chkUseRemoteMachine.UseVisualStyleBackColor = true;
            this.chkUseRemoteMachine.CheckedChanged += new System.EventHandler(this.HandleStateAffectingChange);
            // 
            // btnBrowseWorkingDirectory
            // 
            this.btnBrowseWorkingDirectory.Location = new System.Drawing.Point(514, 103);
            this.btnBrowseWorkingDirectory.Name = "btnBrowseWorkingDirectory";
            this.btnBrowseWorkingDirectory.Size = new System.Drawing.Size(27, 23);
            this.btnBrowseWorkingDirectory.TabIndex = 4;
            this.btnBrowseWorkingDirectory.Text = "...";
            this.btnBrowseWorkingDirectory.UseVisualStyleBackColor = true;
            // 
            // txtWorkingDirectory
            // 
            this.txtWorkingDirectory.Location = new System.Drawing.Point(190, 105);
            this.txtWorkingDirectory.Name = "txtWorkingDirectory";
            this.txtWorkingDirectory.Size = new System.Drawing.Size(318, 20);
            this.txtWorkingDirectory.TabIndex = 3;
            this.txtWorkingDirectory.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Working directory:";
            // 
            // txtExtraOptions
            // 
            this.txtExtraOptions.Location = new System.Drawing.Point(190, 19);
            this.txtExtraOptions.Multiline = true;
            this.txtExtraOptions.Name = "txtExtraOptions";
            this.txtExtraOptions.Size = new System.Drawing.Size(318, 80);
            this.txtExtraOptions.TabIndex = 1;
            this.txtExtraOptions.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(39, 22);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(134, 13);
            label1.TabIndex = 0;
            label1.Text = "Virtual machine arguments:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(39, 108);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(90, 13);
            label5.TabIndex = 0;
            label5.Text = "Agent arguments:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnStartBrowser);
            this.groupBox1.Controls.Add(this.btnStartProgram);
            this.groupBox1.Controls.Add(this.btnStartClass);
            this.groupBox1.Controls.Add(this.txtStartBrowser);
            this.groupBox1.Controls.Add(this.txtStartProgram);
            this.groupBox1.Controls.Add(this.txtStartClass);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(553, 103);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Start Action";
            // 
            // btnStartBrowser
            // 
            this.btnStartBrowser.AutoSize = true;
            this.btnStartBrowser.Location = new System.Drawing.Point(42, 73);
            this.btnStartBrowser.Name = "btnStartBrowser";
            this.btnStartBrowser.Size = new System.Drawing.Size(137, 17);
            this.btnStartBrowser.TabIndex = 4;
            this.btnStartBrowser.TabStop = true;
            this.btnStartBrowser.Text = "Start browser with URL:";
            this.btnStartBrowser.UseVisualStyleBackColor = true;
            this.btnStartBrowser.CheckedChanged += new System.EventHandler(this.HandleStateAffectingChange);
            // 
            // btnStartProgram
            // 
            this.btnStartProgram.AutoSize = true;
            this.btnStartProgram.Location = new System.Drawing.Point(42, 47);
            this.btnStartProgram.Name = "btnStartProgram";
            this.btnStartProgram.Size = new System.Drawing.Size(131, 17);
            this.btnStartProgram.TabIndex = 2;
            this.btnStartProgram.TabStop = true;
            this.btnStartProgram.Text = "Start external program:";
            this.btnStartProgram.UseVisualStyleBackColor = true;
            this.btnStartProgram.CheckedChanged += new System.EventHandler(this.HandleStateAffectingChange);
            // 
            // btnStartClass
            // 
            this.btnStartClass.AutoSize = true;
            this.btnStartClass.Location = new System.Drawing.Point(42, 21);
            this.btnStartClass.Name = "btnStartClass";
            this.btnStartClass.Size = new System.Drawing.Size(142, 17);
            this.btnStartClass.TabIndex = 0;
            this.btnStartClass.TabStop = true;
            this.btnStartClass.Text = "Start class within project:";
            this.btnStartClass.UseVisualStyleBackColor = true;
            this.btnStartClass.CheckedChanged += new System.EventHandler(this.HandleStateAffectingChange);
            // 
            // txtStartBrowser
            // 
            this.txtStartBrowser.Location = new System.Drawing.Point(190, 72);
            this.txtStartBrowser.Name = "txtStartBrowser";
            this.txtStartBrowser.Size = new System.Drawing.Size(318, 20);
            this.txtStartBrowser.TabIndex = 5;
            this.txtStartBrowser.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // txtStartProgram
            // 
            this.txtStartProgram.Location = new System.Drawing.Point(190, 46);
            this.txtStartProgram.Name = "txtStartProgram";
            this.txtStartProgram.Size = new System.Drawing.Size(318, 20);
            this.txtStartProgram.TabIndex = 3;
            this.txtStartProgram.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // txtStartClass
            // 
            this.txtStartClass.Location = new System.Drawing.Point(190, 20);
            this.txtStartClass.Name = "txtStartClass";
            this.txtStartClass.Size = new System.Drawing.Size(318, 20);
            this.txtStartClass.TabIndex = 1;
            this.txtStartClass.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // txtCommandLine
            // 
            this.txtCommandLine.Location = new System.Drawing.Point(42, 208);
            this.txtCommandLine.Multiline = true;
            this.txtCommandLine.Name = "txtCommandLine";
            this.txtCommandLine.ReadOnly = true;
            this.txtCommandLine.Size = new System.Drawing.Size(355, 98);
            this.txtCommandLine.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtCommandLine);
            this.groupBox3.Controls.Add(label3);
            this.groupBox3.Controls.Add(this.txtAgentArguments);
            this.groupBox3.Controls.Add(label5);
            this.groupBox3.Controls.Add(this.txtJvmArguments);
            this.groupBox3.Controls.Add(label1);
            this.groupBox3.Location = new System.Drawing.Point(3, 311);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(553, 320);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Java Virtual Machine";
            // 
            // txtAgentArguments
            // 
            this.txtAgentArguments.Location = new System.Drawing.Point(190, 105);
            this.txtAgentArguments.Multiline = true;
            this.txtAgentArguments.Name = "txtAgentArguments";
            this.txtAgentArguments.Size = new System.Drawing.Size(318, 80);
            this.txtAgentArguments.TabIndex = 1;
            this.txtAgentArguments.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // txtJvmArguments
            // 
            this.txtJvmArguments.Location = new System.Drawing.Point(190, 19);
            this.txtJvmArguments.Multiline = true;
            this.txtJvmArguments.Name = "txtJvmArguments";
            this.txtJvmArguments.Size = new System.Drawing.Size(318, 80);
            this.txtJvmArguments.TabIndex = 1;
            this.txtJvmArguments.TextChanged += new System.EventHandler(this.HandleCommandLineAffectingChange);
            // 
            // JavaDebugPropertyPagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoSize = true;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(563, 533);
            this.Name = "JavaDebugPropertyPagePanel";
            this.Size = new System.Drawing.Size(563, 645);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtExtraOptions;
        private System.Windows.Forms.TextBox txtCommandLine;
        private System.Windows.Forms.RadioButton btnStartBrowser;
        private System.Windows.Forms.RadioButton btnStartProgram;
        private System.Windows.Forms.RadioButton btnStartClass;
        private System.Windows.Forms.TextBox txtStartBrowser;
        private System.Windows.Forms.TextBox txtStartProgram;
        private System.Windows.Forms.TextBox txtStartClass;
        private System.Windows.Forms.TextBox txtRemoteMachine;
        private System.Windows.Forms.CheckBox chkUseRemoteMachine;
        private System.Windows.Forms.Button btnBrowseWorkingDirectory;
        private System.Windows.Forms.TextBox txtWorkingDirectory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtJvmArguments;
        private System.Windows.Forms.TextBox txtAgentArguments;
        private System.Windows.Forms.ComboBox cmdDebugAgent;
    }
}
