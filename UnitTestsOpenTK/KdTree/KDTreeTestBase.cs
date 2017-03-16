using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;


namespace UnitTestsOpenTK.KDTreeTest
{
   
    public class KDTreeTestBase 
    {
        protected List<int> expectedResultIndices;
        protected List<int> resultIndices;

        protected PointCloud target;
        protected PointCloud source;
        protected PointCloud resultVertices;

        protected void CubeCornersTest_Reset()
        {
            GlobalVariables.ResetTime();
            resultVertices = new PointCloud();

            target = PointCloud.CreateCube_Corners_CenteredAt0(10);
            source = PointCloud.CloneAll(target);
            expectedResultIndices = new List<int>();
            resultIndices = new List<int>();

            for (int i = 0; i < 8; i++)
            {
                expectedResultIndices.Add(i);
            }
        }
        protected void CheckResultCubeCorner()
        {
            GlobalVariables.ShowLastTimeSpan("KDTree Test");

            for (int i = 0; i < expectedResultIndices.Count; i++)
            {
                Assert.AreEqual(expectedResultIndices[i], resultIndices[i]);

            }
        }

     
    }
}
