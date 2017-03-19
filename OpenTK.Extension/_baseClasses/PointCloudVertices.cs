// Pogramming by
//     Douglas Andrade ( http://www.cmsoft.com.br, email: cmsoft@cmsoft.com.br)
//               Implementation of most of the functionality
//     Edgar Maass: (email: maass@logisel.de)
//               Code adaption, changed to user control
//
//Software used: 
//    OpenGL : http://www.opengl.org
//    OpenTK : http://www.opentk.com
//
// DISCLAIMER: Users rely upon this software at their own risk, and assume the responsibility for the results. Should this software or program prove defective, 
// users assume the cost of all losses, including, but not limited to, any necessary servicing, repair or correction. In no event shall the developers or any person 
// be liable for any loss, expense or damage, of any type or nature arising out of the use of, or inability to use this software or program, including, but not
// limited to, claims, suits or causes of action involving alleged infringement of copyrights, patents, trademarks, trade secrets, or unfair competition. 
//


using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using OpenTKLib.FastGLControl;



namespace OpenTKLib
{
    public class PointCloudVertices : List<Vertex>// PointCloudGL //List<Vertex>
    {

       
        public Vertex CentroidVertex;
        public double LineLengthGeometricModel;

        public List<LineD> LinesGeometricalModels;
        public List<LineD> LinesNormals;
        
        
        public Vector3d CentroidVector;
        public PointCloudVertices PCAAxes;
        public PointCloudVertices PCAAxesNew;

        public System.Drawing.Color Color;
        public bool ShowLinesConnectingPoints;

        private List<LineD> linesPCAAxes;
        Vertex boundingBoxMax;
        Vertex boundingBoxMin;
        Vertex centroidVertex;
        public string Path;
        public string FileNameShort;

       
        public PointCloudVertices()
        {
        }
        public PointCloudVertices(List<Vertex> source):base(source)
        {
            

        }


            
        public PointCloudVertices PCAAxesNormalized
        {
            get
            {
                PointCloudVertices axesNormalized = new PointCloudVertices();
                {
                    for (int i = 0; i < 3; i++)
                    {
                        axesNormalized.Add(new Vertex(PCAAxes[i].Vector.NormalizeV()));
                    }
                }
                return axesNormalized;
            }

        }
        public string PrintVectors()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for(int i = 0; i < this.Count; i++)
            {
                Vertex v = this[i];
                sb.Append(v.Vector.ToString());

            }
            return sb.ToString();
        }
        public override string ToString()
        {
            
            string returnString = this.Count.ToString();
            return returnString;
        }
        public bool InsideCloud(Vertex v)
        {

            if (v <= BoundingBoxMax && v >= boundingBoxMin)
                return true;
            return false;
            //{

            //}
        }

        //public static Vertex GetVertexMax(PointCloud pointCloud)
        //{
        //    Vertex centerOfGravity = new Vertex();
        //    Vertex maxPoint = new Vertex();
        //    Vertex minPoint = new Vertex();

        //    BoundingBox(pointCloud, ref maxPoint, ref minPoint);
           
        //    return maxPoint;

        //}
   
        public static void AddVertex(PointCloudVertices pointCloud, Vertex newOrigin)
        {
            //reset vertex so that it starts from 0,0,0
            for (int i = 0; i < pointCloud.Count; i++)
            {
                Vector3d v = pointCloud[i].Vector;
                v.X += newOrigin.Vector.X;
                v.Y += newOrigin.Vector.Y;
                v.Z += newOrigin.Vector.Z;
                pointCloud[i].Vector = v;

            }

        }
        public static void AddVector(PointCloudVertices pointCloud, Vector3d centroid)
        {

            for (int i = 0; i < pointCloud.Count; i++)
            {

                Vertex v = pointCloud[i];
                Vector3d translatedV = Vector3d.Add(v.Vector, centroid);
                v.Vector = translatedV;
                pointCloud[i] = v;
            }

        }
        public static void SubtractVectorRef(PointCloudVertices pointCloud, Vector3d centroid)
        {

            for (int i = 0; i < pointCloud.Count; i++)
            {

                Vertex v = pointCloud[i];
                Vector3d translatedV = Vector3d.Subtract(v.Vector, centroid);
                v.Vector = translatedV;
                pointCloud[i] = v;
            }

        }
        public static PointCloudVertices SubtractVector(PointCloudVertices pointCloud, Vector3d centroid)
        {
            PointCloudVertices resultCloud = new PointCloudVertices();
            for (int i = 0; i < pointCloud.Count; i++)
            {

                Vertex v = pointCloud[i];
                Vector3d translatedV = Vector3d.Subtract(v.Vector, centroid);
                resultCloud.Add(new Vertex(v.IndexInCloud, translatedV));

            }
            return resultCloud;
        }
        public PointCloudVertices SubtractClouds(PointCloudVertices pointCloud)
        {
            PointCloudVertices resultCloud = new PointCloudVertices();
            for (int i = 0; i < this.Count; i++)
            {

                Vertex v = pointCloud[i];
                resultCloud.Add(new Vertex(v.IndexInCloud, this[i].Vector - pointCloud[i].Vector));

            }
            return resultCloud;
        }
        //public static void AddVector(List<Vector3d> vectors, Vector3d centroid)
        //{

        //    for (int i = 0; i < vectors.Count; i++)
        //    {

        //        Vector3d v = vectors[i];
        //        Vector3d translatedV = Vector3d.Add(v, centroid);
        //        v = translatedV;
        //        vectors[i] = v;
        //    }

        //}
        public Vertex CalculateCentroid()
        {
            centroidVertex = new Vertex();


            foreach (Vertex vr in this)
                centroidVertex.Vector += vr.Vector;
            centroidVertex.Vector /= (double)this.Count;
            return centroidVertex;
        }
        private static Vector3d CalculateCentroidVector(PointCloudVertices pointCloud)
        {
            Vector3d centroidVector = new Vector3d();


            foreach (Vertex vr in pointCloud)
                centroidVector += vr.Vector;
            centroidVector /= (double)pointCloud.Count;
            return centroidVector;
        }
        public void CalculateBoundingBox()
        {
           
            if (this.Count < 1)
                return;

            boundingBoxMax = new Vertex();
            boundingBoxMin = new Vertex();

            double xMax = this[0].Vector.X;
            double yMax = this[0].Vector.Y;
            double zMax = this[0].Vector.Z;
            double xMin = this[0].Vector.X;
            double yMin = this[0].Vector.Y;
            double zMin = this[0].Vector.Z;
            foreach (Vertex ver in this)
            {
                if (ver.Vector.X > xMax)
                    xMax = ver.Vector.X;
                if (ver.Vector.Y > yMax)
                    yMax = ver.Vector.Y;
                if (ver.Vector.Z > zMax)
                    zMax = ver.Vector.Z;
                if (ver.Vector.X < xMin)
                    xMin = ver.Vector.X;
                if (ver.Vector.Y < yMin)
                    yMin = ver.Vector.Y;
                if (ver.Vector.Z < zMin)
                    zMin = ver.Vector.Z;
            }
            boundingBoxMax.Vector.X = xMax;
            boundingBoxMax.Vector.Y = yMax;
            boundingBoxMax.Vector.Z = zMax;

            boundingBoxMin.Vector.X = xMin;
            boundingBoxMin.Vector.Y = yMin;
            boundingBoxMin.Vector.Z = zMin;

        }
      
