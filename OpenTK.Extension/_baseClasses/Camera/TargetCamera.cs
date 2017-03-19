#define _USE_MATH_DEFINES
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTKExtension.FastGLControl
{

    public class CTargetCamera : CAbstractCamera
    {

        public Vector3 Target = new Vector3();
        //public Vector3 OrientationVector = new Vector3((float)Math.PI, 0f, 0f);
        //public Vector3 OrientationVector = new Vector3(0f, 0f, 0f);
        //public float MoveSpeed = 0.002f;
        //public float MouseSensitivity = 0.00001f;


        
        protected float minRy;
        protected float maxRy;
        protected float distance;
        protected float minDistance;
        protected float maxDistance;


        public CTargetCamera()
        {
            right = new Vector3(1, 0, 0);
            Up = new Vector3(0, 1, 0);
            CenterOfInterest = new Vector3(0, 0, -1);
            minRy = -60F;
            maxRy = 60F;
            minDistance = 1F;
            maxDistance = 10F;
        }
        public new void Dispose()
        {
            base.Dispose();
        }
        private void CalcVMatrix()
        {
            //look = (target - position).Normalize();
            CenterOfInterest = (Target - Position);
            CenterOfInterest.Normalize();
            right = Vector3.Cross(CenterOfInterest, Up);


            V = Matrix4.LookAt(Position, // Camera is here
                Target, // and looks here : at the same position, plus "direction"
                Up);      // Head is up (set to 0,-1,0 to look upside-down)
        }
        public override void Update()
        {

            Vector3 direction = new Vector3(0, 0, distance);
            direction = R.MultiplyVector3(new Vector4(direction, 0.0f));
            Position = Target + direction;
            Up = R.MultiplyVector3(new Vector4(UP, 0.0f));

            CalcVMatrix();


        }
        public void Rotate(float deltaX, float deltaY)
        {
            //float p = ((((((pitch) > (minRy)) ? (pitch) : (minRy))) < (maxRy)) ? ((((pitch) > (minRy)) ? (pitch) : (minRy))) : (maxRy));
            //base.Rotate(yaw, p, roll);

            float mouseSpeed = 0.1f;

            float horizontalAngle = mouseSpeed *  deltaX;
            float verticalAngle = mouseSpeed * deltaY;

            //// Direction : Spherical coordinates to Cartesian coordinates conversion
            Vector3 directionNew = new Vector3(Convert.ToSingle(Math.Cos(verticalAngle) * Math.Sin(horizontalAngle)),
                Convert.ToSingle(Math.Sin(verticalAngle)),
                Convert.ToSingle(Math.Cos(verticalAngle) * Math.Cos(horizontalAngle)));

            //// Right vector
            Vector3 rightV = new Vector3(Convert.ToSingle(Math.Sin(horizontalAngle - 3.14f / 2.0f)),
                    0,
                    Convert.ToSingle(Math.Cos(horizontalAngle - 3.14f / 2.0f)));

            //// Up vector
            Vector3 up = Vector3.Cross(right, directionNew);
            V = Matrix4.LookAt(Position, // Camera is here
             Position + directionNew, // and looks here : at the same position, plus "direction"
              up);      // Head is up (set to 0,-1,0 to look upside-down)

        }
        public new void Rotate(float yaw, float pitch, float roll)
        {
            float p = ((((((pitch) > (minRy)) ? (pitch) : (minRy))) < (maxRy)) ? ((((pitch) > (minRy)) ? (pitch) : (minRy))) : (maxRy));
            base.Rotate(yaw, p, roll);

            
        }

        public void SetTarget(Vector3 tgt)
        {
            Target = tgt;
            distance = (Position - Target).Length;// glm.distance(position, target);
            distance = (((minDistance) > ((((distance) < (maxDistance)) ? (distance) : (maxDistance)))) ? (minDistance) : ((((distance) < (maxDistance)) ? (distance) : (maxDistance))));

        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const Vector3 GetTarget() const
        public Vector3 GetTarget()
        {
            return Target;
        }

        public void Pan(float dx, float dy)
        {
            Vector3 X = right * dx;
            Vector3 Y = Up * dy;
            Position += X + Y;
            Target += X + Y;
            Update();
        }
        public void Zoom(float amount)
        {
            Position += CenterOfInterest * amount;
            distance = Vector3.Subtract(Position, Target).Length;
            distance = (((minDistance) > ((((distance) < (maxDistance)) ? (distance) : (maxDistance)))) ? (minDistance) : ((((distance) < (maxDistance)) ? (distance) : (maxDistance))));
            Update();
        }
        public void Move(float dx, float dy)
        {
            Vector3 X = right * dx;
            Vector3 Y = CenterOfInterest * dy;
            Position += X + Y;
            Target += X + Y;
            Update();
        }
      
     

    }
}