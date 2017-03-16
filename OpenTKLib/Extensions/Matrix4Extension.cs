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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;


namespace OpenTKExtension
{
    //Extensios attached to the object which folloes the "this" 
    public static class Matrix4Extension
    {
        public static void Print(this Matrix4 m, string name)
        {
            
            System.Diagnostics.Debug.WriteLine(name);


            for (int i = 0; i < 4; i++)
            {
                System.Diagnostics.Debug.WriteLine(m[i, 0].ToString("0.00") + " " + m[i, 1].ToString("0.00") + " " + m[i, 2].ToString("0.00") + " " + m[i, 3].ToString("0.00"));

            }
        }
        public static bool CheckNAN(this Matrix4 myMatrix)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    if (float.IsNaN(myMatrix[i, j]))
                        return true;
            }
            return false;


        }
        public static List<Vector3> TransformPoints(this Matrix4 matrix, List<Vector3> a)
        {
            if (a == null || a.Count == 0)
                return null;
            List<Vector3> b = new List<Vector3>();


            float[,] matrixfloat = matrix.ToFloatArray();


            for (int i = 0; i < a.Count; i++)
            {
                Vector3 p1 = a[i];

                //Does not work with pointers...
                //this.LandmarkTransform.InternalTransformPoint(PointerUtils.GetIntPtr(p1), PointerUtils.GetIntPtr(p2));
                float[] pointFloat = new float[3] { p1.X, p1.Y, p1.Z };
                float[] pointReturn = Matrix4Extension.TransformPointfloat(pointFloat, matrixfloat);
                b.Add(new Vector3(pointReturn[0], pointReturn[1], pointReturn[2]));
            }
            return b;
        }
        public static void TransformPointCloud(this Matrix4 matrix, PointCloud pc)
        {
            if (pc == null || pc.Vectors.Length == 0)
                return;
            //List<Vector3> b = new List<Vector3>();

            for (int i = 0; i < pc.Vectors.Length; i++)
            {
                pc.Vectors[i] = matrix.TransformVector(pc.Vectors[i]);               
            }
            
        }
        public static PointCloud TransformPoints(this Matrix4 matrix, PointCloud a)
        {
            if (a == null || a.Vectors == null)
                return null;
            PointCloud b = new PointCloud();
            b.Vectors = new Vector3[a.Vectors.Length];

           
            for (int i = 0; i < a.Vectors.Length; i++)
            {
                b.Vectors[i] = matrix.TransformVector(a.Vectors[i]);
            }

            if (a.Colors != null)
            {
                b.Colors = new Vector3[a.Colors.Length];
                a.Colors.CopyTo(b.Colors, 0);
            }
            if (a.Indices != null)
            {
                b.Indices = new uint[a.Indices.Length];
                a.Indices.CopyTo(b.Indices, 0);
            }
            return b;
        }
        public static void TransformVectorList(this Matrix4 matrix, List<Vector3> vectors)
        {
            if (vectors == null || vectors.Count == 0)
                return;

            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = matrix.TransformVector(vectors[i]);
            }
            
        }
        public static void TransformVectors(this Matrix4 matrix, Vector3[] vectors)
        {
            if (vectors == null || vectors.Length == 0)
                return;

            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = matrix.TransformVector(vectors[i]);
            }

        }
    
        public static float[] TransformPointfloat(float[] pointSource, float[,] matrix)
        {
            float[] pointReturn = new float[3];
            pointReturn[0] = matrix[0, 0] * pointSource[0] + matrix[0, 1] * pointSource[1] + matrix[0, 2] * pointSource[2] + matrix[0, 3];
            pointReturn[1] = matrix[1, 0] * pointSource[0] + matrix[1, 1] * pointSource[1] + matrix[1, 2] * pointSource[2] + matrix[1, 3];
            pointReturn[2] = matrix[2, 0] * pointSource[0] + matrix[2, 1] * pointSource[1] + matrix[2, 2] * pointSource[2] + matrix[2, 3];

            return pointReturn;

        }
        public static Vector3 TransformVector3(this Matrix4 matrix, Vector3 v)
        {
            Vector3 pointReturn = new Vector3();
            pointReturn[0] = matrix[0, 0] * v[0] + matrix[0, 1] * v[1] + matrix[0, 2] * v[2] + matrix[0, 3];
            pointReturn[1] = matrix[1, 0] * v[0] + matrix[1, 1] * v[1] + matrix[1, 2] * v[2] + matrix[1, 3];
            pointReturn[2] = matrix[2, 0] * v[0] + matrix[2, 1] * v[1] + matrix[2, 2] * v[2] + matrix[2, 3];

            return pointReturn;

        }
        public static Vector3 TransformVector(this Matrix4 matrix, Vector3 v)
        {
            Vector3 pointReturn = new Vector3();
            pointReturn[0] = Convert.ToSingle(matrix[0, 0] * v[0] + matrix[0, 1] * v[1] + matrix[0, 2] * v[2] + matrix[0, 3]);
            pointReturn[1] = Convert.ToSingle(matrix[1, 0] * v[0] + matrix[1, 1] * v[1] + matrix[1, 2] * v[2] + matrix[1, 3]);
            pointReturn[2] = Convert.ToSingle(matrix[2, 0] * v[0] + matrix[2, 1] * v[1] + matrix[2, 2] * v[2] + matrix[2, 3]);

            return pointReturn;

        }
        public static Matrix3 ExtractMatrix3(this Matrix4 mat4d)
        {

            Matrix3 mat3d = new Matrix3();
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    mat3d[i, j] = mat4d[i, j];
            }
            return mat3d;

        }
        //public static Matrix3 ExtractMatrix3(this Matrix4 mat4)
        //{

        //    Matrix3 mat3d = new Matrix3();
        //    for (int i = 0; i < 3; i++)
        //    {
        //        for (int j = 0; j < 3; j++)
        //            mat3d[i, j] = mat4[i, j];
        //    }
        //    return mat3d;

        //}
        public static Matrix4 PutTheMatrix4together(this Matrix4 mat4d, Vector3 T, Matrix3 Rotation)
        {

            //put the 4d matrix together
            Matrix3 r3D = Rotation.Clone();
            Matrix4 myMatrix = new Matrix4(r3D);
            myMatrix[0, 3] = T.X;
            myMatrix[1, 3] = T.Y;
            myMatrix[2, 3] = T.Z;
            myMatrix[3, 3] = 1f;

            return myMatrix;

        }
        public static Matrix4 AddTranslation(this Matrix4 mat4d, Vector3 T)
        {
            mat4d[0, 3] += T.X;
            mat4d[1, 3] += T.Y;
            mat4d[2, 3] += T.Z;


            return mat4d;

        }
        public static float TraceMinusEV(this Matrix4 mat4d, Vector3 EV)
        {
            float trace = mat4d.Trace;
            for (int i = 0; i < 3; i++ )
            {
                trace -= EV[i];
            }
           

            return trace;

        }
        public static float AbsSumOfElements(this Matrix4 mat4d)
        {
            float sum = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    sum += Math.Abs(mat4d[i,j]);
                
            }


            return sum;

        }
        public static float AbsSumOfElements3D(this Matrix4 mat4d)
        {
            float sum = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    sum += Math.Abs(mat4d[i, j]);

            }


            return sum;

        }
        public static float RotationAnglesSum(this Matrix4 r)
        {
            Vector3 v = r.RotationAngles();
            System.Diagnostics.Debug.WriteLine("--Rotation: " + v.X.ToString("0.00") + " : " +v.Y.ToString("0.00") + " : " +v.Z.ToString("0.00") + " : "  );
            return v.X + v.Y + v.Z;

        }
        public static Vector3 RotationAngles(this Matrix4 r)
        {
            float thetaX, thetaY, thetaZ;
            if (r[0, 2] < 1)
            {
                if (r[0, 2] > -1)
                {
                    thetaY =Convert.ToSingle(  Math.Asin(r[0, 2]));
                    thetaX =Convert.ToSingle(  Math.Atan2(-r[1, 2], r[2, 2]));
                    thetaZ =Convert.ToSingle(  Math.Atan2(-r[0, 1], r[0, 0]));
                }
                else // r 0 2 = −1
                {
                    // Not a u n i q u e s o l u t i o n : thetaZ −thetaaX = Math.Atan2( r10 , r 1 1 )
                    thetaY = -Convert.ToSingle( Math.PI / 2);
                    thetaX = -Convert.ToSingle( Math.Atan2(r[1, 0], r[1, 1]));
                    thetaZ = 0;
                }
            }

            else // r 0 2 = +1
            {
                // Not a u n i q u e s o l u t i o n : thetaZ +thetaaX = Math.Atan2( r10 , r 1 1 )
                thetaY = Convert.ToSingle(Math.PI / 2);
                thetaX =Convert.ToSingle( Math.Atan2(r[1, 0], r[1, 1]));
                thetaZ = 0;
            }
            Vector3 v = new Vector3(thetaX, thetaY, thetaZ);
            
            for(int i = 0; i < 3; i++)
            {
                v[i] = Math.Abs(v[i]);
                if (v[i] > 2)
                    v[i] -= 2;
                if (v[i] > 1)
                    v[i] -= 1;

            }
            
            return v;

        }

        //public static float[,] ToFloatArray(this Matrix4 myMatrix)
        //{
        //    float[,] myMatrixArray = new float[4, 4];
        //    for (int i = 0; i < 4; i++)
        //        for (int j = 0; j < 4; j++)
        //            myMatrixArray[i, j] = myMatrix[i, j];

        //    return myMatrixArray;
        //}
        public static float[,] ToFloatArray(this Matrix4 myMatrix)
        {
            float[,] myMatrixArray = new float[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    myMatrixArray[i, j] = Convert.ToSingle(myMatrix[i, j]);

            return myMatrixArray;
        }


        public static void GetPosition(this Matrix4 mat, float[] position)
        {

            position[0] = mat[0, 3];
            position[1] = mat[1, 3];
            position[2] = mat[2, 3];
        }


        public static void Translate(this Matrix4 mat, float x, float y, float z)
        {
            if (x == 0.0 && y == 0.0 && z == 0)
            {
                return;
            }

            Matrix4 matrix = Matrix4.Identity;

            matrix[0, 3] = x;
            matrix[1, 3] = y;
            matrix[2, 3] = z;
            Matrix4.Mult(ref mat, ref matrix, out mat);

        }
        public static void Rotate(this Matrix4 mat, float angle, float x, float y, float z)
        {
            if (angle == 0.0 || (x == 0.0 && y == 0.0 && z == 0))
            {
                return;
            }

            // convert to radians
            angle = angle * MathBase.DegreesToRadians_Float;

            // make a normalized quaternion
            float w = Convert.ToSingle(Math.Cos(0.5f * angle));
            float f = Convert.ToSingle(Math.Sin(0.5f * angle) / Math.Sqrt(x * x + y * y + z * z));
            x *= f;
            y *= f;
            z *= f;

            // convert the quaternion to a matrix
            Matrix4 matrix = Matrix4.Identity;


            float ww = w * w;
            float wx = w * x;
            float wy = w * y;
            float wz = w * z;

            float xx = x * x;
            float yy = y * y;
            float zz = z * z;

            float xy = x * y;
            float xz = x * z;
            float yz = y * z;

            float s = ww - xx - yy - zz;

            matrix[0, 0] = xx * 2 + s;
            matrix[1, 0] = (xy + wz) * 2;
            matrix[2, 0] = (xz - wy) * 2;

            matrix[0, 1] = (xy - wz) * 2;
            matrix[1, 1] = yy * 2 + s;
            matrix[2, 1] = (yz + wx) * 2;

            matrix[0, 2] = (xz + wy) * 2;
            matrix[1, 2] = (yz - wx) * 2;
            matrix[2, 2] = zz * 2 + s;

            Matrix4.Mult(ref mat, ref matrix, out mat);

        }
        public static void Scale(this Matrix4 mat, float x, float y, float z)
        {
            if (x == 1 && y == 1 && z == 1)
            {
                return;
            }

            Matrix4 matrix = Matrix4.Identity;

            matrix[0, 0] = x;
            matrix[1, 1] = y;
            matrix[2, 2] = z;

            Matrix4.Mult(ref mat, ref matrix, out mat);

        }
        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector4 MultiplyVector(this Matrix4 mat, Vector4 vec)
        {
            //Vector3 vNew = new Vector3(mat.Row2);
            //float val = Vector3.Dot(vNew, vec);

            return new Vector4(
                Vector4.Dot(new Vector4(mat.Row0), vec),
                Vector4.Dot(new Vector4(mat.Row1), vec),
                Vector4.Dot(new Vector4(mat.Row2), vec),
                Vector4.Dot(new Vector4(mat.Row3), vec));
        }
        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 MultiplyVector3(this Matrix4 mat, Vector4 vec)
        {
            //Vector3 vNew = new Vector3(mat.Row2);
            //float val = Vector3.Dot(vNew, vec);

            return new Vector3(
                Vector4.Dot(new Vector4(mat.Row0), vec),
                Vector4.Dot(new Vector4(mat.Row1), vec),
                Vector4.Dot(new Vector4(mat.Row2), vec));

        }
        public static void PrintMatrix(this Matrix4 m, string name)
        {
            System.Diagnostics.Debug.WriteLine(name);


            for (int i = 0; i < 4; i++)
            {
                System.Diagnostics.Debug.WriteLine(m[i, 0].ToString("0.00") + " " + m[i, 1].ToString("0.00") + " " + m[i, 2].ToString("0.00") + " " + m[i, 3].ToString("0.00"));

            }
        }
        public static Matrix4 Clone(this Matrix4 mat)
        {
            Matrix4 newMat = new Matrix4();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    newMat[i, j] = mat[i, j];
                }
            }
            return newMat;
        }
        public static Matrix4 PerspectiveNew(this Matrix4 mat, float fovy, float aspect, float zNear, float zFar)
        {
            Matrix4 Result = Matrix4.Identity;


            if (aspect == 0 || zFar == zNear)
                return Result;

            float rad = fovy;


            float tanHalfFovy = Convert.ToSingle(Math.Tan(rad / 2));



            Result[0, 0] = 1 / (aspect * tanHalfFovy);
            Result[1, 1] = 1 / (tanHalfFovy);
            Result[2, 2] = -(zFar + zNear) / (zFar - zNear);
            Result[2, 3] = -1;
            Result[3, 2] = -(2 * zFar * zNear) / (zFar - zNear);
            return Result;
        }
        public static Matrix4 Translate(this Matrix4 m, Vector3 v)
        {


            m.Row3 = m.Row0 * v[0] + m.Row1 * v[1] + m.Row2 * v[2] + m.Row3;
            return m;
        }
        public static float DegreesToRadians(float degrees)
        {

            return Convert.ToSingle(degrees * (Math.PI / 180f));
        }
        public static Matrix4 RotateDegrees(this Matrix4 m, float angle, Vector3 v)
        {
            angle = DegreesToRadians(angle);
            float c = Convert.ToSingle(Math.Cos(angle));
            float s = Convert.ToSingle(Math.Sin(angle));

            Vector3 axis = v;
            axis.Normalize();
            Vector3 temp = (1f - c) * axis;
            //Matrix4 R1 = Matrix4.Zero;
            Matrix4 Rotate = new Matrix4();

            Rotate[0, 0] = c + temp[0] * axis[0];
            Rotate[0, 1] = 0 + temp[0] * axis[1] + s * axis[2];
            Rotate[0, 2] = 0 + temp[0] * axis[2] - s * axis[1];

            Rotate[1, 0] = 0 + temp[1] * axis[0] - s * axis[2];
            Rotate[1, 1] = c + temp[1] * axis[1];
            Rotate[1, 2] = 0 + temp[1] * axis[2] + s * axis[0];

            Rotate[2, 0] = 0 + temp[2] * axis[0] + s * axis[1];
            Rotate[2, 1] = 0 + temp[2] * axis[1] - s * axis[0];
            Rotate[2, 2] = c + temp[2] * axis[2];

            Matrix4 Result = new Matrix4();

            Result.Row0 = m.Row0 * Rotate[0, 0] + m.Row1 * Rotate[0, 1] + m.Row2 * Rotate[0, 2];
            Result.Row1 = m.Row0 * Rotate[1, 0] + m.Row1 * Rotate[1, 1] + m.Row2 * Rotate[1, 2];
            Result.Row2 = m.Row0 * Rotate[2, 0] + m.Row1 * Rotate[2, 1] + m.Row2 * Rotate[2, 2];
            Result.Row3 = m.Row3;
            return Result;

        }

        public static void Save(this Matrix4 m, string path, string fileName)
        {

            StringBuilder sb = new StringBuilder();

            List<string> lines = new List<string>();
                        
            for (int i = 0; i < 4; ++i)
            {
                string line = string.Empty;
                for (int j = 0; j < 4; ++j)
                {
                    line += m[i, j].ToString(UtilsPointCloudIO.CultureInfo) + ":";

                }
                lines.Add(line);
            }

            System.IO.File.WriteAllLines(path + "\\" + fileName, lines);


        }
           /// <summary>
        /// reads the OBJ file ONLY with the special format used also in the write_OBJ method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileNameShort"></param>
        /// <param name="depthData"></param>
        /// <returns></returns>
        public static Matrix4 FromFile(this Matrix4 m, string path, string fileNameShort)
        {
            Matrix4 myMatrix = Matrix4.Identity;
            string fileName = path + "\\" + fileNameShort;
            if (!System.IO.File.Exists(fileName))
            {
                System.Diagnostics.Debug.WriteLine("File does not exist: ");
                return myMatrix;

            }

            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);

                for (int i = 0; i < lines.Length; i++)
                //for (i = lines.Length -1 ; i >= startIndex + 1; i--)
                {

                    string[] arrStr1 = lines[i].Split(new Char[] { ':' });
                    for (int j = 0; j < arrStr1.Length; j++)
                    {
                        myMatrix[i, j] = Convert.ToSingle(arrStr1[j], UtilsPointCloudIO.CultureInfo);
                    }
                }
            }
            catch
            {

            }
            return myMatrix;
        }

  
    }
}
