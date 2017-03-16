using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging.Filters;
using System.Drawing;
using AForge.Imaging;
using System.Drawing.Imaging;

using AForge;


namespace PointCloudUtils
{
    public class FeatureDetection
    {
        private static FiltersSequence filter = new FiltersSequence(
           Grayscale.CommonAlgorithms.BT709,
           new Threshold(64)
       );

       // private static FiltersSequence filter = new FiltersSequence(
       //    new ColorFiltering(new AForge.IntRange(0,40),  new AForge.IntRange(0,40), new AForge.IntRange(0,40)),
       //    new Threshold(40)
       //);


        private static HoughLineTransformation lineTransform = new HoughLineTransformation();
        private static HoughCircleTransformation circleTransform = new HoughCircleTransformation(35);


        public static bool CheckLines(Bitmap image, Color filterColor)
        {
            EuclideanColorFiltering ColorFilter = new EuclideanColorFiltering();
            // set center colour and radius
            AForge.Imaging.RGB color = new AForge.Imaging.RGB(filterColor.R, filterColor.G, filterColor.B, filterColor.A);
            ColorFilter.CenterColor = color;
            ColorFilter.Radius = 100;
            // Apply the filter
            ColorFilter.ApplyInPlace(image);

            // Define the Blob counter and use it!
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.MinWidth = 5;
            blobCounter.MinHeight = 5;
            blobCounter.FilterBlobs = true;
            //blobCounter.ObjectsOrder = ObjectsOrder.Size;
            blobCounter.ProcessImage(image);
            System.Drawing.Rectangle[] rects = blobCounter.GetObjectsRectangles();
            if (rects.Length > 0)
            {
                return true;
            }
            return false;
        }
        public static void Lines(Bitmap binarySource)
        {
            HoughLineTransformation lineTransform = new HoughLineTransformation();
            //lineTransform. = 10;
            // apply Hough line transofrm
            lineTransform.ProcessImage(binarySource);
            HoughLine[] lines = lineTransform.GetLinesByRelativeIntensity(0.5);
            if (lines.Count() > 0)
            {
                //Result += "NW: Yes!\n";
            }
            else
            {
                //Result += "NW: No!\n";
            }
        }
        public static Bitmap DetectLines(Bitmap tempImage)
        {
            Bitmap image = tempImage;

            try
            {
                // show file open dialog

                // load image
                //Bitmap tempImage = (Bitmap)Bitmap.FromFile(openFileDialog.FileName);
                image = AForge.Imaging.Image.Clone(tempImage, PixelFormat.Format24bppRgb);
                //tempImage.Dispose();
                // format image
                
                // lock the source image
                BitmapData sourceData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly, image.PixelFormat);
                // binarize the image
                //UnmanagedImage binarySource = new UnmanagedImage(sourceData);
                UnmanagedImage binarySource = filter.Apply(new UnmanagedImage(sourceData));

                // apply Hough line transofrm
                lineTransform.ProcessImage(binarySource);
                // get lines using relative intensity
                HoughLine[] lines = lineTransform.GetLinesByRelativeIntensity(0.2);

                foreach (HoughLine line in lines)
                {
                    //if(line.Le)
                    string s = string.Format("Theta = {0}, R = {1}, I = {2} ({3})", line.Theta, line.Radius, line.Intensity, line.RelativeIntensity);
                    System.Diagnostics.Debug.WriteLine(s);

                    // uncomment to highlight detected lines

                    // get line's radius and theta values
                    int r = line.Radius;
                    double t = line.Theta;

                    // check if line is in lower part of the image
                    if (r < 0)
                    {
                        t += 180;
                        r = -r;
                    }

                    // convert degrees to radians
                    t = (t / 180) * Math.PI;

                    // get image centers (all coordinate are measured relative
                    // to center)
                    int w2 = image.Width / 2;
                    int h2 = image.Height / 2;

                    double x0 = 0, x1 = 0, y0 = 0, y1 = 0;

                    if (line.Theta != 0)
                    {
                        // none vertical line
                        x0 = -w2; // most left point
                        x1 = w2;  // most right point

                        // calculate corresponding y values
                        y0 = (-Math.Cos(t) * x0 + r) / Math.Sin(t);
                        y1 = (-Math.Cos(t) * x1 + r) / Math.Sin(t);
                    }
                    else
                    {
                        // vertical line
                        x0 = line.Radius;
                        x1 = line.Radius;

                        y0 = h2;
                        y1 = -h2;
                    }

                    // draw line on the image
                    Drawing.Line(sourceData,
                        new IntPoint((int)x0 + w2, h2 - (int)y0),
                        new IntPoint((int)x1 + w2, h2 - (int)y1),
                        Color.Red);
                }

                System.Diagnostics.Debug.WriteLine("Found lines: " + lineTransform.LinesCount);
                System.Diagnostics.Debug.WriteLine("Max intensity: " + lineTransform.MaxIntensity);

              
                //FindCircles(binarySource);
                // unlock source image
                image.UnlockBits(sourceData);
                // dispose temporary binary source image
                binarySource.Dispose();

                // show images
                //sourcePictureBox.Image = image;
                //houghLinePictureBox.Image = lineTransform.ToBitmap();
                //houghCirclePictureBox.Image = circleTransform.ToBitmap();

            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Failed loading the image", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return image;
        }
        private static void FindCircles(UnmanagedImage binarySource)
        {
            // apply Hough circle transform
            circleTransform.ProcessImage(binarySource);
            // get circles using relative intensity
            HoughCircle[] circles = circleTransform.GetCirclesByRelativeIntensity(0.5);

            foreach (HoughCircle circle in circles)
            {
                string s = string.Format("X = {0}, Y = {1}, I = {2} ({3})", circle.X, circle.Y, circle.Intensity, circle.RelativeIntensity);
                System.Diagnostics.Debug.WriteLine(s);
            }

            System.Diagnostics.Debug.WriteLine("Found circles: " + circleTransform.CirclesCount);
            System.Diagnostics.Debug.WriteLine("Max intensity: " + circleTransform.MaxIntensity);


        }
    }
}
