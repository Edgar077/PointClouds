//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using System.IO;
//using System.Drawing;
//using System.Globalization;

//namespace OpenTKExtension
//{
//    public class Model
//    {

//        public PointCloud PointCloud;
//        public delegate double[] CoordFuncXYZ(double u, double v);

       

//        public Model()
//        {
//        }
//        public Model(string path, string fileName) : this(path + "\\" + fileName)
//        {
//           // this.Name = fileName;

//        }
//        public Model(string fileName)
//        {
//            string str = Path.GetExtension(fileName).ToLower();

//            //this.Name = IOUtils.ExtractFileNameShort(fileName);
//            //this.Name = IOUtils.ExtractFileNameWithoutExtension(this.Name);

//            if (str == ".obj")
//                this.PointCloud = PointCloud.FromObjFile(fileName);
//            else if (str == ".xyz")
//                this.PointCloud = PointCloud.FromXYZFile(fileName);
//            else
//            {
//                System.Windows.Forms.MessageBox.Show("SW Error - inconsistend use of constructor Model");
//                return;
//            }

//            if (GLSettings.PointCloudCentered )
//            {
//                if (PointCloud != null && !PointCloud.DisregardCenteredShowing)
//                    this.PointCloud.ResetCentroid(true);
//            }
//            if (GLSettings.BoundingBoxLeftStartsAt000 )
//            {
//                if (PointCloud != null && !PointCloud.DisregardCenteredShowing)
//                    this.PointCloud.Translate_StartAt_Y0();
//                    //this.PointCloud.Translate_StartAtBoundingBox000();
//            }

           

//            this.PointCloud.Name = IOUtils.ExtractFileNameWithoutExtension(this.Name); 
//        }
       
//        public string Name
//        {
//            get
//            {
//                if (this.PointCloud != null)
//                    return this.PointCloud.Name;
//                return string.Empty;
//            }
//        }
     
//        //public void CalculateNormals_PCA()
//        //{
//        //    //List<Vector3> normals = Model.CalculateNormals_PCA(this.PointCloud, 4, false, true);
//        //    //this.PointCloud.Normals = normals.ToArray();


//        //}
//        //public static List<Vector3> CalculateNormals_PCA(PointCloud pointCloud, int numberOfNeighbours, bool centerOfMassMethod, bool flipNormalWithOriginVector)
//        //{

//        //    KDTreeVertex kv = new KDTreeVertex();
//        //    kv.NumberOfNeighboursToSearch = numberOfNeighbours;
//        //    kv.BuildKDTree_Rednaxela(pointCloud);
//        //    kv.ResetVerticesSearchResult(pointCloud);
//        //    kv.FindNearest_NormalsCheck_Rednaxela(pointCloud, false, false, 0f);



//        //    PCA pca = new PCA();
//        //    List<Vector3> normals = pca.Normals(pointCloud, centerOfMassMethod, flipNormalWithOriginVector);

//        //    return normals;


//        //}
//        //public List<Vector3> Normals
//        //{
//        //    get
//        //    {
//        //        return this.PointCloud.Normals.ToList<Vector3>();
//        //    }
//        //    set
//        //    {
//        //        this.PointCloud.Normals = value.ToArray();
//        //    }
//        //}
     

//    }
//}
