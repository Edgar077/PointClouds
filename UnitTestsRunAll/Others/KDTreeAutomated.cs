//using System;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Text;
//using System.Drawing;
//using OpenTKExtension;
//using OpenTK;

//using UnitTestsOpenTK.KDTreeTest;

//namespace Automated
//{
//    [TestFixture]
//    [Category("UnitTest")]
//    public class KDTreeTestAutomated : KDTreeTestBase
//    {
     

//        [Test]
//        public void KDTreeTest_Stark_FindItself()
//        {
                     
//            CubeCornersTest_Reset();
           
//            //the expected result - find the same vectors
           
//            KDTree_Stark tree = KDTree_Stark.Build(target);
            
//            for (int i = 0; i < source.Count; i++)
//            {
               
//                int indexNearest = tree.FindNearest(source[i]);
//                resultIndices.Add(indexNearest);
//                resultVertices.Add(target[indexNearest]);
                
//            }
           
//            CheckResultCubeCorner();


//        }
//        [Test]
//        public void KDTree_Rednaxela_FindItself()
//        {
//            CubeCornersTest_Reset();

//            KDTreeVertex kv = new KDTreeVertex();

//            kv.BuildKDTree_Rednaxela(target);
//            kv.ResetVerticesSearchResult(target);
//            kv.NumberOfNeighboursToSearch = 1;
//            kv.FindNearest_NormalsCheck_Rednaxela(source, true, false, 0f);

//            for (int i = 0; i < source.Count; i++)
//            {
//                resultIndices.Add(source[i].KDTreeSearch[0].Key);
//            }

//            CheckResultCubeCorner();

//        }
//        //[Test]
//        //public void KDTreeTest_Stark_Translation()
//        //{
//        //    GlobalVariables.ResetTime();
           
//        //    PointCloud target = Vertices.CreateCube_Corners(10);
//        //    PointCloud source = Vertices.CopyVertices(target);

//        //    Vertices.TranslateVertices(source, 100, 100, 100);

//        //    PointCloud result = new PointCloud();
//        //    KDTree_Stark tree = KDTree_Stark.Build(target);

//        //    for (int i = 0; i < source.Count; i++)
//        //    {

//        //        int indexNearest = tree.FindNearest_ExcludeTakenPoints(source[i]);
//        //        result.Add(target[indexNearest]);

//        //    }
//        //    GlobalVariables.ShowLastTimeSpan("KDTree RednaxelaTest");


//        //}
       
//    }
//}
