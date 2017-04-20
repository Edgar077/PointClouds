using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKExtension;
using System.Drawing;
using OpenTK;

namespace PointCloudUtils
{
    public class ScannerBase : IScanner
    {
        protected OpenGLPart openGLPart;
        protected int openGLRefreshAt;
        public DepthMetaData DepthMetaData { get; set; }
        public IRMetaData IRMetadData { get; set; }

        public static string PathModels = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + GLSettings.PathPointClouds;

        protected int depthQuality;


        protected List<OpenTK.Vector3> listPointsInterpolated;
        protected int numberOfMaxFramesInterpolated = 10;
        protected int iFrameInterpolation = 0;
        protected int numberOfCutPoints = 0;
        protected List<ushort[]> interpolationList;

        //...................................
        protected bool bStopAfterFrameInterpolation = false;

        protected ushort[] pixelsEntropyArray;
        protected List<Vector2d> pointsEntropyChart = null;
        //is an array of 0..numberOfFramesInterplated containing the number of removed pixel
        protected int[] numberOfPixelsRemovedArray;
        protected int[] numberOfCutPointsArray;
        protected System.Windows.Forms.PictureBox pictureBoxEntropy;
        protected System.Windows.Forms.PictureBox pictureBoxPolygon;



        protected Bitmap bitmapColor;//= new System.Drawing.Bitmap(ColorMetaData.XResDefault, ColorMetaData.YResDefault, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        protected Bitmap bitmapDepth;//= new System.Drawing.Bitmap(DepthMetaData.XResDefault, DepthMetaData.YResDefault, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
        protected Bitmap bitmapIR;//= new System.Drawing.Bitmap(DepthMetaData.XResDefault, DepthMetaData.YResDefault, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

        protected Bitmap bitmapEntropy;//= new System.Drawing.Bitmap(DepthMetaData.XResDefault, DepthMetaData.YResDefault, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        protected Bitmap bitmapPolygon;//= new System.Drawing.Bitmap(DepthMetaData.XResDefault, DepthMetaData.YResDefault, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

        protected bool isScanning;



        public OpenGLPart OpenGLPart
        {
            get
            {
                return openGLPart;
            }

        }

        public int OpenGLRefreshAt
        {
            get
            {
                return openGLRefreshAt;
            }
            set
            { openGLRefreshAt = value; }
        }

        public virtual bool SaveAll()
        {
            throw new NotImplementedException();
        }

        public virtual void ShowPointCloud()
        {
            throw new NotImplementedException();
        }

        public virtual bool StartScanner()
        {
            throw new NotImplementedException();
        }
        public virtual bool SavePointCloud(string fileName)
        {
            throw new NotImplementedException();
        }
        public virtual bool SavePointCloud(string path, string fileName)
        {
            throw new NotImplementedException();
        }

        public virtual void StopScanner()
        {
            throw new NotImplementedException();
        }

        public virtual PointCloudRenderable ToPointCloudRenderable(bool resizeTo1)
        {
            throw new NotImplementedException();
        }
        public virtual bool SavePointCloudDefault()
        {
            throw new NotImplementedException();
        }
        public bool IsScanning
        {
            get
            {
                return isScanning;
            }

        }
        protected void HelperInterpolation_Iteration1(int xMax, int yMax)
        {
            if (iFrameInterpolation == 1)
            {
                interpolationList = new List<ushort[]>(numberOfMaxFramesInterpolated);
                pixelsEntropyArray = new ushort[xMax * yMax];

                numberOfPixelsRemovedArray = new int[this.numberOfMaxFramesInterpolated];
                numberOfCutPointsArray = new int[this.numberOfMaxFramesInterpolated];

            }

        }
        protected void HelperInterpolationCalculateEnd(int xMax, int yMax)
        {

            int nCount = this.DepthMetaData.FrameData.GetLength(0);
            for (int i = 0; i < nCount; i++)
            {
                int numberOfDepthItems = 0;

                int minVal = 20000;
                int maxVal = 0;
                long depthSum = Helper_CalculateDepthSum(i, ref numberOfDepthItems, ref minVal, ref maxVal);


                if (numberOfDepthItems > 0)
                {
                    double d = depthSum / numberOfDepthItems;
                    this.DepthMetaData.FrameData[i] = Convert.ToUInt16(d);
                }
                else
                {
                    this.DepthMetaData.FrameData[i] = 0;
                }

                HelperEntropy(i, minVal, maxVal, numberOfDepthItems, depthSum);



            }

            HelperInterpolation_End(xMax, yMax);


        }
        private long Helper_CalculateDepthSum(int iPixel, ref int numberOfDepthItems, ref int minVal, ref int maxVal)
        {
            long sum = 0;

            for (int jInterpolated = 0; jInterpolated < interpolationList.Count; jInterpolated++)
            {
                ushort[] currentPixelList = interpolationList[jInterpolated];
                ushort zVal = currentPixelList[iPixel];

                if (zVal > DepthMetaData.DepthMinDefault && zVal < DepthMetaData.DepthMaxDefault)
                {
                    numberOfDepthItems++;
                    sum += zVal;

                    minVal = Math.Min(zVal, minVal);
                    maxVal = Math.Max(zVal, minVal);
                }


            }
            return sum;

        }
        private void HelperEntropy(int i, int minVal, int maxVal, int numberOfDepthItems, long depthSum)
        {


            //now calculate image entropy
            if (maxVal < minVal)
            {
                pixelsEntropyArray[i] = 0;
            }
            else
            {
                //adjust pixel list for Entropy Histogram

                int iEntropy = maxVal - minVal;
                pixelsEntropyArray[i] = Convert.ToUInt16(iEntropy);
                //consider only depth points if we have at least 2 measurements
                if (iEntropy == 0 && depthSum > 0 && numberOfDepthItems > 1)
                {   //shift this entropy to field 1 , because entropy 0 means: depth information is missing
                    pixelsEntropyArray[i] = 1;
                    //System.Diagnostics.Debug.WriteLine(" Check");
                }

            }
        }
        private void HelperInterpolation_End(int xMax, int yMax)
        {
            iFrameInterpolation = 0;



            listPointsInterpolated = DepthMetaData.CreateListPoints_Depth(this.DepthMetaData.FrameData, xMax, yMax);


        }