        public static Vertex ResetCentroid(PointCloudVertices pointCloud, bool centered)
        {
            if (centered)
            {
                //center point cloud to centroid
                if (pointCloud.CentroidVertexGet.Vector.IsZero())
                    return pointCloud.CentroidVertexGet;
                pointCloud.CentroidVertex = pointCloud.CentroidVertexGet;
                SubtractVectorRef(pointCloud, pointCloud.CentroidVertexGet.Vector);
                pointCloud.CalculateBoundingBox();

                
                //centroid - is now origin
                pointCloud.centroidVertex = new Vertex(0, 0, 0);
                if (pointCloud.ShowLinesConnectingPoints)
                {
                    if (pointCloud.LinesGeometricalModels != null)
                        pointCloud.LinesGeometricalModels_Reset();
                }
                //pointCloud.CalculateLinesPCA();


                
            }
            else
            {
                //reset to old center
                if (pointCloud.CentroidVertex != null && !pointCloud.CentroidVertex.Vector.IsZero())
                {
                    AddVertex(pointCloud, pointCloud.CentroidVertex);
                    pointCloud.CalculateCentroid() ;//recalcs centrooid
                    pointCloud.CalculateBoundingBox();

                    if (pointCloud.ShowLinesConnectingPoints)
                        pointCloud.LinesGeometricalModels_Reset();

                    pointCloud.CentroidVertex = null;
                    //pointCloud.CalculateLinesPCA();

                }

            }
            return pointCloud.CentroidVertex;


        }
        
        //public void ResizeTo1Old()
        //{

        //    boundingBoxMax = new Vertex();
        //    this.boundingBoxMin = new Vertex();

        //    CalculateBoundingBox();
        //    double d = Math.Max(boundingBoxMax.Vector.X, boundingBoxMax.Vector.Y);
        //    d = Math.Max(d, boundingBoxMax.Vector.Z);
        //    if (d > 0)
        //    {

        //        for (int i = 0; i < this.Count; i++)
        //        {
        //            this[i].Vector.X /= d;
        //            this[i].Vector.Y /= d;
        //            this[i].Vector.Z /= d;
        //        }
        //    }


        //}

      

        public static void ResizeTo1(PointCloudVertices pointCloud, ref Vector3d centerOfGravity, ref Vector3d maxPoint, ref Vector3d minPoint)
        {
            double d = Math.Max(maxPoint.X, maxPoint.Y);
            d = Math.Max(d, maxPoint.Z);
            if (d > 0)
            {
                centerOfGravity.X /= d;
                centerOfGravity.Y /= d;
                centerOfGravity.Z /= d;
                for (int i = 0; i < pointCloud.Count; i++)
                {
                    pointCloud[i].Vector.X /= d;
                    pointCloud[i].Vector.Y /= d;
                    pointCloud[i].Vector.Z /= d;
                }
            }


        }
        public static List<Vector3d> ToVectors(PointCloudVertices listPoints)
        {
            List<Vector3d> listOfVectors = new List<Vector3d>();
            int nDim = listPoints.Count;
            for (int i = 0; i < nDim; i++)
            {
                Vertex myPoint = listPoints[i];
                listOfVectors.Add(new Vector3d(myPoint.Vector.X, myPoint.Vector.Y, myPoint.Vector.Z));
            }

            return listOfVectors;
        }
        public static Vector3d[] ToVectorArray(PointCloudVertices listPoints)
        {
            int nDim = listPoints.Count;
            Vector3d[] arrVectors = new Vector3d[nDim];
            for (int i = 0; i < nDim; i++)
            {
                Vertex myPoint = listPoints[i];
                arrVectors[i] = myPoint.Vector;
            }

            return arrVectors;
        }
        public static Vector3d[] ToVector3dArray(PointCloudVertices listPoints)
        {
            int nDim = listPoints.Count;
            Vector3d[] arrVectors = new Vector3d[nDim];
            for (int i = 0; i < nDim; i++)
            {
                Vertex myPoint = listPoints[i];
                arrVectors[i] = new Vector3d(Convert.ToSingle(myPoint.Vector.X), Convert.ToSingle(myPoint.Vector.Y), Convert.ToSingle(myPoint.Vector.Z));
            }

            return arrVectors;
        }
        public static PointCloudVertices FromVectors(List<Vector3d> listPoints)
        {
            PointCloudVertices listOfVeretixes = new PointCloudVertices();
            for (int i = 0; i < listPoints.Count; i++)
            {
                Vector3d myPoint = listPoints[i];
                listOfVeretixes.Add(new Vertex(i, myPoint));
            }

            return listOfVeretixes;
        }
        public static void ColorDelete(PointCloudVertices listPoints)
        {
            
            for (int i = 0; i < listPoints.Count; i++)
            {
                listPoints[i].Color = System.Drawing.Color.Black;
                
            }

            
        }
        public static void ChangeTransparency(PointCloudVertices listPoints, double f)
        {

            for (int i = 0; i < listPoints.Count; i++)
            {
                if (listPoints[i].Color != default(System.Drawing.Color))
                {
                    listPoints[i].Color = System.Drawing.Color.FromArgb(Convert.ToByte(f * 255), listPoints[i].Color);
                }
                
                
            }


        }
        public static void AssignNewVectorList(PointCloudVertices listVertices, List<Vector3d> listPoints)
        {

            for (int i = 0; i < listVertices.Count; i++)
            {
                listVertices[i].Vector = listPoints[i];
                
            }


        }
        public static void SetColorToList(PointCloudVertices myPCLList, byte[] color)
        {
            if (color != null)
            {
                List<System.Drawing.Color> myColors = ColorExtensions.ToColorList(myPCLList.Count, color[0], color[1], color[2], color[3]);
                PointCloudVertices.SetColorToList(myPCLList, myColors);

            }

        }
        public static void SetColorToList(PointCloudVertices listVertices, List<System.Drawing.Color> colorList)
        {
            if (listVertices.Count != colorList.Count)
                return;

            
            for (int i = 0; i < listVertices.Count; i++)
            {
                Vertex myPoint = listVertices[i];
                myPoint.Color = colorList[i];

            }

        }
        //public static void SetColorToList(PointCloud listVertices, List<double[]> colorList)
        //{
        //    if (listVertices.Count != colorList.Count)
        //        return;


        //    for (int i = 0; i < listVertices.Count; i++)
        //    {
        //        Vertex myPoint = listVertices[i];
        //        myPoint.Color = colorList[i];

        //    }

        //}
        //public static void SetColorOfListTo(PointCloud listVertices, double r, double g, double b, double a)
        //{
        //    listVertices.Color = System.Drawing.Color.FromArgb(Convert.ToInt32(r * 255), Convert.ToInt32(g * 255), Convert.ToInt32(b * 255));

        //    double[] color = new double[4] { r, g, b, a };
        //    for (int i = 0; i < listVertices.Count; i++)
        //    {
        //        listVertices[i].Color = color;


        //    }

