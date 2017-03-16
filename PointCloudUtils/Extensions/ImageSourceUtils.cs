using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Drawing;

namespace PointCloudUtils
{
    public static class ImageSourceUtils
    {
        public static ImageSource CreateImageSource(byte[] _colorData, int width, int height)
        {
            PixelFormat format = PixelFormats.Bgr32;

            int stride = width * format.BitsPerPixel / 8;
            try
            {
                return BitmapSource.Create(width, height, 96, 96, format, null, _colorData, stride);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error " + err.Message);
                return null;
            }
        }
    }
}
