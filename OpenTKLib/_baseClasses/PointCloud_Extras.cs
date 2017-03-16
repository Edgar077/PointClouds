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
        public PointCloud PCAAxes;
        public PointCloud PCAAxesNew;

       
        public static PointCloud CloneAll(PointCloud pc)
        {
            return pc.Clone();
        }


        //public IEnumerator<Vector3> GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}

        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}

        public static void RotateVectors(List<Vector3> vectors, Matrix3 R)
        {
            for (int i = 0; i < vectors.Count; i++)
            {
                Vector3 v = vectors[i];
                vectors[i] = R.MultiplyVector(v);
                // Vector3 v1 = Multiply3x3(R, v);

            }
        }
        public static void RotateVectors(PointCloud pc, Matrix3 R)
        {
            for (int i = 0; i < pc.Vectors.Length; i++)
            {
                Vector3 v = pc.Vectors[i];
                pc.Vectors[i] = R.MultiplyVector(v);
                // Vector3 v1 = Multiply3x3(R, v);

            }
        }
     
        /// <summary>
        /// x,y and z are the angles in degrees
        /// </summary>
        /// <param name="pointCloud"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void RotateDegrees(PointCloud pointCloud, float x, float y, float z)
        {
            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(x, y, z);
            PointCloud.RotateVectors(pointCloud, R);

        }
        public static void RotateRadiants(PointCloud pointCloud, float x, float y, float z)
        {
            Matrix3 R = new Matrix3();
            //R = R.RotationXYZDegrees(x, y, z);
            R = R.RotationXYZRadiants(x, y, z);

            PointCloud.RotateVectors(pointCloud, R);

        }
        public static void Rotate(PointCloud pointCloud, Matrix3 R)
        {
            pointCloud.Rotate(R);

            //List<Vector3> listVectors = new List<Vector3>(pointCloud.Vectors);

            //PointCloud.RotateVectors(listVectors, R);
            //PointCloud.AssignNewVectorList(pointCloud, listVectors);


        }

        public static void Translate(PointCloud pointCloud, float x, float y, float z)
        {
            Vector3 translation = new Vector3(x, y, z);

            for (int i = 0; i < pointCloud.Count; i++)
            {

                Vector3 v = pointCloud.Vectors[i];
                Vector3 translatedV = Vector3.Add(v, translation);
                v = translatedV;
                pointCloud.Vectors[i] = v;
            }


        }
        public static void InhomogenousTransform(PointCloud vectorlList, float d)
        {
            //Vector3 scaleVector = new Vector3(x, y, z);
            for (int i = 0; i < vectorlList.Count; i++)
            {
                Vector3 v = vectorlList.Vectors[i];
                v.X = v.X - v.Z / d;
                v.Y = v.Y - v.Z / d;
                //v.Vector.Z = d;
                vectorlList.Vectors[i] = v;
            }

        }
        public static void AddVector3(PointCloud pointCloud, Vector3 newOrigin)
        {
            //reset vertex so that it starts from 0,0,0
            for (int i = 0; i < pointCloud.Count; i++)
            {
                Vector3 v = pointCloud.Vectors[i];
                v.X += newOrigin.X;
                v.Y += newOrigin.Y;
                v.Z += newOrigin.Z;
                pointCloud.Vectors[i] = v;

            }

        }

        public static PointCloud Shuffle(PointCloud pc)
        {

            List<VertexKDTree> lNew = pc.VectorsWithIndex;
            lNew.Shuffle();

            PointCloud pcReturn = PointCloud.FromListVertexKDTree(lNew);

            return pcReturn;

        }
        private static void ShuffleAtIndex(PointCloud pointCloud, int i, int j)
        {
            Vector3 v = pointCloud.Vectors[i];
            pointCloud.Vectors[i] = pointCloud.Vectors[j];
            pointCloud.Vectors[j] = v;

        }
        public static void ShuffleTest(PointCloud pointCloud)
        {
            ShuffleAtIndex(pointCloud, 0, pointCloud.Count - 2);
            ShuffleAtIndex(pointCloud, 1, pointCloud.Count - 5);
            ShuffleAtIndex(pointCloud, 3, pointCloud.Count - 1);
            ShuffleAtIndex(pointCloud, 4, pointCloud.Count - 3);


        }
        public static void ShuffleRandom(PointCloud pointCloud)
        {
            IList<Vector3> lNew = new List<Vector3>(pointCloud.Vectors);
            lNew.Shuffle();
        }
        public static PointCloud FromPoints2d(List<System.Drawing.Point> pointList, PointCloud pointsTarget, List<System.Drawing.Point> pointOther)
        {

            PointCloud pointNew = new PointCloud();
            bool pointFound = false;

            for (int i = pointList.Count - 1; i >= 0; i--)
            {
                System.Drawing.Point pNew = pointList[i];
                for (int j = 0; j < pointsTarget.Count; j++)
                {
                    Vector3 p = pointsTarget.Vectors[j];
                    //add point only if it is found in the original point list
                    if (pNew.X == Convert.ToInt32(p[0]) && pNew.Y == Convert.ToInt32(p[1]))
                    {
                        pointFound = true;
                        pointNew.AddVector(p);
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
        public List<Vector3> ListVectors
        {
            get
            {
                return new List<Vector3>(this.Vectors);

            }
        }
        public List<Vector3> ListColors
        {
            get
            {
                if (this.Colors != null)
                    return new List<Vector3>(this.Colors);
                return null;
            }
        }
     
         public static Matrix3 CorrelationMatrix(List<Vector3> a, List<Vector3> b)
         {
             //consists of elements
             //axbx axby axbz
             //aybx ayby aybz
             //azbx azby azbz
             int maxNumber = b.Count;
             if (a.Count < maxNumber)
                 maxNumber = a.Count;
             Matrix3 H = new Matrix3();
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
         public static Matrix3 CorrelationMatrix(PointCloud a, PointCloud b)
         {
             //consists of elements
             //axbx axby axbz
             //aybx ayby aybz
             //azbx azby azbz
             int maxNumber = b.Vectors.Length;
             if (a.Vectors.Length < maxNumber)
                 maxNumber = a.Vectors.Length;
             Matrix3 H = new Matrix3();
             for (int i = 0; i < maxNumber; i++)
             {

                 H[0, 0] += b.Vectors[i].X * a.Vectors[i].X;
                 H[0, 1] += b.Vectors[i].X * a.Vectors[i].Y;
                 H[0, 2] += b.Vectors[i].X * a.Vectors[i].Z;

                 H[1, 0] += b.Vectors[i].Y * a.Vectors[i].X;
                 H[1, 1] += b.Vectors[i].Y * a.Vectors[i].Y;
                 H[1, 2] += b.Vectors[i].Y * a.Vectors[i].Z;

                 H[2, 0] += b.Vectors[i].Z * a.Vectors[i].X;
                 H[2, 1] += b.Vectors[i].Z * a.Vectors[i].Y;
                 H[2, 2] += b.Vectors[i].Z * a.Vectors[i].Z;


             }
             H = H.MultiplyScalar(1.0f / maxNumber);
             return H;
         }
         public static Matrix3d CorrelationMatrix_Double(PointCloud a, PointCloud b)
         {
             //consists of elements
             //axbx axby axbz
             //aybx ayby aybz
             //azbx azby azbz
             int maxNumber = b.Vectors.Length;
             if (a.Vectors.Length < maxNumber)
                 maxNumber = a.Vectors.Length;
             Matrix3d H = new Matrix3d();
             for (int i = 0; i < maxNumber; i++)
             {

                 H[0, 0] += b.Vectors[i].X * a.Vectors[i].X;
                 H[0, 1] += b.Vectors[i].X * a.Vectors[i].Y;
                 H[0, 2] += b.Vectors[i].X * a.Vectors[i].Z;

                 H[1, 0] += b.Vectors[i].Y * a.Vectors[i].X;
                 H[1, 1] += b.Vectors[i].Y * a.Vectors[i].Y;
                 H[1, 2] += b.Vectors[i].Y * a.Vectors[i].Z;

                 H[2, 0] += b.Vectors[i].Z * a.Vectors[i].X;
                 H[2, 1] += b.Vectors[i].Z * a.Vectors[i].Y;
                 H[2, 2] += b.Vectors[i].Z * a.Vectors[i].Z;


             }
             H = H.MultiplyScalar(1.0f / maxNumber);
             return H;
         }
        public void CheckAndAdjustColorsIndices()
         {
             if (this.Vectors == null)
                 return;

             if (this.Colors == null || this.Colors.Length != this.Vectors.Length)
                 this.Colors = new Vector3[this.Vectors.Length];
             if (this.Indices == null || this.Indices.Length != this.Indices.Length)
                 this.Indices = new uint[this.Indices.Length];

         }
         public void AddPointCloud(PointCloud pcToAdd)
         {
             CheckAndAdjustColorsIndices();
             pcToAdd.CheckAndAdjustColorsIndices();

             List<Vector3> v = new List<Vector3>(this.Vectors);
             List<Vector3> c = new List<Vector3>(this.Colors);
             List<uint> ind = new List<uint>(this.Indices);

             

             for (int i = 0; i < pcToAdd.Vectors.Length; i++)
             {
                 v.Add(pcToAdd.Vectors[i]);
                 c.Add(pcToAdd.Colors[i]);
                 ind.Add(Convert.ToUInt32(i));

             }
             this.Vectors = v.ToArray();
             this.Colors = c.ToArray();
             this.Indices = ind.ToArray();
         }
       
         public bool CheckCloud(float threshold, out float fMax)
         {
             fMax = float.MinValue;
             for (int i = 0; i < this.Vectors.Length; i++)
             {
               
                 fMax = Math.Max(Math.Abs(Vectors[i].X), fMax);
                 fMax = Math.Max(Math.Abs(Vectors[i].Y), fMax);
                 fMax = Math.Max(Math.Abs(Vectors[i].Z), fMax);

                 

             }

             if (fMax > threshold)
                 return false;
             return true;
         }
         //public static bool CheckCloud(PointCloud mypointCloudTarget, PointCloud mypointCloudResult)
         //{
         //    if (mypointCloudResult == null || mypointCloudTarget == null)
         //        return false;

         //    float meanDistance = PointCloud.MeanDistance(mypointCloudTarget, mypointCloudResult);
         //    if (meanDistance > threshold)
         //        return false;
         //    return true;
             

         //    //float diffMax = float.MinValue;
         //    //for (int i = 0; i < mypointCloudTarget.Count; i++)
         //    //{
         //    //    float dx = Math.Abs(mypointCloudTarget.Vectors[i].X - mypointCloudResult.Vectors[i].X);
         //    //    float dy = Math.Abs(mypointCloudTarget.Vectors[i].Y - mypointCloudResult.Vectors[i].Y);
         //    //    float dz = Math.Abs(mypointCloudTarget.Vectors[i].Z - mypointCloudResult.Vectors[i].Z);
         //    //    if (dx > diffMax)
         //    //        diffMax = dx;
         //    //    if (dy > diffMax)
         //    //        diffMax = dy;
         //    //    if (dz > diffMax)
         //    //        diffMax = dz;
         //    //    if (float.IsNaN(dx) || float.IsNaN(dy) || float.IsNaN(dz))
         //    //        return false;
         //    //    if (dx > threshold || dy > threshold || dz > threshold)
         //    //    {
         //    //        System.Diagnostics.Debug.WriteLine("Check result - is : " + dx.ToString() + " : " + dy.ToString() + " : " + dz.ToString() + " : " + "--- Should be: " + threshold);
         //    //        return false;
         //    //    }
         //    //    //needs a lot of exection time - only for error cases  
         //    //    //Vector3 v = new Vector3(dx, dy, dz);
         //    //    //Debug.WriteLine(i.ToString() + "Vector is OK, distance difference is: " + v.Length.ToString());

         //    //}
         //    //System.Diagnostics.Debug.WriteLine("---");
         //    //System.Diagnostics.Debug.WriteLine("Check Cloud, difference: " + diffMax.ToString("G") + " :  allowed: " + threshold.ToString("G"));
         //    return true;
         //}
         public static Matrix3 CovarianceMatrix(List<Vector3> a, bool normalsCovariance)
         {
             //consists of elements
             //axbx axby axbz
             //aybx ayby aybz
             //azbx azby azbz
             Matrix3 H = new Matrix3();
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
         public static Matrix3 CovarianceMatrix3(PointCloud a, bool normalsCovariance)
         {
             //consists of elements
             //axbx axby axbz
             //aybx ayby aybz
             //azbx azby azbz
             Matrix3 H = new Matrix3();
             for (int i = 0; i < a.Vectors.Length; i++)
             {

                 H[0, 0] += a.Vectors[i].X * a.Vectors[i].X;
                 H[0, 1] += a.Vectors[i].X * a.Vectors[i].Y;
                 H[0, 2] += a.Vectors[i].X * a.Vectors[i].Z;

                 H[1, 0] += a.Vectors[i].Y * a.Vectors[i].X;
                 H[1, 1] += a.Vectors[i].Y * a.Vectors[i].Y;
                 H[1, 2] += a.Vectors[i].Y * a.Vectors[i].Z;

                 H[2, 0] += a.Vectors[i].Z * a.Vectors[i].X;
                 H[2, 1] += a.Vectors[i].Z * a.Vectors[i].Y;
                 H[2, 2] += a.Vectors[i].Z * a.Vectors[i].Z;


             }
             H.Transpose();
             if (!normalsCovariance)
                 H = H.MultiplyScalar(1f / a.Vectors.Length);
             return H;
         }
         public static Matrix3d CovarianceMatrix3d(PointCloud a, bool normalsCovariance)
         {
             //consists of elements
             //axbx axby axbz
             //aybx ayby aybz
             //azbx azby azbz
             Matrix3d H = new Matrix3d();
             for (int i = 0; i < a.Vectors.Length; i++)
             {

                 H[0, 0] += a.Vectors[i].X * a.Vectors[i].X;
                 H[0, 1] += a.Vectors[i].X * a.Vectors[i].Y;
                 H[0, 2] += a.Vectors[i].X * a.Vectors[i].Z;

                 H[1, 0] += a.Vectors[i].Y * a.Vectors[i].X;
                 H[1, 1] += a.Vectors[i].Y * a.Vectors[i].Y;
                 H[1, 2] += a.Vectors[i].Y * a.Vectors[i].Z;

                 H[2, 0] += a.Vectors[i].Z * a.Vectors[i].X;
                 H[2, 1] += a.Vectors[i].Z * a.Vectors[i].Y;
                 H[2, 2] += a.Vectors[i].Z * a.Vectors[i].Z;


             }
             H.Transpose();
             if (!normalsCovariance)
                 H = H.MultiplyScalar(1f / a.Vectors.Length);
             return H;
         }
      
         public static void ResizeTo1(PointCloud pointCloud, ref Vector3 centerOfGravity, ref Vector3 maxPoint, ref Vector3 minPoint)
         {
             double dd = Math.Max(maxPoint.X, maxPoint.Y);
             dd = Math.Max(dd, maxPoint.Z);
             float d = Convert.ToSingle(dd);
             if (d > 0)
             {
                 centerOfGravity.X /= d;
                 centerOfGravity.Y /= d;
                 centerOfGravity.Z /= d;
                 for (int i = 0; i < pointCloud.Count; i++)
                 {
                     pointCloud.Vectors[i].X /= d;
                     pointCloud.Vectors[i].Y /= d;
                     pointCloud.Vectors[i].Z /= d;
                 }
             }


         }
         public void ResizeVerticesTo1()
         {
             ResetCentroid(this, false);


             this.CalculateBoundingBox();
             Vector3 vDiff = this.BoundingBoxMax - this.BoundingBoxMin;

             float scale = Math.Max(BoundingBoxMax.X, BoundingBoxMax.Y);
             scale = Math.Max(scale, BoundingBoxMax.Z);



             for (int i = 0; i < this.Count; i++)
             {
                 // int x = centroid.X + (int)((point.X - centroid.X) * scale) );
                 this.Vectors[i].X /= scale;
                 this.Vectors[i].Y /= scale;
                 this.Vectors[i].Z /= scale;
             }



         }
    
       

    }
}
