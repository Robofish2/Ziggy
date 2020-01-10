namespace AbbCom.Forms
{
    partial class ComIO
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonSaveToDisk = new System.Windows.Forms.Button();
            this.listViewComIOMsgs = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(502, 291);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(44, 291);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonSaveToDisk
            // 
            this.buttonSaveToDisk.Location = new System.Drawing.Point(239, 291);
            this.buttonSaveToDisk.Name = "buttonSaveToDisk";
            this.buttonSaveToDisk.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveToDisk.TabIndex = 8;
            this.buttonSaveToDisk.Text = "Save to disk";
            this.buttonSaveToDisk.UseVisualStyleBackColor = true;
            this.buttonSaveToDisk.Click += new System.EventHandler(this.buttonSaveToDisk_Click);
            // 
            // listViewComIOMsgs
            // 
            this.listViewComIOMsgs.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.listViewComIOMsgs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewComIOMsgs.Location = new System.Drawing.Point(13, 13);
            this.listViewComIOMsgs.Name = "listViewComIOMsgs";
            this.listViewComIOMsgs.Size = new System.Drawing.Size(607, 272);
            this.listViewComIOMsgs.TabIndex = 9;
            this.listViewComIOMsgs.UseCompatibleStateImageBehavior = false;
            this.listViewComIOMsgs.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Time";
            this.columnHeader2.Width = 125;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Message";
            this.columnHeader3.Width = 2000;
            // 
            // ComIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 328);
            this.ControlBox = false;
            this.Controls.Add(this.listViewComIOMsgs);
            this.Controls.Add(this.buttonSaveToDisk);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "ComIO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ComIO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ComIO_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonSaveToDisk;
        private System.Windows.Forms.ListView listViewComIOMsgs;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}