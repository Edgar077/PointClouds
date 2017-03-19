using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTKExtension.FastGLControl
{
  
    public class CFreeCamera : CAbstractCamera
    {
        public CFreeCamera()
        {
            translation = new Vector3(0);
            speed = 0.5f; // 0.5f m/s
        }
        public new void Dispose()
        {
            base.Dispose();
        }

        public override void Update()
        {
            Matrix4 R = yawPitchRoll(yaw, pitch, roll);
            Position += translation;

            //set this when no movement decay is needed
            //translation=Vector3(0);

            CenterOfInterest = R.MultiplyVector3(new Vector4(0, 0, 1, 0));
            Up = R.MultiplyVector3(new Vector4(0, 1, 0, 0));
            right = Vector3.Cross(CenterOfInterest, Up);

            Vector3 direction = Position + CenterOfInterest;
            // V = lookAt(Position, tgt, up);
            V = Matrix4.LookAt(Position, // Camera is here
                direction, // and looks here : at the same position, plus "direction"
                Up);      // Head is up (set to 0,-1,0 to look upside-down)
            //

        }

        public void Walk(float dt)
        {
            translation += (CenterOfInterest * speed * dt);
            Update();
        }
        public void Strafe(float dt)
        {
            translation += (right * speed * dt);
            Update();
        }
        public void Lift(float dt)
        {
            translation += (Up * speed * dt);
            Update();
        }

        public void SetTranslation(Vector3 t)
        {
            translation = t;
            Update();
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Vector3 GetTranslation() const
        public Vector3 GetTranslation()
        {
            return translation;
        }

        public void SetSpeed(float s)
        {
            speed = s;
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const float GetSpeed() const
        public float GetSpeed()
        {
            return speed;
        }


        protected float speed; //move speed of camera in m/s
        protected Vector3 translation = new Vector3();
    }
}