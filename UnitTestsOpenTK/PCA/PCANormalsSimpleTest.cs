//using System;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Text;
//using System.Drawing;
//using OpenTKExtension;
//using OpenTK;

//namespace UnitTestsOpenTK.PrincipalComponentAnalysis
//{
//    [TestFixture]
//    [Category("UnitTest")]
//    public class PCANormals_SimpleTest : TestBase
//    {
//        [Test]
//        public void Cube_56_StartAt0()
//        {
//            ResetTest();
//            
//            pointCloudSource = PointCloud.CreateCube_RegularGrid_StartAt0_Empty(1, 3);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 6, false, true);
//            this.ShowCubeLinesAndNormals(normals);
//            //ShowCubeLinesAndNormals(cubeSize, normals);


//        }
//        [Test]
//        public void Cube_64_StartAt0_Error()
//        {
//            ResetTest();
//            
//            pointCloudSource = PointCloud.CreateCube_RegularGrid_StartAt0_Filled(1, 3);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 7, false, true);
           
//            this.ShowCubeLinesAndNormals(normals);

//            //ShowCubeLinesAndNormals(cubeSize, normals);


//        }
//        [Test]
//        public void Cube_56()
//        {
//            ResetTest();
//            
//            checkMultipleNormals = true;
//            pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(3);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, true, true);
//            this.ShowCubeLinesAndNormals(normals);
//            //ShowCubeLinesAndNormals(cubeSize, normals);

//            checkMultipleNormals = false;

//        }
//        [Test]
//        public void Cube_64_Filled_Error()
//        {
//            ResetTest();
//            
//            checkMultipleNormals = true;
//            pointCloudSource = PointCloud.CreateCube_RegularGrid_Filled(3);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, true, true);
//            this.ShowCubeLinesAndNormals(normals);
//            //ShowCubeLinesAndNormals(cubeSize, normals);

//            checkMultipleNormals = false;

//        }
//        [Test]
//        public void Cube_26p_CM()
//        {
//            ResetTest();
//            
//            checkMultipleNormals = true;
//            pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(2);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, true, true);
//            this.ShowCubeLinesAndNormals(normals);
//            //ShowCubeLinesAndNormals(cubeSize, normals);

//            checkMultipleNormals = false;

//        }
//        [Test]
//        public void Cube_26p_CM_Only3Neighbours_Error()
//        {
//            ResetTest();
//            
//            checkMultipleNormals = true;
//            pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(2);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 3, true, true);
//            this.ShowCubeLinesAndNormals(normals);
//            checkMultipleNormals = false;

//        }
//        [Test]
//        public void PlaneRotate()
//        {
//            ResetTest();
//            

//            pointCloudSource.Add(new Vector3(0, 0, 0, 0));
//            pointCloudSource.Add(new Vector3(1, 0, 1, 0));
//            pointCloudSource.Add(new Vector3(2, 1, 0, 0));
//            pointCloudSource.Add(new Vector3(3, 1, 1, 0));
            
//            Matrix3 R = Matrix3.CreateRotationX(Convert.ToSingle(Math.PI / 20));


//            PointCloud.Rotate(pointCloudSource, R);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, false, true);
//            ShowCubeLinesAndNormals(normals);
//        }
//        [Test]
//        public void PlaneXY()
//        {
//            ResetTest();
//            

//            pointCloudSource.Add(new Vector3(0, 0, 0, 0));
//            pointCloudSource.Add(new Vector3(1, 0, 1, 0));
//            pointCloudSource.Add(new Vector3(2, 1, 0, 0));
//            pointCloudSource.Add(new Vector3(3, 1, 1, 0));


//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, false, true);
//            ShowCubeLinesAndNormals(normals);
//        }
//        [Test]
//        public void PlaneRotate_CM()
//        {
//            ResetTest();
//            

//            pointCloudSource.Add(new Vector3(0, 0, 0, 0));
//            pointCloudSource.Add(new Vector3(1, 0, 1, 0));
//            pointCloudSource.Add(new Vector3(2, 1, 0, 0));
//            pointCloudSource.Add(new Vector3(3, 1, 1, 0));

           
//            Matrix3 R = Matrix3.CreateRotationX(Convert.ToSingle(Math.PI / 20));
//            PointCloud.Rotate(pointCloudSource, R);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 3, true, true);
//            ShowCubeLinesAndNormals(normals);
//        }
   
//        [Test]
//        public void PlaneRotate_5p_CM_Alglib()
//        {
//            ResetTest();
//            

            
//            pointCloudSource.Add(new Vector3(0, 0, 0, -1));
//            pointCloudSource.Add(new Vector3(1, 0, 1, -1));
//            pointCloudSource.Add(new Vector3(2, 1, 0, -1));
//            pointCloudSource.Add(new Vector3(3, 1, 1, -1));

//            pointCloudSource.Add(new Vector3(4, 2, 2.5f, -1));

            
//            Matrix3 R = Matrix3.CreateRotationX(45);
//            PointCloud.Rotate(pointCloudSource, R);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, true, true);
//            ShowCubeLinesAndNormals(normals);
            
