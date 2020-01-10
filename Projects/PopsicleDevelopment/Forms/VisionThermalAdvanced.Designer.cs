namespace AbbCom.Forms
{
    partial class VisionThermalAdvanced
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownMinFrameCount = new System.Windows.Forms.NumericUpDown();
            this.buttonPerformNUC = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableAutoNUC = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownManualNUCtime = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1AutoFocus = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinFrameCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownManualNUCtime)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "for NUC";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Max Popsicle Count";
            // 
            // numericUpDownMinFrameCount
            // 
            this.numericUpDownMinFrameCount.Location = new System.Drawing.Point(17, 70);
            this.numericUpDownMinFrameCount.Maximum = new decimal(new int[] {
            13,
            0,
            0,
            0});
            this.numericUpDownMinFrameCount.Name = "numericUpDownMinFrameCount";
            this.numericUpDownMinFrameCount.Size = new System.Drawing.Size(37, 20);
            this.numericUpDownMinFrameCount.TabIndex = 3;
            this.numericUpDownMinFrameCount.ValueChanged += new System.EventHandler(this.numericUpDownMinFrameCount_ValueChanged);
            // 
            // buttonPerformNUC
            // 
            this.buttonPerformNUC.Location = new System.Drawing.Point(17, 129);
            this.buttonPerformNUC.Name = "buttonPerformNUC";
            this.buttonPerformNUC.Size = new System.Drawing.Size(85, 23);
            this.buttonPerformNUC.TabIndex = 2;
            this.buttonPerformNUC.Text = "Perform NUC";
            this.buttonPerformNUC.UseVisualStyleBackColor = true;
            this.buttonPerformNUC.Click += new System.EventHandler(this.buttonPerformNUC_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(349, 225);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonPerformNUC);
            this.groupBox2.Controls.Add(this.checkBoxEnableAutoNUC);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numericUpDownManualNUCtime);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.numericUpDownMinFrameCount);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(22, 33);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 172);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "NUC Control";
            // 
            // checkBoxEnableAutoNUC
            // 
            this.checkBoxEnableAutoNUC.AutoSize = true;
            this.checkBoxEnableAutoNUC.Location = new System.Drawing.Point(17, 37);
            this.checkBoxEnableAutoNUC.Name = "checkBoxEnableAutoNUC";
            this.checkBoxEnableAutoNUC.Size = new System.Drawing.Size(110, 17);
            this.checkBoxEnableAutoNUC.TabIndex = 8;
            this.checkBoxEnableAutoNUC.Text = "Enable Auto NUC";
            this.checkBoxEnableAutoNUC.UseVisualStyleBackColor = true;
            this.checkBoxEnableAutoNUC.CheckedChanged += new System.EventHandler(this.checkBoxEnableAutoNUC_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Manual NUC time (Minutes)";
            // 
            // numericUpDownManualNUCtime
            // 
            this.numericUpDownManualNUCtime.Location = new System.Drawing.Point(17, 99);
            this.numericUpDownManualNUCtime.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownManualNUCtime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownManualNUCtime.Name = "numericUpDownManualNUCtime";
            this.numericUpDownManualNUCtime.Size = new System.Drawing.Size(37, 20);
            this.numericUpDownManualNUCtime.TabIndex = 6;
            this.numericUpDownManualNUCtime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownManualNUCtime.ValueChanged += new System.EventHandler(this.numericUpDownManualNUCtime_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button1AutoFocus);
            this.groupBox1.Location = new System.Drawing.Point(236, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 172);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AutoFocus";
            // 
            // button1AutoFocus
            // 
            this.button1AutoFocus.Location = new System.Drawing.Point(40, 129);
            this.button1AutoFocus.Name = "button1AutoFocus";
            this.button1AutoFocus.Size = new System.Drawing.Size(75, 23);
            this.button1AutoFocus.TabIndex = 0;
            this.button1AutoFocus.Text = "AutoFocus";
            this.button1AutoFocus.UseVisualStyleBackColor = true;
            this.button1AutoFocus.Click += new System.EventHandler(this.button1AutoFocus_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "1. Stop the machine.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "2. Take the vision offline.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(169, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "3. Position a full wrap in the center";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = " of the vision window.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "4. Press the AutoFocus button.";
            // 
            // VisionThermalAdvanced
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 260);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "VisionThermalAdvanced";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VisionThermalAdvanced";
            this.Load += new System.EventHandler(this.VisionThermalAdvanced_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinFrameCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownManualNUCtime)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPerformNUC;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.NumericUpDown numericUpDownMinFrameCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownManualNUCtime;
        private System.Windows.Forms.CheckBox checkBoxEnableAutoNUC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1AutoFocus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}