        //}
        public static void SetColorOfListTo(PointCloudVertices listVertices, System.Drawing.Color color)
        {
           // double[] color = new double[4] { Convert.ToSingle(color[0]), Convert.ToSingle(color[1]), Convert.ToSingle(color[2]), 1f };
            for (int i = 0; i < listVertices.Count; i++)
            {
                //double[] colorF = color.ToFloats();
                

                listVertices[i].Color = color;


            }

        }
        public static PointCloudVertices CopyVertices(PointCloudVertices pointsTarget)
        {
            if (pointsTarget == null)
                return null;

            PointCloudVertices tempPoints = new PointCloudVertices();

            for (int i = 0; i < pointsTarget.Count; i++)
            {
                Vertex point1 = pointsTarget[i];
                Vertex v = new Vertex(point1.IndexInCloud, point1.Vector, point1.Color);
                //v.IndexNormals = point1.IndexNormals;
                tempPoints.Add(v);

            }
            tempPoints.PCAAxes = pointsTarget.PCAAxes;
            tempPoints.ShowLinesConnectingPoints = pointsTarget.ShowLinesConnectingPoints;
            
            return tempPoints;

        }
        public static List<Vector3d> CopyVectors(List<Vector3d> pointsTarget)
        {
            List<Vector3d> tempPoints = new List<Vector3d>();

            for (int i = 0; i < pointsTarget.Count; i++)
            {
                Vector3d point1 = pointsTarget[i];
                Vector3d v = new Vector3d(point1);
                tempPoints.Add(v);

            }


            return tempPoints;

        }
        public static PointCloudVertices CreateSomePoints()
        {

            PointCloudVertices points = new PointCloudVertices();
            // Create points
            double[] origin = { 0.0f, 0.0f, 0.0f };
        
            points.Add(new Vertex(0, 100, 0, 0));
            points.Add(new Vertex(1, 0, 100, 0));
            points.Add(new Vertex(2, 0, 0, 100));
           

            return points;

        }
        public static PointCloudVertices CreateCuboid(double u, double v, int numberOfPoints)
        {
            PointCloudVertices points = new PointCloudVertices();
            double v0 = 0f;
            int indexInModel = -1;
            for (int i = 0; i < numberOfPoints; i++)
            {
                indexInModel++;
                points.Add(new Vertex(indexInModel, 0, v0, 0));
                indexInModel++;
                points.Add(new Vertex(indexInModel, 0, v0, u));
                indexInModel++;
                points.Add(new Vertex(indexInModel, u, v0, u));
                indexInModel++;
                points.Add(new Vertex(indexInModel, u, v0, 0));

                v0 += v / numberOfPoints;

            }
            points.ShowLinesConnectingPoints = true;
            return points;

        }
        public static PointCloudVertices FromVector3List(List<Vector3> listVectors)
        {
            PointCloudVertices points = new PointCloudVertices();
            for(int i = 0; i < listVectors.Count; i++)
            {
                points.Add(new Vertex(i, listVectors[i].X, listVectors[i].Y, listVectors[i].Z));
            }
            return points;

        }
        public static PointCloudVertices FromVector3dList(List<Vector3d> listVectors)
        {
            PointCloudVertices points = new PointCloudVertices();
            for (int i = 0; i < listVectors.Count; i++)
            {
                points.Add(new Vertex(i, listVectors[i].X, listVectors[i].Y, listVectors[i].Z));
            }
            return points;

        }
        public static PointCloudVertices FromPointCloud(PointCloud pcl)
        {
            PointCloudVertices points = new PointCloudVertices();
            for (int i = 0; i < pcl.Vectors.Length; i++)
            {
                points.Add(new Vertex(i, pcl.Vectors[i].X, pcl.Vectors[i].Y, pcl.Vectors[i].Z));
            }

            if(pcl.Colors != null)
            {
                for (int i = 0; i < pcl.Colors.GetLength(0); i++)
                {
                    Vector3 col = pcl.Colors[i];
                    points[i].Color = System.Drawing.Color.FromArgb(255, Convert.ToByte(col.X * 255f), Convert.ToByte(col.Y * 255f), Convert.ToByte(col.Z * 255f)); 

                }

            }
            points.Path = pcl.Path;
            points.FileNameShort = pcl.FileNameLong;

            return points;

        }

        public static PointCloudVertices CreateCuboid_Corners(double cubeSizeX, double cubeSizeY, double cubeSizeZ)
        {
            List<Vector3> listVectors = Example3DModels.Cube_Corners(cubeSizeX, cubeSizeY, cubeSizeZ);
            return FromVector3List(listVectors);
                     
        }
        public static PointCloudVertices CreateCube_Corners(double cubeSizeX)
        {
            List<Vector3> listVectors = Example3DModels.Cube_Corners(cubeSizeX, cubeSizeX, cubeSizeX);
            return FromVector3List(listVectors);

        }
        public static PointCloudVertices CreateCube_RegularGrid_Filled(double cubeSize, int numberOfPointsPerPlane)
        {
            List<Vector3> listVectors = Example3DModels.CreateCube_RegularGrid_Filled(cubeSize, numberOfPointsPerPlane);
            return FromVector3List(listVectors);


        }
        public static PointCloudVertices CreateCube_RegularGrid_Empty(double cubeSize, int numberOfPointsPerPlane)
        {
            List<Vector3> listVectors = Example3DModels.CreateCube_RegularGrid_Empty_List(cubeSize, numberOfPointsPerPlane);
            return FromVector3List(listVectors);
            

        }
        public static PointCloudVertices CreateCube_RegularGrid_StartAt0_Filled(double cubeSize, int numberOfPointsPerPlane)
        {
            PointCloudVertices points = new PointCloudVertices();

            //double startP = -cubeSize/2;


            int indexInModel = -1;
            for (int i = 0; i <= numberOfPointsPerPlane; i++)
            {
                for (int j = 0; j <= numberOfPointsPerPlane; j++)
                {
                    for (int k = 0; k <= numberOfPointsPerPlane; k++)
                    {
                        indexInModel++;
                        Vertex v = new Vertex(indexInModel, cubeSize * (1 - i / Convert.ToSingle(numberOfPointsPerPlane)), cubeSize * (1 - j / Convert.ToSingle(numberOfPointsPerPlane)), cubeSize * (1 - k / Convert.ToSingle(numberOfPointsPerPlane)));
                        points.Add(v);

                    }
                }
            }
            points.ShowLinesConnectingPoints = true;
            return points;

        }

        public static PointCloudVertices CreateCube_RegularGrid_StartAt0_Empty(double cubeSize, int numberOfPointsPerPlane)
        {
            PointCloudVertices points = new PointCloudVertices();

            //double startP = -cubeSize/2;


            int indexInModel = -1;
            for (int i = 0; i <= numberOfPointsPerPlane; i++)
            {
                for (int j = 0; j <= numberOfPointsPerPlane; j++)
                {
                    if ((i == 0 || i == numberOfPointsPerPlane) || (j == 0 || j == numberOfPointsPerPlane))
                    {
                        for (int k = 0; k <= numberOfPointsPerPlane; k++)
                        {

                            indexInModel++;
                            Vertex v = new Vertex(indexInModel, cubeSize * (1 - i / Convert.ToSingle(numberOfPointsPerPlane)), cubeSize * (1 - j / Convert.ToSingle(numberOfPointsPerPlane)), cubeSize * (1 - k / Convert.ToSingle(numberOfPointsPerPlane)));
                            points.Add(v);
                        }
                    }
                    else
                    {
                        indexInModel++;
                        Vertex v = new Vertex(indexInModel, cubeSize * (1f - i / Convert.ToSingle(numberOfPointsPerPlane)), cubeSize * (1 - j / Convert.ToSingle(numberOfPointsPerPlane)), cubeSize);
                        points.Add(v);

                        indexInModel++;
                        v = new Vertex(indexInModel, cubeSize * (1 - i / Convert.ToSingle(numberOfPointsPerPlane)), cubeSize * (1 - j / Convert.ToSingle(numberOfPointsPerPlane)), 0);
                        points.Add(v);
                    }

                }

            }
            points.ShowLinesConnectingPoints = true;
            return points;

        }
   
