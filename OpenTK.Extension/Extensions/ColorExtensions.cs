
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKExtension
{
    //Extensios attached to the object which folloes the "this" 
    public static class ColorExtensions
    {

        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static System.Drawing.Color FromfloatsRGB(this System.Drawing.Color color, float r, float g, float b)
        {
            int ri = Convert.ToInt32(r * byte.MaxValue);
            int rg = Convert.ToInt32(g * byte.MaxValue);
            int rb = Convert.ToInt32(b * byte.MaxValue);
            color = System.Drawing.Color.FromArgb(ri, rg, rb);

            return color;
            
        }
        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static System.Drawing.Color FromFloatsARGB(this System.Drawing.Color color, float a, float r,float g, float b)
        {
            byte ra = Convert.ToByte(a * byte.MaxValue);
            byte ri = Convert.ToByte(r * byte.MaxValue);
            byte rg = Convert.ToByte(g * byte.MaxValue);
            byte rb = Convert.ToByte(r * byte.MaxValue);
            color = System.Drawing.Color.FromArgb(ra, ri, rg, rb);

            return color;

        }
        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static System.Drawing.Color FromfloatARGB(this System.Drawing.Color color, float a, float r, float g, float b)
        {
            byte ra = Convert.ToByte(a * byte.MaxValue);
            byte ri = Convert.ToByte(r * byte.MaxValue);
            byte rg = Convert.ToByte(g * byte.MaxValue);
            byte rb = Convert.ToByte(r * byte.MaxValue);
            color = System.Drawing.Color.FromArgb(ra, ri, rg, rb);

            return color;

        }
        ///// <summary>Transform a direction vector by the given Matrix
        ///// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        ///// </summary>
        ///// <param name="vec">The vector to transform</param>
        ///// <param name="mat">The desired transformation</param>
        ///// <returns>The transformed vector</returns>
        //public static System.Drawing.Color FromBytes(this System.Drawing.Color color, byte r, byte g, byte b)
        //{
        //    int ri = Convert.ToInt32(r * byte.MaxValue);
        //    int rg = Convert.ToInt32(r * byte.MaxValue);
        //    int rb = Convert.ToInt32(r * byte.MaxValue);
        //    color = new System.Drawing.Color()

        //    return color;

        //}
        public static float[] ToFloats(this System.Drawing.Color color)
        {
            float[] myColorF = new float[4];

            myColorF[0] = color.R * 1F / byte.MaxValue;
            myColorF[1] = color.G * 1F/ byte.MaxValue;
            myColorF[2] = color.B * 1F / byte.MaxValue;
            myColorF[3] = color.A * 1F / byte.MaxValue;

           
            return myColorF;
            
        }

        public static float[] Tofloats(this System.Drawing.Color color)
        {
            float[] myColorF = new float[4];

            myColorF[0] = color.R * 1F / byte.MaxValue;
            myColorF[1] = color.G * 1F / byte.MaxValue;
            myColorF[2] = color.B * 1F / byte.MaxValue;
            myColorF[3] = color.A * 1F / byte.MaxValue;


            return myColorF;

        }
        public static List<float[]> ToColorInfo(byte[] arrayColor, ushort[] arrayDepth, int width, int height)
        {

            int BYTES_PER_PIXEL = (System.Windows.Media.PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

            List<float[]> listOfColors = new List<float[]>();
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    int depthIndex = (y * width) + x;
                    int colorIndex = depthIndex * BYTES_PER_PIXEL;
                    ushort z = arrayDepth[depthIndex];
                    if (z > 0)
                    {
                        float[] color = new float[4] { 0, 0, 0, 0 };
                        color[0] = Convert.ToSingle(arrayColor[colorIndex]) / 255F;
                        color[1] = Convert.ToSingle(arrayColor[colorIndex + 1]) / 255F;
                        color[2] = Convert.ToSingle(arrayColor[colorIndex + 2]) / 255F;
                        color[3] = 1F;
                        listOfColors.Add(color);

                    }

                }
            }

            return listOfColors;
        }
        public static List<System.Drawing.Color> ToColorList(int numberOfItems, byte r, byte g, byte b, byte a)
        {

            int BYTES_PER_PIXEL = (System.Windows.Media.PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

            List<System.Drawing.Color> listOfColors = new List<System.Drawing.Color>();
            System.Drawing.Color color = System.Drawing.Color.FromArgb(a, r, g, b);


            for (int i = 0; i < numberOfItems; ++i)
            {
                listOfColors.Add(color);
            }

            return listOfColors;
        }
        public static Vector3[] ToVector3Array(int numberOfItems, byte r, byte g, byte b)
        {

            int BYTES_PER_PIXEL = (System.Windows.Media.PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

            Vector3[] listOfColors = new Vector3[numberOfItems];

            Vector3 v = new Vector3(r, g, b);

            //System.Drawing.Color color = System.Drawing.Color.FromArgb(a, r, g, b);
            for (int i = 0; i < numberOfItems; ++i)
            {
                listOfColors[i] = v;
            }

            return listOfColors;
        }
     
       
    }
}
