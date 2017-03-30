using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Windows.Media;
using System.Diagnostics;
using OpenTK;
using System.Linq;

namespace OpenTKExtension
{


    public class Outliers
    {
        public static void StandardDeviation(PointCloud source, int numberOfNeighbours, out float meanDistance, out float standardDeviation, out float[] distances)
        {
            meanDistance = 0;
            standardDeviation = 0f;
            distances = new float[source.Count];

            KDTreeKennell kdTree = new KDTreeKennell();
            kdTree.Build(source);


            PointCloud pcResult = new PointCloud();
           
            
            VertexKDTree[] resultArray = new VertexKDTree[source.Count];
            VertexKDTree[] outliers = new VertexKDTree[source.Count];

            try
            {

                List<Vector3> listV = new List<Vector3>();
                List<Vector3> listC = new List<Vector3>();
                


                //1. mean distance of one point to his next "numberOfNeighbours" neighbours - stored in the "distances" array
                for (int i = 0; i < source.Count; i++)
                {
                    VertexKDTree vSource = new VertexKDTree(source.Vectors[i], source.Colors[i], i);

                    ListKDTreeResultVectors listResult = kdTree.Find_N_Nearest(vSource.Vector, numberOfNeighbours);

                    float distSum = 0f;
                    for (int k = 1; k < listResult.Count; ++k)  // k = 0 is the query point
                        distSum += listResult[k].Distance;

                    distances[i] = (distSum / (listResult.Count - 1));

                }
                //2. calculate the mean distance of ALL points 

                

                for (int i = 0; i < distances.Length; ++i)
                {
                    meanDistance += distances[i];
                }
                meanDistance /= distances.Length;

                //3. calculate the deviation of each data point from the mean, and square the result of each
               
                for (int i = 0; i < distances.Length; i++)
                {
                    float dev = distances[i] - meanDistance;
                    dev *= dev;
                    standardDeviation += dev;
                }
                standardDeviation /= distances.Length;
                standardDeviation = Convert.ToSingle(Math.Sqrt(standardDeviation));
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in KDTreeKennnellRemoveDuplicates: " + err.Message);
            }


        }


        /// <summary>
        /// tree.RemoveOutliersByDeviation(PointCloudDirty, 10, 1.3f);
        /// </summary>
        /// <param name="source"></param>
        /// <param name="numberOfNeighbours"></param>
        /// <param name="stdMultiplier"></param>
        /// <returns></returns>
        public static PointCloud ByStandardDeviation(PointCloud source, int numberOfNeighbours, float stdDeviationMultiplier, out PointCloud pcOutliersMarkedRed)
        {


            PointCloud pcResult = new PointCloud();

            //the outliers are marked red
            pcOutliersMarkedRed = source.Clone();

            float meanDistance, standardDeviation;
            float[] distances;
            //1. get standard deviation, meanDistance and array of distances
            StandardDeviation(source, numberOfNeighbours, out meanDistance, out standardDeviation, out distances);


            int numberRemoved = 0;
            VertexKDTree[] resultArray = new VertexKDTree[source.Count];
            VertexKDTree[] outliers = new VertexKDTree[source.Count];

            try
            {
                

                List<Vector3> listV = new List<Vector3>();
                List<Vector3> listC = new List<Vector3>();
               
                //2. distance threshold: deviation plus the overall mean distance
                float distanceThreshold = meanDistance + standardDeviation;


                //3. remove all points according to the distance threshold
                for (int i = 0; i < source.Count; i++)
                {
                    VertexKDTree vSource = new VertexKDTree(source.Vectors[i], source.Colors[i], i);

                    if (distances[i] > distanceThreshold)
                    {
                        pcOutliersMarkedRed.Colors[i] = new Vector3(1,0,0);
                        numberRemoved++;
                        continue;
                    }
                    else
                    {
                        resultArray[i] = vSource;
                    }
                }

                List<Vector3> listOutliers = new List<Vector3>();
                List<Vector3> listOutliersColors = new List<Vector3>();
                //build resulting cloud and outliers
                for (int i = 0; i < source.Count; i++)
                {
                    if (resultArray[i] != null)
                    {
                        listV.Add(resultArray[i].Vector);
                        listC.Add(resultArray[i].Color);
                    }
                  
                }

                pcResult.Vectors = listV.ToArray();
                pcResult.Colors = listC.ToArray();
                pcResult.SetDefaultIndices();

                System.Diagnostics.Debug.WriteLine("Outliers: Mean distance---" + meanDistance.ToString("G") + " ---- standard deviation ---" + standardDeviation.ToString("G") + "---Number of outliers: " + numberRemoved.ToString());

            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in KDTreeKennnellRemoveDuplicates: " + err.Message);
            }

            return pcResult;
        }


        /// <summary>
        /// Algorithm based on ignoring points with less neighbours    int thresholdNeighboursCount = 10; float thresholdDistance = 15e-5f;
        /// at given distance.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static PointCloud ByLessNeighbours(PointCloud source, float thresholdDistance, int thresholdNeighboursCount)
        {
            PointCloud pcResult = new PointCloud();

            KDTreeKennell kdTree = new KDTreeKennell();
            kdTree.Build(source);


            VertexKDTree[] resultArray = new VertexKDTree[source.Count];

            try
            {
                List<Vector3> listV = new List<Vector3>();
                List<Vector3> listC = new List<Vector3>();
                System.Threading.Tasks.Parallel.For(0, source.Count, i =>
                {
                    VertexKDTree vSource = new VertexKDTree(source.Vectors[i], source.Colors[i], i);
                    int neighboursCount = 0;

                    kdTree.FindClosestPoints_Radius(vSource, thresholdDistance, ref neighboursCount);

                    if (neighboursCount >= thresholdNeighboursCount)
                    {
                        resultArray[i] = vSource;
                    }
                });

                for (int i = 0; i < source.Count; i++)
                {
                    if (resultArray[i] != null)
                    {
                        listV.Add(resultArray[i].Vector);
                        listC.Add(resultArray[i].Color);
                    }
                }

                pcResult.Vectors = listV.ToArray();
                pcResult.Colors = listC.ToArray();
                pcResult.SetDefaultIndices();
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in KDTreeKennnellRemoveDuplicates: " + err.Message);
            }

            return pcResult;
        }

    }


}