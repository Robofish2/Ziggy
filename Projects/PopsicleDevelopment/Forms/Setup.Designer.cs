namespace AbbCom.Forms
{
    partial class Setup
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
            this.comboBoxAutoLoadRecipe = new System.Windows.Forms.ComboBox();
            this.groupBoxAutoLoadRecipe = new System.Windows.Forms.GroupBox();
            this.groupBoxAutoLoadRecipe.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(45, 164);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 42;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(126, 164);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 41;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // comboBoxAutoLoadRecipe
            // 
            this.comboBoxAutoLoadRecipe.FormattingEnabled = true;
            this.comboBoxAutoLoadRecipe.Location = new System.Drawing.Point(15, 29);
            this.comboBoxAutoLoadRecipe.Name = "comboBoxAutoLoadRecipe";
            this.comboBoxAutoLoadRecipe.Size = new System.Drawing.Size(157, 21);
            this.comboBoxAutoLoadRecipe.TabIndex = 43;
            this.comboBoxAutoLoadRecipe.SelectedIndexChanged += new System.EventHandler(this.comboBoxAutoLoadRecipe_SelectedIndexChanged);
            // 
            // groupBoxAutoLoadRecipe
            // 
            this.groupBoxAutoLoadRecipe.Controls.Add(this.comboBoxAutoLoadRecipe);
            this.groupBoxAutoLoadRecipe.Location = new System.Drawing.Point(12, 12);
            this.groupBoxAutoLoadRecipe.Name = "groupBoxAutoLoadRecipe";
            this.groupBoxAutoLoadRecipe.Size = new System.Drawing.Size(189, 85);
            this.groupBoxAutoLoadRecipe.TabIndex = 44;
            this.groupBoxAutoLoadRecipe.TabStop = false;
            this.groupBoxAutoLoadRecipe.Text = "Startup Recipe";
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 199);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxAutoLoadRecipe);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Setup_FormClosing);
            this.Load += new System.EventHandler(this.Setup_Load);
            this.groupBoxAutoLoadRecipe.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBoxAutoLoadRecipe;
        private System.Windows.Forms.GroupBox groupBoxAutoLoadRecipe;
    }
}