using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Maf.Tools;

namespace AbbCom.Forms
{
    public partial class Stats : Form
    {

        MainForm mainForm;
        RuntimeParameters runtimeParameters;

        public Stats(MainForm mf, RuntimeParameters rp)
        {
            InitializeComponent();

            mainForm = mf;
            runtimeParameters = rp;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Stats_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        /// <summary>
        /// Event fired when this.Invalidate() method is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stats_Paint(object sender, PaintEventArgs e)
        {
            textBoxProcessingTime.Text = mainForm.VisionProcessTime.ToString();
            textBoxItemsLocated.Text = mainForm.VisionItemsLocated.ToString();
            textBoxAngleFailures.Text = mainForm.VisionAngleFailures.ToString();
            textBoxThermalFailures.Text = mainForm.VisionThermalFailures.ToString();
        }

        /// <summary>
        /// Event fired when timer interval time elapses.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Force the form to redraw (Paint event will fire)
            this.Invalidate();
        }

        /// <summary>
        /// Check for F5 press to snap picture.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            bool handled = false;        // we haven't handled this key

            if ((keyData == Keys.F5) && (mainForm.RecipeName != string.Empty) && (!mainForm.VisionOnline))
            {
                mainForm.RunQuickBuildJob();
                handled = true;       // we handled the key press
            }

            return (handled || base.ProcessCmdKey(ref msg, keyData));
        }

        private void buttonResetStats_Click(object sender, EventArgs e)
        {

            if (DialogResult.Yes == Utilities.AreYouSure())
            {
                mainForm.ResetStats();
            }
        }
    }
}
