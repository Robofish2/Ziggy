namespace AbbCom.Forms
{
    partial class Stats
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxProcessingTime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBoxItemsLocated = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAngleFailures = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxThermalFailures = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonResetStats = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(88, 208);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 42;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(173, 208);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 41;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxProcessingTime
            // 
            this.textBoxProcessingTime.Location = new System.Drawing.Point(21, 19);
            this.textBoxProcessingTime.Name = "textBoxProcessingTime";
            this.textBoxProcessingTime.ReadOnly = true;
            this.textBoxProcessingTime.Size = new System.Drawing.Size(47, 20);
            this.textBoxProcessingTime.TabIndex = 43;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 44;
            this.label1.Text = "Processing Time (ms)";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBoxItemsLocated
            // 
            this.textBoxItemsLocated.Location = new System.Drawing.Point(21, 45);
            this.textBoxItemsLocated.Name = "textBoxItemsLocated";
            this.textBoxItemsLocated.ReadOnly = true;
            this.textBoxItemsLocated.Size = new System.Drawing.Size(79, 20);
            this.textBoxItemsLocated.TabIndex = 45;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 46;
            this.label2.Text = "Good Bars Located";
            // 
            // textBoxAngleFailures
            // 
            this.textBoxAngleFailures.Location = new System.Drawing.Point(21, 71);
            this.textBoxAngleFailures.Name = "textBoxAngleFailures";
            this.textBoxAngleFailures.ReadOnly = true;
            this.textBoxAngleFailures.Size = new System.Drawing.Size(79, 20);
            this.textBoxAngleFailures.TabIndex = 47;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "Angle Failures";
            // 
            // textBoxThermalFailures
            // 
            this.textBoxThermalFailures.Location = new System.Drawing.Point(21, 97);
            this.textBoxThermalFailures.Name = "textBoxThermalFailures";
            this.textBoxThermalFailures.ReadOnly = true;
            this.textBoxThermalFailures.Size = new System.Drawing.Size(79, 20);
            this.textBoxThermalFailures.TabIndex = 49;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(106, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 50;
            this.label4.Text = "Thermal Failures";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonResetStats);
            this.groupBox1.Controls.Add(this.textBoxProcessingTime);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxThermalFailures);
            this.groupBox1.Controls.Add(this.textBoxItemsLocated);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxAngleFailures);
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 168);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Stats";
            // 
            // buttonResetStats
            // 
            this.buttonResetStats.Location = new System.Drawing.Point(72, 129);
            this.buttonResetStats.Name = "buttonResetStats";
            this.buttonResetStats.Size = new System.Drawing.Size(75, 23);
            this.buttonResetStats.TabIndex = 51;
            this.buttonResetStats.Text = "Reset Stats";
            this.buttonResetStats.UseVisualStyleBackColor = true;
            this.buttonResetStats.Click += new System.EventHandler(this.buttonResetStats_Click);
            // 
            // Stats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 250);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Stats";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stats";
            this.Load += new System.EventHandler(this.Stats_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Stats_Paint);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxProcessingTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBoxItemsLocated;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxAngleFailures;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxThermalFailures;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonResetStats;
    }
}