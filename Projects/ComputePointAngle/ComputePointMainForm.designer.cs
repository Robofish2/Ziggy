namespace ComputePointAngle
{
    partial class ComputePointMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComputePointMainForm));
            this.textBoxAngle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDownYpoint = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownXpoint = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownYorigin = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownXorigin = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCompute = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYpoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXpoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYorigin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXorigin)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxAngle
            // 
            this.textBoxAngle.Location = new System.Drawing.Point(208, 130);
            this.textBoxAngle.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxAngle.Name = "textBoxAngle";
            this.textBoxAngle.ReadOnly = true;
            this.textBoxAngle.Size = new System.Drawing.Size(70, 22);
            this.textBoxAngle.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "X (mm)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Y (mm)";
            // 
            // buttonClose
            // 
            this.buttonClose.AutoSize = true;
            this.buttonClose.Location = new System.Drawing.Point(289, 205);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 29);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.numericUpDownYpoint);
            this.panel1.Controls.Add(this.numericUpDownXpoint);
            this.panel1.Controls.Add(this.numericUpDownYorigin);
            this.panel1.Controls.Add(this.numericUpDownXorigin);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.buttonCompute);
            this.panel1.Controls.Add(this.textBoxAngle);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(362, 178);
            this.panel1.TabIndex = 7;
            // 
            // numericUpDownYpoint
            // 
            this.numericUpDownYpoint.DecimalPlaces = 1;
            this.numericUpDownYpoint.Location = new System.Drawing.Point(138, 74);
            this.numericUpDownYpoint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownYpoint.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownYpoint.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDownYpoint.Name = "numericUpDownYpoint";
            this.numericUpDownYpoint.Size = new System.Drawing.Size(78, 22);
            this.numericUpDownYpoint.TabIndex = 13;
            this.numericUpDownYpoint.ValueChanged += new System.EventHandler(this.numericUpDownYpoint_ValueChanged);
            // 
            // numericUpDownXpoint
            // 
            this.numericUpDownXpoint.DecimalPlaces = 1;
            this.numericUpDownXpoint.Location = new System.Drawing.Point(44, 74);
            this.numericUpDownXpoint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownXpoint.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownXpoint.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDownXpoint.Name = "numericUpDownXpoint";
            this.numericUpDownXpoint.Size = new System.Drawing.Size(78, 22);
            this.numericUpDownXpoint.TabIndex = 12;
            this.numericUpDownXpoint.ValueChanged += new System.EventHandler(this.numericUpDownXpoint_ValueChanged);
            // 
            // numericUpDownYorigin
            // 
            this.numericUpDownYorigin.DecimalPlaces = 1;
            this.numericUpDownYorigin.Location = new System.Drawing.Point(138, 44);
            this.numericUpDownYorigin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownYorigin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownYorigin.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDownYorigin.Name = "numericUpDownYorigin";
            this.numericUpDownYorigin.Size = new System.Drawing.Size(78, 22);
            this.numericUpDownYorigin.TabIndex = 11;
            this.numericUpDownYorigin.ValueChanged += new System.EventHandler(this.numericUpDownYorigin_ValueChanged);
            // 
            // numericUpDownXorigin
            // 
            this.numericUpDownXorigin.DecimalPlaces = 1;
            this.numericUpDownXorigin.Location = new System.Drawing.Point(44, 44);
            this.numericUpDownXorigin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownXorigin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownXorigin.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDownXorigin.Name = "numericUpDownXorigin";
            this.numericUpDownXorigin.Size = new System.Drawing.Size(78, 22);
            this.numericUpDownXorigin.TabIndex = 8;
            this.numericUpDownXorigin.ValueChanged += new System.EventHandler(this.numericUpDownXorigin_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(226, 79);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Point 1 (on +X)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(226, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Origin";
            // 
            // buttonCompute
            // 
            this.buttonCompute.AutoSize = true;
            this.buttonCompute.Location = new System.Drawing.Point(62, 124);
            this.buttonCompute.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCompute.Name = "buttonCompute";
            this.buttonCompute.Size = new System.Drawing.Size(142, 34);
            this.buttonCompute.TabIndex = 4;
            this.buttonCompute.Text = "Compute Angle";
            this.buttonCompute.UseVisualStyleBackColor = true;
            this.buttonCompute.Click += new System.EventHandler(this.buttonCompute_Click);
            // 
            // ComputePointMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(391, 246);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "ComputePointMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ComputePointAngle(Cognex Grid)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYpoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXpoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYorigin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXorigin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonCompute;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownYorigin;
        private System.Windows.Forms.NumericUpDown numericUpDownXorigin;
        private System.Windows.Forms.NumericUpDown numericUpDownYpoint;
        private System.Windows.Forms.NumericUpDown numericUpDownXpoint;
    }
}

