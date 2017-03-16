using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using System.Diagnostics;

namespace UnitTestsOpenTK.KDTreeTest
{
    [TestFixture]
    [Category("UnitTest")]
    public class MyKDTree 
    {
        protected PointCloud target;
        protected PointCloud source;
        protected PointCloud resultVertices;

        private void CreateTreeEven()
        {

            target = new PointCloud();
            target.Vectors = new Vector3[8];

            target.Vectors[0] = new Vector3(1, 1, 0);
            target.Vectors[1] = new Vector3(1, 2, 0);
            target.Vectors[2] = new Vector3(1, 2, 1);
            target.Vectors[3] = new Vector3(1, 2, 2);
            target.Vectors[4] = new Vector3(2, 1, 2);
            target.Vectors[5] = new Vector3(2, 0, 1);
            target.Vectors[6] = new Vector3(2, 1, 1);
            target.Vectors[7] = new Vector3(2, 2, 2);
            

            Debug.WriteLine("Index should be 3: " + target.VectorsWithIndex[3].Index.ToString());

            KDTree kdTree = new KDTree(target); 


        }
        private void CreateTreeOdd()
        {

            target = new PointCloud();
            target.Vectors = new Vector3[9];

            target.Vectors[0] = new Vector3(1, 1, 0);
            target.Vectors[1] = new Vector3(1, 0, 0);
            target.Vectors[2] = new Vector3(3, 1, 0);
            target.Vectors[3] = new Vector3(3, 2, 0);
            target.Vectors[4] = new Vector3(2, 3, 0);
            target.Vectors[5] = new Vector3(2, 1, 0);
            target.Vectors[6] = new Vector3(2, 0, 0);
            target.Vectors[7] = new Vector3(0, 2, 0);
            target.Vectors[8] = new Vector3(0, 0, 0);

            //target.Vectors[0] = new Vector3(1, 1, 0);
            //target.Vectors[1] = new Vector3(1, 2, 0);
            //target.Vectors[2] = new Vector3(1, 2, 1);
            //target.Vectors[3] = new Vector3(1, 2, 2);
            //target.Vectors[4] = new Vector3(2, 1, 2);
            //target.Vectors[5] = new Vector3(2, 0, 1);
            //target.Vectors[6] = new Vector3(2, 1, 1);
            //target.Vectors[7] = new Vector3(2, 2, 2);
            //target.Vectors[8] = new Vector3(0, 0, 0);

            Debug.WriteLine("Index should be 3: " + target.VectorsWithIndex[3].Index.ToString());

            KDTree kdTree = new KDTree(target);


        }
        [Test]
        public void CreateTreeTest()
        {
            
            CreateTreeOdd();

            CreateTreeEven();

        }
       
     
    }
}
