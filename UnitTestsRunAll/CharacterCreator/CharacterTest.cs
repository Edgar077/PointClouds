using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;
using OpenTK;
using CharacterCreator;

namespace UnitTestsOpenTK.Characters
{
    [TestFixture]
    [Category("UnitTest")]
    public class CharacterTest : TestBaseICP
    {
        protected FaceMatcher faceMatcher;

        
        [Test]
        public void SerializeListVectors()
        {
            CharactersBO cc = new CharactersBO();
            Humanoid c = new Humanoid(cc.Names[0]);

           

            //JsonSerializer.SerializeListFloats(new Object());
           //Character c = new Character()


        }

        [Test]
        public void SerializeListFloats()
        {

            CharacterCreator.tester t = new CharacterCreator.tester();
            t.SerializeListFloats();

        }
     
        [Test]
        public void ShowCutFace()
        {
            string fileName = this.pathModels + "\\FaceMatching\\CutFace.json";
            if (System.IO.File.Exists(fileName))
            {
                List<Vector3> listV = JsonUtils.DeserializeVectors(fileName);
                PointCloud pc = PointCloud.FromListVector3(listV);

                
                ShowPointCloud(pc);

            }
           
        }
        [Test]
        public void ShowModelWithoutFace()
        {
            string fileName = this.pathModels + "\\FaceMatching\\ModelWithoutFace.json";
            if (System.IO.File.Exists(fileName))
            {
                List<Vector3> listV = JsonUtils.DeserializeVectors(fileName);
                PointCloud pc = PointCloud.FromListVector3(listV);

                ShowPointCloud(pc);
                

            }

        }
        [Test]
        public void AdjustEdsFaceToModel_Ed1()
        {
            this.faceMatcher = new FaceMatcher();

            this.faceMatcher.LoadHumanoid(4);
            this.faceMatcher.LoadFace(this.pathModels + "\\FaceMatching", "EdFace1.obj");
            this.faceMatcher.Rotate_AdjustFaceDepth();
            this.faceMatcher.LoadSkeleton(this.pathModels + "\\FaceMatching", "EdJoints1.json");
            this.faceMatcher.UpdateModel_Joints();
            this.faceMatcher.CutFace();
            this.faceMatcher.Humanoid.ToJson(this.pathModels + "\\FaceMatching\\ModelWithoutFace1.json");

            this.faceMatcher.AdjustFaceToHumanoid();

            //PointCloud result = this.faceMatcher.MergeResultModelAndSave_FullFace(this.pathModels + "\\FaceMatching", "EdResult1.obj");
            PointCloud result = this.faceMatcher.MergeResultModelAndSave(this.pathModels + "\\FaceMatching", "EdResult1.obj");



            this.pointCloudSource = this.faceMatcher.Skeleton.PointCloud;
            this.pointCloudTarget = null;
            this.pointCloudResult = result;

            this.faceMatcher.SaveCutFace(this.pathModels + "\\FaceMatching\\CutFace1.json");
            this.faceMatcher.Humanoid.ToJson(this.pathModels + "\\FaceMatching\\ModelWithNewFace1.json");


            Show3PointCloudsInWindow(false);


        }
        [Test]
        public void AdjustEdsFaceToModel_Ed2()
        {
            this.faceMatcher = new FaceMatcher();

            this.faceMatcher.LoadHumanoid(7);
            this.faceMatcher.LoadFace(this.pathModels + "\\FaceMatching", "EdFace1.obj");
            this.faceMatcher.Rotate_AdjustFaceDepth();
            this.faceMatcher.LoadSkeleton(this.pathModels + "\\FaceMatching", "EdJoints1.json");
            this.faceMatcher.UpdateModel_Joints();
            this.faceMatcher.CutFace();
            this.faceMatcher.AdjustFaceToHumanoid();

            PointCloud result = this.faceMatcher.MergeResultModelAndSave(this.pathModels + "\\FaceMatching", "EdResult1.obj");
            //PointCloud result = this.faceMatcher.MergeResultModelAndSave_FullFace(this.pathModels + "\\FaceMatching", "EdResult1.obj");



            this.pointCloudSource = this.faceMatcher.Skeleton.PointCloud;
            this.pointCloudTarget = null;
            this.pointCloudResult = result;

            this.faceMatcher.SaveCutFace(this.pathModels + "\\FaceMatching\\CutFace2.json");
            this.faceMatcher.Humanoid.ToJson(this.pathModels + "\\FaceMatching\\ModelWithNewFace2.json");

            Show3PointCloudsInWindow(false);


        }
        [Test]
        public void AlignFace()
        {
            this.faceMatcher = new FaceMatcher();

           
            PointCloud faceCut = PointCloud.FromJsonFile(this.pathModels + "\\FaceMatching\\CutFace2.json");
            PointCloud facenNew = PointCloud.FromObjFile(this.pathModels + "\\FaceMatching", "EdFace1.obj");
            facenNew.RotateDegrees(0, 180, 0);

            

            //this.faceMatcher.SaveCutFace(this.pathModels + "\\FaceMatching\\CutFace2.json");
            //this.faceMatcher.Humanoid.SaveVectors(this.pathModels + "\\FaceMatching\\ModelWithNewFace2.json");


            this.pointCloudSource = faceCut;
            this.pointCloudTarget = facenNew;

            //icp.ICPSettings.ShuffleEffect = true;
            icp.ICPSettings.MaximumNumberOfIterations = 10;
            icp.ICPSettings.SingleSourceTargetMatching = true;

            icp.ICPSettings.ICPVersion = ICP_VersionUsed.NoScaling;
            icp.TakenAlgorithm = true;
            pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, pointCloudTarget);

           

            Show3PointCloudsInWindow(false);


        }

        [Test]
        public void AdjustEdsFaceToModel_3()
        {
            this.faceMatcher = new FaceMatcher();

            this.faceMatcher.LoadHumanoid(7);
            this.faceMatcher.LoadFace(this.pathModels + "\\FaceMatching", "EdFace1.obj");
            this.faceMatcher.Rotate_AdjustFaceDepth();
            this.faceMatcher.LoadSkeleton(this.pathModels + "\\FaceMatching", "EdJoints1.json");
            this.faceMatcher.UpdateModel_Joints();
            this.faceMatcher.CutFace();
            this.faceMatcher.AdjustFaceToHumanoid();

            this.faceMatcher.AlignFaces();

            PointCloud result = this.faceMatcher.MergeResultModelAndSave(this.pathModels + "\\FaceMatching", "EdResult3.obj");
            //PointCloud result = this.faceMatcher.MergeResultModelAndSave_FullFace(this.pathModels + "\\FaceMatching", "EdResult1.obj");



            this.pointCloudSource = this.faceMatcher.Skeleton.PointCloud;
            this.pointCloudTarget = null;
            this.pointCloudResult = result;

            this.faceMatcher.SaveCutFace(this.pathModels + "\\FaceMatching\\CutFace3.json");
            this.faceMatcher.Humanoid.ToJson(this.pathModels + "\\FaceMatching\\ModelWithNewFace3.json");

            Show3PointCloudsInWindow(false);


        }
    }
}
