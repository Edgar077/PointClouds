using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTKExtension.FastGLControl
{
  


    public abstract class CAbstractCamera
    {
        public Matrix4 V = Matrix4.Identity; //view matrix
        //public Matrix4 P = new Matrix4(); //projection matrix
        
        //vectors
        public Vector3 Position = new Vector3();
        protected Vector3 CenterOfInterest = new Vector3();
        protected Vector3 Up = new Vector3();
        //------------
        public Matrix4 R = Matrix4.Identity;


         //frustum points
        public Vector3[] farPts = new Vector3[4];
        public Vector3[] nearPts = new Vector3[4];
        



        protected float yaw;
        protected float pitch;
        protected float roll;
        protected float fov;
        protected float aspect_ratio;
        protected float Znear;
        protected float Zfar;
        protected static Vector3 UP = new Vector3(0, 1, 0);
       
        protected Vector3 right = new Vector3();
        
        //Frsutum planes
        protected CPlane[] planes = new CPlane[6]; 

        public CAbstractCamera()
        {
            Znear = 0.1f;
            Zfar = 1000F;
        }
        public void Dispose()
        {
        }
        //public void SetupProjection(float fovy, float aspRatio)
        //{
        //    SetupProjection(fovy, aspRatio, 0.1f, 1000.0f);
        //}
        //public void SetupProjection(float fovy, float aspRatio, float nr)
        //{
        //    SetupProjection(fovy, aspRatio, nr, 1000.0f);
        //}
        //public void SetupProjection(float fovy, float aspRatio, float ZNear, float ZFar)
        //{

        //    this.Znear = ZNear;
        //    this.Zfar = ZFar;
        //    this.fov = fovy;
        //    this.aspect_ratio = aspRatio;

        //   // P = P.PerspectiveNew(fovy, aspRatio, ZNear, ZFar);
        //    P = Matrix4.CreatePerspectiveFieldOfView(fovy, aspRatio, ZNear, ZFar);
            

           
        //}

        public abstract void Update();

        public virtual void Rotate(float y, float p, float r)
        {
            yaw = Matrix4Extension.DegreesToRadians(y);
            pitch = Matrix4Extension.DegreesToRadians(p);
            roll = Matrix4Extension.DegreesToRadians(r);
            Matrix4 rNew = yawPitchRoll(yaw, pitch, 0.0f);
            R = Matrix4.Mult(R, rNew);

            Update();
        }

      

        public void CalcFrustumPlanes()
        {


            Vector3 cN = Position + CenterOfInterest * Znear;
            Vector3 cF = Position + CenterOfInterest * Zfar;

            float Hnear = 2.0f * Convert.ToSingle(Math.Tan(Matrix4Extension.DegreesToRadians(fov / 2.0f)) * Znear);
            float Wnear = Hnear * aspect_ratio;
            float Hfar = 2.0f * Convert.ToSingle(Math.Tan(Matrix4Extension.DegreesToRadians(fov / 2.0f)) * Zfar);
            float Wfar = Hfar * aspect_ratio;
            float hHnear = Hnear / 2.0f;
            float hWnear = Wnear / 2.0f;
            float hHfar = Hfar / 2.0f;
            float hWfar = Wfar / 2.0f;


            farPts[0] = cF + Up * hHfar - right * hWfar;
            farPts[1] = cF - Up * hHfar - right * hWfar;
            farPts[2] = cF - Up * hHfar + right * hWfar;
            farPts[3] = cF + Up * hHfar + right * hWfar;

            nearPts[0] = cN + Up * hHnear - right * hWnear;
            nearPts[1] = cN - Up * hHnear - right * hWnear;
            nearPts[2] = cN - Up * hHnear + right * hWnear;
            nearPts[3] = cN + Up * hHnear + right * hWnear;

            planes[0] = CPlane.FromPoints(nearPts[3], nearPts[0], farPts[0]);
            planes[1] = CPlane.FromPoints(nearPts[1], nearPts[2], farPts[2]);
            planes[2] = CPlane.FromPoints(nearPts[0], nearPts[1], farPts[1]);
            planes[3] = CPlane.FromPoints(nearPts[2], nearPts[3], farPts[2]);
            planes[4] = CPlane.FromPoints(nearPts[0], nearPts[3], nearPts[2]);
            planes[5] = CPlane.FromPoints(farPts[3], farPts[0], farPts[1]);
        }
        public bool IsPointInFrustum(Vector3 point)
        {
            for (int i = 0; i < 6; i++)
            {
                if (planes[i].GetDistance(point) < 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsSphereInFrustum(Vector3 center, float radius)
        {
            for (int i = 0; i < 6; i++)
            {
                float d = planes[i].GetDistance(center);
                if (d < -radius)
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsBoxInFrustum(Vector3 min, Vector3 max)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3 p = min;
                Vector3 n = max;
                Vector3 N = planes[i].N;
                if (N.X >= 0)
                {
                    p.X = max.X;
                    n.X = min.X;
                }
                if (N.Y >= 0)
                {
                    p.Y = max.Y;
                    n.Y = min.Y;
                }
                if (N.Z >= 0)
                {
                    p.Z = max.Z;
                    n.Z = min.Z;
                }

                if (planes[i].GetDistance(p) < 0)
                {
                    return false;
                }
            }
            return true;
        }
        public void GetFrustumPlanes(ref Vector4[] fp)
        {
            for (int i = 0; i < 6; i++)
            {
                fp[i] = new Vector4(planes[i].N, planes[i].d);
            }
        }

      

     
        protected Matrix4 yawPitchRoll(float yaw, float pitch, float roll)
        {
            Matrix4 Result = Matrix4.Identity;

            float tmp_ch = Convert.ToSingle(Math.Cos(yaw));
            float tmp_sh = Convert.ToSingle(Math.Sin(pitch));
            float tmp_cp = Convert.ToSingle(Math.Cos(pitch));
            float tmp_sp = Convert.ToSingle(Math.Sin(pitch));

            float tmp_cb = Convert.ToSingle(Math.Cos(roll));
            float tmp_sb = Convert.ToSingle(Math.Sin(roll));



            Result[0, 0] = tmp_ch * tmp_cb + tmp_sh * tmp_sp * tmp_sb;
            Result[0, 1] = tmp_sb * tmp_cp;
            Result[0, 2] = -tmp_sh * tmp_cb + tmp_ch * tmp_sp * tmp_sb;
            Result[0, 3] = 0f;
            Result[1, 0] = -tmp_ch * tmp_sb + tmp_sh * tmp_sp * tmp_cb;
            Result[1, 1] = tmp_cb * tmp_cp;
            Result[1, 2] = tmp_sb * tmp_sh + tmp_ch * tmp_sp * tmp_cb;
            Result[1, 3] = 0f;
            Result[2, 0] = tmp_sh * tmp_cp;
            Result[2, 1] = -tmp_sp;
            Result[2, 2] = tmp_ch * tmp_cp;
            Result[2, 3] = 0f;
            Result[3, 0] = 0f;
            Result[3, 1] = 0f;
            Result[3, 2] = 0f;
            Result[3, 3] = 0f;
            return Result;
        }
        //protected Matrix4 lookAt(Vector3 eye, Vector3 center, Vector3 up)
        //{
        //    Matrix4 Result = Matrix4.Identity;
        //    Vector3 f = (center - eye);
        //    f.Normalize();
        //    Vector3 u = new Vector3(up);
        //    u.Normalize();
        //    Vector3 s = Vector3.Cross(f, u);
        //    s.Normalize();
        //    u = Vector3.Cross(s, f);

        //    Result[0, 0] = s.X;
        //    Result[1, 0] = s.Y;
        //    Result[2, 0] = s.Z;
        //    Result[0, 1] = u.X;
        //    Result[1, 1] = u.Y;
        //    Result[2, 1] = u.Z;
        //    Result[0, 2] = -f.X;
        //    Result[1, 2] = -f.Y;
        //    Result[2, 2] = -f.Z;
        //    Result[3, 0] = - Vector3.Dot(s, eye);
        //    Result[3, 1] = - Vector3.Dot(u, eye);
        //    Result[3, 2] = Vector3.Dot(f, eye);
        //    return Result;
        //}
    }
 
}