        public static PointCloudVertices CreateCube_RandomPoints(double cubeSize, int numberOfRandomPoints)
        {
            PointCloudVertices points = new PointCloudVertices();
            var r = new Random();
            for (var i = 0; i < numberOfRandomPoints; i++)
            {
                var vi = new Vertex(Convert.ToSingle(i * r.NextDouble() - cubeSize / 2),
                                    Convert.ToSingle(cubeSize * r.NextDouble() - cubeSize / 2),
                                    Convert.ToSingle(cubeSize * r.NextDouble() - cubeSize / 2));
                points.Add(vi);
                
            }
            points.ShowLinesConnectingPoints = true;
            return points;
        }


  
            
        //public static List<Vector3d> Vector3dDList_FromArray(ushort[] arrayDepth, int width, int height)
        //{
        //    List<Vector3d> listOfVectors = new List<Vector3d>();
            
        //    for (ushort x = 0; x < width; ++x)
        //    {
        //        for (ushort y = 0; y < height; ++y)
        //        {

        //            int depthIndex = (y * width) + x;
        //            ushort z = arrayDepth[depthIndex];

        //            if (z != 0)
        //            {
        //                listOfVectors.Add(new Vector3d(Convert.ToSingle(x), Convert.ToSingle(y), Convert.ToSingle(z)));
        //            }
        //        }
        //    }

        //    return listOfVectors;
        //}
        public static List<double[]> ToListdoubles(PointCloudVertices pointCloud)
        {
            List<double[]> a = new List<double[]>();

            for (int i = 0; i < pointCloud.Count; ++i)
            {
                Vector3d v = pointCloud[i].Vector;
                double[] d = new double[3]{v.X, v.Y, v.Z};
                a.Add(d);
            }

            return a;
        }
        public static double[][] TodoubleArray(PointCloudVertices pointCloud)
        {
           double[][] a = new double[pointCloud.Count][];

            for (int i = 0; i < pointCloud.Count; ++i)
            {
                Vector3d v = pointCloud[i].Vector;
                double[] d = new double[3]{v.X, v.Y, v.Z};
                a[i] = d;
            }

            return a;
        }
        public static List<Vector3d> VectorsFromPoint3D(List<Point3D> listPoints)
        {
            List<Vector3d> listOfVectors = new List<Vector3d>();
            for (int i = 0; i < listPoints.Count; i++)
            {
                Point3D myPoint = listPoints[i];

                listOfVectors.Add(new Vector3d(Convert.ToSingle(myPoint.X), Convert.ToSingle(myPoint.Y), Convert.ToSingle(myPoint.Z)));
            }


            return listOfVectors;
        }



        public PointCloudVertices Clone()
        {
            return CloneVertices(this);
            
        }

      
        public static List<Vector3d> CloneVectors(List<Vector3d> myOldList)
        {
            List<Vector3d> myListNew = new List<Vector3d>();
            for (int i = 0; i < myOldList.Count; i++)
            {
                Vector3d v = myOldList[i];
                Vector3d vNew = new Vector3d(v);
                myListNew.Add(vNew);
            }
            return myListNew;
        }
        public static PointCloudVertices CloneVertices(PointCloudVertices myOldList)
        {
            PointCloudVertices myListNew = new PointCloudVertices();
            for (int i = 0; i < myOldList.Count; i++)
            {
                Vertex v = myOldList[i];
                Vertex vNew = new Vertex(v.IndexInCloud, v.Vector);
                if (v.Color != default(System.Drawing.Color))
                    vNew.Color = v.Color;
                myListNew.Add(vNew);
            }

            myListNew.ShowLinesConnectingPoints = myOldList.ShowLinesConnectingPoints;
            return myListNew;
        }


        public static void ScaleByVertex(PointCloudVertices vectorlList, Vertex v)
        {


            Matrix3d R = Matrix3d.Identity;
            R[0, 0] = v.Vector.X;
            R[1, 1] = v.Vector.Y;
            R[2, 2] = v.Vector.Z;

            PointCloudVertices.RotateVertices(vectorlList, R);

        }
        public static void ScaleByVector(PointCloudVertices vectorlList, Vector3d v)
        {


            Matrix3d R = Matrix3d.Identity;
            R[0, 0] = v.X;
            R[1, 1] = v.Y;
            R[2, 2] = v.Z;

            PointCloudVertices.RotateVertices(vectorlList, R);

        }
        public static void ScaleByFactor(PointCloudVertices vectorlList, double scale)
        {
            Vector3d scaleVector = new Vector3d(scale, scale, scale);

            Matrix3d R = Matrix3d.Identity;
            R[0, 0] = scaleVector[0];
            R[1, 1] = scaleVector[1];
            R[2, 2] = scaleVector[2];

            PointCloudVertices.RotateVertices(vectorlList, R);

        }
        //public static void ScaleByFactor(PointCloudVertices vectorlList, double scale)
        //{
        //    Vector3d scaleVector = new Vector3d(scale, scale, scale);

        //    Matrix3d R = Matrix3d.Identity;
        //    R[0, 0] = scaleVector[0];
        //    R[1, 1] = scaleVector[1];
        //    R[2, 2] = scaleVector[2];

        //    PointCloudVertices.RotateVertices(vectorlList, R);

        //}
        public static void Rotate(PointCloudVertices pointCloud, Matrix3d R)
        {
            List<Vector3d> listVectors = PointCloudVertices.ToVectors(pointCloud);

            PointCloudVertices.RotateVectors(listVectors, R);
            PointCloudVertices.AssignNewVectorList(pointCloud, listVectors);


        }
        /// <summary>
        /// x,y and z are the angles in degrees
        /// </summary>
        /// <param name="pointCloud"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void RotateDegrees(PointCloudVertices pointCloud, double x, double y, double z)
        {
            Matrix3d R = new Matrix3d();
            R = R.RotationXYZDegrees(x, y, z);
            PointCloudVertices.RotateVertices(pointCloud, R);

        }
         public static void RotateRadiants(PointCloudVertices pointCloud, double x, double y, double z)
        {
            Matrix3d R = new Matrix3d();
            //R = R.RotationXYZDegrees(x, y, z);
            R = R.RotationXYZRadiants(x, y, z);

            PointCloudVertices.RotateVertices(pointCloud, R);

        }
     
        
        public static void Translate(PointCloudVertices pointCloud, double x, double y, double z)
        {
            Vector3d translation = new Vector3d(x, y, z);

            for (int i = 0; i < pointCloud.Count; i++)
            {
                
                Vertex v = pointCloud[i];
                Vector3d translatedV = Vector3d.Add(v.Vector, translation);
                v.Vector = translatedV;
                pointCloud[i] = v;
            }


        }
        public static void InhomogenousTransform(PointCloudVertices vectorlList, double d)
        {
            //Vector3d scaleVector = new Vector3d(x, y, z);
            for (int i = 0; i < vectorlList.Count; i++)
            {
                Vertex v = vectorlList[i];
                v.Vector.X = v.Vector.X - v.Vector.Z / d;
                v.Vector.Y = v.Vector.Y - v.Vector.Z / d;
                //v.Vector.Z = d;
                vectorlList[i] = v;
            }

        }

