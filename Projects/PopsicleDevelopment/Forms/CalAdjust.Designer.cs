namespace AbbCom.Forms
{
    partial class CalAdjust
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
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxXorigin = new System.Windows.Forms.TextBox();
            this.textBoxYorigin = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownXadjust = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownYadjust = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXadjust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYadjust)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(54, 149);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 0;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "X Origin:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y Origin:";
            // 
            // textBoxXorigin
            // 
            this.textBoxXorigin.Location = new System.Drawing.Point(72, 17);
            this.textBoxXorigin.Name = "textBoxXorigin";
            this.textBoxXorigin.ReadOnly = true;
            this.textBoxXorigin.Size = new System.Drawing.Size(57, 20);
            this.textBoxXorigin.TabIndex = 3;
            // 
            // textBoxYorigin
            // 
            this.textBoxYorigin.Location = new System.Drawing.Point(72, 44);
            this.textBoxYorigin.Name = "textBoxYorigin";
            this.textBoxYorigin.ReadOnly = true;
            this.textBoxYorigin.Size = new System.Drawing.Size(57, 20);
            this.textBoxYorigin.TabIndex = 4;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(154, 149);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "X Adjust:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Y Adjust:";
            // 
            // numericUpDownXadjust
            // 
            this.numericUpDownXadjust.DecimalPlaces = 1;
            this.numericUpDownXadjust.Location = new System.Drawing.Point(74, 88);
            this.numericUpDownXadjust.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownXadjust.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDownXadjust.Name = "numericUpDownXadjust";
            this.numericUpDownXadjust.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownXadjust.TabIndex = 10;
            // 
            // numericUpDownYadjust
            // 
            this.numericUpDownYadjust.DecimalPlaces = 1;
            this.numericUpDownYadjust.Location = new System.Drawing.Point(74, 114);
            this.numericUpDownYadjust.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownYadjust.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDownYadjust.Name = "numericUpDownYadjust";
            this.numericUpDownYadjust.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownYadjust.TabIndex = 11;
            // 
            // CalAdjust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 185);
            this.ControlBox = false;
            this.Controls.Add(this.numericUpDownYadjust);
            this.Controls.Add(this.numericUpDownXadjust);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxYorigin);
            this.Controls.Add(this.textBoxXorigin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CalAdjust";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CalAdjust";
            this.Load += new System.EventHandler(this.CalAdjust_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXadjust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYadjust)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxXorigin;
        private System.Windows.Forms.TextBox textBoxYorigin;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownXadjust;
        private System.Windows.Forms.NumericUpDown numericUpDownYadjust;
    }
}