using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace OpenTKExtension
{
    public class Camera
    {
   
        public float ZFar = 1000f;
        public float ZNear = 0.5f;
        //public float FieldOfView = Convert.ToSingle(Math.PI / 4);

        public float FieldOfView = Convert.ToSingle(Math.PI /8);

        public int XTrans, YTrans, XRot, YRot;
        public float Xangle, Yangle;

        public bool MouseLeftDown = false;
        public bool MouseRightDown = false;

        public bool Recalculate = true;  // true if the viewMatrix must be recalculated

        private Vector3 position;
        private QuaternionNew centerOfInterest;

        private Matrix4 v; 
        private Matrix4 p;
        private Matrix4 m;
        
        private Matrix4 mvp;

        private float perspectiveAspectRatio;
  
        public float MoveSpeed = 4e-4f;

        /// <summary>
        /// Create a new camera object with a certain eye position and orientation.
        /// </summary>
        public Camera()
        {
            Reset();
        }

        /// <summary>
        /// Modify the position of the eye of the camera.
        /// Triggers the view matrix to be recalculated.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                Recalculate = true;
            }
        }

        /// <summary>
        /// Modify the orientation of the camera.
        /// Triggers the view matrix to be recalculated.
        /// </summary>
        public QuaternionNew CenterOfInterest
        {
            get { return centerOfInterest; }
            set
            {
                centerOfInterest = value;
                Recalculate = true;
            }
        }

        /// <summary>
        /// Requests the view matrix of the camera (taking into account position and orientation).
        /// This property will recalculate the matrix automatically if necessary,
        /// otherwise it will use a cached copy (which is useful if your camera isn't moving often).
        /// </summary>
        public Matrix4 V
        {
            get
            {
                if (Recalculate)
                {
                    v = Matrix4.CreateTranslation(-position) * centerOfInterest.Matrix4;
                    Recalculate = false;
                }
                return v;
            }
        }
        /// <summary>
        /// Requests the view matrix of the camera (taking into account position and orientation).
        /// This property will recalculate the matrix automatically if necessary,
        /// otherwise it will use a cached copy (which is useful if your camera isn't moving often).
        /// </summary>
        public Matrix4 P
        {
            get
            {
                return p;
            }
        }
        /// <summary>
        /// Requests the view matrix of the camera (taking into account position and orientation).
        /// This property will recalculate the matrix automatically if necessary,
        /// otherwise it will use a cached copy (which is useful if your camera isn't moving often).
        /// </summary>
        public Matrix4 M
        {
            get
            {
                m = Matrix4.CreateRotationY(Yangle) * Matrix4.CreateRotationX(Xangle);
              
                return m;
            }
        }
        public Matrix4 MVP
        {
            get
            {
                mvp = M * V * p;

                return mvp;
            }
        }
     
        public void PerspectiveUpdate(int width, int height)
        {
            if (height != 0)
            {
                perspectiveAspectRatio = width / Convert.ToSingle(height);
                this.p = Matrix4.CreatePerspectiveFieldOfView(FieldOfView, perspectiveAspectRatio, ZNear, ZFar);
            }
             
        }
        public void PerspectiveUpdate()
        {

            try
            {
                this.p = Matrix4.CreatePerspectiveFieldOfView(FieldOfView, perspectiveAspectRatio, ZNear, ZFar);

            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("SW Error updating perspectice " + err.Message);
            }

        }

      

        /// <summary>
        /// Modifies the orientation of the camera to get the camera to look in a particular direction.
        /// </summary>
        /// <param name="direction">The direction to have the camera look.</param>
        public void SetDirection(Vector3 direction)
        {
            if (direction == Vector3.Zero) return;

            Vector3 zvec = -direction.NormalizeV();
            Vector3 xvec = zvec.Up().CrossProduct(zvec).NormalizeV();
            Vector3 yvec = zvec.CrossProduct(xvec).NormalizeV();
            CenterOfInterest = QuaternionNew.FromAxis(xvec, yvec, zvec);
        }

        /// <summary>
        /// Moves the camera by modifying the position directly.
        /// </summary>
        /// <param name="by">The amount to move the position by.</param>
        public void Move(Vector3 by)
        {
            Position += by;
            AdjustPositionToMinMax();
           
        }

        /// <summary>
        /// Moves the camera taking into account the orientation of the camera.
        /// This is useful if you want to move in the direction that the camera is facing.
        /// </summary>
        /// <param name="by">The amount to move the position by, relative to the camera.</param>
        public void MoveRelative(Vector3 by)
        {
           
            Position += CenterOfInterest * by;

            AdjustPositionToMinMax();

        }
        private void AdjustPositionToMinMax()
        {
            //if (position.Z < ZNear)
            //    position.Z = ZNear;
            if (position.Z > ZFar - 5)
                position.Z = ZFar - 5;
            //System.Diagnostics.Debug.WriteLine("Position z " + position.Z.ToString());
        }
        /// <summary>
        /// Rotates the camera by a supplied rotation quaternion.
        /// </summary>
        /// <param name="rotation">The amount to rotate the camera by.</param>
        public void Rotate(QuaternionNew rotation)
        {
            CenterOfInterest = rotation * centerOfInterest;
        }

        /// <summary>
        /// Rotates the camera around a specific axis.
        /// </summary>
        /// <param name="angle">The amount to rotate the camera.</param>
        /// <param name="axis">The axis about which the rotation occurs.</param>
        public void Rotate(float angle, Vector3 axis)
        {
            Rotate(QuaternionNew.FromAngleAxis(angle, axis));
        }

        /// <summary>
        /// Rotates the camera about the Z axis.
        /// </summary>
        /// <param name="angle">The amount to rotate the camera by.</param>
        public void Roll(float angle)
        {
            angle = angle * MoveSpeed;
            Vector3 axis = centerOfInterest * Vector3.UnitZ;
            Rotate(angle, axis);
        }

        /// <summary>
        /// Rotates the camera about the Y axis.  Assumes a fixed Y axis of Vector3.Up.
        /// </summary>
        /// <param name="angle">The amount to rotate the camera by.</param>
        public void Yaw(float angle)
        {
            // this method assumes that the y direction will always be 'up', so we've fixed the yaw
            // which is more useful for FPS games, etc.  For flight simulators, or other applications
            // of an unfixed yaw, simply replace Vector3.Up with (orientation * Vector3.UnitY)
            angle = angle * MoveSpeed;
            Rotate(angle, new Vector3().Up());
        }

        /// <summary>
        /// Rotates the camera about the X axis.
        /// </summary>
        /// <param name="angle">The amount to rotate the camera by.</param>
        public void Pitch(float angle)
        {
            angle = angle * MoveSpeed;
            Vector3 axis = centerOfInterest * Vector3.UnitX;
            Rotate(angle, axis);
        }
        public void Reset()
        {
            this.Position = new Vector3(0, 0, -10);
            this.CenterOfInterest = QuaternionNew.Identity;

         
        }
    }
}
