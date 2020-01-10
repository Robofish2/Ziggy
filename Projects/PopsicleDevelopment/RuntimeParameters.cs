using System;
using System.Collections;
using System.Text;
using System.IO.Ports;

namespace AbbCom
{
    [Serializable]
    public class RuntimeParameters
    {

        public RuntimeParameters ShallowCopy()
        {
            return (RuntimeParameters)this.MemberwiseClone();
        }


        #region Vision parameters
        public double VisGreyExposure = 0;
        public double VisGreyBrightness = 0;
        public double VisGreyContrast = 0;
        public double VisGreyThreshold = 0;
        public double VisGreyAreaMin = 0;
        public double VisGreyAreaMax = 0;
        public double VisThermAreaMin = 0;
        public double VisThermAreaMax = 0;
        public double VisThermThreshold = 0;
        public double VisThermMinHistCount = 0;
        public double VisEvenSortAngle = 0;
        public double VisOddSortAngle = 0;

        public bool VisSendLocsToR1 = false;
        public bool VisSendLocsToR2 = false;
        public bool VisSendLocsToR3 = false;
        public bool VisSendLocsToR4 = false;
        public bool VisShowEvenSortOrder = false;
        public bool VisShowOddSortOrder = false;
        public bool VisDisableTherm = false;
        public bool VisShowItemID = false;
        public bool VisShowArea = false;
        public bool VisShowHistCount = false;
        public bool VisShowItemCount = false;

        public string VisMode = string.Empty;
        public double VisMinAngle = 0;
        public double VisMaxAngle = 0;
        public double VisSideXLength = 200;
        public double VisSideYLength = 70;
        public double VisXwindowMin = 0;
        public double VisXwindowMax = 2000;
        public double VisFlirRegionXadj = 0;
        public double VisFlirRegionYadj = 0;
        public int VisPictureFrequency = 500;
        public int VisionJobNumber = 1;
        public int VisionBeltYoverlap = 30;
        public bool VisShowVisionTriggers = false;

        public bool VisUseBlob = true;
        public bool VisUsePatternMatch = false;
        public double VisPatMatchAcceptThreshold = .75;
        public double VisPatRunParamsZoneScaleLow = .9;
        public double VisPatRunParamsZoneScaleHigh = 1.1;
        public double VisPatRunParamsZoneAngleLow = -0.523598776; //-30
        public double VisPatRunParamsZoneAngleHigh = 0.523598776; //30
        public double VisPatRunParamsZoneAngleOverlap = 3.14159265; //180
        public double VisPatRunParamsContrastThreshold = 33;
        public bool VisPatRunParamsCoarseAcceptThresholdChecked = false;
        public double VisPatRunParamsCoarseAcceptThreshold = .5;
        public double VisPatRunParamsElasticity = 5; //Pixels
        public string VisCogJobBlobName = "CogJobBlob";
        public string VisCogJobPatternName = "CogJobPattern";


        #endregion








    }


}
