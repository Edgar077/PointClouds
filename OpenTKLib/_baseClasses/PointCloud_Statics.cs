using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Globalization;
using System.Drawing;

namespace OpenTKExtension
{

    public partial class PointCloud 
    {
        public static Vector3 ResetCentroid(PointCloud pc, bool centered)
        {
            return pc.ResetCentroid(centered);
        }
        public static void CreateOutliers(PointCloud pointCloud, int numberOfOutliers)
        {
            int indexInModel = pointCloud.Vectors.Length - 1;
            int numberIterate = 0;
            List<Vector3> listV = new List<Vector3>(pointCloud.Vectors);
            for (int i = pointCloud.Vectors.Length - 1; i >= 0; i--)
            {
                //Vector3 p = vectors[i].Vector;
                Vector3 perturb = new Vector3(pointCloud.Vectors[i]);

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
                listV.Add(perturb);

            }

            pointCloud.Vectors = listV.ToArray();


        }

       
     
        /// <summary>
        /// creates color info for all DEPTH pixels (to later e.g. write ply file from it) - needed also for image creation
        /// </summary>
        /// <param name="myColorMetaData"></param>
        /// <param name="myDepthMetaData"></param>
        /// <param name="myCoordinateMapper"></param>
        /// <returns></returns>
        public static byte[] ToColorArrayBytes(PointCloud pointCloud, int width, int height)
        {
            byte[] colorPixels = new byte[width * height * 4];
            int xInt = 0;
            int yInt = 0;
            int zInt = 0;
            int depthIndex = 0;


            try
            {
                ////out ushort[] depthPixels = PointCloud.ToUshort1Dim(pc, width, height);
                float xMin = pointCloud.BoundingBoxMin.X;
                float yMin = pointCloud.BoundingBoxMin.Y;
                float dx = pointCloud.BoundingBoxMax.X - pointCloud.BoundingBoxMin.X;
                float dy = pointCloud.BoundingBoxMax.Y - pointCloud.BoundingBoxMin.Y;


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

                // set color -------------------------------------
                for (int i = 0; i < pointCloud.Count; i++)
                {
                    Vector3 v = pointCloud.Vectors[i];
                    Vector3 color = pointCloud.Colors[i];


                    xInt = Convert.ToInt32((v.X - xMin) * width / dx);
                    yInt = Convert.ToInt32((v.Y - yMin) * height / dy);
                    zInt = Convert.ToInt32(v.Z);

                    //exclude rounding errors
                    if (xInt < width && yInt < height)
                    {
                        //rotate the cloud to 180 degrees, so that display as image is OK
                        depthIndex = ((height - yInt - 1) * width) + xInt;

                        int colorIndex = depthIndex * 4;
                        colorPixels[colorIndex] = Convert.ToByte(color.X * 255);
                        colorPixels[colorIndex + 1] = Convert.ToByte(color.Y * 255);
                        colorPixels[colorIndex + 2] = Convert.ToByte(color.Z * 255);

                        colorPixels[colorIndex + 3] = 255;

                    }
                }

            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Error in ToColorArrayBytes: " + xInt.ToString() + " : " + yInt.ToString() + " : " + zInt.ToString() + " : " + depthIndex.ToString() + " : ");

            }
            return colorPixels;

        }
    
