namespace AbbCom.Forms
{
    partial class AcqFifoAdjust
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonManualTrigger = new System.Windows.Forms.Button();
            this.numericUpDownGreyContrast = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownGreyBrightness = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownGreyExposure = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGreyContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGreyBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGreyExposure)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.buttonManualTrigger);
            this.groupBox1.Controls.Add(this.numericUpDownGreyContrast);
            this.groupBox1.Controls.Add(this.numericUpDownGreyBrightness);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numericUpDownGreyExposure);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 173);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Greyscale Camera Settings";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(160, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "(0 -1)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(161, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "(0 -1)";
            // 
            // buttonManualTrigger
            // 
            this.buttonManualTrigger.Location = new System.Drawing.Point(38, 129);
            this.buttonManualTrigger.Name = "buttonManualTrigger";
            this.buttonManualTrigger.Size = new System.Drawing.Size(117, 29);
            this.buttonManualTrigger.TabIndex = 3;
            this.buttonManualTrigger.Text = "Manual Trigger (F5)";
            this.buttonManualTrigger.UseVisualStyleBackColor = true;
            this.buttonManualTrigger.Click += new System.EventHandler(this.buttonManualTrigger_Click);
            // 
            // numericUpDownGreyContrast
            // 
            this.numericUpDownGreyContrast.DecimalPlaces = 3;
            this.numericUpDownGreyContrast.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDownGreyContrast.Location = new System.Drawing.Point(95, 90);
            this.numericUpDownGreyContrast.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownGreyContrast.Name = "numericUpDownGreyContrast";
            this.numericUpDownGreyContrast.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownGreyContrast.TabIndex = 6;
            this.numericUpDownGreyContrast.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // numericUpDownGreyBrightness
            // 
            this.numericUpDownGreyBrightness.DecimalPlaces = 4;
            this.numericUpDownGreyBrightness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDownGreyBrightness.Location = new System.Drawing.Point(95, 62);
            this.numericUpDownGreyBrightness.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownGreyBrightness.Name = "numericUpDownGreyBrightness";
            this.numericUpDownGreyBrightness.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownGreyBrightness.TabIndex = 5;
            this.numericUpDownGreyBrightness.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(160, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "(ms)";
            // 
            // numericUpDownGreyExposure
            // 
            this.numericUpDownGreyExposure.Location = new System.Drawing.Point(109, 36);
            this.numericUpDownGreyExposure.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownGreyExposure.Name = "numericUpDownGreyExposure";
            this.numericUpDownGreyExposure.Size = new System.Drawing.Size(46, 20);
            this.numericUpDownGreyExposure.TabIndex = 3;
            this.numericUpDownGreyExposure.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Contrast:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Brightness:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Exposure:";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(37, 199);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(125, 199);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // AcqFifoAdjust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 238);
            this.ControlBox = false;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AcqFifoAdjust";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Acquisition Fifo Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AcqFifo_FormClosing);
            this.Load += new System.EventHandler(this.AcqFifo_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGreyContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGreyBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGreyExposure)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDownGreyContrast;
        private System.Windows.Forms.NumericUpDown numericUpDownGreyBrightness;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownGreyExposure;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonManualTrigger;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}