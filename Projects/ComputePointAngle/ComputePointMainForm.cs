using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ComputePointAngle
{

    public partial class ComputePointMainForm : Form
    {

        public ComputePointMainForm()
        {
            InitializeComponent();

        }

        private void buttonCompute_Click(object sender, EventArgs e)
        {

            double x1, x2, y1, y2, angle;
            double radiansToDegrees = 180 / Math.PI;

            x1 = (double)numericUpDownXorigin.Value;
            y1 = (double)numericUpDownYorigin.Value;
            x2 = (double)numericUpDownXpoint.Value;
            y2 = (double)numericUpDownYpoint.Value;

            // Calculate angle
            angle = Math.Atan2(y2 - y1, x2 - x1) * radiansToDegrees;

            // Format the angle field
            angle = double.Parse(angle.ToString("0000.000"));

            // Update textbox
            textBoxAngle.Text = Convert.ToString(angle);

            Properties.Settings.Default.angle = Convert.ToDouble(textBoxAngle.Text);

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Populate fields
            numericUpDownXorigin.Value = (decimal)Properties.Settings.Default.originX;
            numericUpDownYorigin.Value = (decimal)Properties.Settings.Default.originY;
            numericUpDownXpoint.Value = (decimal)Properties.Settings.Default.pointOnX;
            numericUpDownYpoint.Value = (decimal)Properties.Settings.Default.pointOnY;

            textBoxAngle.Text = Convert.ToString(Properties.Settings.Default.angle);

        }

        private void numericUpDownXorigin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.originX = (double)numericUpDownXorigin.Value;
        }

        private void numericUpDownYorigin_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.originY = (double)numericUpDownYorigin.Value;
        }

        private void numericUpDownXpoint_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.pointOnX = (double)numericUpDownXpoint.Value;
        }

        private void numericUpDownYpoint_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.pointOnY= (double)numericUpDownYpoint.Value;
        }

    }
}