        /// <summary>
        /// merge result and target points
        /// </summary>
        /// <param name="result"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static PointCloud CalculateMergedPoints_SimpleAdd(PointCloud result, PointCloud target, bool changeColorOfTarget)
        {


            if (result.Colors == null || result.Colors.Length != result.Vectors.Length)
                result.Colors = new Vector3[result.Vectors.Length];

            if (target.Colors == null || target.Colors.Length != target.Vectors.Length)
                target.Colors = new Vector3[target.Vectors.Length];


            List<Vector3> listV = result.ListVectors;
            List<Vector3> listC = result.ListColors;


            List<Vector3> listVTarget = new List<Vector3>(target.Vectors);
            List<Vector3> listCTarget = new List<Vector3>(target.Colors);
            //remove points target already there in result points

            if (changeColorOfTarget)
            {
                for (int i = 0; i < listCTarget.Count; i++)
                {
                    listCTarget[i] = new Vector3(0, 0, 1);
                }
            }


            //-----------------------
            listV.AddRange(listVTarget);
            listC.AddRange(listCTarget);
            //------------------------

            



            PointCloud pMerged = new PointCloud(listV, listC, null, null, null, null);
            return pMerged;


        }
        /// <summary>
        /// mergedPoints
        /// </summary>
        /// <param name="result"></param>
        /// <param name="pointsTarget"></param>
        /// <param name="kdTree"></param>
        /// <param name="meanDistance"></param>
        /// <returns></returns>
        public static PointCloud CalculateMergedPoints(PointCloud result, PointCloud pointsTarget, IKDTree kdTree, bool changeColorOfNewPoints, float threshold, out int pointsAdded)
        {
            if (result.Colors == null || result.Colors.Length != result.Vectors.Length)
                result.Colors = new Vector3[result.Vectors.Length];

            if (pointsTarget.Colors == null || pointsTarget.Colors.Length != pointsTarget.Vectors.Length)
                pointsTarget.Colors = new Vector3[pointsTarget.Vectors.Length];


            //search in tree
            

           // PointCloud resultKDTree = kdTree.FindClosestPointCloud_Parallel(result);
            KDTreeKennell kdTreeKennell = kdTree as KDTreeKennell;
            PointCloud pcToAdd = kdTreeKennell.RemoveDuplicates(result, threshold);
            pointsAdded = pcToAdd.Vectors.Length;
            System.Diagnostics.Debug.WriteLine("target points added : " + pcToAdd.Vectors.Length.ToString() + " - outof " + result.Vectors.Length.ToString());
            if (changeColorOfNewPoints)
                pcToAdd.SetColor(new Vector3(0, 1, 1));

            List<Vector3> listV = pointsTarget.ListVectors;
            List<Vector3> listC = pointsTarget.ListColors;
            listV.AddRange(pcToAdd.ListVectors);
            listC.AddRange(pcToAdd.ListColors);



            PointCloud pMerged = new PointCloud(listV, listC, null, null, null, null);
            return pMerged;


        }

        /// <summary>
        /// colors and indices are lost!!
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static PointCloud ResizeAndSort_XYZ(PointCloud pc)
        {
            pc.ResizeTo1();
            List<Vector3> listSource = pc.ListVectors;
            listSource.Sort(new Vector3_XYZ());

            PointCloud pcNew = PointCloud.FromListVector3(listSource);
            return pcNew;

        }
        /// <summary>
        /// colors and indices are lost!!
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static PointCloud ResizeAndSort_Distance(PointCloud pc)
        {
            pc.ResizeTo1();

            List<Vertex> vList = new List<Vertex>();
            if(pc.Colors == null || pc.Colors.Length != pc.Vectors.Length)
            {
                pc.SetDefaultColors();
            }
            for (int i = 0; i < pc.Vectors.Length; i++)
            {
                Vertex v = new Vertex(pc.Vectors[i]);
                v.Color = pc.Colors[i];
                vList.Add(v);

            }


            vList.Sort(new Vector_Length());

            PointCloud pcNew = PointCloud.FromListVertices(vList);
            return pcNew;

        }
        public static void AddVectorToAll(PointCloud pointCloud, Vector3 vToAdd)
        {

            for (int i = 0; i < pointCloud.Vectors.Length; i++)
            {

                Vector3 v = pointCloud.Vectors[i];
                Vector3 translatedV = Vector3.Add(v, vToAdd);
                v = translatedV;
                pointCloud.Vectors[i] = v;
            }

        }
        /// <summary>
        /// resets all vectors so that there are no negative values 
        /// </summary>
        /// <param name="pc"></param>
        public static void ResetToOriginAxis(PointCloud pc)
        {
            PointCloud.AddVectorToAll(pc, pc.BoundingBox.Min.Abs());

        }
        public static void SubtractVectorRef(PointCloud pointCloud, Vector3 centroid)
        {

            for (int i = 0; i < pointCloud.Vectors.Length; i++)
            {

                Vector3 v = pointCloud.Vectors[i];
                Vector3 translatedV = Vector3.Subtract(v, centroid);
                v = translatedV;
                pointCloud.Vectors[i] = v;
            }

        }
        public static PointCloud SubtractVector(PointCloud pointCloud, Vector3 centroid)
        {
            List<Vector3> list = new List<Vector3>();
            //List<Vector3> colors = new List<Vector3>();

            for (int i = 0; i < pointCloud.Vectors.Length; i++)
            {
                Vector3 v = pointCloud.Vectors[i];
                Vector3 translatedV = Vector3.Subtract(v, centroid);
                v = translatedV;
                list.Add(v);
            }
            PointCloud pc = PointCloud.FromListVector3(list);
            pc.Colors = pointCloud.Colors;
            pc.Indices = pointCloud.Indices;
            return pc;

        }


