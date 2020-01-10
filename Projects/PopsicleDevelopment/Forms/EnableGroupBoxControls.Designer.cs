namespace AbbCom.Forms
{
    partial class EnableGroupBoxControls
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
            this.button1Close = new System.Windows.Forms.Button();
            this.checkBoxEnableFields = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1Close
            // 
            this.button1Close.Location = new System.Drawing.Point(73, 48);
            this.button1Close.Name = "button1Close";
            this.button1Close.Size = new System.Drawing.Size(75, 23);
            this.button1Close.TabIndex = 0;
            this.button1Close.Text = "Close";
            this.button1Close.UseVisualStyleBackColor = true;
            this.button1Close.Click += new System.EventHandler(this.button1Close_Click);
            // 
            // checkBoxEnableFields
            // 
            this.checkBoxEnableFields.AutoSize = true;
            this.checkBoxEnableFields.Location = new System.Drawing.Point(11, 12);
            this.checkBoxEnableFields.Name = "checkBoxEnableFields";
            this.checkBoxEnableFields.Size = new System.Drawing.Size(209, 17);
            this.checkBoxEnableFields.TabIndex = 1;
            this.checkBoxEnableFields.Text = "Enable controls in GroupBoxes for edit.";
            this.checkBoxEnableFields.UseVisualStyleBackColor = true;
            this.checkBoxEnableFields.CheckedChanged += new System.EventHandler(this.checkBoxEnableFields_CheckedChanged);
            // 
            // EnableGroupBoxControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 94);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxEnableFields);
            this.Controls.Add(this.button1Close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EnableGroupBoxControls";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Groupbox Control Enable";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1Close;
        private System.Windows.Forms.CheckBox checkBoxEnableFields;
    }
}