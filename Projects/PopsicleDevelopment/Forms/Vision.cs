using Cognex.VisionPro.Blob;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.QuickBuild;
using Cognex.VisionPro.ToolGroup;
using System;
using System.Windows.Forms;

namespace AbbCom.Forms
{
    public partial class Vision : Form
    {
        // Initialize
        MainForm mainForm;
        RuntimeParameters runtimeParameters;
        RuntimeParameters restoreParameters;
        MachineConfig machineParameters;
        CogJobManager mcjmAcq;

        bool formLoadComplete = false;
        bool saveParameters = false;
        bool normalExit = false;

        public Vision(MainForm mf, RuntimeParameters rp, CogJobManager cjm, MachineConfig mc)
        {
            InitializeComponent();

            mainForm = mf;
            runtimeParameters = rp;
            machineParameters = mc;
            restoreParameters = rp.ShallowCopy();
            mcjmAcq = cjm;

        }

        private void any_numeric_ValueChanged(object sender, EventArgs e)
        {

            if (!formLoadComplete) { return; }

            string name = ((NumericUpDown)sender).Name;
            CogToolGroup lctg;
            CogBlobTool myTool;
            CogPMAlignTool myPMAlignTool;

            // Update object members
            switch (name)
            {
                case "numericUpDownGreyVisThreshold":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisGreyThreshold; //Restore value
                        return;
                    }

                    runtimeParameters.VisGreyThreshold = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    myTool = (CogBlobTool)lctg.Tools["FindWrappersInBasler"];
                    myTool.RunParams.SegmentationParams.HardFixedThreshold = (int)runtimeParameters.VisGreyThreshold;

                    break;
                case "numericUpDownVisGreyAreaMin":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisGreyAreaMin; //Restore value
                        return;
                    }

                    runtimeParameters.VisGreyAreaMin = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    myTool = (CogBlobTool)lctg.Tools["FindWrappersInBasler"];
                    myTool.RunParams.RunTimeMeasures[0].FilterRangeLow = (double)runtimeParameters.VisGreyAreaMin;

                    break;
                case "numericUpDownVisGreyAreaMax":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisGreyAreaMax; //Restore value
                        return;
                    }

                    runtimeParameters.VisGreyAreaMax = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    myTool = (CogBlobTool)lctg.Tools["FindWrappersInBasler"];
                    myTool.RunParams.RunTimeMeasures[0].FilterRangeHigh = (double)runtimeParameters.VisGreyAreaMax;