        public static float MeanDistance(PointCloud a, PointCloud b)
        {
            //can have different point sizes
            if (a.Vectors == null || b.Vectors == null)
                return float.MaxValue;

            int numberOfPoints = Math.Min(a.Vectors.Length, b.Vectors.Length);
            if (numberOfPoints == 0)
                return float.MaxValue;

            float totaldist = 0f;

            for (int i = 0; i < numberOfPoints; i++)
            {
                Vector3 p1 = a.Vectors[i];
                Vector3 p2 = b.Vectors[i];
                float dist = Vector3.Subtract(p1, p2).Length;
                totaldist += dist;
            }

            float meanDistance = totaldist / Convert.ToSingle(numberOfPoints);
            return meanDistance;

        }
        public static void RemoveEntriesByIndices(ref PointCloud pointsSource, ref PointCloud pointsTarget, List<int> indices)
        {

            List<Vector3> temp1 = new List<Vector3>();
            List<Vector3> temp2 = new List<Vector3>();

            //temp.ShallowCopy(this.PointsTarget.GetPoints());

            indices.Sort();
            int indexNew = -1;
            for (int iPoint = (pointsTarget.Count - 1); iPoint >= 0; iPoint--)
            {
                Vector3 point1 = pointsTarget.Vectors[iPoint];
                Vector3 point2 = pointsSource.Vectors[iPoint];
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


            pointsTarget = PointCloud.FromListVector3(temp1);
            pointsSource = PointCloud.FromListVector3(temp2);


        }
        public static void ScaleByFactor(PointCloud pc, float scale)
        {
            Vector3 scaleVector = new Vector3(scale, scale, scale);

            Matrix3 R = Matrix3.Identity;
            R[0, 0] = scaleVector[0];
            R[1, 1] = scaleVector[1];
            R[2, 2] = scaleVector[2];

            PointCloud.Rotate(pc, R);

        }


        public static void ScaleByVector(PointCloud pc, Vector3 v)
        {

            Matrix3 R = Matrix3.Identity;
            R[0, 0] = v.X;
            R[1, 1] = v.Y;
            R[2, 2] = v.Z;
            pc.Rotate(R);



        }
        public static void SetColorOfListTo(PointCloud pc, System.Drawing.Color color)
        {
            // float[] color = new float[4] { Convert.ToSingle(color[0]), Convert.ToSingle(color[1]), Convert.ToSingle(color[2]), 1f };
            pc.Colors = new Vector3[pc.Vectors.Length];
            float[] colorF = color.ToFloats();
            for (int i = 0; i < pc.Vectors.Length; i++)
            {



                pc.Colors[i] = new Vector3(colorF[0], colorF[1], colorF[2]);


            }

        }
        public static void SetColorToList(PointCloud pc, byte[] color)
        {
            // float[] color = new float[4] { Convert.ToSingle(color[0]), Convert.ToSingle(color[1]), Convert.ToSingle(color[2]), 1f };
            float[] colorF = new float[] { color[0] / 255, color[1] / 255, color[2] / 255 };

            pc.Colors = new Vector3[pc.Vectors.Length];

            for (int i = 0; i < pc.Vectors.Length; i++)
            {

                pc.Colors[i] = new Vector3(colorF[0], colorF[1], colorF[2]);


            }

        }
        public static void SetColorToList(PointCloud pc, List<System.Drawing.Color> colorList)
        {


            // float[] color = new float[4] { Convert.ToSingle(color[0]), Convert.ToSingle(color[1]), Convert.ToSingle(color[2]), 1f };
            pc.Colors = new Vector3[pc.Vectors.Length];

            for (int i = 0; i < pc.Vectors.Length; i++)
            {

                float[] colorF = colorList[i].ToFloats();

                pc.Colors[i] = new Vector3(colorF[0], colorF[1], colorF[2]);


            }


        }
        public static PointCloud ShiftByCenterOfMass(PointCloud pointsSource)
        {
            Vector3 Centroid = pointsSource.CentroidVectorRecalc;
            PointCloud pcNew = PointCloud.SubtractVector(pointsSource, Centroid);
            return pcNew;
        }
        public static PointCloud RemoveDuplicates(PointCloud pc)
        {
            List<Vector3> listV = new List<Vector3>(pc.Vectors);
            List<Vector3> listC = new List<Vector3>(pc.Colors);

           

            for (int i = (listV.Count - 1); i >= 0; i--)
            {
                Vector3 vi = listV[i];
                for (int j = 0; j < i; j++)
                {
                    Vector3 vj = listV[j];
                    if (vi.X == vj.X && vi.Y == vj.Y && vi.Z == vj.Z)
                    {
                        listV.RemoveAt(i);
                        break;

                    }
                }
            }
            PointCloud pcNew = new PointCloud();
            pcNew.Vectors = listV.ToArray();
            pcNew.Colors = listC.ToArray();
            int removedN = pc.Vectors.Length - pcNew.Vectors.Length;
            System.Diagnostics.Debug.WriteLine("Number of duplicates removed: " + removedN.ToString() + " - out of " + pc.Vectors.Length.ToString());

            return pcNew;
        }




    }
}