        public static void RotateVertices30Degrees(PointCloudVertices vectorlList)
        {
            Matrix3d R = Matrix3d.Identity;
            //rotation 30 degrees
            R[0, 0] = 1F;
            R[1, 1] = R[2, 2] = 0.86603F;
            R[1, 2] = -0.5F;
            R[2, 1] = 0.5F;


            PointCloudVertices.RotateVertices(vectorlList, R);


        }

        public static void CreateOutliers(PointCloudVertices pointCloud, int numberOfOutliers)
        {
            int indexInModel = pointCloud.Count - 1;
            int numberIterate = 0;
            for (int i = pointCloud.Count - 1; i >= 0; i--)
            {
                //Vector3d p = vectors[i].Vector;
                Vector3d perturb = new Vector3d(pointCloud[i].Vector);

                numberIterate++;
                if (numberIterate > numberOfOutliers)
                    return;


                if (i % 3 == 0)
                {
                    perturb.X *= 1.2f; perturb.Y *= 1.3f; perturb.Z *= 1.05f;
                }
                else if (i % 3 == 1)
                {
                    perturb.X *= 1.4f; perturb.Y *= 0.9f; perturb.Z *= 1.2f;
                }
                else
                {
                    perturb.X *= 0.9f; perturb.Y *= 1.2f; perturb.Z *= 1.1f;
                }
                indexInModel++;
                pointCloud.Add(new Vertex(indexInModel, perturb));

            }



        }
        public static void PerturbVectors(List<Vector3d> vectors)
        {

            for (int i = 0; i < vectors.Count; i++)
            {
                Vector3d p = vectors[i];


                Vector3d perturb = new Vector3d();
                if (i % 3 == 0)
                {
                    perturb.X = 2; perturb.Y = 0; perturb.Z = 1;
                }
                else if (i % 3 == 1)
                {
                    perturb.X = 0; perturb.Y = 2; perturb.Z = 2;
                }
                else
                {
                    perturb.X = 1; perturb.Y = 3; perturb.Z = 4;
                }
                p = Vector3d.Add(p, perturb);


                vectors[i] = p;

            }

        }


        public static void Shuffle(PointCloudVertices pointCloud)
        {

            IList<Vertex> lNew = pointCloud as IList<Vertex>;
            lNew.Shuffle();
            for(int i = 0; i < lNew.Count; i++)
            {
                lNew[i].IndexInCloud = i;
            }


        }
        private static void ShuffleAtIndex(PointCloudVertices pointCloud, int i, int j)
        {
            Vertex v = pointCloud[i];
            pointCloud[i] = pointCloud[j];
            pointCloud[j] = v;

        }
        public static void ShuffleTest(PointCloudVertices pointCloud)
        {
            ShuffleAtIndex(pointCloud, 0, pointCloud.Count - 2);
            ShuffleAtIndex(pointCloud, 1, pointCloud.Count - 5);
            ShuffleAtIndex(pointCloud, 3, pointCloud.Count - 1);
            ShuffleAtIndex(pointCloud, 4, pointCloud.Count - 3);


        }
        public static void ShuffleRandom(PointCloudVertices pointCloud)
        {
            IList<Vertex> lNew = pointCloud as IList<Vertex>;
            lNew.Shuffle();
        }

        //public static double[] SplitVertices(PointCloud listVectors, ref byte[] myColorPixels, int width, int height)
        //{
        //    double[] pointArray = new double[width * height];

        //    int BYTES_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
        //    myColorPixels = new byte[width * height * BYTES_PER_PIXEL];

        //    for (int i = 0; i < listVectors.Count; i++)
        //    {
        //        int x = Convert.ToInt32(listVectors[i].Vector.X);
        //        int y = Convert.ToInt32(listVectors[i].Vector.Y);
        //        int depthIndex = (y * width) + x;
        //        Vertex p3D = listVectors[i];
        //        pointArray[depthIndex] = listVectors[i].Vector.Z;

        //        myColorPixels[depthIndex + 0] = Convert.ToByte(listVectors[i].Color[0] * 255); //colorInfoR[x, y];
        //        myColorPixels[depthIndex + 1] = Convert.ToByte(listVectors[i].Color[1] * 255);
        //        myColorPixels[depthIndex + 2] = Convert.ToByte(listVectors[i].Color[2] * 255);
        //        myColorPixels[depthIndex + 3] = Convert.ToByte(listVectors[i].Color[3] * 255);

        //        //colorArray2D[Convert.ToInt32(listVectors[i].Vector.X), Convert.ToInt32(listVectors[i].Vector.Y)] = Convert.ToUInt16(listVectors[i].Vector.Z);

        //    }



        //    return pointArray;


        //}
        //public static ushort[] ToUshortMatrix(PointCloud listVectors, ref byte[] myColorPixels, int width, int height)
        //{
        //    ushort[] pointArray = new ushort[width * height];

        //    int BYTES_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
        //    myColorPixels = new byte[width * height * BYTES_PER_PIXEL];

        //    for (int i = 0; i < listVectors.Count; i++)
        //    {
        //        int x = Convert.ToInt32(listVectors[i].Vector.X);
        //        int y = Convert.ToInt32(listVectors[i].Vector.Y);
        //        int depthIndex = (y * width) + x;
        //        Vertex p3D = listVectors[i];
        //        pointArray[depthIndex] = Convert.ToUInt16(listVectors[i].Vector.Z);

        //        myColorPixels[depthIndex + 0] = Convert.ToByte(listVectors[i].Color[0] * 255); //colorInfoR[x, y];
        //        myColorPixels[depthIndex + 1] = Convert.ToByte(listVectors[i].Color[1] * 255);
        //        myColorPixels[depthIndex + 2] = Convert.ToByte(listVectors[i].Color[2] * 255);
        //        myColorPixels[depthIndex + 3] = Convert.ToByte(listVectors[i].Color[3] * 255);

        //        //colorArray2D[Convert.ToInt32(listVectors[i].Vector.X), Convert.ToInt32(listVectors[i].Vector.Y)] = Convert.ToUInt16(listVectors[i].Vector.Z);

        //    }

        //    return pointArray;


        //}

