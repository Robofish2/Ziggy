using System;
using System.Windows.Forms;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolGroup;

namespace AbbCom.Forms
{
    public partial class CalAdjust : Form
    {
        // Initialize
        MainForm mainForm;
        RuntimeParameters runtimeParameters;
        RuntimeParameters restoreParameters;
        CogJobManager mcjmAcq;

        public CalAdjust(MainForm mf, RuntimeParameters rp, CogJobManager cjm)
        {
            InitializeComponent();

            mainForm = mf;
            runtimeParameters = rp;
            restoreParameters = rp.ShallowCopy();
            mcjmAcq = cjm;

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {

            if (numericUpDownXadjust.Value == 0 && numericUpDownYadjust.Value == 0) { return; }

            // Prompt to save the adjustment
            bool saved = mainForm.SaveVisionParametersToQuickBuild();
            if (!saved) { return; }

            CogToolGroup lctg;
            CogCalibCheckerboardTool myCalibTool;

            try
            {
                // Adjust the Basler cal
                lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                //myCalibTool = (CogCalibCheckerboardTool)lctg.Tools["CalBasler"];
                myCalibTool = (CogCalibCheckerboardTool)lctg.Tools["CalBasler"];
                myCalibTool.Calibration.CalibratedOriginX = myCalibTool.Calibration.CalibratedOriginX + (double)numericUpDownXadjust.Value;
                myCalibTool.Calibration.CalibratedOriginY = myCalibTool.Calibration.CalibratedOriginY + (double)numericUpDownYadjust.Value;
                myCalibTool.Calibration.Calibrate();

                // Adjust the Flir cal
                //myCalibTool = (CogCalibCheckerboardTool)lctg.Tools["CalFlir"];
                myCalibTool = (CogCalibCheckerboardTool)lctg.Tools["CalFlir"];
                myCalibTool.Calibration.CalibratedOriginX = myCalibTool.Calibration.CalibratedOriginX + (double)numericUpDownXadjust.Value;
                myCalibTool.Calibration.CalibratedOriginY = myCalibTool.Calibration.CalibratedOriginY + (double)numericUpDownYadjust.Value;
                myCalibTool.Calibration.Calibrate();

                // Read adjusted results and update the read-only text boxes
                lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                //myCalibTool = (CogCalibCheckerboardTool)lctg.Tools["CalBasler"];
                myCalibTool = (CogCalibCheckerboardTool)lctg.Tools["CalBasler"];
                textBoxXorigin.Text = myCalibTool.Calibration.CalibratedOriginX.ToString();
                textBoxYorigin.Text = myCalibTool.Calibration.CalibratedOriginY.ToString();

                // Reset adjust boxes
                numericUpDownXadjust.Value = 0;
                numericUpDownYadjust.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CalAdjust_Load(object sender, EventArgs e)
        {
            CogToolGroup lctg;
            CogCalibCheckerboardTool myCalibTool;

            lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
            //myCalibTool = (CogCalibCheckerboardTool)lctg.Tools["CalBasler"];
            myCalibTool = (CogCalibCheckerboardTool)lctg.Tools["CalBasler"];
            textBoxXorigin.Text = myCalibTool.Calibration.CalibratedOriginX.ToString();
            textBoxYorigin.Text = myCalibTool.Calibration.CalibratedOriginY.ToString();

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
