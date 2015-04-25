namespace Tvl.VisualStudio.Language.Java.Project.PropertyPages
{
    partial class JavaApplicationPropertyPagePanel
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.Panel panel2;
            System.Windows.Forms.Panel panel3;
            System.Windows.Forms.Panel panel5;
            System.Windows.Forms.Panel panel6;
            System.Windows.Forms.Panel panel4;
            this.cmbStartupObject = new System.Windows.Forms.ComboBox();
            this.txtPackageName = new System.Windows.Forms.TextBox();
            this.cmbTargetVirtualMachine = new System.Windows.Forms.ComboBox();
            this.cmbOutputType = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            panel5 = new System.Windows.Forms.Panel();
            panel6 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(82, 13);
            label1.TabIndex = 0;
            label1.Text = "Package name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(65, 13);
            label2.TabIndex = 0;
            label2.Text = "Output type:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(76, 13);
            label3.TabIndex = 0;
            label3.Text = "Startup object:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(3, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(115, 13);
            label4.TabIndex = 0;
            label4.Text = "Target virtual machine:";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 2);
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Controls.Add(panel3, 0, 1);
            tableLayoutPanel1.Controls.Add(panel5, 1, 1);
            tableLayoutPanel1.Controls.Add(panel6, 1, 2);
            tableLayoutPanel1.Controls.Add(panel4, 1, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanel1.Size = new System.Drawing.Size(588, 145);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(this.cmbStartupObject);
            panel1.Controls.Add(label3);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(3, 99);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(288, 43);
            panel1.TabIndex = 5;
            // 
            // cmbStartupObject
            // 
            this.cmbStartupObject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStartupObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartupObject.FormattingEnabled = true;
            this.cmbStartupObject.Location = new System.Drawing.Point(3, 16);
            this.cmbStartupObject.Name = "cmbStartupObject";
            this.cmbStartupObject.Size = new System.Drawing.Size(282, 21);
            this.cmbStartupObject.TabIndex = 1;
            this.cmbStartupObject.SelectedIndexChanged += new System.EventHandler(this.HandleBuildSettingChanged);
            // 
            // panel2
            // 
            panel2.Controls.Add(this.txtPackageName);
            panel2.Controls.Add(label1);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(288, 42);
            panel2.TabIndex = 0;
            // 
            // txtPackageName
            // 
            this.txtPackageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPackageName.Location = new System.Drawing.Point(3, 16);
            this.txtPackageName.Name = "txtPackageName";
            this.txtPackageName.Size = new System.Drawing.Size(282, 20);
            this.txtPackageName.TabIndex = 1;
            this.txtPackageName.TextChanged += new System.EventHandler(this.HandleBuildSettingChanged);
            // 
            // panel3
            // 
            panel3.Controls.Add(this.cmbTargetVirtualMachine);
            panel3.Controls.Add(label4);
            panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            panel3.Location = new System.Drawing.Point(3, 51);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(288, 42);
            panel3.TabIndex = 3;
            // 
            // cmbTargetVirtualMachine
            // 
            this.cmbTargetVirtualMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTargetVirtualMachine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetVirtualMachine.FormattingEnabled = true;
            this.cmbTargetVirtualMachine.Location = new System.Drawing.Point(3, 16);
            this.cmbTargetVirtualMachine.Name = "cmbTargetVirtualMachine";
            this.cmbTargetVirtualMachine.Size = new System.Drawing.Size(282, 21);
            this.cmbTargetVirtualMachine.TabIndex = 1;
            this.cmbTargetVirtualMachine.SelectedIndexChanged += new System.EventHandler(this.HandleBuildSettingChanged);
            // 
            // panel5
            // 
            panel5.Controls.Add(label2);
            panel5.Controls.Add(this.cmbOutputType);
            panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            panel5.Location = new System.Drawing.Point(297, 51);
            panel5.Name = "panel5";
            panel5.Size = new System.Drawing.Size(288, 42);
            panel5.TabIndex = 4;
            // 
            // cmbOutputType
            // 
            this.cmbOutputType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbOutputType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutputType.FormattingEnabled = true;
            this.cmbOutputType.Location = new System.Drawing.Point(3, 16);
            this.cmbOutputType.Name = "cmbOutputType";
            this.cmbOutputType.Size = new System.Drawing.Size(282, 21);
            this.cmbOutputType.TabIndex = 1;
            this.cmbOutputType.SelectedIndexChanged += new System.EventHandler(this.HandleBuildSettingChanged);
            // 
            // panel6
            // 
            panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            panel6.Location = new System.Drawing.Point(297, 99);
            panel6.Name = "panel6";
            panel6.Size = new System.Drawing.Size(288, 43);
            panel6.TabIndex = 5;
            // 
            // panel4
            // 
            panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            panel4.Location = new System.Drawing.Point(297, 3);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(288, 42);
            panel4.TabIndex = 1;
            // 
            // JavaApplicationPropertyPagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(tableLayoutPanel1);
            this.Name = "JavaApplicationPropertyPagePanel";
            this.Size = new System.Drawing.Size(594, 241);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbOutputType;
        private System.Windows.Forms.ComboBox cmbStartupObject;
        private System.Windows.Forms.TextBox txtPackageName;
        private System.Windows.Forms.ComboBox cmbTargetVirtualMachine;
    }
}
