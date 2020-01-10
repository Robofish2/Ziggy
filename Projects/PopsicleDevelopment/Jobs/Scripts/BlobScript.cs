using System;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.CalibFix;

public class UserScript : CogToolGroupBaseScript
{

    //An array to hold a series of labels for the tool group's run record.
    ArrayList BaslerLabels = new ArrayList();
    ArrayList FlirLabels = new ArrayList();
    ArrayList Crosshairs = new ArrayList();

    //An array to hold a series of graphic rectangles for the tool groups's run record
    ArrayList FlirRectangles = new ArrayList();
    //List<Int32> binHist = new List<Int32>();
    Int32[] binHist = new Int32[512];
    ArrayList malBlobPoses = new ArrayList();

    public double minPickAngle;
    public double maxPickAngle;
    public double minPopsicleHistCount;
    public bool disableHist;
    public bool VisStudio_Running;
    public bool showArea;
    public bool showHist;
    public Int32 histCntr;
    public Int32 angleFailures;
    public Int32 thermalFailures;
    public double visSideXLength;
    public double visSideYLength;
    public double visFlirRegionXadj;
    public double visFlirRegionYadj;

    //#region "when the tool group is run"

    // The GroupRun function is called when the tool group is run.  The default
    // implementation provided here is equivalent to the normal behavior of the
    // tool group.  Modifying this function will allow you to change the behavior
    // when the tool group is run.
    public override bool GroupRun(ref string message, ref CogToolResultConstants result)
    {
        // To let the execution stop in this script when a debugger is attached, uncomment the following lines.
#if DEBUG
    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
#endif

        histCntr = 0;
        CogGraphicLabel myBaslerLabel = new CogGraphicLabel();
        CogGraphicLabel myFlirLabel = new CogGraphicLabel();
        CogPointMarker baslerMarker; //= new CogPointMarker();
        double blobAngle = 0;
        double blobAngleRadians = 0;

        //Get references to the tools
        CogBlobTool blobBaslerTool = (CogBlobTool)toolGroup.Tools["FindWrappersInBasler"];
        CogBlobTool blobFlirTool = (CogBlobTool)toolGroup.Tools["PopsicleBlobFinder"];
        CogHistogramTool popsicleHistTool = (CogHistogramTool)toolGroup.Tools["PopsicleHistogramTool"];

        //Define the regions
        CogRectangleAffine popsicleRegion = blobFlirTool.Region as CogRectangleAffine;
        CogRectangleAffine histogramRegion = blobFlirTool.Region as CogRectangleAffine;


        //Define the fonts
        System.Drawing.Font myBaslerFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        System.Drawing.Font myFlirFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


        //Reset stats
        angleFailures = 0;
        thermalFailures = 0;

        //Reset any labels and rectangles from previous runs
        BaslerLabels.Clear();
        FlirLabels.Clear();
        FlirRectangles.Clear();


        //***** Run the tools to perform the search in the basler image *****

        //Update status strings for Visual Studio. Do not popup a message box for errors, this causes confusion with the operators.
        toolGroup.SetScriptTerminalData("BaslerStatus", "OK");
        toolGroup.SetScriptTerminalData("FlirStatus", "OK");

        bool acquireException = false;
        try
        {
            //Aquire an image from the Basler, send Exception to Visual Studio
            toolGroup.RunTool(toolGroup.Tools["BaslerAcqFifo"], ref message, ref result);
        }
        catch (Exception ex)
        {
            toolGroup.SetScriptTerminalData("BaslerStatus", "Script Error: " + ex.Message + " Check power connections and communication cables.");
            acquireException = true;
        }

        try
        {
            //Aquire an image from the Flir, send Exception to Visual Studio
            toolGroup.RunTool(toolGroup.Tools["FlirAcqFifo"], ref message, ref result);
        }
        catch (Exception ex)
        {
            toolGroup.SetScriptTerminalData("FlirStatus", "Script Error: " + ex.Message + " Check power connections and communication cables.");
            acquireException = true;
        }
        if (acquireException)
        {
            return false;
        }

        try
        {
            // Scale the Flir image to zoom in to smaller temperature range
            toolGroup.RunTool(toolGroup.Tools["ScaleFlirImagePmap"], ref message, ref result);
        }
        catch (Exception ex)
        {
            toolGroup.SetScriptTerminalData("FlirStatus", "Script Error: " + ex.Message + " Failed: ScaleFlirImagePmap");
            return false;
        }


        try
        {
            // Transform the images to calibrated space
            toolGroup.RunTool(toolGroup.Tools["CalBasler"], ref message, ref result);
            toolGroup.RunTool(toolGroup.Tools["CalFlir"], ref message, ref result);
        }
        catch (Exception ex)
        {
            //MessageBox.Show("Calibration tool error: " + ex.Message, "Script Exception");
            toolGroup.SetScriptTerminalData("BaslerStatus", "Script Error: " + ex.Message + " Failed: CalBasler");
            toolGroup.SetScriptTerminalData("FlirStatus", "Script Error: " + ex.Message + " Failed: CalFlir");
            return false;
        }

        try
        {
            toolGroup.RunTool(toolGroup.Tools["CogPixelMapBasler"], ref message, ref result);
        }
        catch (Exception ex)
        {
            toolGroup.SetScriptTerminalData("BaslerStatus", "Script Error: " + ex.Message + " Failed: CogPixelMapBasler");
            return false;
        }

        try
        {
            toolGroup.RunTool(toolGroup.Tools["CogIPOneImageTool1"], ref message, ref result);
        }
        catch (Exception ex)
        {
            toolGroup.SetScriptTerminalData("BaslerStatus", "Script Error: " + ex.Message + " Failed: CogIPOneImageTool1");
            return false;
        }

        try
        {
            // Run the Flir hist tool
            toolGroup.RunTool(toolGroup.Tools["PopsicleHistogramTool"], ref message, ref result);
        }
        catch (Exception ex)
        {
            toolGroup.SetScriptTerminalData("FlirStatus", "Script Error: " + ex.Message + " Failed: PopsicleHistogramTool");
            return false;
        }

        try
        {
            // Run the blob tool and get a reference to the results.     
            toolGroup.RunTool(blobBaslerTool, ref message, ref result);
        }
        catch (Exception ex)
        {
            toolGroup.SetScriptTerminalData("BaslerStatus", "Script Error: " + ex.Message + " Failed: Basler blobBaslerTool");
            return false;
        }

        CogBlobResultCollection blobResults = blobBaslerTool.Results.GetBlobs();


        // Clear list before starting loop
        malBlobPoses.Clear();
        Crosshairs.Clear();

        // Get group input terminal data
        try
        {
            disableHist = (bool)toolGroup.GetScriptTerminalData("DisableHistogramInspection");
            minPickAngle = (double)toolGroup.GetScriptTerminalData("MinPickAngle");
            maxPickAngle = (double)toolGroup.GetScriptTerminalData("MaxPickAngle");
            minPopsicleHistCount = (double)toolGroup.GetScriptTerminalData("MinPopsicleHistCount");
            VisStudio_Running = (bool)toolGroup.GetScriptTerminalData("VS_Running");
            showArea = (bool)toolGroup.GetScriptTerminalData("ShowArea");
            showHist = (bool)toolGroup.GetScriptTerminalData("ShowHistCount");
            visSideXLength = (double)toolGroup.GetScriptTerminalData("VisSideXLength");
            visSideYLength = (double)toolGroup.GetScriptTerminalData("VisSideYLength");
            visFlirRegionXadj = (double)toolGroup.GetScriptTerminalData("VisFlirRegionXadj");
            visFlirRegionYadj = (double)toolGroup.GetScriptTerminalData("VisFlirRegionYadj");
        }
        catch (Exception ex)
        {
            //MessageBox.Show("Getting terminal data exception: ", ex.Message);
            toolGroup.SetScriptTerminalData("BaslerStatus", "Getting script data: " + ex.Message);
        }

        // Set run variables for manual triggering
        if (!VisStudio_Running)
        {
            minPopsicleHistCount = 0;
            minPickAngle = -20;
            maxPickAngle = 20;
            showArea = true;
            showHist = true;
            visSideXLength = 200;
            visSideYLength = 70;
            visFlirRegionXadj = 0;
            visFlirRegionYadj = 0;
        }


        // ***************************************
        // ******** Process the blobs *********
        // ***************************************
        try
        {
            foreach (CogBlobResult blob in blobResults)
            {

                // Set the transform for collections
                CogTransform2DLinear l2d = new CogTransform2DLinear();
                l2d.TranslationX = blob.GetBoundingBoxAtAngle(blob.Angle).CenterX;
                l2d.TranslationY = blob.GetBoundingBoxAtAngle(blob.Angle).CenterY;
                l2d.Rotation = blob.Angle;
                blobAngleRadians = blob.Angle;

                // Crosshair setup for the Basler
                baslerMarker = new CogPointMarker();
                baslerMarker.X = l2d.TranslationX;
                baslerMarker.Y = l2d.TranslationY;
                baslerMarker.Color = CogColorConstants.Green;
                baslerMarker.GraphicType = CogPointMarkerGraphicTypeConstants.Crosshair;

                // Flir region
                CogRectangleAffine myFlirRegion = new CogRectangleAffine();
                myFlirRegion.CenterX = l2d.TranslationX + visFlirRegionXadj;
                myFlirRegion.CenterY = l2d.TranslationY + visFlirRegionYadj;
                myFlirRegion.Rotation = l2d.Rotation;
                myFlirRegion.SideXLength = visSideXLength;
                myFlirRegion.SideYLength = visSideYLength;

                blobFlirTool.Region = myFlirRegion;
                toolGroup.RunTool(blobFlirTool, ref message, ref result);

                popsicleHistTool.Region = myFlirRegion;
                toolGroup.RunTool(popsicleHistTool, ref message, ref result);

                // Get the histogram results from the bin
                binHist = popsicleHistTool.Result.GetHistogram();

                // Count total pixels
                histCntr = 1;
                for (int i = 0; i < blobFlirTool.RunParams.SegmentationParams.HardFixedThreshold; i++)
                {
                    histCntr = histCntr + binHist[i];
                }

                myBaslerLabel = new CogGraphicLabel();
                myBaslerLabel.SetXYText(0, 0, "");
                myFlirLabel = new CogGraphicLabel();

                myBaslerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                myFlirLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                myFlirRegion.Visible = true;

                myBaslerLabel.Alignment = CogGraphicLabelAlignmentConstants.BaselineCenter;
                myFlirLabel.Alignment = CogGraphicLabelAlignmentConstants.BaselineCenter;

                // Decide to add the popsicle blob to the collection list  
                if ((histCntr < minPopsicleHistCount) && (!disableHist))
                {

                    myBaslerLabel.Color = CogColorConstants.Red;
                    myFlirRegion.Color = CogColorConstants.Red;


                    // Show the hist count in the Flir image
                    myFlirLabel.Color = CogColorConstants.Red;
                    if (showHist)
                    {
                        myFlirLabel.SetXYText(blob.CenterOfMassX, blob.CenterOfMassY, "Size: " + histCntr.ToString());
                    }
                    thermalFailures++;
                }
                else
                {

                    myBaslerLabel.Color = CogColorConstants.Green;


                    // If histogram check is disabled, draw rectangle in yellow, else green
                    if (disableHist)
                    {
                        myFlirLabel.Color = CogColorConstants.Red;
                        myFlirRegion.Color = CogColorConstants.Yellow;
                    }
                    else
                    {
                        myFlirLabel.Color = CogColorConstants.Green;
                        myFlirRegion.Color = CogColorConstants.Green;
                    }

                    if (showHist)
                    {
                        myFlirLabel.SetXYText(blob.CenterOfMassX, blob.CenterOfMassY, "Size: " + histCntr.ToString());
                    }


                    // Convert blob angle to degrees
                    blobAngle = blob.Angle * 180 / Math.PI;

                    if ((blobAngle > (double)minPickAngle) && (blobAngle < (double)maxPickAngle))
                    {

                        malBlobPoses.Add(l2d);

                        if (showArea)
                        {
                            myBaslerLabel.Color = CogColorConstants.Green;
                            myBaslerLabel.SetXYText(l2d.TranslationX, l2d.TranslationY - 15, "Size: " + blob.Area.ToString("0"));
                        }
                    }
                    else
                    {
                        myBaslerLabel.Color = CogColorConstants.Red;
                        myBaslerLabel.SetXYText(l2d.TranslationX, l2d.TranslationY, "Angle: " + blobAngle.ToString("0"));
                        myFlirLabel.Color = CogColorConstants.Red;
                        myFlirRegion.Color = CogColorConstants.Red;
                        angleFailures++;
                    }
                }

                myBaslerLabel.Rotation = blobAngleRadians;

                BaslerLabels.Add(myBaslerLabel);
                FlirLabels.Add(myFlirLabel);
                FlirRectangles.Add(myFlirRegion);
                Crosshairs.Add(baslerMarker);

                // Update group output terminals
                toolGroup.SetScriptTerminalData("AngleFailures", angleFailures);
                toolGroup.SetScriptTerminalData("ThermalFailures", thermalFailures);
                toolGroup.SetScriptTerminalData("BlobCollection", malBlobPoses);

            }

        }
        catch (Exception ex)
        {
            toolGroup.SetScriptTerminalData("BaslerStatus", "Script Error during blob processing: " + ex.Message);
            malBlobPoses.Clear(); // Clear positional data for this frame
        }

        // Returning False indicates we ran the tools in script, and they should not be
        // run by VisionPro 
        return false;
    }

