/*=========================================================================

  Program:   Visualization Toolkit
  Module:    $RCSfile: vtkMath.cxx,v $

  Copyright (c) Ken Martin, Will Schroeder, Bill Lorensen
  All rights reserved.
  See Copyright.txt or http://www.kitware.com/Copyright.htm for details.

     This software is distributed WITHOUT ANY WARRANTY; without even
     the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
     PURPOSE.  See the above copyright notice for more information.

=========================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKExtension;

namespace ICPLib
{
    public class MatrixUtilsVTK
    {


        Matrix4 Matrix;

        public const float AXIS_EPSILON = 0.001f;

     


        public void GetPosition(float[] position)
        {

            position[0] = this.Matrix[0, 3];
            position[1] = this.Matrix[1, 3];
            position[2] = this.Matrix[2, 3];
        }

        public void GetScale(float[] scale)
        {
            float[,] U = new float[3, 3];
            float[,] VT = new float[3, 3];

            for (int i = 0; i < 3; i++)
            {
                U[0, i] = Matrix[0, i];
                U[1, i] = Matrix[1, i];
                U[2, i] = Matrix[2, i];
            }

            MathUtilsVTK.SingularValueDecomposition3x3(U, U, scale, VT);
        }

        public void GetOrientation(float[] orientation, Matrix4 amatrix)
        {

            int i;

            // convenient access to matrix
            //float[] matrixElement = amatrix;
            //float[,] ortho = new float[3, 3];
            Matrix3 ortho = new Matrix3();
            for (i = 0; i < 3; i++)
            {
                ortho[0, i] = amatrix[0, i];
                ortho[1, i] = amatrix[1, i];
                ortho[2, i] = amatrix[2, i];
            }

            if (ortho.Determinant < 0)
            {
                ortho[0, 2] = -ortho[0, 2];
                ortho[1, 2] = -ortho[1, 2];
                ortho[2, 2] = -ortho[2, 2];
            }
            float[,] orthoArray = ortho.ToFloatArray();
            MathUtilsVTK.Orthogonalize3x3(orthoArray, orthoArray);

            // first rotate about y axis
            float x2 = ortho[2, 0];
            float y2 = ortho[2, 1];
            float z2 = ortho[2, 2];

            float x3 = ortho[1, 0];
            float y3 = ortho[1, 1];
            float z3 = ortho[1, 2];

            float d1 = Convert.ToSingle(Math.Sqrt(x2 * x2 + z2 * z2));

            float cosTheta;
            float sinTheta;
            if (d1 < AXIS_EPSILON)
            {
                cosTheta = 1.0f;
                sinTheta = 0.0f;
            }
            else
            {
                cosTheta = z2 / d1;
                sinTheta = x2 / d1;
            }

            float theta =Convert.ToSingle( Math.Atan2(sinTheta, cosTheta));
            orientation[1] = -theta / Convert.ToSingle(MathBase.DegreesToRadians);

            // now rotate about x axis
            float d =Convert.ToSingle( Math.Sqrt(x2 * x2 + y2 * y2 + z2 * z2));

            float sinPhi;
            float cosPhi;
            if (d < AXIS_EPSILON)
            {
                sinPhi = 0.0f;
                cosPhi = 1.0f;
            }
            else if (d1 < AXIS_EPSILON)
            {
                sinPhi = y2 / d;
                cosPhi = z2 / d;
            }
            else
            {
                sinPhi = y2 / d;
                cosPhi = (x2 * x2 + z2 * z2) / (d1 * d);
            }

            float phi = Convert.ToSingle(Math.Atan2(sinPhi, cosPhi));
            orientation[0] = Convert.ToSingle(phi / MathBase.DegreesToRadians);

            // finally, rotate about z
            float x3p = x3 * cosTheta - z3 * sinTheta;
            float y3p = -sinPhi * sinTheta * x3 + cosPhi * y3 - sinPhi * cosTheta * z3;
            float d2 = Convert.ToSingle(Math.Sqrt(x3p * x3p + y3p * y3p));

            float cosAlpha;
            float sinAlpha;
            if (d2 < AXIS_EPSILON)
            {
                cosAlpha = 1.0f;
                sinAlpha = 0.0f;
            }
            else
            {
                cosAlpha = y3p / d2;
                sinAlpha = x3p / d2;
            }

            float alpha = Convert.ToSingle(Math.Atan2(sinAlpha, cosAlpha));
            orientation[2] = alpha / MathBase.DegreesToRadians;
        }

        void GetOrientation(float[] orientation)
        {

            this.GetOrientation(orientation, this.Matrix);
        }
        public void GetOrientationWXYZ(float[] wxyz)
        {
            int i;

            Matrix3 ortho = new Matrix3();

            for (i = 0; i < 3; i++)
            {
                ortho[0, i] = Matrix[0, i];
                ortho[1, i] = Matrix[1, i];
                ortho[2, i] = Matrix[2, i];
            }
            if (ortho.Determinant < 0)
            {
                ortho[0, i] = -ortho[0, i];
                ortho[1, i] = -ortho[1, i];
                ortho[2, i] = -ortho[2, i];
            }
            float[,] orthoArray = ortho.ToFloatArray();
            MathUtilsVTK.Matrix3x3ToQuaternion(orthoArray, wxyz);

            // calc the return value wxyz
            float mag = Convert.ToSingle(Math.Sqrt(wxyz[1] * wxyz[1] + wxyz[2] * wxyz[2] + wxyz[3] * wxyz[3]));

            if ((int)mag != 0)
            {
                wxyz[0] =Convert.ToSingle(2* Math.Acos(wxyz[0]) / MathBase.DegreesToRadians);
                wxyz[1] /= mag;
                wxyz[2] /= mag;
                wxyz[3] /= mag;
            }
            else
            {
                wxyz[0] = 0.0f;
                wxyz[1] = 0.0f;
                wxyz[2] = 0.0f;
                wxyz[3] = 1.0f;
            }
        }

     

    }
}

  