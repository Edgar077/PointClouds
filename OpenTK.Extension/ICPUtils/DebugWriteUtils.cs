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
using OpenTKExtension;

namespace ICPLib
{
    public class DebugWriteUtils
    {
        
        public static void WriteTestOutput(string nameDisplayed, Matrix4 m, List<Vector3> mypointsSource, List<Vector3> myPointsTransformed, List<Vector3> myPointsTarget)
        {
            m.Print(nameDisplayed);

            long resultsWritten = mypointsSource.Count;
            if (resultsWritten > 5)
                resultsWritten = 5;
            System.Diagnostics.Debug.WriteLine("Points:");
            double meanDistance = 0;
            for (int i = 0; i < resultsWritten; i++)
            {
                Vector3 pToBeMatched = mypointsSource[i];
                Vector3 pTransformed = myPointsTransformed[i];
                Vector3 pReference = myPointsTarget[i];

                string p1 = pToBeMatched[0].ToString("0.0") + " " + pToBeMatched[1].ToString("0.0") + " " + pToBeMatched[2].ToString("0.0");
                string p2 = pTransformed[0].ToString("0.0") + " " + pTransformed[1].ToString("0.0") + " " + pTransformed[2].ToString("0.0");
                string p3 = pReference[0].ToString("0.0") + " " + pReference[1].ToString("0.0") + " " + pReference[2].ToString("0.0");

                double distance = MathBase.DistanceBetweenVectors(pTransformed, pReference);
                meanDistance += distance;
                Debug.WriteLine(i.ToString() + " : " + p1 + " :transformed: " + p2 + " :target: " + p3 + " : Distance: " + distance.ToString("0.0"));

            }
         //   Debug.WriteLine("--Mean Distance: " + (meanDistance / resultsWritten).ToString("0.0"));
        }
        public static void WriteTestOutputVertex(string nameDisplayed, Matrix4 m, PointCloud mypointsSource, PointCloud myPointsTransformed, PointCloud myPointsTarget)
        {
            m.Print(nameDisplayed);

            long resultsWritten = mypointsSource.Count;
            if (resultsWritten > 5)
                resultsWritten = 5;
            System.Diagnostics.Debug.WriteLine("Points:");
            double meanDistance = 0;
            for (int i = 0; i < resultsWritten; i++)
            {
                Vector3 pToBeMatched = mypointsSource[i].Vector;
                Vector3 pTransformed = myPointsTransformed[i].Vector;
                Vector3 pReference = myPointsTarget[i].Vector;

                string p1 = pToBeMatched[0].ToString("0.0") + " " + pToBeMatched[1].ToString("0.0") + " " + pToBeMatched[2].ToString("0.0");
                string p2 = pTransformed[0].ToString("0.0") + " " + pTransformed[1].ToString("0.0") + " " + pTransformed[2].ToString("0.0");
                string p3 = pReference[0].ToString("0.0") + " " + pReference[1].ToString("0.0") + " " + pReference[2].ToString("0.0");

                double distance = MathBase.DistanceBetweenVectors(pTransformed, pReference);
                meanDistance += distance;
                Debug.WriteLine(i.ToString() + " : " + p1 + " :transformed: " + p2 + " :target: " + p3 + " : Distance: " + distance.ToString("0.0"));

            }
            //Debug.WriteLine("--Mean Distance: " + (meanDistance / resultsWritten).ToString("0.0"));
        }
        public static void WriteTestOutputVector3(string nameDisplayed, Matrix4 m, PointCloud mypointsSource, PointCloud myPointsTransformed, PointCloud myPointsTarget)
        {
            m.Print(nameDisplayed);

            long resultsWritten = mypointsSource.Vectors.Length;
            if (resultsWritten > 5)
                resultsWritten = 5;
            System.Diagnostics.Debug.WriteLine("Points:");
            double meanDistance = 0;
            for (int i = 0; i < resultsWritten; i++)
            {
                Vector3 pToBeMatched = mypointsSource.Vectors[i];
                Vector3 pTransformed = myPointsTransformed.Vectors[i];
                Vector3 pReference = myPointsTarget.Vectors[i];

                string p1 = pToBeMatched[0].ToString("0.0") + " " + pToBeMatched[1].ToString("0.0") + " " + pToBeMatched[2].ToString("0.0");
                string p2 = pTransformed[0].ToString("0.0") + " " + pTransformed[1].ToString("0.0") + " " + pTransformed[2].ToString("0.0");
                string p3 = pReference[0].ToString("0.0") + " " + pReference[1].ToString("0.0") + " " + pReference[2].ToString("0.0");

                double distance = MathBase.DistanceBetweenVectors(pTransformed, pReference);
                meanDistance += distance;
                Debug.WriteLine(i.ToString() + " : " + p1 + " :transformed: " + p2 + " :target: " + p3 + " : Distance: " + distance.ToString("0.0"));

            }
            
        }
    
    }
}
