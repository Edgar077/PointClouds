using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Threading.Tasks;


namespace PointCloudUtils
{
    public static class Extensions
    {
        #region Mesh

      
        /// <summary>
        /// Converts a 16-bit grayscale depth frame which includes player indexes into a 3D point array: depthFramePoints
        /// </summary>
        /// <param name="depthFrame">The depth frame.</param>
        /// <param name="depthStream">The depth stream.</param>
        private static Point3D[]  ConvertDepthFrameTo3D(  DepthFrame frameDepth)
        {
            ushort[] depthPixelData = new ushort[frameDepth.FrameDescription.LengthInPixels];
            int[] rawDepth = new int[frameDepth.FrameDescription.LengthInPixels];
            
            Point3D[] depthFramePoints = new Point3D[frameDepth.FrameDescription.LengthInPixels];


            int tooNearDepth = 100;
            int tooFarDepth = 8000;
            int unknownDepth = 0;

            int width = frameDepth.FrameDescription.Width;
            int height = frameDepth.FrameDescription.Height;

            int cx = width / 2;
            int cy = height / 2;

            double fxinv = 1/ 476f;
            double fyinv = 1/ 476f;

            double scale = 0.001f;

            Parallel.For(
                0,
                height,
                iy =>
                {
                    for (int ix = 0; ix < width; ix++)
                    {
                        int i = (iy * width) + ix;
                        //this.rawDepth[i] = depthFrame[(iy * width) + ix] >> DepthImageFrame.PlayerIndexBitmaskWidth;
                        rawDepth[i] = depthPixelData[(iy * width) + ix];


                        if (rawDepth[i] == unknownDepth || rawDepth[i] < tooNearDepth || rawDepth[i] > tooFarDepth)
                        {
                            rawDepth[i] = -1;
                            depthFramePoints[i] = new Point3D();
                        }
                        else
                        {
                            double zz = rawDepth[i] * scale;
                            double x = (cx - ix) * zz * fxinv;
                            double y = zz;
                            double z = (cy - iy) * zz * fyinv;
                            depthFramePoints[i] = new Point3D(x, y, z);
                        }
                    }
                });
            return depthFramePoints;

        }

        #endregion

        #region Camera

        //int height = frame.FrameDescription.Height;
        public static byte[] ToByteArray(this ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;//1920
            int height = frame.FrameDescription.Height;//1080
            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            //int stride = width * format.BitsPerPixel / 8;
            return pixels;

        }
     
      

        public static ImageSource ToBitmap(this DepthFrame frame)
        {
            int width = frame.FrameDescription.Width;//512
            int height = frame.FrameDescription.Height;//424

            PixelFormat format = PixelFormats.Bgr32;

            ushort minDepth = frame.DepthMinReliableDistance;//500
            ushort maxDepth = frame.DepthMaxReliableDistance;//4500

            ushort[] pixelData = new ushort[width * height];
            byte[] pixels = new byte[width * height * (format.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(pixelData);

            int colorIndex = 0;
            for (int depthIndex = 0; depthIndex < pixelData.Length; ++depthIndex)
            {
                ushort depth = pixelData[depthIndex];

                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                pixels[colorIndex++] = intensity; // Blue
                pixels[colorIndex++] = intensity; // Green
                pixels[colorIndex++] = intensity; // Red

                ++colorIndex;
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }

        public static ImageSource ToBitmap(this InfraredFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            ushort[] frameData = new ushort[width * height];
            byte[] pixels = new byte[width * height * (format.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(frameData);

            int colorIndex = 0;
            for (int infraredIndex = 0; infraredIndex < frameData.Length; infraredIndex++)
            {
                ushort ir = frameData[infraredIndex];

                byte intensity = (byte)(ir >> 7);

                pixels[colorIndex++] = (byte)(intensity / 1); // Blue
                pixels[colorIndex++] = (byte)(intensity / 1); // Green   
                pixels[colorIndex++] = (byte)(intensity / 0.4); // Red

                colorIndex++;
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }

        #endregion

        #region Body

        public static Joint ScaleTo(this Joint joint, double width, double height, double skeletonMaxX, double skeletonMaxY)
        {
            joint.Position = new CameraSpacePoint
            {
                X = Scale(width, skeletonMaxX, Convert.ToDouble(joint.Position.X)),
                Y = Scale(height, skeletonMaxY, -joint.Position.Y),
                Z = joint.Position.Z
            };

            return joint;
        }

        public static Joint ScaleTo(this Joint joint, double width, double height)
        {
            return ScaleTo(joint, width, height, 1.0f, 1.0f);
        }

        private static float Scale(double maxPixel, double maxSkeleton, double position)
        {
            float value = (float)((((maxPixel / maxSkeleton) / 2) * position) + (maxPixel / 2));

            if (value > maxPixel)
            {
                return (float)maxPixel;
            }

            if (value < 0)
            {
                return 0;
            }

            return value;
        }

        #endregion

        #region Drawing

        public static void DrawSkeleton(this Canvas canvas, Body body)
        {
            if (body == null) return;

            foreach (Joint joint in body.Joints.Values)
            {
                canvas.DrawPoint(joint);
            }

            canvas.DrawLine(body.Joints[JointType.Head], body.Joints[JointType.Neck]);
            canvas.DrawLine(body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder]);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft]);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight]);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid]);
            canvas.DrawLine(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft]);
            canvas.DrawLine(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight]);
            canvas.DrawLine(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft]);
            canvas.DrawLine(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight]);
            canvas.DrawLine(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft]);
            canvas.DrawLine(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]);
            canvas.DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft]);
            canvas.DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight]);
            canvas.DrawLine(body.Joints[JointType.HandTipLeft], body.Joints[JointType.ThumbLeft]);
            canvas.DrawLine(body.Joints[JointType.HandTipRight], body.Joints[JointType.ThumbRight]);
            canvas.DrawLine(body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase]);
            canvas.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft]);
            canvas.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight]);
            canvas.DrawLine(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft]);
            canvas.DrawLine(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]);
            canvas.DrawLine(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft]);
            canvas.DrawLine(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight]);
            canvas.DrawLine(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft]);
            canvas.DrawLine(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight]);
        }

        public static void DrawPoint(this Canvas canvas, Joint joint)
        {
            if (joint.TrackingState == TrackingState.NotTracked) return;

            joint = joint.ScaleTo(Convert.ToSingle(canvas.ActualWidth), Convert.ToSingle(canvas.ActualHeight));

            Ellipse ellipse = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = new SolidColorBrush(Colors.LightBlue)
            };

            Canvas.SetLeft(ellipse, joint.Position.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, joint.Position.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

        public static void DrawLine(this Canvas canvas, Joint first, Joint second)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

            first = first.ScaleTo(Convert.ToSingle(canvas.ActualWidth), Convert.ToSingle(canvas.ActualHeight));
            second = second.ScaleTo(Convert.ToSingle(canvas.ActualWidth), Convert.ToSingle(canvas.ActualHeight));

            Line line = new Line
            {
                X1 = first.Position.X,
                Y1 = first.Position.Y,
                X2 = second.Position.X,
                Y2 = second.Position.Y,
                StrokeThickness = 8,
                Stroke = new SolidColorBrush(Colors.LightBlue)
            };

            canvas.Children.Add(line);
        }

        #endregion
    }
}
