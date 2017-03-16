using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace OpenTK.Extension
{
    public class Camera
    {
        private Vector3 position;
        private QuaternionNew centerOfInterest;
        public float zFar = 1000f;
        public float zNear = 0.001f;
        //public float fieldOfView = Convert.ToSingle(Math.PI / 4);
        public float fieldOfView = 0.45f;

        

        private Matrix4 v; // a cached version of the view matrix
        private Matrix4 p;
        private Matrix4 m;

        private Matrix4 mvp;

        public bool Recalculate = true;  // true if the viewMatrix must be recalculated

        public int XTrans, YTrans, XRot, YRot;
        public float Xangle, Yangle;

        public bool mouseLeftDown = false;
        public bool mouseRightDown = false;

        private float moveSpeed = 4e-4f;

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
                //mvp = M * V * P;
                mvp = P * V * M;
                return mvp;
            }
        }

        public void PerspectiveUpdate(int width, int height)
        {
            if (height != 0)
            {
                float aspect = width / Convert.ToSingle(height);
                this.p = Matrix4.CreatePerspectiveFieldOfView(fieldOfView, aspect, zNear, zFar);
            }
            //shaderProgram["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)GLSettings.Width / GLSettings.Height, 0.1f, 1000f));
            //shaderProgram["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up));

        }



        /// <summary>
        /// Modifies the orientation of the camera to get the camera to look in a particular direction.
        /// </summary>
        /// <param name="direction">The direction to have the camera look.</param>
        public void SetDirection(Vector3 direction)
        {
            if (direction == Vector3.Zero) return;

            Vector3 zvec = -direction.Normalize();
            
            Vector3 xvec = Vector3.Up.Cross(zvec).Normalize();
            Vector3 yvec = zvec.Cross(xvec).Normalize();
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
            if (position.Z < zNear)
                position.Z = zNear;
            if (position.Z > zFar - 5)
                position.Z = zNear - 5;
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
            angle = angle * moveSpeed;
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
            angle = angle * moveSpeed;
            Rotate(angle, Vector3.Up);
        }

        /// <summary>
        /// Rotates the camera about the X axis.
        /// </summary>
        /// <param name="angle">The amount to rotate the camera by.</param>
        public void Pitch(float angle)
        {
            angle = angle * moveSpeed;
            Vector3 axis = centerOfInterest * Vector3.UnitX;
            Rotate(angle, axis);
        }
        public void Reset()
        {
            this.Position = new Vector3(0, 0, 10);
            this.CenterOfInterest = QuaternionNew.Identity;


        }
    }
}