        public static void RotateVectors(List<Vector3d> vectors, Matrix3d R)
        {
            for (int i = 0; i < vectors.Count; i++)
            {
                Vector3d v = vectors[i];
                vectors[i] = R.MultiplyVector(v);
                // Vector3d v1 = Multiply3x3(R, v);

            }
        }
        public static void RotateVertices(PointCloudVertices vectors, Matrix3d R)
        {
            for (int i = 0; i < vectors.Count; i++)
            {
                Vertex v = vectors[i];
                vectors[i].Vector = R.MultiplyVector(v.Vector);

            }
        }
        public static float MeanDistance(PointCloudVertices a, PointCloudVertices b)
        {
            //can have different point sizes
            int numberOfPoints = Math.Min(a.Count, b.Count);
            float totaldist = 0;
            for (int i = 0; i < numberOfPoints; i++)
            {
                Vertex p1 = a[i];
                Vertex p2 = b[i];
                
                float dist = Convert.ToSingle( (Vector3d.Subtract(p1.Vector, p2.Vector)).Length);

                totaldist += dist;

            }

            float meanDistance = totaldist / Convert.ToSingle(numberOfPoints);
            return meanDistance;


        }
        public static void RemoveEntriesByIndices(ref PointCloudVertices pointsSource, ref PointCloudVertices pointsTarget, List<int> indices)
        {
            PointCloudVertices temp1 = new PointCloudVertices();
            PointCloudVertices temp2 = new PointCloudVertices();

            //temp.ShallowCopy(this.PointsTarget.GetPoints());

            indices.Sort();
            int indexNew = -1;
            for (int iPoint = (pointsTarget.Count - 1); iPoint >= 0; iPoint--)
            {
                Vertex point1 = pointsTarget[iPoint];
                Vertex point2 = pointsSource[iPoint];
                bool bfound = false;
                for (int i = (indices.Count - 1); i >= 0; i--)
                {
                    if (indices[i] == iPoint)
                    {
                        bfound = true;
                        break;
                    }
                }
                if (!bfound)
                {
                    indexNew++;
                    temp1.Add(point1);
                    temp2.Add(point2);

                }
            }
            pointsTarget = temp1;
            pointsSource = temp2;

        }
        public static void RemoveVector3d(ref List<Vector3d> pointsTarget, ref List<Vector3d> pointsSource, List<int> indices)
        {
            List<Vector3d> temp1 = new List<Vector3d>();
            List<Vector3d> temp2 = new List<Vector3d>();

            //temp.ShallowCopy(this.PointsTarget.GetPoints());

            indices.Sort();
            int indexNew = -1;
            for (int iPoint = (pointsTarget.Count - 1); iPoint >= 0; iPoint--)
            {
                Vector3d point1 = pointsTarget[iPoint];
                Vector3d point2 = pointsSource[iPoint];
                bool bfound = false;
                for (int i = (indices.Count - 1); i >= 0; i--)
                {
                    if (indices[i] == iPoint)
                    {
                        bfound = true;
                        break;
                    }
                }
                if (!bfound)
                {
                    indexNew++;
                    temp1.Add(point1);
                    temp2.Add(point2);

                }
            }
            pointsTarget = temp1;
            pointsSource = temp2;

        }
        public void ResizeVerticesTo1()
        {
            ResetCentroid(this, false);

            
            this.CalculateBoundingBox();
            Vector3d vDiff = this.boundingBoxMax.Vector - boundingBoxMin.Vector;

            double scale = Math.Max(boundingBoxMax.Vector.X, boundingBoxMax.Vector.Y);
            scale = Math.Max(scale, boundingBoxMax.Vector.Z);



            for (int i = 0; i < this.Count; i++)
            {
                // int x = centroid.X + (int)((point.X - centroid.X) * scale) );
                this[i].Vector.X /= scale;
                this[i].Vector.Y /= scale;
                this[i].Vector.Z /= scale;
            }



        }

        public Vector3d CentroidVectorGet
        {
            get
            {
                if (centroidVertex == null)
                    centroidVertex = this.CalculateCentroid();
                return centroidVertex.Vector;

            }

        }
        public Vertex CentroidVertexGet
        {
            get
            {
                if (centroidVertex == null)
                    centroidVertex = this.CalculateCentroid();
                return centroidVertex;

            }

        }
        public Vertex BoundingBoxMax
        {
            get
            {
                if (boundingBoxMax == null)
                    this.CalculateBoundingBox();
                return boundingBoxMax;
            }

        }
        public float BoundingBoxMaxFloat
        {
            get
            {
                Vertex v = this.BoundingBoxMax;

                if (BoundingBoxMax != null)
                {
                    double f = float.MinValue;
                    for (int i = 0; i < 2; i++)
                    {
                        f = System.Math.Max(f, System.Math.Abs(this.boundingBoxMin.Vector[i]));
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        f = System.Math.Max(f, System.Math.Abs(boundingBoxMax.Vector[i]));
                    }

                    return Convert.ToSingle(f);
                }
                return 0f;
            }

        }
        public Vertex BoundingBoxMin
        {
            get
            {
                if (boundingBoxMin == null)
                    this.CalculateBoundingBox();
                return boundingBoxMin;
            }

        }
    
   
        public void LinesGeometricalModels_Reset()
        {

            this.LinesGeometricalModels = new List<LineD>();

            for (int i = 0; i < this.Count; i++)
            {

                for (int j = 0; j < this.Count; j++)
                {
                    if (i != j)
                    {
                        //OpenTK.Vector3d sub = OpenTK.Vector3d.Subtract(myVertex[i].Vector, myVertex[j].Vector);
                        double dist = this[i].Vector.Distance(this[j].Vector);
                        if (dist <= LineLengthGeometricModel)
                        //if (dist == cubeSize)
                        {
                            LineD myLine = new LineD(this[i].Vector, this[j].Vector, Color);
                            LinesGeometricalModels.Add(myLine);
                        }
                    }

                }

            }
        }
        public void LinesGeometricalModels_Add(double lineLengthMax, System.Drawing.Color myColor)
        {
            this.Color = myColor;
            LineLengthGeometricModel = lineLengthMax;
            LinesGeometricalModels_Reset();
           
        }
      
