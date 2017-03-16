using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Collections.Specialized;
using System.Collections.Generic;
using PointCloudUtils;
using OpenTK;

namespace ScannerWPF
{
    public partial class PointCloudUC
    {
        ushort[] pixelsEntropyChart;

        private PointCollection pointsEntropyChart = null;
        //is an array of 0..numberOfFramesInterplated containing the number of removed pixel
        private int[] numberOfPixelsRemovedArray;
        private int[] numberOfCutPointsArray;
        
        List<Vector3> listPointsInterpolated;
             
        private int numberOfFramesInterpolated = 10;

        private void CalculateInterpolatedPixels()
        {
            iFrameInterpolation++;

            HelperInterpolation_Iteration1();


            if (PointCloudScannerSettings.BackgroundRemoved)
            {
                if (BodyMetaData != null)
                {
                    int numberOfPixRemoved = backgroundRemovalTool.DepthFrameData_RemoveBackground(this.DepthMetaData, this.BodyMetaData);
                    if (numberOfPixRemoved == -1) // the method returns an error ...
                        return;
                    numberOfPixelsRemovedArray[iFrameInterpolation - 1] = numberOfPixRemoved;
                }

            }
            numberOfCutPointsArray[iFrameInterpolation - 1] = numberOfCutPoints;

            pixelsList.Add(this.DepthMetaData.FrameData);
            HelperInterpolationCalculateEnd();



        }
        private void ShowImageEntropy()
        {

            this.imageEntropy.Source = DepthMetaData.ToEntropyImage_BitmapSource(DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect, pixelsEntropyChart);
            this.pointsEntropyChart = Helper_CreateAndShowEntropyImageStatistics(pixelsEntropyChart);
            PolygonEntropy.Points = PointsEntropyChart;

        }

        
        public PointCollection PointsEntropyChart
        {
            get
            {
              
                if(pointsEntropyChart == null)
                    this.pointsEntropyChart = Helper_CreateAndShowEntropyImageStatistics(pixelsEntropyChart);
                return this.pointsEntropyChart;
            }
         
        }
        private void HelperInterpolation_Iteration1()
        {
            if (iFrameInterpolation == 1)
            {
                pixelsList = new List<ushort[]>(numberOfFramesInterpolated);
                pixelsEntropyChart = new ushort[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect];

                numberOfPixelsRemovedArray = new int[this.numberOfFramesInterpolated];
                numberOfCutPointsArray = new int[this.numberOfFramesInterpolated];

            }
            
        }
        private void HelperInterpolation_End()
        {
            iFrameInterpolation = 0;

            if (bStopAfterFrameInterpolation)
                ScannerClose();


            //ushort[] rotatedPoints = DepthMetaData.RotateDepthFrame(this.DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);
            listPointsInterpolated = DepthMetaData.CreateListPoints_Depth(this.DepthMetaData.FrameData, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);


            if (PointCloudScannerSettings.EntropyImage)
                ShowImageEntropy();
                
        }
        private long Helper_CalculateDepthSum(int iPixel, ref int numberOfDepthItems, ref int minVal, ref int maxVal)
        {
            long sum = 0;

            for (int jInterpolated = 0; jInterpolated < pixelsList.Count; jInterpolated++)
            {
                ushort[] currentPixelList = pixelsList[jInterpolated];
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
        private void HelperInterpolationCalculateEnd()
        {
            if (iFrameInterpolation == this.numberOfFramesInterpolated)
            {
                int nCount = this.DepthMetaData.FrameData.GetLength(0);
                for (int i = 0; i < nCount; i++)
                {
                    int numberOfDepthItems = 0;
                    
                    int minVal = 20000;
                    int maxVal = 0;
                    long sum = Helper_CalculateDepthSum(i, ref numberOfDepthItems, ref minVal, ref maxVal);
                    
                    
                    if (numberOfDepthItems > 0)
                    {
                        double d = sum / numberOfDepthItems;
                        this.DepthMetaData.FrameData[i] = Convert.ToUInt16(d);
                    }
                    else
                    {
                        this.DepthMetaData.FrameData[i] = 0;
                    }
                    if (maxVal < minVal)
                    {
                        pixelsEntropyChart[i] = 0;
                    }
                    else
                    {
                        //adjust pixel list for Entropy Histogram

                        int iEntropy = maxVal - minVal;
                        pixelsEntropyChart[i] = Convert.ToUInt16(iEntropy);
                        //consider only depth points if we have at least 2 measurements
                        if (iEntropy == 0 && sum > 0 && numberOfDepthItems > 1)
                        {   //shift this entropy to field 1 , because entropy 0 means: depth information is missing
                            pixelsEntropyChart[i] = 1;
                            //System.Diagnostics.Debug.WriteLine(" Check");
                        }
                        
                    }

                }

                HelperInterpolation_End();

            }
        }

        private PointCollection Helper_CreateHistogramPolygon(int[] newValues)
        {
            PointCollection points = new PointCollection();
            int maxValue = newValues.Max();

            points.Add(new Point(0, 0));
            for (int i = 1; i < newValues.Length; i++)
            {
                int val = newValues[i];
                points.Add(new Point(i, maxValue - val));

            }
            //add values with depth 0 (indefinite) at the end of the polygon
            points.Add(new Point(newValues.Length, maxValue - newValues[0]));

            //add a last point to finish polygon
            points.Add(new Point(newValues.Length , 0));
            
            return points;
        }
        private PointCollection Helper_CreateAndShowEntropyImageStatistics(ushort[] values)
        {
            
            if (values == null)
                return null;

            //consider only entropy of maximum 32
            int[] newValues = Helper_CreateDepthPolygonArray(values);

            int totalValues = Helper_SubstractCutAndRemovedPixels(newValues, values.GetLength(0));

            Helper_ShowValuesOnControl(newValues, totalValues);
            
            if (newValues == null)
                return null;


            PointCollection points = Helper_CreateHistogramPolygon(newValues);
            
            return points;
        }
        private int[] Helper_CreateDepthPolygonArray(ushort[] values)
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
        private int[] Helper_CreateStatisticSums(int[] polygonArray)
        {
            int max = polygonArray.GetLength(0);

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
                        statisticValuesSum[j] += polygonArray[i];
                    }
                }

            }
            return statisticValuesSum;

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
                catch(Exception err)
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
            catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error " + err.Message);
            }


        }
        /// <summary>
        /// given the polygon Array which contains some kind of histogram (0...32) of values
        /// -put the values in 5 different categories (depth <=1, etc
        /// and display them in the labels
        /// </summary>
        /// <param name="ploygonArray"></param>
        /// <param name="totalNumberOfValues"></param>
        private void Helper_ShowValuesOnControl(int[] polygonArray, int totalNumberOfValues)
        {

            int[] statisticValuesSum = Helper_CreateStatisticSums(polygonArray);

            Helper_CheckSum(statisticValuesSum, totalNumberOfValues);
            Helper_SetLabelContent(statisticValuesSum);

            if (statisticValuesSum[1] > PointCloudScannerSettings.SaveImageIfQualityIsBetterThan && checkBoxSaveAndStop.IsChecked == true)
            {
                SaveDepthPointsInterpolated();
                if (PointCloudScannerSettings.ScannerMode == PointCloudUtils.ScannerMode.Color_Depth || PointCloudScannerSettings.ScannerMode == PointCloudUtils.ScannerMode.Color_Depth_3DDisplay)
                    SaveDepthAndColor_DataAndImage();
                checkBoxSaveAndStop.IsChecked = false;
                ScannerClose();
                
            }
            
        }
        private void Helper_SetLabelContent(int[] statisticValuesSum)
        {
            labelDepth1.Content = statisticValuesSum[1].ToString() + " %";
            labelDepth2.Content = statisticValuesSum[2].ToString() + " %";
            labelDepth3.Content = statisticValuesSum[3].ToString() + " %";
            labelDepth4.Content = statisticValuesSum[4].ToString() + " %";
            labelDepth0.Content = statisticValuesSum[0].ToString() + " %";
            
            Helper_SetLabelBackground(statisticValuesSum[1]);
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
        private void Helper_SetLabelBackground(int intVal)
        {

            if (intVal < 20)
            {
                labelDepth1.Background = Brushes.Red;
                BorderPolygon.Background = Brushes.Red;
            }
            else if (intVal < 30)
            {
                labelDepth1.Background = Brushes.Yellow;
                BorderPolygon.Background = Brushes.Yellow;
            }
            else if (intVal < 40)
            {
                labelDepth1.Background = Brushes.GreenYellow;
                BorderPolygon.Background = Brushes.GreenYellow;
            }
            else if (intVal < 50)
            {
                labelDepth1.Background = Brushes.LightGreen;
                BorderPolygon.Background = Brushes.LightGreen;
            }
            else
            {
                labelDepth1.Background = Brushes.Green;
                BorderPolygon.Background = Brushes.Green;
            }

        }
        //private ushort[] SmoothEntropyValues(ushort[] originalValues)
        //{
        //    ushort[] smoothedValues = new ushort[originalValues.Length];

        //    double[] mask = new double[] { 0.25, 0.5, 0.25 };

        //    for (int bin = 1; bin < originalValues.Length - 1; bin++)
        //    {
        //        double smoothedValue = 0;
        //        for (int i = 0; i < mask.Length; i++)
        //        {
        //            smoothedValue += originalValues[bin - 1 + i] * mask[i];
        //        }
        //        smoothedValues[bin] = (ushort)smoothedValue;
        //    }

        //    return smoothedValues;
        //}

    }
}
