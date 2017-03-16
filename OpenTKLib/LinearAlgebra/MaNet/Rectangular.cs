using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace OpenTKExtension
{
  public  class Rectangular
    {

        public Random Random
        {
            get { return rand; }
            set { rand = value; }


        }

        protected Random rand = new Random();

        public Matrix3 Randomfloat(int m)
        {
            return Randomfloat(m, m, 0, 1);

        }

        public Matrix3 Randomfloat(int m, int n)
        {
            return Randomfloat(m, n, 0, 1);

        }


        public Matrix3 Randomfloat(int m, int n, float min, float max)
        {
            Matrix3 A = new Matrix3();
            float[,] X = A.ToFloatArray();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    X[i,j] = min + Convert.ToSingle(rand.NextDouble()) * (max - min);
                }
            }
            return A;
        }


        public Matrix3 RandomInt(int m, int n, int min, int max)
        {
            Matrix3 A = new Matrix3();
            float[,] X = A.ToFloatArray();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    X[i,j] = rand.Next(min, max);
                }
            }
            return A;

        }

        public Matrix3 RandomInt(int m, int n)
        {
            return RandomInt(m, n, 0, 9);
        }

        public Matrix3 RandomInt(int m )
        {
            return RandomInt(m, m, 0, 9);
        }
    }
}