        public static bool CheckCloudAbs(PointCloudVertices myPCLTarget, PointCloudVertices myPCLResult, double threshold)
        {

            PointCloudVertices pt = PointCloudVertices.CopyVertices(myPCLTarget);
            PointCloudVertices pr = PointCloudVertices.CopyVertices(myPCLResult);
            for(int i = 0; i < myPCLTarget.Count; i++)
            {
                pt[i].Vector.X = Math.Abs(pt[i].Vector.X);
                pt[i].Vector.Y = Math.Abs(pt[i].Vector.Y);
                pt[i].Vector.Z = Math.Abs(pt[i].Vector.Z);

                pr[i].Vector.X = Math.Abs(pr[i].Vector.X);
                pr[i].Vector.Y = Math.Abs(pr[i].Vector.Y);
                pr[i].Vector.Z = Math.Abs(pr[i].Vector.Z);

            }
            return CheckCloud( pt,  pr, threshold);


        }
        public static bool CheckCloud(PointCloudVertices myPCLTarget, PointCloudVertices myPCLResult, double threshold)
        {
            if (myPCLResult == null || myPCLTarget == null)
                return false;

            double diffMax = double.MinValue;
            for (int i = 0; i < myPCLTarget.Count; i++)
            {
                double dx = Math.Abs(myPCLTarget[i].Vector.X - myPCLResult[i].Vector.X);
                double dy = Math.Abs(myPCLTarget[i].Vector.Y - myPCLResult[i].Vector.Y);
                double dz = Math.Abs(myPCLTarget[i].Vector.Z - myPCLResult[i].Vector.Z);
                if (dx > diffMax)
                    diffMax = dx;
                if (dy > diffMax)
                    diffMax = dy;
                if (dz > diffMax)
                    diffMax = dz;
                if (double.IsNaN(dx) || double.IsNaN(dy) || double.IsNaN(dz))
                    return false;
                if (dx > threshold || dy > threshold || dz > threshold)
                {
                    System.Diagnostics.Debug.WriteLine("Check result - is : " + dx.ToString() + " : " + dy.ToString() + " : " + dz.ToString() + " : " + "--- Should be: " + threshold);
                    return false;
                }
                //needs a lot of exection time - only for error cases  
                //Vector3d v = new Vector3d(dx, dy, dz);
                //Debug.WriteLine(i.ToString() + "Vector is OK, distance difference is: " + v.Length.ToString());

            }
            System.Diagnostics.Debug.WriteLine("---");
            System.Diagnostics.Debug.WriteLine("Check Cloud, difference: " + diffMax.ToString("G") + " :  allowed: " + threshold.ToString("G"));
            return true;
        }
        /// <summary>
        /// used by the Scanner project mainly
        /// </summary>
        /// <param name="vectors"></param>
        /// <param name="colorInfo"></param>
        /// <returns></returns>
        public static PointCloudVertices FromVectorsColors(List<Vector3d> vectors, List<System.Drawing.Color> colorInfo)
        {
            PointCloudVertices myVertexList = new PointCloudVertices();
            if (colorInfo != null)
            {
                for (int i = 0; i < vectors.Count; i++)
                {

                    Vertex vertex = new Vertex(i, vectors[i], colorInfo[i]);

                    myVertexList.Add(vertex);

                }

            }
            else
            {
                for (int i = 0; i < vectors.Count; i++)
                {
                    Vertex vertex = new Vertex();
                    vertex.Vector = vectors[i];
                    myVertexList.Add(vertex);

                }
            }


            return myVertexList;
        }
        //public static PointCloud FromDepthColors(byte[] mycolorInfo, ushort[] depthInfo, int width, int height)
        //{

        //    List<Vector3d> myVectors = PointCloud.Vector3dDList_FromArray(depthInfo, width, height);
        //    List<double[]> myColors = PointCloudUtils.CreateColorInfo(mycolorInfo, depthInfo, width, height);

        //    return FromVectorsColors(myVectors, myColors);



        //}
        //public static PointCloud FromDepth(ushort[] depthInfo, int width, int height)
        //{
        //    List<Vector3d> myVectors = PointCloud.Vector3dDList_FromArray(depthInfo, width, height);
        //    PointCloud pc = PointCloud.FromVector3List(myVectors);

        //    return pc;
            

        //}