        protected void ShowpictureBoxEntropy()
        {

            this.pictureBoxEntropy.Image = DepthMetaData.ToEntropyImage_Bitmap(this.bitmapEntropy, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect, pixelsEntropyArray);

            this.pointsEntropyChart = Helper_CreateAndShowEntropyImageStatistics(pixelsEntropyArray);
            this.pictureBoxPolygon.Image = ImageUtils.CreatePolygon(PointsEntropyChart);

        }


        public List<Vector2d> PointsEntropyChart
        {
            get
            {

                if (pointsEntropyChart == null)
                    this.pointsEntropyChart = Helper_CreateAndShowEntropyImageStatistics(pixelsEntropyArray);
                return this.pointsEntropyChart;
            }

        }


        protected List<Vector2d> Helper_CreateHistogramPolygon(int[] newValues)
        {
            List<Vector2d> points = new List<Vector2d>();
            int maxValue = newValues.Max();

            points.Add(new Vector2d(0, 0));
            for (int i = 1; i < newValues.Length; i++)
            {
                int val = newValues[i];
                points.Add(new Vector2d(i, maxValue - val));

            }
            //add values with depth 0 (indefinite) at the end of the polygon
            points.Add(new Vector2d(newValues.Length, maxValue - newValues[0]));

            //add a last point to finish polygon
            points.Add(new Vector2d(newValues.Length, 0));

            return points;
        }
        protected List<Vector2d> Helper_CreateAndShowEntropyImageStatistics(ushort[] values)
        {

            if (values == null)
                return null;

            //consider only entropy of maximum 32
            int[] newValues = Helper_CreateDepthPolygonArray(values);

            int totalValues = Helper_SubstractCutAndRemovedPixels(newValues, values.GetLength(0));


            if (PointCloudScannerSettings.SaveAndStop)
            {
                Helper_CalculateSums(newValues, totalValues);
                
            }

            if (newValues == null)
                return null;


            List<Vector2d> points = Helper_CreateHistogramPolygon(newValues);

            return points;
        }
        protected int[] Helper_CreateDepthPolygonArray(ushort[] values)
        {
            int max = 32;
            int[] polygonArray = new int[max];

            //copy all values in a small array of 0 .. max (all depths above max are in the last element)
            for (int i = 0; i < values.Length; i++)
            {
                int val = values[i];
                if (val >= max)
                {
                    //all values above max are in the last element
                    val = (ushort)(max - 1);
                }
                polygonArray[val]++;

            }
            return polygonArray;
        }
        protected int[] Helper_CreateStatisticSums(int[] ploygonArray)
        {
            int max = ploygonArray.GetLength(0);

            int[] statisticValuesMax = new int[5];
            int[] statisticValuesSum = new int[5];

            for (int i = 0; i < statisticValuesSum.GetLongLength(0); i++)
            {
                statisticValuesSum[i] = 0;
            }
            statisticValuesMax[0] = 0;
            statisticValuesMax[1] = 1;
            statisticValuesMax[2] = 2;
            statisticValuesMax[3] = 6;
            statisticValuesMax[4] = max - 1;//for ALL depths ABOVE (max -1)



            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < (statisticValuesSum.GetLongLength(0) - 1); j++)
                {
                    if (i >= statisticValuesMax[j] && i < statisticValuesMax[j + 1])
                    {
                        statisticValuesSum[j] += ploygonArray[i];
                    }
                }

            }
            return statisticValuesSum;

        }
        /// <summary>
        /// given the polygon Array which contains some kind of histogram (0...32) of values
        /// -put the values in 5 different categories (depth <=1, etc
        /// and display them in the labels
        /// </summary>
        /// <param name="ploygonArray"></param>
        /// <param name="totalNumberOfValues"></param>
        protected void Helper_CalculateSums(int[] polygonArray, int totalNumberOfValues)
        {


            int[] statisticValuesSum = Helper_CreateStatisticSums(polygonArray);
            Helper_CheckSum(statisticValuesSum, totalNumberOfValues);

            depthQuality = statisticValuesSum[1];
            

        }
      
        private void Helper_CheckSum(int[] statisticValuesSum, int totalNumberOfValues)
        {
            double totalSum = 0F;
            for (int i = 0; i < statisticValuesSum.GetLongLength(0); i++)
            {
                totalSum += statisticValuesSum[i];
                double d = Convert.ToSingle(statisticValuesSum[i]) * 100 / totalNumberOfValues;
                try
                {
                    statisticValuesSum[i] = Convert.ToInt32(d);
                }
                catch (Exception err)
                {
                    System.Diagnostics.Debug.WriteLine("Error " + err.Message);
                    statisticValuesSum[i] = 0;
                }
            }

            if (statisticValuesSum[0] < 0)
                statisticValuesSum[0] = 0;

            totalSum = totalSum * 100 / totalNumberOfValues;
            try
            {
                int iTotal = Convert.ToInt32(totalSum);
                //if (iTotal < 98 || iTotal > 100)
                //{
                //    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToLongTimeString() + " : SUM (should be 100) : " + iTotal.ToString());
                //}
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error " + err.Message);
            }


        }
        private int Helper_SubstractCutAndRemovedPixels(int[] polygonArray, int totalNumberOfValues)
        {


            int cutPixels = Helper_ComputeCutPixels();
            totalNumberOfValues -= cutPixels;
            polygonArray[0] -= cutPixels;

            if (PointCloudScannerSettings.BackgroundRemoved)
            {

                int removedPixels = Helper_ComputeRemovedPixels(); // is only an average of removed pixels, therefore the error...
                polygonArray[0] -= removedPixels;
                totalNumberOfValues -= removedPixels;


            }

            return totalNumberOfValues;
        }
        private int Helper_ComputeCutPixels()
        {
            long sumCut = 0;
            if (PointCloudScannerSettings.CutFrames)
            {

                //add the number of removed pixels
                for (int i = 0; i < numberOfCutPointsArray.Length; i++)
                {
                    sumCut += numberOfCutPointsArray[i];

                }
                double f = Convert.ToSingle(sumCut) / numberOfCutPointsArray.Length;
                sumCut = Convert.ToInt32(f);
            }
            return Convert.ToInt32(sumCut);
        }
        protected void SavePointCloud_XYZ()
        {
            if (listPointsInterpolated == null)
                return;
            //-----------------------------------------------
            //now interpolate last 10 frames to one frame and save 

            PointCloudScannerSettings.InterpolateFrames = true;
            GLSettings.FileNamePointCloudLast1 = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "_PointCloudInterpolated.xyz";
            UtilsPointCloudIO.ToXYZFile(listPointsInterpolated, GLSettings.FileNamePointCloudLast1, PathModels);

        }
        protected void SaveDepth_BW_Interpolated()
        {
            if (listPointsInterpolated == null)
                return;
            //-----------------------------------------------
            //now interpolate last 10 frames to one frame and save 

            System.Drawing.Image depthInterpolated = DepthMetaData.ToImage(listPointsInterpolated);
            depthInterpolated.SaveImage(PathModels, "DepthInterpolated", true);

        }

        private int Helper_ComputeRemovedPixels()
        {
            long sumRemoved = 0;
            //add the number of removed pixels
            for (int i = 0; i < numberOfPixelsRemovedArray.Length; i++)
            {
                sumRemoved += numberOfPixelsRemovedArray[i];

            }
            double f = Convert.ToSingle(sumRemoved) / numberOfPixelsRemovedArray.Length;
            sumRemoved = Convert.ToInt32(f);
            return Convert.ToInt32(sumRemoved);
        }

    }
}
