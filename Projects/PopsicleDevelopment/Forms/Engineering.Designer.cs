namespace AbbCom.Forms
{
    partial class Engineering
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
            this.components = new System.ComponentModel.Container();
            this.buttonStartLogger = new System.Windows.Forms.Button();
            this.buttonStopLogger = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBoxDataLogger = new System.Windows.Forms.GroupBox();
            this.textBoxMessageText = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.groupBoxCogJobs = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.checkBoxCogJobEnable = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCogJobPattern = new System.Windows.Forms.TextBox();
            this.textBoxCogJobBlob = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxDataLogger.SuspendLayout();
            this.groupBoxCogJobs.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStartLogger
            // 
            this.buttonStartLogger.Location = new System.Drawing.Point(15, 29);
            this.buttonStartLogger.Name = "buttonStartLogger";
            this.buttonStartLogger.Size = new System.Drawing.Size(134, 23);
            this.buttonStartLogger.TabIndex = 0;
            this.buttonStartLogger.Text = "Start Data Logger";
            this.buttonStartLogger.UseVisualStyleBackColor = true;
            this.buttonStartLogger.Click += new System.EventHandler(this.buttonStartLogger_Click);
            // 
            // buttonStopLogger
            // 
            this.buttonStopLogger.Location = new System.Drawing.Point(15, 58);
            this.buttonStopLogger.Name = "buttonStopLogger";
            this.buttonStopLogger.Size = new System.Drawing.Size(134, 23);
            this.buttonStopLogger.TabIndex = 1;
            this.buttonStopLogger.Text = "Stop Data Logger";
            this.buttonStopLogger.UseVisualStyleBackColor = true;
            this.buttonStopLogger.Click += new System.EventHandler(this.buttonStopLoggerThread_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(261, 295);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // groupBoxDataLogger
            // 
            this.groupBoxDataLogger.Controls.Add(this.textBoxMessageText);
            this.groupBoxDataLogger.Controls.Add(this.buttonSend);
            this.groupBoxDataLogger.Controls.Add(this.buttonStopLogger);
            this.groupBoxDataLogger.Controls.Add(this.buttonStartLogger);
            this.groupBoxDataLogger.Location = new System.Drawing.Point(27, 22);
            this.groupBoxDataLogger.Name = "groupBoxDataLogger";
            this.groupBoxDataLogger.Size = new System.Drawing.Size(200, 146);
            this.groupBoxDataLogger.TabIndex = 3;
            this.groupBoxDataLogger.TabStop = false;
            this.groupBoxDataLogger.Text = "Logger";
            // 
            // textBoxMessageText
            // 
            this.textBoxMessageText.Location = new System.Drawing.Point(68, 109);
            this.textBoxMessageText.Name = "textBoxMessageText";
            this.textBoxMessageText.Size = new System.Drawing.Size(126, 20);
            this.textBoxMessageText.TabIndex = 3;
            this.textBoxMessageText.Text = "Test,1,2,3";
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(15, 107);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(50, 23);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // groupBoxCogJobs
            // 
            this.groupBoxCogJobs.Controls.Add(this.buttonSave);
            this.groupBoxCogJobs.Controls.Add(this.checkBoxCogJobEnable);
            this.groupBoxCogJobs.Controls.Add(this.label4);
            this.groupBoxCogJobs.Controls.Add(this.label3);
            this.groupBoxCogJobs.Controls.Add(this.textBoxCogJobPattern);
            this.groupBoxCogJobs.Controls.Add(this.textBoxCogJobBlob);
            this.groupBoxCogJobs.Location = new System.Drawing.Point(27, 174);
            this.groupBoxCogJobs.Name = "groupBoxCogJobs";
            this.groupBoxCogJobs.Size = new System.Drawing.Size(321, 100);
            this.groupBoxCogJobs.TabIndex = 5;
            this.groupBoxCogJobs.TabStop = false;
            this.groupBoxCogJobs.Text = "CogJobs";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(229, 57);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 10;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // checkBoxCogJobEnable
            // 
            this.checkBoxCogJobEnable.AutoSize = true;
            this.checkBoxCogJobEnable.Location = new System.Drawing.Point(229, 23);
            this.checkBoxCogJobEnable.Name = "checkBoxCogJobEnable";
            this.checkBoxCogJobEnable.Size = new System.Drawing.Size(74, 17);
            this.checkBoxCogJobEnable.TabIndex = 9;
            this.checkBoxCogJobEnable.Text = "Edit Fields";
            this.checkBoxCogJobEnable.UseVisualStyleBackColor = true;
            this.checkBoxCogJobEnable.CheckedChanged += new System.EventHandler(this.checkBoxCogJobEnable_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(121, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Pattern Job Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Blob Job Name";
            // 
            // textBoxCogJobPattern
            // 
            this.textBoxCogJobPattern.Location = new System.Drawing.Point(15, 54);
            this.textBoxCogJobPattern.Name = "textBoxCogJobPattern";
            this.textBoxCogJobPattern.Size = new System.Drawing.Size(100, 20);
            this.textBoxCogJobPattern.TabIndex = 7;
            // 
            // textBoxCogJobBlob
            // 
            this.textBoxCogJobBlob.Location = new System.Drawing.Point(15, 19);
            this.textBoxCogJobBlob.Name = "textBoxCogJobBlob";
            this.textBoxCogJobBlob.Size = new System.Drawing.Size(100, 20);
            this.textBoxCogJobBlob.TabIndex = 6;
            // 
            // Engineering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 330);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxCogJobs);
            this.Controls.Add(this.groupBoxDataLogger);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Engineering";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Engineering";
            this.Load += new System.EventHandler(this.Engineering_Load);
            this.groupBoxDataLogger.ResumeLayout(false);
            this.groupBoxDataLogger.PerformLayout();
            this.groupBoxCogJobs.ResumeLayout(false);
            this.groupBoxCogJobs.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStartLogger;
        private System.Windows.Forms.Button buttonStopLogger;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox groupBoxDataLogger;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxMessageText;
        private System.Windows.Forms.GroupBox groupBoxCogJobs;
        private System.Windows.Forms.CheckBox checkBoxCogJobEnable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCogJobPattern;
        private System.Windows.Forms.TextBox textBoxCogJobBlob;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}