        public static ushort[] Ushort1DFrom2D(ushort[,] pointMatrix, int width, int height)
        {
            ushort[] pointArray = new ushort[width * height];

            //int nIndex = -1;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int depthIndex = (y * width) + x;
                    pointArray[depthIndex] = pointMatrix[x, y];

                }


            }
            return pointArray;
            
        }

        public static ushort[,] ToUshortArray(PointCloudVertices pointCloud, int width, int height)
        {
            double xMin = pointCloud.BoundingBoxMin.Vector.X;
            double yMin = pointCloud.BoundingBoxMin.Vector.Y;

            double dx = pointCloud.BoundingBoxMax.Vector.X - pointCloud.BoundingBoxMin.Vector.X;
            double dy = pointCloud.BoundingBoxMax.Vector.Y - pointCloud.BoundingBoxMin.Vector.Y;

            ushort[,] points = new ushort[width, height];
            Vector3d p3D = new Vector3d();
            try
            {
                for (int i = 0; i < pointCloud.Count; i++)
                {
                    p3D = pointCloud[i].Vector;
                    int xInt = Convert.ToInt32((p3D.X - xMin) * width / dx);
                    int yInt = Convert.ToInt32((p3D.Y - yMin) * width / dy);
                    if (xInt < width && yInt < height)
                    {
                        //points[xInt, yInt] = Convert.ToUInt16(p3D.Z * 1000);
                        points[xInt, yInt] = Convert.ToUInt16(p3D.Z);
                    }

                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Error at: " + p3D.ToString());

            }
            return points;


        }

        public static ushort[] ToUshort1Dim(PointCloudVertices pc, int width, int height)
        {

            ushort[,] points = ToUshortArray(pc, width, height);
            ushort[] pointArr = Ushort1DFrom2D(points, width, height);
            return pointArr;


        }
        //}
        /// <summary>
        /// creates color info for all DEPTH pixels (to later e.g. write ply file from it)
        /// </summary>
        /// <param name="myColorMetaData"></param>
        /// <param name="myDepthMetaData"></param>
        /// <param name="myCoordinateMapper"></param>
        /// <returns></returns>
        public static byte[] ToColorInfo(PointCloudVertices pointCloud, int width, int height)
        {
            byte[] colorPixels = new byte[width * height * 4];
            int xInt = 0;
            int yInt = 0;
            int zInt = 0;
            int depthIndex = 0;

            
            try
            {
                //out ushort[] depthPixels = PointCloud.ToUshort1Dim(pc, width, height);
                double xMin = pointCloud.BoundingBoxMin.Vector.X;
                double yMin = pointCloud.BoundingBoxMin.Vector.Y;
                double dx = pointCloud.BoundingBoxMax.Vector.X - pointCloud.BoundingBoxMin.Vector.X;
                double dy = pointCloud.BoundingBoxMax.Vector.Y - pointCloud.BoundingBoxMin.Vector.Y;

               
                //init to black background
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        depthIndex = (y * width) + x;
                        //depthIndex = ((height - y - 1) * width) + x;

                        int colorIndex = depthIndex * 4;
                        colorPixels[colorIndex] = (byte)0;
                        colorPixels[colorIndex + 1] = (byte)0;
                        colorPixels[colorIndex + 2] = (byte)0;
                        colorPixels[colorIndex + 3] = (byte)255;
                    }
                }

                //-------------------------------------
                for (int i = 0; i < pointCloud.Count; i++)
                {
                    Vector3d v = pointCloud[i].Vector;
                    

                    xInt = Convert.ToInt32((v.X - xMin) * width / dx);
                    yInt = Convert.ToInt32((v.Y - yMin) * height / dy);
                    zInt = Convert.ToInt32(v.Z);

                    //exclude rounding errors
                    if (xInt < width && yInt < height)
                    {

                        //rotate the cloud to 180 degrees, so that display as image is OK
                        //depthIndex = (yInt * width) + xInt;
                        depthIndex = ((height - yInt - 1) * width) + xInt;
                       
                        int colorIndex = depthIndex * 4;
                        colorPixels[colorIndex] = pointCloud[i].Color.R;
                        colorPixels[colorIndex + 1] = pointCloud[i].Color.G;
                        colorPixels[colorIndex + 2] = pointCloud[i].Color.B;
                        colorPixels[colorIndex + 3] = pointCloud[i].Color.A;

                        //colorPixels[colorIndex] = Convert.ToByte(color[0] * 255);
                        //colorPixels[colorIndex + 1] = Convert.ToByte(color[1] * 255);
                        //colorPixels[colorIndex + 2] = Convert.ToByte(color[2] * 255);
                        //colorPixels[colorIndex + 3] = Convert.ToByte(color[3] * 255);
                    }
                }

            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Error setting colorInfo : " + xInt.ToString() + " : " + yInt.ToString() + " : " + zInt.ToString() + " : " + depthIndex.ToString() + " : ");

            }
            return colorPixels;

        }
        public PointCloud ToPointCloud()
        {
            PointCloud pcgl = new PointCloud();
            pcgl.Vectors = new Vector3[this.Count];
            pcgl.Colors = new Vector3[this.Count];
            pcgl.Indices = new uint[this.Count];
            pcgl.FileNameLong = this.FileNameShort;
            pcgl.Path = this.Path;

            int i = 0;
            try
            {
                for (i = 0; i < this.Count; i++)
                {
                    System.Drawing.Color c = this[i].Color;
                    Vector3 v = this[i].Vector.ToVector();

                    pcgl.Vectors[i] = v;
                    pcgl.Colors[i] = new Vector3(c.R / 255f, c.G / 255f, c.B / 255f);
                    pcgl.Indices[i] = Convert.ToUInt32(i);

                }

                
            }

            catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error in ToPointCloudGL at : " + i.ToString() + " : " + err.Message);
            }
            return pcgl;

        }
        public static List<Vector3d> RotatePointCloudXY(List<Vector3d> oldList, int Width, int Height)
        {

            List<Vector3d> listOfVectors = new List<Vector3d>();


            for (int i = 0; i < oldList.Count; i++)
            {
                Vector3d v = oldList[i];

                double newX = Width - v.X;
                //double newX = Convert.ToSingle(p.GetValue(0));
                double newY = Height - v.Y;

                listOfVectors.Add(new Vector3d(newX, newY, v.Z));

            }

            return listOfVectors;

        }
        public static List<Point3D> CreatePoint3DListFromVertices(PointCloudVertices myListVertices)
        {
            List<Point3D> myListPoint3D = new List<Point3D>();
            for (int i = 0; i < myListVertices.Count; i++)
            {
                Vertex myVertex = myListVertices[i];
                Point3D p3D = new Point3D(myVertex.Vector.X, myVertex.Vector.Y, myVertex.Vector.Z);
                myListPoint3D.Add(p3D);
            }
            return myListPoint3D;

        }
        public static PointCloudVertices FromPoints2d(List<System.Drawing.Point> pointList, PointCloudVertices pointsTarget, List<System.Drawing.Point> pointOther)
        {

            PointCloudVertices pointNew = new PointCloudVertices();
            bool pointFound = false;

            for (int i = pointList.Count - 1; i >= 0; i--)
            {
                System.Drawing.Point pNew = pointList[i];
                for (int j = 0; j < pointsTarget.Count; j++)
                {
                    Vertex p = pointsTarget[j];
                    //add point only if it is found in the original point list
                    if (pNew.X == Convert.ToInt32(p.Vector[0]) && pNew.Y == Convert.ToInt32(p.Vector[1]))
                    {
                        pointFound = true;
                        pointNew.Add(p);
                        break;
                    }

                }
                //some error - have to check!
                if (!pointFound)
                {
                    System.Windows.Forms.MessageBox.Show("Error in identifying point from cloud with the stitched result: " + i.ToString());
                    pointOther.RemoveAt(i);
                }



            }
            return pointNew;

        }
        public static Matrix3d CovarianceMatrix(List<Vector3d> a, bool normalsCovariance)
        {
            //consists of elements
            //axbx axby axbz
            //aybx ayby aybz
            //azbx azby azbz
            Matrix3d H = new Matrix3d();
            for (int i = 0; i < a.Count; i++)
            {

                H[0, 0] += a[i].X * a[i].X;
                H[0, 1] += a[i].X * a[i].Y;
                H[0, 2] += a[i].X * a[i].Z;

                H[1, 0] += a[i].Y * a[i].X;
                H[1, 1] += a[i].Y * a[i].Y;
                H[1, 2] += a[i].Y * a[i].Z;

                H[2, 0] += a[i].Z * a[i].X;
                H[2, 1] += a[i].Z * a[i].Y;
                H[2, 2] += a[i].Z * a[i].Z;


            }
            H.Transpose();
            if (!normalsCovariance)
                H = H.MultiplyScalar(1f / a.Count);
            return H;
        }
        //public static Matrix3d CovarianceMatrix(PointCloudVertices a, bool normalsCovariance)
        //{
        //    //consists of elements
        //    //axbx axby axbz
        //    //aybx ayby aybz
        //    //azbx azby azbz

        //    Matrix3d H = new Matrix3d();
        //    for (int i = 0; i < a.Count; i++)
        //    {

        //        H[0, 0] += a[i].Vector.X * a[i].Vector.X;
        //        H[0, 1] += a[i].Vector.X * a[i].Vector.Y;
        //        H[0, 2] += a[i].Vector.X * a[i].Vector.Z;

        //        H[1, 0] += a[i].Vector.Y * a[i].Vector.X;
        //        H[1, 1] += a[i].Vector.Y * a[i].Vector.Y;
        //        H[1, 2] += a[i].Vector.Y * a[i].Vector.Z;

        //        H[2, 0] += a[i].Vector.Z * a[i].Vector.X;
        //        H[2, 1] += a[i].Vector.Z * a[i].Vector.Y;
        //        H[2, 2] += a[i].Vector.Z * a[i].Vector.Z;


        //    }
        //    H.Transpose();
        //    if (!normalsCovariance)
        //        H = H.MultiplyScalar(1.0f / a.Count);
        //    return H;
        //}
  
        public static Matrix3d CorrelationMatrix(List<Vector3d> a, List<Vector3d> b)
        {
            //consists of elements
            //axbx axby axbz
            //aybx ayby aybz
            //azbx azby azbz
            int maxNumber = b.Count;
            if (a.Count < maxNumber)
                maxNumber = a.Count;
            Matrix3d H = new Matrix3d();
            for (int i = 0; i < maxNumber; i++)
            {

                H[0, 0] += b[i].X * a[i].X;
                H[0, 1] += b[i].X * a[i].Y;
                H[0, 2] += b[i].X * a[i].Z;

                H[1, 0] += b[i].Y * a[i].X;
                H[1, 1] += b[i].Y * a[i].Y;
                H[1, 2] += b[i].Y * a[i].Z;

                H[2, 0] += b[i].Z * a[i].X;
                H[2, 1] += b[i].Z * a[i].Y;
                H[2, 2] += b[i].Z * a[i].Z;


            }
            H = H.MultiplyScalar(1.0f / maxNumber);
            return H;
        }
   
        public void Save(string path, string fileName)
        {
            UtilsPointCloudIO.ToObjFile(this, path, fileName);

        }
  
        public void AddPointCloud(PointCloudVertices pcToAdd)
        {
            for (int i = 0; i < pcToAdd.Count; i++)
            {
                this.Add(pcToAdd[i]);

            }
        }

    }
   
    
}
