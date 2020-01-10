namespace AbbCom.Forms
{
    partial class PatternParams
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownVisPatZoneScaleHigh = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownVisPatZoneScaleLow = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownVisPatElasticity = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownVisPatCoarseAcceptThreshold = new System.Windows.Forms.NumericUpDown();
            this.checkBoxVisPatCoarseAcceptThreshold = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownVisPatContrastThreshold = new System.Windows.Forms.NumericUpDown();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonManualTrigger = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownZoneAngOverlap = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownZoneAngHigh = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownZoneAngLow = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxEnablePatternClipping = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDownVisXwindowMax = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDownVisXwindowMin = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisPatZoneScaleHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisPatZoneScaleLow)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisPatElasticity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisPatCoarseAcceptThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisPatContrastThreshold)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoneAngOverlap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoneAngHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoneAngLow)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisXwindowMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisXwindowMin)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 59;
            this.label3.Text = "High(1-2)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 58;
            this.label2.Text = "Low (0-1)";
            // 
            // numericUpDownVisPatZoneScaleHigh
            // 
            this.numericUpDownVisPatZoneScaleHigh.DecimalPlaces = 2;
            this.numericUpDownVisPatZoneScaleHigh.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownVisPatZoneScaleHigh.Location = new System.Drawing.Point(15, 48);
            this.numericUpDownVisPatZoneScaleHigh.Maximum = new decimal(new int[] {
            19,
            0,
            0,
            65536});
            this.numericUpDownVisPatZoneScaleHigh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVisPatZoneScaleHigh.Name = "numericUpDownVisPatZoneScaleHigh";
            this.numericUpDownVisPatZoneScaleHigh.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownVisPatZoneScaleHigh.TabIndex = 56;
            this.numericUpDownVisPatZoneScaleHigh.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVisPatZoneScaleHigh.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // numericUpDownVisPatZoneScaleLow
            // 
            this.numericUpDownVisPatZoneScaleLow.DecimalPlaces = 2;
            this.numericUpDownVisPatZoneScaleLow.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownVisPatZoneScaleLow.Location = new System.Drawing.Point(15, 19);
            this.numericUpDownVisPatZoneScaleLow.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVisPatZoneScaleLow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownVisPatZoneScaleLow.Name = "numericUpDownVisPatZoneScaleLow";
            this.numericUpDownVisPatZoneScaleLow.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownVisPatZoneScaleLow.TabIndex = 55;
            this.numericUpDownVisPatZoneScaleLow.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownVisPatZoneScaleLow.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownVisPatZoneScaleLow);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numericUpDownVisPatZoneScaleHigh);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 81);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pattern Scaling";
            // 
            // numericUpDownVisPatElasticity
            // 
            this.numericUpDownVisPatElasticity.DecimalPlaces = 2;
            this.numericUpDownVisPatElasticity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownVisPatElasticity.Location = new System.Drawing.Point(206, 59);
            this.numericUpDownVisPatElasticity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownVisPatElasticity.Name = "numericUpDownVisPatElasticity";
            this.numericUpDownVisPatElasticity.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownVisPatElasticity.TabIndex = 61;
            this.numericUpDownVisPatElasticity.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownVisPatElasticity.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(274, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 60;
            this.label4.Text = "Elasticity (pxl)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(273, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 13);
            this.label5.TabIndex = 62;
            this.label5.Text = "Accept Threshold (0-1)";
            // 
            // numericUpDownVisPatCoarseAcceptThreshold
            // 
            this.numericUpDownVisPatCoarseAcceptThreshold.DecimalPlaces = 2;
            this.numericUpDownVisPatCoarseAcceptThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownVisPatCoarseAcceptThreshold.Location = new System.Drawing.Point(210, 128);
            this.numericUpDownVisPatCoarseAcceptThreshold.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVisPatCoarseAcceptThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownVisPatCoarseAcceptThreshold.Name = "numericUpDownVisPatCoarseAcceptThreshold";
            this.numericUpDownVisPatCoarseAcceptThreshold.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownVisPatCoarseAcceptThreshold.TabIndex = 63;
            this.numericUpDownVisPatCoarseAcceptThreshold.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownVisPatCoarseAcceptThreshold.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // checkBoxVisPatCoarseAcceptThreshold
            // 
            this.checkBoxVisPatCoarseAcceptThreshold.AutoSize = true;
            this.checkBoxVisPatCoarseAcceptThreshold.Location = new System.Drawing.Point(223, 98);
            this.checkBoxVisPatCoarseAcceptThreshold.Name = "checkBoxVisPatCoarseAcceptThreshold";
            this.checkBoxVisPatCoarseAcceptThreshold.Size = new System.Drawing.Size(149, 17);
            this.checkBoxVisPatCoarseAcceptThreshold.TabIndex = 64;
            this.checkBoxVisPatCoarseAcceptThreshold.Text = "Coarse Accept Threshold:";
            this.checkBoxVisPatCoarseAcceptThreshold.UseVisualStyleBackColor = true;
            this.checkBoxVisPatCoarseAcceptThreshold.CheckedChanged += new System.EventHandler(this.checkBoxVisPatCoarseAcceptThresholdEnable_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(275, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 65;
            this.label1.Text = "Contrast Threshold(0-255)";
            // 
            // numericUpDownVisPatContrastThreshold
            // 
            this.numericUpDownVisPatContrastThreshold.Location = new System.Drawing.Point(207, 32);
            this.numericUpDownVisPatContrastThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownVisPatContrastThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVisPatContrastThreshold.Name = "numericUpDownVisPatContrastThreshold";
            this.numericUpDownVisPatContrastThreshold.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownVisPatContrastThreshold.TabIndex = 66;
            this.numericUpDownVisPatContrastThreshold.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVisPatContrastThreshold.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(301, 329);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 68;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(213, 329);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 67;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonManualTrigger
            // 
            this.buttonManualTrigger.Location = new System.Drawing.Point(27, 261);
            this.buttonManualTrigger.Name = "buttonManualTrigger";
            this.buttonManualTrigger.Size = new System.Drawing.Size(117, 29);
            this.buttonManualTrigger.TabIndex = 69;
            this.buttonManualTrigger.Text = "Manual Trigger (F5)";
            this.buttonManualTrigger.UseVisualStyleBackColor = true;
            this.buttonManualTrigger.Click += new System.EventHandler(this.buttonManualTrigger_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numericUpDownZoneAngOverlap);
            this.groupBox2.Controls.Add(this.numericUpDownZoneAngHigh);
            this.groupBox2.Controls.Add(this.numericUpDownZoneAngLow);
            this.groupBox2.Location = new System.Drawing.Point(12, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 100);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Zone Angle";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(82, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 75;
            this.label8.Text = "Overlap";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(81, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 74;
            this.label7.Text = "Max";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(82, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 71;
            this.label6.Text = "Min";
            // 
            // numericUpDownZoneAngOverlap
            // 
            this.numericUpDownZoneAngOverlap.Location = new System.Drawing.Point(14, 72);
            this.numericUpDownZoneAngOverlap.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownZoneAngOverlap.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDownZoneAngOverlap.Name = "numericUpDownZoneAngOverlap";
            this.numericUpDownZoneAngOverlap.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownZoneAngOverlap.TabIndex = 73;
            this.numericUpDownZoneAngOverlap.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownZoneAngOverlap.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // numericUpDownZoneAngHigh
            // 
            this.numericUpDownZoneAngHigh.Location = new System.Drawing.Point(15, 45);
            this.numericUpDownZoneAngHigh.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDownZoneAngHigh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownZoneAngHigh.Name = "numericUpDownZoneAngHigh";
            this.numericUpDownZoneAngHigh.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownZoneAngHigh.TabIndex = 72;
            this.numericUpDownZoneAngHigh.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownZoneAngHigh.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // numericUpDownZoneAngLow
            // 
            this.numericUpDownZoneAngLow.Location = new System.Drawing.Point(14, 19);
            this.numericUpDownZoneAngLow.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownZoneAngLow.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.numericUpDownZoneAngLow.Name = "numericUpDownZoneAngLow";
            this.numericUpDownZoneAngLow.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownZoneAngLow.TabIndex = 71;
            this.numericUpDownZoneAngLow.Value = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.numericUpDownZoneAngLow.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxEnablePatternClipping);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.numericUpDownVisXwindowMax);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.numericUpDownVisXwindowMin);
            this.groupBox3.Location = new System.Drawing.Point(197, 172);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(226, 143);
            this.groupBox3.TabIndex = 71;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pattern Clipping";
            // 
            // checkBoxEnablePatternClipping
            // 
            this.checkBoxEnablePatternClipping.AutoSize = true;
            this.checkBoxEnablePatternClipping.Location = new System.Drawing.Point(16, 118);
            this.checkBoxEnablePatternClipping.Name = "checkBoxEnablePatternClipping";
            this.checkBoxEnablePatternClipping.Size = new System.Drawing.Size(74, 17);
            this.checkBoxEnablePatternClipping.TabIndex = 78;
            this.checkBoxEnablePatternClipping.Text = "Edit Fields";
            this.checkBoxEnablePatternClipping.UseVisualStyleBackColor = true;
            this.checkBoxEnablePatternClipping.CheckedChanged += new System.EventHandler(this.checkBoxEnablePatternClipping_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.DarkRed;
            this.label12.Location = new System.Drawing.Point(10, 97);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(156, 13);
            this.label12.TabIndex = 77;
            this.label12.Text = "Clipping reduces dup checking.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.DarkRed;
            this.label11.Location = new System.Drawing.Point(9, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(200, 13);
            this.label11.TabIndex = 76;
            this.label11.Text = "Values are relative to Robot Base Frame,";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(81, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 13);
            this.label10.TabIndex = 75;
            this.label10.Text = "X Window Max (mm)";
            // 
            // numericUpDownVisXwindowMax
            // 
            this.numericUpDownVisXwindowMax.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownVisXwindowMax.Location = new System.Drawing.Point(13, 48);
            this.numericUpDownVisXwindowMax.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownVisXwindowMax.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.numericUpDownVisXwindowMax.Name = "numericUpDownVisXwindowMax";
            this.numericUpDownVisXwindowMax.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownVisXwindowMax.TabIndex = 74;
            this.numericUpDownVisXwindowMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVisXwindowMax.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(81, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 13);
            this.label9.TabIndex = 73;
            this.label9.Text = "X Window Min (mm)";
            // 
            // numericUpDownVisXwindowMin
            // 
            this.numericUpDownVisXwindowMin.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownVisXwindowMin.Location = new System.Drawing.Point(13, 19);
            this.numericUpDownVisXwindowMin.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownVisXwindowMin.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.numericUpDownVisXwindowMin.Name = "numericUpDownVisXwindowMin";
            this.numericUpDownVisXwindowMin.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownVisXwindowMin.TabIndex = 72;
            this.numericUpDownVisXwindowMin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownVisXwindowMin.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(197, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 137);
            this.panel1.TabIndex = 72;
            // 
            // PatternParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 377);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonManualTrigger);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownVisPatContrastThreshold);
            this.Controls.Add(this.checkBoxVisPatCoarseAcceptThreshold);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDownVisPatCoarseAcceptThreshold);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDownVisPatElasticity);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "PatternParams";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PatternParams";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PatternParams_FormClosing);
            this.Load += new System.EventHandler(this.PatternParams_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisPatZoneScaleHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisPatZoneScaleLow)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisPatElasticity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisPatCoarseAcceptThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisPatContrastThreshold)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoneAngOverlap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoneAngHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoneAngLow)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisXwindowMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisXwindowMin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownVisPatZoneScaleHigh;
        private System.Windows.Forms.NumericUpDown numericUpDownVisPatZoneScaleLow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDownVisPatElasticity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownVisPatCoarseAcceptThreshold;
        private System.Windows.Forms.CheckBox checkBoxVisPatCoarseAcceptThreshold;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownVisPatContrastThreshold;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonManualTrigger;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownZoneAngOverlap;
        private System.Windows.Forms.NumericUpDown numericUpDownZoneAngHigh;
        private System.Windows.Forms.NumericUpDown numericUpDownZoneAngLow;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDownVisXwindowMax;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDownVisXwindowMin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBoxEnablePatternClipping;
        private System.Windows.Forms.Panel panel1;
    }
}