    #region "When the Current Run Record is Created"
    public override void ModifyCurrentRunRecord(Cognex.VisionPro.ICogRecord currentRecord)
    {
    }
    #endregion

    #region "When the Last Run Record is Created"
    // Allows you to add or modify the contents of the last run record when it is
    // created.  For example, you might add custom graphics to the run record here.
    public override void ModifyLastRunRecord(Cognex.VisionPro.ICogRecord lastRecord)
    {
        foreach (CogGraphicLabel label in BaslerLabels)
        {
            toolGroup.AddGraphicToRunRecord(label, lastRecord, "CalBasler.OutputImage", "script");
        }

        foreach (CogGraphicLabel label in FlirLabels)
        {
            toolGroup.AddGraphicToRunRecord(label, lastRecord, "CalFlir.OutputImage", "script");
        }

        foreach (CogRectangleAffine rectangle in FlirRectangles)
        {
            toolGroup.AddGraphicToRunRecord(rectangle, lastRecord, "CalFlir.OutputImage", "script");
        }

        foreach (CogPointMarker marker in Crosshairs)
        {
            toolGroup.AddGraphicToRunRecord(marker, lastRecord, "CalBasler.OutputImage", "script");
        }
    }
    #endregion

    #region "When the Script is Initialized"
    // Perform any initialization required by your script here
    public override void Initialize(CogToolGroup host)
    {
        // DO NOT REMOVE - Call the base class implementation first - DO NOT REMOVE
        base.Initialize(host);

        //Define the terminals
        toolGroup.DefineScriptTerminal("System.Collections.ArrayList", "BlobCollection", false);

        toolGroup.DefineScriptTerminal("Boolean", disableHist, "DisableHistogramInspection", true);
        toolGroup.DefineScriptTerminal("double", minPickAngle, "MinPickAngle", true);
        toolGroup.DefineScriptTerminal("double", maxPickAngle, "MaxPickAngle", true);
        toolGroup.DefineScriptTerminal("double", minPopsicleHistCount, "MinPopsicleHistCount", true);
        toolGroup.DefineScriptTerminal("Boolean", VisStudio_Running, "VS_Running", true);
        toolGroup.DefineScriptTerminal("Boolean", showArea, "ShowArea", true);
        toolGroup.DefineScriptTerminal("Boolean", showHist, "ShowHistCount", true);
        toolGroup.DefineScriptTerminal("double", visSideXLength, "VisSideXLength", true);
        toolGroup.DefineScriptTerminal("double", visSideYLength, "VisSideYLength", true);
        toolGroup.DefineScriptTerminal("double", visFlirRegionXadj, "VisFlirRegionXadj", true);
        toolGroup.DefineScriptTerminal("double", visFlirRegionYadj, "VisFlirRegionYadj", true);

        toolGroup.DefineScriptTerminal("Int32", angleFailures, "AngleFailures", false);
        toolGroup.DefineScriptTerminal("Int32", thermalFailures, "ThermalFailures", false);
        toolGroup.DefineScriptTerminal("String", "", "BaslerStatus", false);
        toolGroup.DefineScriptTerminal("String", "", "FlirStatus", false);

        //Set the default values for the terminals
        toolGroup.SetScriptTerminalData("DisableHistogramInspection", false);
        toolGroup.SetScriptTerminalData("VS_Running", false);
        toolGroup.SetScriptTerminalData("ShowArea", false);
        toolGroup.SetScriptTerminalData("ShowHistCount", false);


    }
    #endregion

}

