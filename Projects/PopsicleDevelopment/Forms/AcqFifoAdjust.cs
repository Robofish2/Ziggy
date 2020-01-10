using System;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolGroup;

namespace AbbCom.Forms
{
    public partial class AcqFifoAdjust : Form
    {
        // Initialize
        MainForm mainForm;
        RuntimeParameters runtimeParameters;
        RuntimeParameters restoreParameters;
        CogJobManager mcjmAcq;

        bool formLoadComplete = false;
        bool saveParameters = false;
        bool normalExit = false;

        public AcqFifoAdjust(MainForm mf, RuntimeParameters rp, CogJobManager cjm)
        {
            InitializeComponent();

            mainForm = mf;
            runtimeParameters = rp;
            restoreParameters = rp.ShallowCopy();
            mcjmAcq = cjm;

        }

        private void any_numeric_ValueChanged(object sender, EventArgs e)
        {
            if (!formLoadComplete) { return; }

            string name = ((NumericUpDown)sender).Name;
            CogToolGroup lctg;
            CogAcqFifoTool myAcqTool;
           

            // Update object members
            switch (name)
            {
                case "numericUpDownGreyExposure":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisGreyExposure; //Restore value
                        return;
                    }

                    try
                    {
                        lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                        myAcqTool = (CogAcqFifoTool)lctg.Tools["BaslerAcqFiFo"];
                        myAcqTool.Operator.OwnedExposureParams.Exposure = (double)((NumericUpDown)sender).Value;
                        runtimeParameters.VisGreyExposure = (double)((NumericUpDown)sender).Value;
                    }
                    catch (Cognex.VisionPro.Exceptions.CogAcqNoFrameGrabberException ex)
                    {
                        this.numericUpDownGreyExposure.ValueChanged -= new System.EventHandler(this.any_numeric_ValueChanged);
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisGreyExposure; //Restore value
                        MessageBox.Show(ex.Message);
                        this.numericUpDownGreyExposure.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);

                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }

                    break;
                case "numericUpDownGreyBrightness":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisGreyBrightness; //Restore value
                        return;
                    }

                    try
                    {
                        lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                        myAcqTool = (CogAcqFifoTool)lctg.Tools["BaslerAcqFifo"];
                        myAcqTool.Operator.OwnedBrightnessParams.Brightness = (double)((NumericUpDown)sender).Value;
                        runtimeParameters.VisGreyBrightness = (double)((NumericUpDown)sender).Value;
                    }
                    catch (Cognex.VisionPro.Exceptions.CogAcqNoFrameGrabberException ex)
                    {
                        this.numericUpDownGreyBrightness.ValueChanged -= new System.EventHandler(this.any_numeric_ValueChanged);
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisGreyBrightness; //Restore value
                        MessageBox.Show(ex.Message);
                        this.numericUpDownGreyBrightness.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    break;

                case "numericUpDownGreyContrast":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisGreyContrast; //Restore value
                        return;
                    }

                    try
                    {
                        lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                        myAcqTool = (CogAcqFifoTool)lctg.Tools["BaslerAcqFifo"];

                        myAcqTool.Operator.OwnedContrastParams.Contrast = (double)((NumericUpDown)sender).Value;
                        runtimeParameters.VisGreyContrast = (double)((NumericUpDown)sender).Value;
                    }
                    catch (Cognex.VisionPro.Exceptions.CogAcqNoFrameGrabberException ex)
                    {
                        this.numericUpDownGreyContrast.ValueChanged -= new System.EventHandler(this.any_numeric_ValueChanged);
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisGreyContrast; //Restore value
                        MessageBox.Show(ex.Message);
                        this.numericUpDownGreyContrast.ValueChanged += new System.EventHandler(this.any_numeric_ValueChanged);
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    break;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            normalExit = true;
            saveParameters = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            normalExit = true;

            bool cond1 = false;

            // Decide to restore the parameters
            cond1 = runtimeParameters.VisGreyExposure != restoreParameters.VisGreyExposure;
            cond1 = cond1 || runtimeParameters.VisGreyBrightness != restoreParameters.VisGreyBrightness;
            cond1 = cond1 || runtimeParameters.VisGreyContrast != restoreParameters.VisGreyContrast;

            if (cond1)
            {
                RestoreAcqFifoParameters();
            }

            Close();
        }

        private void RestoreAcqFifoParameters()
        {

            CogToolGroup lctg;
            CogAcqFifoTool myAcqTool;

            lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
            myAcqTool = (CogAcqFifoTool)lctg.Tools["BaslerAcqFifo"];

            // Restore the Exposure
            myAcqTool.Operator.OwnedExposureParams.Exposure = (double)restoreParameters.VisGreyExposure;

            // Restore the Brightness
            myAcqTool.Operator.OwnedBrightnessParams.Brightness = (double)restoreParameters.VisGreyBrightness;

            // Restore the Contrast
            myAcqTool.Operator.OwnedContrastParams.Contrast = (double)restoreParameters.VisGreyContrast;

            mainForm.SerializeRuntimeParameters(restoreParameters);

        }

        private void AcqFifo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!normalExit)
            {
                if (!mainForm.RecipeChangeover)
                {
                    if (DialogResult.Yes == MessageBox.Show("Save changes?", "Save?", MessageBoxButtons.YesNo))
                    {
                        saveParameters = true;
                    }
                }
            }

            if (saveParameters)
            {
                mainForm.SerializeRuntimeParameters(runtimeParameters);
            }
            else
            {
                mainForm.SerializeRuntimeParameters(restoreParameters);
            }
        }

        private void AcqFifo_Load(object sender, EventArgs e)
        {
            // Update controls
            try
            {
                numericUpDownGreyExposure.Value = (decimal)runtimeParameters.VisGreyExposure;
                numericUpDownGreyBrightness.Value = (decimal)runtimeParameters.VisGreyBrightness;
                numericUpDownGreyContrast.Value = (decimal)runtimeParameters.VisGreyContrast;

                if (mainForm.VisionOnline)
                {
                    buttonManualTrigger.Enabled = false;
                }
                else
                {
                    buttonManualTrigger.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception @AcqFifo_Load:" + ex.Message);
            }
            finally
            {
                formLoadComplete = true;
            }

        }

        private void buttonManualTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                mainForm.RunQuickBuildJob();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Check for F key presses
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
    }
}