//        }
//        [Test]
//        public void PlaneRotate_5p()
//        {
//            ResetTest();
//            

           
//            pointCloudSource.Add(new Vector3(0, 0, 0, -1));
//            pointCloudSource.Add(new Vector3(1, 0, 1, -1));
//            pointCloudSource.Add(new Vector3(2, 1, 0, -1));
//            pointCloudSource.Add(new Vector3(3, 1, 1, -1));

//            pointCloudSource.Add(new Vector3(4, 2, 2.5f, -1));

//            Matrix3 R = Matrix3.CreateRotationX(45);
//            PointCloud.Rotate(pointCloudSource, R);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, false, true);
//            ShowCubeLinesAndNormals(normals);
//        }
//        [Test]
//        public void PlaneRotate_5p_CM()
//        {
//            ResetTest();
//            

            
//            pointCloudSource.Add(new Vector3(0, 0, 0, -1));
//            pointCloudSource.Add(new Vector3(1, 0, 1, -1));
//            pointCloudSource.Add(new Vector3(2, 1, 0, -1));
//            pointCloudSource.Add(new Vector3(3, 1, 1, -1));
            
//            pointCloudSource.Add(new Vector3(4, 2, 2.5f, -1));

//            Matrix3 R = Matrix3.CreateRotationX(45);
//            PointCloud.Rotate(pointCloudSource, R);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, true, true);
//            ShowCubeLinesAndNormals(normals);
//        }
//        protected void CreatePlane()
//        {
//            pointCloudSource.Add(new Vector3(0, 0, 0, -1));
//            pointCloudSource.Add(new Vector3(1, 0, 1, -1));
//            pointCloudSource.Add(new Vector3(2, 1, 0, -1));
//            pointCloudSource.Add(new Vector3(3, 1, 1, -1));

//            pointCloudSource.Add(new Vector3(4, 2, 2.5f, -1));

//            //pointCloud.Add(new Vector3(4, 0, 0.5, -1));
//            //pointCloud.Add(new Vector3(5, 0, 0.75, -1));
//            //pointCloud.Add(new Vector3(6, 0.5, 0.5, -1));
//            //pointCloud.Add(new Vector3(7, 0.25, 0.5, -1));


//            //pointCloud.Add(new Vector3(9, -1, 0.5, -1));

//        }
   
       
//        [Test]
//        public void PlaneShift()
//        {
//            ResetTest();
//            

//            pointCloudSource.Add(new Vector3(0, 0, 0, -1));
//            pointCloudSource.Add(new Vector3(1, 0, 1, -1));
//            pointCloudSource.Add(new Vector3(2, 1, 0, -1));
//            pointCloudSource.Add(new Vector3(3, 1, 1, -1));
//            Matrix3 R = Matrix3.CreateRotationX(45);
//            PointCloud.Rotate(pointCloudSource, R);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, false, true);
//            ShowCubeLinesAndNormals(normals);
//        }
    
      
      
//        [Test]
//        public void PlaneXY_CM()
//        {
//            ResetTest();
//            
//            CreatePlane();


//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, true, true);
//            ShowCubeLinesAndNormals(normals);
//        }
//        [Test]
//        public void Facet1()
//        {
//            ResetTest();
//            
          
//            pointCloudSource.Add(new Vector3(0, 0, 0, -1));
//            pointCloudSource.Add(new Vector3(1, 0, 1, 0));
//            pointCloudSource.Add(new Vector3(2, 1, 0, 0));
//            pointCloudSource.Add(new Vector3(3, 0, 0, 0));

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 3, false, true);
//            ShowCubeLinesAndNormals(normals);
//        }
//        [Test]
//        public void Facet2()
//        {
//            ResetTest();
//            
          
//            pointCloudSource.Add(new Vector3(0, 1, 1, 0));
//            pointCloudSource.Add(new Vector3(1, 1, 0, 0));
//            pointCloudSource.Add(new Vector3(2, 0, 1, 0));
//            pointCloudSource.Add(new Vector3(3, 1, 1, 1));




//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 3, false, true);
//            ShowCubeLinesAndNormals(normals);



//        }
            
     
//        [Test]
//        public void Cube()
//        {
//            ResetTest();
//            
//            pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(1);
//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 3, false, true);
//            ShowCubeLinesAndNormals(normals);



//        }
//        [Test]
//        public void CubeCorners_CM()
//        {
//            ResetTest();
//         
//            pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(1, 1);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 3, true, true);
//            ShowCubeLinesAndNormals(normals);


//        }
//        [Test]
//        public void CubeCorners()
//        {
//            ResetTest();
//         
//            pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(1, 1);

//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 3, false, true);
//            ShowCubeLinesAndNormals(normals);
//        }
      
     
      
//        [Test]
//        public void Cube_StartAt0_CM()
//        {
//            ResetTest();
//         
//            pointCloudSource = PointCloud.CreateCube_RegularGrid_StartAt0_Filled(1, 1);


//            List<Vector3> normals = Model.CalculateNormals_PCA(pointCloudSource, 4, true, true);
//            ShowCubeLinesAndNormals(normals);

//        }
      
     

//    }
//}
