using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Diagnostics;
using OpenTKExtension;

namespace ICPLib
{
    public class ICPTestData
    {
        //private static int cubeSize ;

        //private static PointCloud pointCloudTarget;
        //private static PointCloud pointCloudSource;
       // private static PointCloud vectorsResult;
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "\\Models\\UnitTests";

        public static float Test1_Translation(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {
            mypointCloudTarget = PointCloud.CreateSomePoints();
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);
            PointCloud.Translate(mypointCloudSource, 10, 3, 8);

            
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;

        }
        public static float Test2_RotationX30Degrees(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {
            
            
            //mypointCloudTarget = Vertices.CreateSomePoints();
            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            Matrix3 R = Matrix3.CreateRotationX(30);
            PointCloud.Rotate(mypointCloudSource, R);


            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;

        }
        public static float Test2_Identity(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {


            //mypointCloudTarget = Vertices.CreateSomePoints();
            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);


            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;

        }
  
        public static float Test2_RotationXYZ(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {
            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);


            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(90, 124, -274);

            PointCloud.Rotate(mypointCloudSource, R);


            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test3_Scale(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {
          
            //mypointCloudTarget = CreateSomePoints();
            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.ScaleByFactor(mypointCloudSource, 0.2f);
            
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
       
        public static float Test5_CubeTranslation(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(cubeSize);
            
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.Translate(mypointCloudSource, 0, -300, 0);
            
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_CubeTranslation2(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(cubeSize);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.Translate(mypointCloudSource, 10, 10,-10);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_CubeRotate(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(90, 124, -274);
            PointCloud.Rotate(mypointCloudSource, R);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        private static Matrix3 CreateAndPrintMatrix(float x, float y, float z)
        {
            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(x,y,z);
            R.WriteMatrix();

            return R;
        }
        public static float Test5_CubeRotate45(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {


            mypointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(cubeSize, 10);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            Matrix3 R = CreateAndPrintMatrix(45, 45, 45);
            PointCloud.Rotate(mypointCloudSource, R);
         
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_Cube8RotateShuffle(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(cubeSize);
            
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            Matrix3 R = CreateAndPrintMatrix(45, 45, 45);
            PointCloud.Rotate(mypointCloudSource, R);

            PointCloud.ShuffleTest(mypointCloudSource);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_Cube8Shuffle(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(cubeSize);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.ShuffleTest(mypointCloudSource);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_Cube8Shuffle_60000(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = Example3DModels.Cube_RegularGrid_Empty(cubeSize, 100);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.ShuffleRandom(mypointCloudSource);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_Cube8Shuffle_1Milion(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = Example3DModels.Cube_RegularGrid_Empty(cubeSize, 409);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.ShuffleRandom(mypointCloudSource);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_Cube8TranslateRotateShuffleNew(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(cubeSize);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);
            //Model3D myModel = Example3DModels.Cuboid("Cuboid", 20f, 40f, 100, System.Drawing.Color.White, null);

            Matrix3 R = CreateAndPrintMatrix(65, -123, 35);
            PointCloud.Rotate(mypointCloudSource, R);
            PointCloud.Translate(mypointCloudSource, cubeSize * 1.2f, -cubeSize * 2.5f, cubeSize *2);

            PointCloud.ShuffleTest(mypointCloudSource);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_CubeInhomogenous(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.ScaleByVector(mypointCloudSource, new Vector3(1,2,3));
            
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_CubeScale_Uniform(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);
            PointCloud.ScaleByFactor(mypointCloudSource, 0.2f);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_CubeScale_Inhomogenous(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);
            
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);
            PointCloud.ScaleByVector(mypointCloudSource, new Vector3(1, 2, 3));
            
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_CuboidIdentity(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            float cubeSize = 1;
            float cubeSizeY = 2;
            int numberOfPoints = 3;
            Model myModel = Example3DModels.Cuboid("Cuboid", cubeSize, cubeSizeY, numberOfPoints, System.Drawing.Color.White, null);
            mypointCloudTarget = myModel.PointCloud;



            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            //Vertices.Translate(mypointCloudSource, 220, -300, 127);
            //Vertices.ScaleByFactor(mypointCloudSource, 0.2);

            //Matrix3 R = new Matrix3();
            //R = R.RotationXYZDegrees(90, 124, -274);
            //Vertices.Rotate(mypointCloudSource, R);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_CuboidRotate(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            float cubeSize = 1;
            float cubeSizeY = 2;
            int numberOfPoints = 3;
            Model myModel = Example3DModels.Cuboid("Cuboid", cubeSize, cubeSizeY, numberOfPoints, System.Drawing.Color.White, null);
            mypointCloudTarget = myModel.PointCloud;



            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            //Vertices.Translate(mypointCloudSource, 220, -300, 127);
            //Vertices.ScaleByFactor(mypointCloudSource, 0.2);
            
            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(90, 124, -274);
            PointCloud.Rotate(mypointCloudSource, R);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_CubeRotateTranslate_ScaleUniform(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

           

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            //Vertices.Translate(mypointCloudSource, 220, -300, 127);
            //Vertices.ScaleByFactor(mypointCloudSource, 0.2);

            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(90, 124, -274);
            PointCloud.Rotate(mypointCloudSource, R);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test5_CubeRotateTranslate_ScaleInhomogenous(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.Translate(mypointCloudSource, 0, 0, 149);
            PointCloud.ScaleByVector(mypointCloudSource, new Vector3(1, 2, 3));

            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(90, 124, -274);
            PointCloud.Rotate(mypointCloudSource, R);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
     
        public static float Test6_Bunny(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {


            Model model3DTarget = new Model(path + "\\bunny.obj");
            mypointCloudTarget = model3DTarget.PointCloud;
            

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.Translate(mypointCloudSource, -0.15f, 0.05f, 0.02f);
            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(30, 30, 30);
            PointCloud.Rotate(mypointCloudSource, R);
            PointCloud.ScaleByFactor(mypointCloudSource, 0.8f);


            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);

            return IterativeClosestPointTransform.Instance.MeanDistance;


        }
        public static float Test6_Bunny_PCA(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {


            Model model3DTarget = new Model(path + "\\bunny.obj");
            mypointCloudTarget = model3DTarget.PointCloud;
            
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);


            
            Matrix3 R = new Matrix3();
            //ICP converges with a rotation of
            //R = R.RotationXYZ(60, 60, 60);
            R = R.RotationXYZDegrees(124, 124, 124);
            

            PointCloud.Rotate(mypointCloudSource, R);
            PCA pca = new PCA();
            mypointCloudResult = pca.AlignPointClouds_SVD(mypointCloudSource, mypointCloudTarget);
            if (pca.MeanDistance > 1e-5)
                mypointCloudResult = pca.AlignPointClouds_SVD( mypointCloudResult, mypointCloudTarget);

            //mypointCloudResult = pca.AlignPointClouds_SVD(mypointCloudResult, mypointCloudTarget);

            //mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudResult, mypointCloudTarget);

            return IterativeClosestPointTransform.Instance.MeanDistance;


        }
        public static float Test6_Bunny_ExpectedError(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {


            Model model3DTarget = new Model(path + "\\bunny.obj");
            mypointCloudTarget = model3DTarget.PointCloud;

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);



            Matrix3 R = new Matrix3();
            //ICP converges with a rotation of
            //R = R.RotationXYZ(60, 60, 60);
            R = R.RotationXYZRadiants(65, 65, 65);


            PointCloud.Rotate(mypointCloudSource, R);
           

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);

            return IterativeClosestPointTransform.Instance.MeanDistance;


        }
        public static float Test7_Face_KnownTransformation_15000(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
            Model model3DTarget = new Model(path + "\\KinectFace_1_15000.obj");
            mypointCloudTarget = model3DTarget.PointCloud;
            

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);


           

            PointCloud.ScaleByFactor(mypointCloudSource, 0.9f);
            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(60, 60, 60);
            PointCloud.Rotate(mypointCloudSource, R);
            PointCloud.Translate(mypointCloudSource, 0.3f, 0.5f, -0.4f);

            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 44;
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test7_Face_KnownTransformation_PCA_55000(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
            Model model3DTarget = new Model(path + "\\KinectFace_1_55000.obj");
            mypointCloudTarget = model3DTarget.PointCloud;


            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);


            PointCloud.ScaleByFactor(mypointCloudSource, 0.9f);
            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(60, 60, 60);
            PointCloud.Rotate(mypointCloudSource, R);
            PointCloud.Translate(mypointCloudSource, 0.3f, 0.5f, -0.4f);

            PCA pca = new PCA();
            mypointCloudResult = pca.AlignPointClouds_SVD( mypointCloudSource, mypointCloudTarget);
            mypointCloudSource = mypointCloudResult;

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test7_Face_KnownTransformation_PCA_15000(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
            Model model3DTarget = new Model(path + "\\KinectFace_1_15000.obj");
            mypointCloudTarget = model3DTarget.PointCloud;
            
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);


            PointCloud.ScaleByFactor(mypointCloudSource, 0.9f);
            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(60, 60, 60);
            PointCloud.Rotate(mypointCloudSource, R);
            PointCloud.Translate(mypointCloudSource, 0.3f, 0.5f, -0.4f);

            PCA pca = new PCA();
            mypointCloudResult = pca.AlignPointClouds_SVD(mypointCloudSource, mypointCloudTarget);
            mypointCloudSource = mypointCloudResult;

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
      
        public static float Test8_CubeOutliers_Translate(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(20);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);
            PointCloud.Translate(mypointCloudSource, 0, -300, 0);
            PointCloud.CreateOutliers(mypointCloudSource, 5);
            
            
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test8_CubeOutliers_Rotate(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {

            //mypointCloudTarget = Vertices.CreateCube_Corners(50);
            Model myModel = Example3DModels.Cuboid("Cuboid", 20f, 40f, 100, System.Drawing.Color.White, null);
            mypointCloudTarget = myModel.PointCloud;

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);
            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(30, 30, 30);
            PointCloud.Rotate(mypointCloudSource, R);

            PointCloud.CreateOutliers(mypointCloudSource, 5);
            

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test9_Inhomogenous(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {
            mypointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(50);
            //mypointCloudTarget = Vertices.CreateSomePoints();
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.InhomogenousTransform(mypointCloudSource, 2);

            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test9_Face_Stitch(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {


            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
            Model model3DTarget = new Model(path + "\\KinectFace_1_15000.obj");
            mypointCloudTarget = model3DTarget.PointCloud;
            
            Model model3DSource = new Model(path + "\\KinectFace_2_15000.obj");
            mypointCloudSource = model3DSource.PointCloud;
            
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
 
        public static float Test10_Cube8pRotateShuffle(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {
            
            mypointCloudTarget = Example3DModels.Cube_RegularGrid_Empty(cubeSize, 1);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);
            
            Matrix3 R = CreateAndPrintMatrix(65, -123, 35);
            PointCloud.Rotate(mypointCloudSource, R);
            
            PointCloud.ShuffleTest(mypointCloudSource);

         
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test10_Cube8pRotateTranslateShuffle(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = Example3DModels.Cube_RegularGrid_Empty(cubeSize, 1);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.Translate(mypointCloudSource , cubeSize* 1.2f, -cubeSize * 2.5f , cubeSize * 2);

            Matrix3 R = CreateAndPrintMatrix(65, -123, 35);
            PointCloud.Rotate(mypointCloudSource, R);

            PointCloud.ShuffleTest(mypointCloudSource);


            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test10_Cube8pRotateTranslateScaleShuffle(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = Example3DModels.Cube_RegularGrid_Empty(cubeSize, 1);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.Translate(mypointCloudSource, cubeSize * 1.2f, -cubeSize * 2.5f, cubeSize * 2);
            PointCloud.ScaleByFactor(mypointCloudSource, 0.2f);
            Matrix3 R = CreateAndPrintMatrix(65, -123, 35);
            PointCloud.Rotate(mypointCloudSource, R);

            PointCloud.ShuffleTest(mypointCloudSource);


            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test10_Cube26pRotateTranslateScaleShuffle(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = Example3DModels.Cube_RegularGrid_Empty(cubeSize, 2);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            PointCloud.Translate(mypointCloudSource, cubeSize * 1.2f, -cubeSize * 2.5f, cubeSize * 2);

            PointCloud.ScaleByFactor(mypointCloudSource, 0.2f);
            Matrix3 R = CreateAndPrintMatrix(65, -123, 35);
            PointCloud.Rotate(mypointCloudSource, R);

            PointCloud.ShuffleTest(mypointCloudSource);


            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test10_Cube26p_RotateShuffle(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {

            mypointCloudTarget = Example3DModels.Cube_RegularGrid_Empty(cubeSize, 2);
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);

            Matrix3 R = CreateAndPrintMatrix(65, -123, 35);
            PointCloud.Rotate(mypointCloudSource, R);

            PointCloud.ShuffleRandom(mypointCloudSource);


            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test10_Cube98p_Rotate(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {
            mypointCloudTarget = Example3DModels.Cube_RegularGrid_Empty(cubeSize, 4);
            //mypointCloudTarget = Vertices.CreateCube_Corners(10);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);


            Matrix3 R = CreateAndPrintMatrix(65, -123, 35);
            PointCloud.Rotate(mypointCloudSource, R);
            


            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test10_Cube26p_Rotate(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {
            mypointCloudTarget = Example3DModels.Cube_RegularGrid_Empty(cubeSize, 2);
            //mypointCloudTarget = Vertices.CreateCube_Corners(10);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);


            Matrix3 R = CreateAndPrintMatrix(65, -123, 35);
            PointCloud.Rotate(mypointCloudSource, R);



            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test10_CubeRTranslate(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult, float cubeSize)
        {
            mypointCloudTarget = Example3DModels.Cube_RegularGrid_Empty(cubeSize, 5);
            //mypointCloudTarget = Vertices.CreateCube_Corners(10);

            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);


            PointCloud.Translate(mypointCloudSource, cubeSize * 1.2f, -cubeSize * 2.5f, cubeSize * 2);



            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }


        public static float Test11_Person(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {


            Model model3DTarget = new Model(path + "\\1.obj");
            mypointCloudTarget = model3DTarget.PointCloud;
            
            mypointCloudSource = PointCloud.CloneAll(mypointCloudTarget);
            PointCloud.RotateDegrees(mypointCloudSource, 25, 10, 25);

            
            
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }
        public static float Test11_Person_TwoScans(ref PointCloud mypointCloudTarget, ref PointCloud mypointCloudSource, ref PointCloud mypointCloudResult)
        {


            Model model3DTarget = new Model(path + "\\1.obj");
            mypointCloudTarget = model3DTarget.PointCloud;
            
            Model model3DSource = new Model(path + "\\2.obj");
            mypointCloudSource = model3DSource.PointCloud;
            
            mypointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(mypointCloudSource, mypointCloudTarget);
            return IterativeClosestPointTransform.Instance.MeanDistance;
        }

    
    }
}
