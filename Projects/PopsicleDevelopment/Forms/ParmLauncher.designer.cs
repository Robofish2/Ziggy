namespace AbbCom
{
    partial class ParmLauncher
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
            this.buttonParmLauncherClose = new System.Windows.Forms.Button();
            this.buttonStats = new System.Windows.Forms.Button();
            this.buttonCnvSpdCtrl = new System.Windows.Forms.Button();
            this.buttonVision = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonManualTrigger = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonParmLauncherClose
            // 
            this.buttonParmLauncherClose.Location = new System.Drawing.Point(102, 219);
            this.buttonParmLauncherClose.Margin = new System.Windows.Forms.Padding(2);
            this.buttonParmLauncherClose.Name = "buttonParmLauncherClose";
            this.buttonParmLauncherClose.Size = new System.Drawing.Size(56, 60);
            this.buttonParmLauncherClose.TabIndex = 1;
            this.buttonParmLauncherClose.Text = "Close";
            this.buttonParmLauncherClose.UseVisualStyleBackColor = true;
            this.buttonParmLauncherClose.Click += new System.EventHandler(this.buttonParmLauncherClose_Click);
            // 
            // buttonStats
            // 
            this.buttonStats.Location = new System.Drawing.Point(137, 11);
            this.buttonStats.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStats.Name = "buttonStats";
            this.buttonStats.Size = new System.Drawing.Size(132, 60);
            this.buttonStats.TabIndex = 11;
            this.buttonStats.Text = "Statistics";
            this.buttonStats.UseVisualStyleBackColor = true;
            this.buttonStats.Click += new System.EventHandler(this.buttonStats_Click);
            // 
            // buttonCnvSpdCtrl
            // 
            this.buttonCnvSpdCtrl.Location = new System.Drawing.Point(0, 0);
            this.buttonCnvSpdCtrl.Name = "buttonCnvSpdCtrl";
            this.buttonCnvSpdCtrl.Size = new System.Drawing.Size(75, 23);
            this.buttonCnvSpdCtrl.TabIndex = 13;
            // 
            // buttonVision
            // 
            this.buttonVision.Location = new System.Drawing.Point(1, 133);
            this.buttonVision.Margin = new System.Windows.Forms.Padding(2);
            this.buttonVision.Name = "buttonVision";
            this.buttonVision.Size = new System.Drawing.Size(132, 60);
            this.buttonVision.TabIndex = 4;
            this.buttonVision.Text = "Vision";
            this.buttonVision.UseVisualStyleBackColor = true;
            this.buttonVision.Click += new System.EventHandler(this.buttonVision_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.Color.Red;
            this.buttonStop.Location = new System.Drawing.Point(1, 69);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(132, 60);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonStart.Location = new System.Drawing.Point(1, 5);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(132, 60);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonManualTrigger
            // 
            this.buttonManualTrigger.Location = new System.Drawing.Point(137, 85);
            this.buttonManualTrigger.Margin = new System.Windows.Forms.Padding(2);
            this.buttonManualTrigger.Name = "buttonManualTrigger";
            this.buttonManualTrigger.Size = new System.Drawing.Size(132, 60);
            this.buttonManualTrigger.TabIndex = 12;
            this.buttonManualTrigger.Text = "Trigger";
            this.buttonManualTrigger.UseVisualStyleBackColor = true;
            this.buttonManualTrigger.Click += new System.EventHandler(this.buttonTrigger_Click);
            // 
            // ParmLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(272, 308);
            this.ControlBox = false;
            this.Controls.Add(this.buttonManualTrigger);
            this.Controls.Add(this.buttonParmLauncherClose);
            this.Controls.Add(this.buttonStats);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonCnvSpdCtrl);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonVision);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParmLauncher";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Parameter Adjustments";
            this.Load += new System.EventHandler(this.ParmLauncher_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonParmLauncherClose;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonVision;
        private System.Windows.Forms.Button buttonStats;
        private System.Windows.Forms.Button buttonCnvSpdCtrl;
        private System.Windows.Forms.Button buttonManualTrigger;
    }
}