namespace AbbCom.Forms
{
    partial class SystemOptions
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxStartup = new System.Windows.Forms.GroupBox();
            this.comboBoxUserName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxMachineName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxDefaultBackupPath = new System.Windows.Forms.TextBox();
            this.buttonSelectDefaultBackupPath = new System.Windows.Forms.Button();
            this.buttonRestoreDefaultBackupPath = new System.Windows.Forms.Button();
            this.groupBoxStartup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(105, 303);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(191, 303);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBoxStartup
            // 
            this.groupBoxStartup.Controls.Add(this.comboBoxUserName);
            this.groupBoxStartup.Controls.Add(this.label1);
            this.groupBoxStartup.Location = new System.Drawing.Point(12, 21);
            this.groupBoxStartup.Name = "groupBoxStartup";
            this.groupBoxStartup.Size = new System.Drawing.Size(254, 63);
            this.groupBoxStartup.TabIndex = 4;
            this.groupBoxStartup.TabStop = false;
            this.groupBoxStartup.Text = "Startup";
            // 
            // comboBoxUserName
            // 
            this.comboBoxUserName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUserName.FormattingEnabled = true;
            this.comboBoxUserName.Location = new System.Drawing.Point(107, 18);
            this.comboBoxUserName.Name = "comboBoxUserName";
            this.comboBoxUserName.Size = new System.Drawing.Size(121, 21);
            this.comboBoxUserName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "User Name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxMachineName);
            this.groupBox1.Location = new System.Drawing.Point(12, 101);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(254, 70);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Machine Name";
            // 
            // textBoxMachineName
            // 
            this.textBoxMachineName.Location = new System.Drawing.Point(30, 27);
            this.textBoxMachineName.Name = "textBoxMachineName";
            this.textBoxMachineName.Size = new System.Drawing.Size(188, 20);
            this.textBoxMachineName.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRestoreDefaultBackupPath);
            this.groupBox2.Controls.Add(this.buttonSelectDefaultBackupPath);
            this.groupBox2.Controls.Add(this.textBoxDefaultBackupPath);
            this.groupBox2.Location = new System.Drawing.Point(12, 189);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(254, 108);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Backup Path";
            // 
            // textBoxDefaultBackupPath
            // 
            this.textBoxDefaultBackupPath.Location = new System.Drawing.Point(6, 30);
            this.textBoxDefaultBackupPath.Name = "textBoxDefaultBackupPath";
            this.textBoxDefaultBackupPath.Size = new System.Drawing.Size(212, 20);
            this.textBoxDefaultBackupPath.TabIndex = 0;
            // 
            // buttonSelectDefaultBackupPath
            // 
            this.buttonSelectDefaultBackupPath.Location = new System.Drawing.Point(218, 28);
            this.buttonSelectDefaultBackupPath.Name = "buttonSelectDefaultBackupPath";
            this.buttonSelectDefaultBackupPath.Size = new System.Drawing.Size(34, 23);
            this.buttonSelectDefaultBackupPath.TabIndex = 1;
            this.buttonSelectDefaultBackupPath.Text = ">>";
            this.buttonSelectDefaultBackupPath.UseVisualStyleBackColor = true;
            this.buttonSelectDefaultBackupPath.Click += new System.EventHandler(this.buttonSelectDefaultBackupPath_Click);
            // 
            // buttonRestoreDefaultBackupPath
            // 
            this.buttonRestoreDefaultBackupPath.Location = new System.Drawing.Point(6, 66);
            this.buttonRestoreDefaultBackupPath.Name = "buttonRestoreDefaultBackupPath";
            this.buttonRestoreDefaultBackupPath.Size = new System.Drawing.Size(186, 23);
            this.buttonRestoreDefaultBackupPath.TabIndex = 2;
            this.buttonRestoreDefaultBackupPath.Text = "Restore To Default Backup Path";
            this.buttonRestoreDefaultBackupPath.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaultBackupPath.Click += new System.EventHandler(this.buttonRestoreDefaultBackupPath_Click);
            // 
            // SystemOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 357);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxStartup);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SystemOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SystemOptions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SystemOptions_FormClosing);
            this.Load += new System.EventHandler(this.SystemOptions_Load);
            this.groupBoxStartup.ResumeLayout(false);
            this.groupBoxStartup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxStartup;
        private System.Windows.Forms.ComboBox comboBoxUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxMachineName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxDefaultBackupPath;
        private System.Windows.Forms.Button buttonSelectDefaultBackupPath;
        private System.Windows.Forms.Button buttonRestoreDefaultBackupPath;
    }
}