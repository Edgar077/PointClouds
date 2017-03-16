using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PointCloudUtils
{
    public class ColorMap
    {
        private byte colormapLength = 64;
        private byte alphaValue = 255;

        public ColorMap()
        {
        }

        public ColorMap(byte colorLength)
        {
            colormapLength = colorLength;
        }

        public ColorMap(byte colorLength, byte alpha)
        {
            colormapLength = colorLength;
            alphaValue = alpha;
        }



        public byte[,] Gray()
        {
            byte[,] cmap = new byte[colormapLength, 4];
            double[] gray = new double[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                gray[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (byte)(255 * gray[i]);
                cmap[i, 2] = (byte)(255 * gray[i]);
                cmap[i, 3] = (byte)(255 * gray[i]);
            }
            return cmap;
        }

   

        public byte[,] YellowRedBlack()
        {
            byte[,] cmap = new byte[colormapLength, 4];
            int n = 3 * colormapLength / 8;
            double[] red = new double[colormapLength];
            double[] green = new double[colormapLength];
            double[] blue = new double[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                if (i < n)
                {
                    red[i] = 1.0f * (i + 1) / n;
                }
                else
                    red[i] = 1.0f;
                if (i < n)
                    green[i] = 0f;
                else if (i >= n && i < 2 * n)
                    green[i] = 1.0f * (i + 1 - n) / n;
                else
                    green[i] = 1f;
                if (i < 2 * n)
                    blue[i] = 0f;
                else
                    blue[i] = 1.0f * (i + 1 - 2 * n) / (colormapLength - 2 * n);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (byte)(255 * red[i]);
                cmap[i, 2] = (byte)(255 * green[i]);
                cmap[i, 3] = (byte)(255 * blue[i]);
            }
            return cmap;
        }

        public byte[,] VioletAzure()
        {
            byte[,] cmap = new byte[colormapLength, 4];
            double[] cool = new double[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                cool[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (byte)(255 * cool[i]);
                cmap[i, 2] = (byte)(255 * (1 - cool[i]));
                cmap[i, 3] = 255;
            }
            return cmap;
        }
        public byte[,] YellowRead()
        {
            byte[,] cmap = new byte[colormapLength, 4];
            double[] spring = new double[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                spring[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = 255;
                cmap[i, 2] = (byte)(255 * spring[i]);
                cmap[i, 3] = (byte)(255 - cmap[i, 1]);
            }
            return cmap;
        }

        public byte[,] YellowGreen()
        {
            byte[,] cmap = new byte[colormapLength, 4];
            double[] summer = new double[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                summer[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (byte)(255 * summer[i]);
                cmap[i, 2] = (byte)(255 * 0.5f * (1 + summer[i]));
                cmap[i, 3] = (byte)(255 * 0.4f);
            }
            return cmap;
        }

        public byte[,] RedYellow()
        {
            byte[,] cmap = new byte[colormapLength, 4];
            double[] autumn = new double[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                autumn[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = 255;
                cmap[i, 2] = (byte)(255 - (byte)(255 * autumn[i]));
                cmap[i, 3] = 0;
            }
            return cmap;
        }

        public byte[,] GreenBlue()
        {
            byte[,] cmap = new byte[colormapLength, 4];
            double[] winter = new double[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                winter[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = 0;
                cmap[i, 2] = (byte)(255 * winter[i]);
                cmap[i, 3] = (byte)(255 * (1.0f - 0.5f * winter[i]));
            }
            return cmap;
        }

        public byte[,] AllColors()
        {
            byte[,] cmap = new byte[colormapLength, 4];
            double[,] cMatrix = new double[colormapLength, 3];
            int n = (int)Math.Ceiling(colormapLength / 4.0f);
            int nMod = 0;
            double[] fArray = new double[3 * n - 1];
            int[] red = new int[fArray.Length];
            int[] green = new int[fArray.Length];
            int[] blue = new int[fArray.Length];

            if (colormapLength % 4 == 1)
            {
                nMod = 1;
            }

            for (int i = 0; i < fArray.Length; i++)
            {
                if (i < n)
                    fArray[i] = (double)(i + 1) / n;
                else if (i >= n && i < 2 * n - 1)
                    fArray[i] = 1.0f;
                else if (i >= 2 * n - 1)
                    fArray[i] = (double)(3 * n - 1 - i) / n;
                green[i] = (int)Math.Ceiling(n / 2.0f) - nMod + i;
                red[i] = green[i] + n;
                blue[i] = green[i] - n;
            }

            int nb = 0;
            for (int i = 0; i < blue.Length; i++)
            {
                if (blue[i] > 0)
                    nb++;
            }

            for (int i = 0; i < colormapLength; i++)
            {
                for (int j = 0; j < red.Length; j++)
                {
                    if (i == red[j] && red[j] < colormapLength)
                    {
                        cMatrix[i, 0] = fArray[i - red[0]];
                    }
                }
                for (int j = 0; j < green.Length; j++)
                {
                    if (i == green[j] && green[j] < colormapLength)
                        cMatrix[i, 1] = fArray[i - (int)green[0]];
                }
                for (int j = 0; j < blue.Length; j++)
                {
                    if (i == blue[j] && blue[j] >= 0)
                        cMatrix[i, 2] = fArray[fArray.Length - 1 - nb + i];
                }
            }

            for (int i = 0; i < colormapLength; i++)
            {
                cmap[i, 0] = alphaValue;
                for (int j = 0; j < 3; j++)
                {
                    cmap[i, j + 1] = (byte)(cMatrix[i, j] * 255);
                }
            }
            return cmap;
        }


     

    }
}
