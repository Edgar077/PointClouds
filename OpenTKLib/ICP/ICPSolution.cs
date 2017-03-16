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
using OpenTK;
using OpenTKExtension;

namespace ICPLib
{
    public class ICPSolution
    {
        public List<int> RandomIndices;
        public Matrix4 Matrix = Matrix4.Identity;
        public float MeanDistanceSubset;
        public float MeanDistance;

        public PointCloud PointsTarget;
        public PointCloud PointsSource;
        public PointCloud PointsTransformed;
        public ICPSolution()
        {
            RandomIndices = new List<int>();
        }
        public override string ToString()
        {
            string str = this.MeanDistance.ToString("0.0") + "(" + this.MeanDistanceSubset.ToString("0.0") + ")" + " : Matrix row 1: " + this.Matrix[0, 0].ToString("0.0") + ":" + this.Matrix[0, 1].ToString("0.0") + ":" + 
                this.Matrix[0, 2].ToString("0.0") + ":" + this.Matrix[0, 3].ToString("0.0") + ":";

            return str;
        }
        private static bool ListEqual(List<int> a , List<int> b)
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i])
                    return false;
                
            }
            return true;

        }
        public static bool IndicesAreNew(List<int> newIndices, List<ICPSolution> solutionsList)
        {
            if (solutionsList.Count == 0)
                return true;

            try
            {

                for (int i = 0; i < solutionsList.Count; i++)
                {
                    List<int> indicesOfList = solutionsList[i].RandomIndices;
                    //System.Diagnostics.Debug.WriteLine( (indicesOfList[0] - newIndices[0]).ToString() + "; " + (indicesOfList[1] - newIndices[1]).ToString() );

                    if (ListEqual(newIndices, indicesOfList))
                        return false;

                    //if (newIndices.Equals(indicesOfList))
                    //    return false;


                    //bool isNew = true;
                    ////because indices are sorted, if one of them is not equal, return
                    //for (int j = 0; j < newIndices.Count; j++)
                    //{
                    //    if (indicesOfList[j] != newIndices[j])
                    //    {
                    //        isNew = true;
                    //    }
                    //}
                    ////if (equal)
                    ////    return false;
                }
                return true;
            }
            catch(Exception err)
            {
                MessageBox.Show("Error in IndicesAlreadyTried " + err.Message);
                return false;

            }

        }
        public static List<int> GetRandomIndices(int maxNumber, int myNumberPoints)
        {   
            List<int> randomIndices = RandomUtils.UniqueRandomIndices(myNumberPoints, maxNumber);
            randomIndices.Sort();
                    
            return randomIndices;
        }
        public static ICPSolution SetRandomIndices(int myNumberPoints, int maxNumber, List<ICPSolution> solutionList)
        {
            int i;
            List<int> randomIndices;
            try
            {
                //set trial points 
                for (i = 0; i < 1000; i++)
                {
                    try
                    {
                        
                        randomIndices = ICPSolution.GetRandomIndices(maxNumber, myNumberPoints);
                        if (ICPSolution.IndicesAreNew(randomIndices, solutionList))
                        {

                            ICPSolution res = new ICPSolution();
                            res.RandomIndices = randomIndices;
                            return res;


                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Error in SetRandomIndices " + err.Message);
                        return null;

                    }
                }
               // MessageBox.Show("SetRandomIndices: No indices could be found!!");
                // 1000 trials
                return null;
            }
            catch(Exception err)
            {
                MessageBox.Show("Error in SetRandomIndices " + err.Message);
                return null;

            }

        }
    }
    public class ICPSolutionComparer : IComparer<ICPSolution>
    {

        public int Compare(ICPSolution a, ICPSolution b)
        {
            if (a == null || b == null)
            {
                return 0;
            }
            if (float.IsNaN(a.MeanDistance))
                return 1;
            if (float.IsNaN(b.MeanDistance))
                return -1;

            if (a.MeanDistance < b.MeanDistance)
                return -1;
            else
                return 1;


        }
    }
   
    
}