                    break;
                case "numericUpDownVisThermAreaMin":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisThermAreaMin; //Restore value
                        return;
                    }

                    runtimeParameters.VisThermAreaMin = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    myTool = (CogBlobTool)lctg.Tools["PopsicleBlobFinder"];
                    myTool.RunParams.RunTimeMeasures[0].FilterRangeLow = (double)runtimeParameters.VisThermAreaMin;

                    break;
                case "numericUpDownVisThermAreaMax":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisThermAreaMax; //Restore value
                        return;
                    }

                    runtimeParameters.VisThermAreaMax = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    myTool = (CogBlobTool)lctg.Tools["PopsicleBlobFinder"];
                    myTool.RunParams.RunTimeMeasures[0].FilterRangeHigh = (double)runtimeParameters.VisThermAreaMax;

                    break;
                case "numericUpDownThermVisThreshold":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisThermThreshold; //Restore value
                        return;
                    }

                    runtimeParameters.VisThermThreshold = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    myTool = (CogBlobTool)lctg.Tools["PopsicleBlobFinder"];
                    myTool.RunParams.SegmentationParams.HardFixedThreshold = (int)runtimeParameters.VisThermThreshold;

                    break;

                case "numericUpDownPatternAcceptThreshold":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisPatMatchAcceptThreshold * 100; //Restore value
                        return;
                    }

                    runtimeParameters.VisPatMatchAcceptThreshold = (double)((NumericUpDown)sender).Value / 100;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    myPMAlignTool = (CogPMAlignTool)lctg.Tools["FindWrapperPatterns"];
                    myPMAlignTool.RunParams.AcceptThreshold = (double)runtimeParameters.VisPatMatchAcceptThreshold;

                    break;

                case "numericUpDownMinHistCount":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisThermMinHistCount; //Restore value
                        return;
                    }

                    runtimeParameters.VisThermMinHistCount = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    lctg.SetScriptTerminalData("MinPopsicleHistCount", (double)numericUpDownMinHistCount.Value);

                    break;

                case "numericUpDownVisEvenSortAngle":
                    runtimeParameters.VisEvenSortAngle = (double)((NumericUpDown)sender).Value;
                    break;

                case "numericUpDownVisOddSortAngle":
                    runtimeParameters.VisOddSortAngle = (double)((NumericUpDown)sender).Value;
                    break;

                case "numericUpDownVisSideXLength":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisSideXLength; //Restore value
                        return;
                    }
                    runtimeParameters.VisSideXLength = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    lctg.SetScriptTerminalData("VisSideXLength", (double)numericUpDownVisSideXLength.Value);

                    break;

                case "numericUpDownVisSideYLength":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisSideYLength; //Restore value
                        return;
                    }
                    runtimeParameters.VisSideYLength = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    lctg.SetScriptTerminalData("VisSideYLength", (double)numericUpDownVisSideYLength.Value);

                    break;

                case "numericUpDownFlirRegionXadj":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisFlirRegionXadj; //Restore value
                        return;
                    }
                    runtimeParameters.VisFlirRegionXadj = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    lctg.SetScriptTerminalData("VisFlirRegionXadj", (double)numericUpDownFlirRegionXadj.Value);

                    break;

                case "numericUpDownFlirRegionYadj":

                    if (mcjmAcq == null)
                    {
                        MessageBox.Show("Unable to read value from Vision System @any_numeric_ValueChanged", "Warning");
                        ((NumericUpDown)sender).Value = (decimal)runtimeParameters.VisFlirRegionYadj; //Restore value
                        return;
                    }
                    runtimeParameters.VisFlirRegionYadj = (double)((NumericUpDown)sender).Value;

                    lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
                    lctg.SetScriptTerminalData("VisFlirRegionYadj", (double)numericUpDownFlirRegionYadj.Value);

                    break;
            }

        }

        private void Vision_Load(object sender, EventArgs e)
        {

            // Update controls
            try
            {
                numericUpDownGreyVisThreshold.Value = (decimal)runtimeParameters.VisGreyThreshold;
                numericUpDownVisGreyAreaMin.Value = (decimal)runtimeParameters.VisGreyAreaMin;
                numericUpDownVisGreyAreaMax.Value = (decimal)runtimeParameters.VisGreyAreaMax;
                numericUpDownVisThermAreaMin.Value = (decimal)runtimeParameters.VisThermAreaMin;
                numericUpDownVisThermAreaMax.Value = (decimal)runtimeParameters.VisThermAreaMax;
                numericUpDownThermVisThreshold.Value = (decimal)runtimeParameters.VisThermThreshold;
                numericUpDownMinHistCount.Value = (decimal)runtimeParameters.VisThermMinHistCount;

                checkBoxVisSendLocsToR1.Checked = runtimeParameters.VisSendLocsToR1;
                checkBoxVisSendLocsToR2.Checked = runtimeParameters.VisSendLocsToR2;
                checkBoxVisSendLocsToR3.Checked = runtimeParameters.VisSendLocsToR3;
                checkBoxVisSendLocsToR4.Checked = runtimeParameters.VisSendLocsToR4;

                checkBoxVisShowLeftSortOrder.Checked = runtimeParameters.VisShowEvenSortOrder;
                checkBoxVisShowRightSortOrder.Checked = runtimeParameters.VisShowOddSortOrder;
                checkBoxVisDisableThermInspection.Checked = runtimeParameters.VisDisableTherm;
                checkBoxShowItemID.Checked = runtimeParameters.VisShowItemID;
                checkBoxShowArea.Checked = runtimeParameters.VisShowArea;
                checkBoxShowHistCount.Checked = runtimeParameters.VisShowHistCount;
                checkBoxShowItemCountInIO.Checked = runtimeParameters.VisShowItemCount;

                comboBoxVisMode.Text = runtimeParameters.VisMode;

                numericUpDownVisEvenSortAngle.Value = (decimal)runtimeParameters.VisEvenSortAngle;
                numericUpDownVisOddSortAngle.Value = (decimal)runtimeParameters.VisOddSortAngle;
                numericUpDownPictureFrequency.Value = (decimal)runtimeParameters.VisPictureFrequency;

                numericUpDownVisionJobNumber.Value = runtimeParameters.VisionJobNumber;
                checkBoxShowTriggerOutput.Checked = runtimeParameters.VisShowVisionTriggers;

                numericUpDownVisSideXLength.Value = (decimal)runtimeParameters.VisSideXLength;
                numericUpDownVisSideYLength.Value = (decimal)runtimeParameters.VisSideYLength;
                numericUpDownFlirRegionXadj.Value = (decimal)runtimeParameters.VisFlirRegionXadj;
                numericUpDownFlirRegionYadj.Value = (decimal)runtimeParameters.VisFlirRegionYadj;
                numericUpDownVisSideXLength.Enabled = false;
                numericUpDownVisSideYLength.Enabled = false;
                numericUpDownFlirRegionXadj.Enabled = false;
                numericUpDownFlirRegionYadj.Enabled = false;

                checkBoxEnableFlirRegionParms.Enabled = false;
                buttonCalAdjust.Enabled = false;
                if (mainForm.GetUserAccountLevel(mainForm.CurrentLoggedOnUser) >= 3)
                {
                    buttonCalAdjust.Enabled = true;
                    checkBoxEnableFlirRegionParms.Enabled = true;
                }

                radioButtonBlob.Enabled = false;
                radioButtonPatternMatch.Enabled = false;
                if (runtimeParameters.VisUseBlob)
                {
                    radioButtonBlob.Checked = true;
                    numericUpDownGreyVisThreshold.Value = (decimal)runtimeParameters.VisGreyThreshold;
                    EnableBlobFields();
                    buttonPatternParams.Enabled = false;
                }
                if (runtimeParameters.VisUsePatternMatch)
                {
                    radioButtonPatternMatch.Checked = true;
                    numericUpDownPatternAcceptThreshold.Value = (decimal)runtimeParameters.VisPatMatchAcceptThreshold * 100;
                    EnablePatternMatchFields();
                }


                groupBoxImageAcquireType.Enabled = false;
                if (mainForm.GetUserAccountLevel(mainForm.CurrentLoggedOnUser) >= 3)
                {
                    groupBoxImageAcquireType.Enabled = true;
                }


                // Disable certain items if vision is online
                if (mainForm.VisionOnline)
                {
                    buttonCalAdjust.Enabled = false;        // Disable the Cal Adjust button
                    buttonThermalAdvanced.Enabled = false;  // Disable the Advanced button (Thermal Image Settings)
                    checkBoxEnableProcessingType.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception @Vision_Load:" + ex.Message);
            }
            finally
            {
                formLoadComplete = true;
            }
        }

        private void comboBoxVisMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisMode = comboBoxVisMode.Text;
        }

        private void checkBoxVisSendLocsToR1_CheckedChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisSendLocsToR1 = checkBoxVisSendLocsToR1.Checked;
        }
        private void checkBoxVisSendLocsToR2_CheckedChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisSendLocsToR2 = checkBoxVisSendLocsToR2.Checked;
        }

        private void checkBoxVisSendLocsToR3_CheckedChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisSendLocsToR3 = checkBoxVisSendLocsToR3.Checked;
        }

        private void checkBoxVisSendLocsToR4_CheckedChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisSendLocsToR4 = checkBoxVisSendLocsToR4.Checked;
        }

        private void checkBoxVisShowLeftSortOrder_CheckedChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisShowEvenSortOrder = checkBoxVisShowLeftSortOrder.Checked;
        }

        private void checkBoxVisShowRightSortOrder_CheckedChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisShowOddSortOrder = checkBoxVisShowRightSortOrder.Checked;
        }

        private void checkBoxVisDisableThermInspection_CheckedChanged(object sender, EventArgs e)
        {
            if (mcjmAcq == null)
            {
                MessageBox.Show("Unable to disable inspection. Check that the camera is online.", "Warning!");
                ((CheckBox)sender).Checked = false;
                return;
            }

            CogToolGroup lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
            lctg.SetScriptTerminalData("DisableHistogramInspection", checkBoxVisDisableThermInspection.Checked);
            runtimeParameters.VisDisableTherm = checkBoxVisDisableThermInspection.Checked;
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
            Close();
        }
        private void Vision_FormClosing(object sender, FormClosingEventArgs e)
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

                // Reload the recipe
                bool success;
                restoreParameters = mainForm.DeserializeRuntimeParamters(mainForm.RecipeName + ".xml", out success);
                if (!success)
                {
                    MessageBox.Show("Error reading recipe file.", "Warning!");
                    return;
                }

            }
        }

        private void buttonGreyscaleAdvanced_Click(object sender, EventArgs e)
        {
            bool success;
            runtimeParameters = mainForm.DeserializeRuntimeParamters(mainForm.RecipeName + ".xml", out success);
            if (!success)
            {
                MessageBox.Show("Error reading 'runtimeParameters' file.", "Warning!");
                return;
            }

            using (AcqFifoAdjust acq = new AcqFifoAdjust(mainForm, runtimeParameters, mcjmAcq))
            {
                acq.ShowDialog();
            }

        }

        private void checkBoxShowItemID_CheckedChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisShowItemID = checkBoxShowItemID.Checked;
        }

        private void checkBoxShowArea_CheckedChanged(object sender, EventArgs e)
        {
            if (mcjmAcq == null)
            {
                MessageBox.Show("Unable to show Area. Check that the camera is online.", "Warning!");
                ((CheckBox)sender).Checked = false;
                return;
            }

            CogToolGroup lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
            lctg.SetScriptTerminalData("ShowArea", checkBoxShowArea.Checked);
            runtimeParameters.VisShowArea = checkBoxShowArea.Checked;
        }

        private void checkBoxShowHistCount_CheckedChanged(object sender, EventArgs e)
        {

            if (mcjmAcq == null)
            {
                MessageBox.Show("Unable to show Hist Count. Check that the camera is online.", "Warning!");
                ((CheckBox)sender).Checked = false;
                return;
            }

            CogToolGroup lctg = (CogToolGroup)mcjmAcq.Job(mainForm.CogJobName).VisionTool;
            lctg.SetScriptTerminalData("ShowHistCount", checkBoxShowHistCount.Checked);
            runtimeParameters.VisShowHistCount = checkBoxShowHistCount.Checked;
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

        private void buttonCalAdjust_Click(object sender, EventArgs e)
        {

            using (CalAdjust ca = new CalAdjust(mainForm, runtimeParameters, mcjmAcq))
            {
                ca.ShowDialog();
            }

        }

        private void numericUpDownPictureFrequency_ValueChanged(object sender, EventArgs e)
        {
            mainForm.VisionPictureFrequency = (int)numericUpDownPictureFrequency.Value;
            runtimeParameters.VisPictureFrequency = (int)numericUpDownPictureFrequency.Value;
            mainForm.timerPictureTimer.Interval = runtimeParameters.VisPictureFrequency;
            mainForm.Invalidate();  // Force redraw
        }

        private void checkBoxShowItemCountInIO_CheckedChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisShowItemCount = checkBoxShowItemCountInIO.Checked;
        }

        private void numericUpDownVisionJobNumber_ValueChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisionJobNumber = (int)numericUpDownVisionJobNumber.Value;
        }

        private void checkBoxShowTriggerOutput_CheckedChanged(object sender, EventArgs e)
        {
            runtimeParameters.VisShowVisionTriggers = checkBoxShowTriggerOutput.Checked;
        }

        private void buttonThermalAdvanced_Click(object sender, EventArgs e)
        {
            VisionThermalAdvanced vta = new VisionThermalAdvanced(mainForm, machineParameters);
            vta.ShowDialog();
        }

        private void radioButtonBlob_CheckedChanged(object sender, EventArgs e)
        {

            if (!formLoadComplete) { return; }

            string dummy = string.Empty;

            bool found = mainForm.VerifyCogJobExists(mainForm.JobsPath, mainForm.RecipeName, runtimeParameters.VisCogJobPatternName, out dummy);

            if (!found)
            {
                string str = runtimeParameters.VisCogJobPatternName.ToString() + " is not found in Quickbuild";
                MessageBox.Show(str, "Error");

                this.radioButtonPatternMatch.CheckedChanged -= new System.EventHandler(this.radioButtonPatternMatch_CheckedChanged);
                this.radioButtonBlob.CheckedChanged -= new System.EventHandler(this.radioButtonBlob_CheckedChanged);
                radioButtonBlob.Checked = !radioButtonBlob.Checked;
                return;
            }

            mainForm.CogJobName = runtimeParameters.VisCogJobBlobName;
            runtimeParameters.VisUseBlob = true;
            runtimeParameters.VisUsePatternMatch = false;

            numericUpDownGreyVisThreshold.Value = (decimal)runtimeParameters.VisGreyThreshold;
            EnableBlobFields();
            mainForm.UpdateQuickBuild();

        }

        private void EnableBlobFields()
        {
            numericUpDownVisGreyAreaMin.Visible = true;
            numericUpDownVisGreyAreaMax.Visible = true;
            numericUpDownGreyVisThreshold.Visible = true;
            labelMinArea.Visible = true;
            labelMaxArea.Visible = true;
            labelGreyscale.Visible = true;
            labelThreshold0to255.Visible = true;
            labelPatternAcceptThreshold.Visible = false;
            numericUpDownPatternAcceptThreshold.Visible = false;
            checkBoxShowArea.Text = "Show Area (in Greyscale Image)";
            buttonPatternParams.Enabled = false;

        }

        private void radioButtonPatternMatch_CheckedChanged(object sender, EventArgs e)
        {
            if (!formLoadComplete) { return; }

            string dummy = string.Empty;

            bool found = mainForm.VerifyCogJobExists(mainForm.JobsPath, mainForm.RecipeName, runtimeParameters.VisCogJobPatternName, out dummy);

            if (!found)
            {
                string str = runtimeParameters.VisCogJobPatternName.ToString() + " is not found in Quickbuild";
                MessageBox.Show(str, "Error");

                this.radioButtonPatternMatch.CheckedChanged -= new System.EventHandler(this.radioButtonPatternMatch_CheckedChanged);
                this.radioButtonBlob.CheckedChanged -= new System.EventHandler(this.radioButtonBlob_CheckedChanged);
                radioButtonPatternMatch.Checked = !radioButtonPatternMatch.Checked;
                return;
            }

            mainForm.CogJobName = runtimeParameters.VisCogJobPatternName;

            runtimeParameters.VisUsePatternMatch = true;
            runtimeParameters.VisUseBlob = false;

            numericUpDownPatternAcceptThreshold.Value = (decimal)runtimeParameters.VisPatMatchAcceptThreshold * 100;

            EnablePatternMatchFields();
            mainForm.UpdateQuickBuild();
        }

        private void EnablePatternMatchFields()
        {
            numericUpDownVisGreyAreaMin.Visible = false;
            numericUpDownVisGreyAreaMax.Visible = false;
            numericUpDownGreyVisThreshold.Visible = false;
            numericUpDownPatternAcceptThreshold.Visible = true;
            labelMinArea.Visible = false;
            labelMaxArea.Visible = false;
            labelGreyscale.Visible = false;
            labelThreshold0to255.Visible = false;
            labelPatternAcceptThreshold.Visible = true;
            checkBoxShowArea.Text = "Show Score (in Greyscale Image)";
            buttonPatternParams.Enabled = true;
            //mainForm.Invalidate();

        }


        private void checkBoxEnableProcessingType_CheckedChanged(object sender, EventArgs e)
        {

            radioButtonBlob.Enabled = false;
            radioButtonPatternMatch.Enabled = false;
            if (checkBoxEnableProcessingType.Checked)
            {
                if (mainForm.GetUserAccountLevel(mainForm.CurrentLoggedOnUser) < 3)
                {
                    checkBoxEnableProcessingType.Checked = false;
                    return;
                }
                radioButtonBlob.Enabled = true;
                radioButtonPatternMatch.Enabled = true;
            }
        }

        private void buttonPatternParams_Click(object sender, EventArgs e)
        {

            bool success;
            runtimeParameters = mainForm.DeserializeRuntimeParamters(mainForm.RecipeName + ".xml", out success);
            if (!success)
            {
                MessageBox.Show("Error reading 'runtimeParameters' file.", "Warning!");
                return;
            }

            using (PatternParams ca = new PatternParams(mainForm, runtimeParameters, machineParameters, mcjmAcq))
            {
                ca.ShowDialog();
            }
        }

        private void checkBoxEnableFlirRegionParms_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEnableFlirRegionParms.Checked)
            {
                numericUpDownVisSideXLength.Enabled = true;
                numericUpDownVisSideYLength.Enabled = true;
                numericUpDownFlirRegionXadj.Enabled = true;
                numericUpDownFlirRegionYadj.Enabled = true;
            }
        }


    }
}
