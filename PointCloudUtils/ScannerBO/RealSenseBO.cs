using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Controls;
using AForge.Video.DirectShow;
using AForge.Video;

using OpenTKExtension;
using OpenTK;
using System.Drawing;
using System.Diagnostics;

namespace PointCloudUtils
{
    public class RealsenseBO : ScannerBase
    {
       
        public int NumberOfDevices;
        public FilterInfoCollection videoDevices;
        public List<VideoCaptureDevice> DevicesDepth;
        public List<VideoCaptureDevice> DevicesColor;

        

        System.Windows.Forms.PictureBox pictureBoxColor;
        System.Windows.Forms.PictureBox pictureBoxDepth;
        System.Windows.Forms.PictureBox pictureBoxIR;
        System.Windows.Forms.Label cameraFpsLabel;
        
        System.Windows.Forms.Control parentControl;

        public List<string> CameraStrings;
        private delegate void UpdateShowFormOpenGLDelegate();

        int openGLCounter;
        OGLControl openGLControl;

        //bool ShowingIn3DControl;

        public DisplayType DisplayType;

    


        private Size depthFrameSize = new Size(640, 480);
        //private Size colorFrameSize = new Size(1920, 1080);
        private Size colorFrameSize = new Size(640, 480);

      

        byte[] depthForImage;
        byte[] infrared;
        private Stopwatch stopWatch = null;

        private int scannerID = 0;
        PointCloud pointCloudBase;
        
        System.Timers.Timer timer = new System.Timers.Timer();
        VideoSourcePlayer videoSourcePlayerDepth;
        VideoSourcePlayer videoSourcePlayerColor;

        ///  VideoSourceType VideoSourceType;
        private Matrix3 DepthConversionMatrix;
        private Matrix3 ColorConversionMatrix;

        
        //private Bitmap bitmapIR;
        private Bitmap bitmapIRSave;
       
        private Bitmap bitmapDepthSave;
        private Bitmap bitmapColorForSave;

        private bool colorPendingForSave;
        private bool depthPendingForSave;
       
        //private bool prepareSave;

        public RealsenseBO()
        {
            CameraStrings = new List<string>();
        }
        public RealsenseBO(System.Windows.Forms.Control myParentControl , System.Windows.Forms.PictureBox myPictureBoxColor, System.Windows.Forms.PictureBox myPictureBoxDepth, System.Windows.Forms.PictureBox mypictureBoxIR, OGLControl myOpenGLControl,
            System.Windows.Forms.Label mycameraFpsLabel):this()
        {
            parentControl = myParentControl;
            openGLPart = new OpenGLPart(this, myParentControl, myOpenGLControl);
            DepthMetaData = new DepthMetaData();
            PathModels = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + GLSettings.PathPointClouds;


            try
            {

                pictureBoxDepth = myPictureBoxDepth;
                pictureBoxColor = myPictureBoxColor;
                pictureBoxIR = mypictureBoxIR;
                cameraFpsLabel = mycameraFpsLabel;
                openGLControl = myOpenGLControl;
               

                GetRealSenseCameras(out DevicesDepth, out DevicesColor , out videoDevices);
                NumberOfDevices = DevicesDepth.Count;
                

                SetConversionMatrices();
                
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("SW Error initializing Scanner : " + err.Message);
            }
        }

        public override bool StartScanner()
        {
            colorPendingForSave = false;
            depthPendingForSave = false;
            //prepareSave = false;
            isScanning = true;
            if (this.DevicesDepth.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No Scanner connected to computer");
                return false;
            }
            else
            {
                VideoCaptureDevice videoCaptureDepth = this.DevicesDepth[ScannerID];
                videoSourcePlayerDepth = new VideoSourcePlayer();
                videoSourcePlayerDepth.VideoSource = videoCaptureDepth;

                //   SetFrameSize(videoCaptureDepth, this.depthFrameSize);
                videoCaptureDepth.SourceType = VideoSourceType.Depth;

                videoCaptureDepth.NewFrame += new AForge.Video.NewFrameEventHandler(OnNewFrameDepth);


                videoSourcePlayerColor = new VideoSourcePlayer();

                VideoCaptureDevice videoCaptureColor = this.DevicesColor[ScannerID];
                videoSourcePlayerColor.VideoSource = videoCaptureColor;
                videoCaptureColor.NewFrame += new AForge.Video.NewFrameEventHandler(OnNewFrameColor);
                SetFrameSize(videoCaptureColor, this.colorFrameSize);

                // videoSource1.DesiredPixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;


                videoSourcePlayerColor.Start();
                videoSourcePlayerDepth.Start();
                // reset stop watch
                stopWatch = null;
                // start timer
                timer.Start();
                return true;

            }
        }

        // Stop cameras
        public override void StopScanner()
        {
            timer.Stop();
            isScanning = false;
            if (videoSourcePlayerDepth != null)
            {
                videoSourcePlayerDepth.SignalToStop();
                videoSourcePlayerDepth.WaitForStop();
            }
            if (videoSourcePlayerColor != null)
            {
                videoSourcePlayerColor.SignalToStop();
                videoSourcePlayerColor.WaitForStop();
            }


        }
        public override bool SaveAll()
        {
            //stop scanner done in SaveBitmaps
            SaveBitmaps();
            PointCloudScannerSettings.FileNameOBJ = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "_PointCloud.obj";

            return SavePointCloud(PointCloudScannerSettings.FileNameOBJ);
        }
        public override void ShowPointCloud()
        {

        }
        public void OpenSaved()
        {

        }
        //public void UpdateOpenGLFramesPerSecond()
        //{
        //    this.framesOpenGL.Add(DateTime.Now);
        //    if (framesOpenGL.Count < 10)
        //        return;

        //    //int frameRate = frameListTimes.Count;
        //    TimeSpan ts = (DateTime)framesOpenGL[9] - (DateTime)framesOpenGL[0];
        //    if (ts.TotalSeconds > 0)
        //    {
        //        int frameRate = Convert.ToInt32(10 / ts.TotalSeconds);

        //        this.labelRefreshRateOpenGL.Text = frameRate.ToString() + " f/s";
        //        this.framesOpenGL = new System.Collections.ArrayList();
        //    }

        //}
        public void GetRealSenseCameras(out List<VideoCaptureDevice> depthDevices, out List<VideoCaptureDevice> colorDevices, out FilterInfoCollection videoDevices)
        {
            depthDevices = new List<VideoCaptureDevice>();
            colorDevices = new List<VideoCaptureDevice>();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);


            for (int i = 0; i < videoDevices.Count; i++)
            {
                string name = videoDevices[i].Name;
                this.CameraStrings.Add(name);
                if (name.Contains("Depth"))
                {
                    string monikerStringdepth = videoDevices[i].MonikerString;
                    VideoCaptureDevice videoCaptureDepth = new VideoCaptureDevice(monikerStringdepth);
                    depthDevices.Add(videoCaptureDepth);
                }
                if (name.Contains("RGB"))
                {
                    string monikerStringcolor = videoDevices[i].MonikerString;
                    VideoCaptureDevice videoCaptureColor = new VideoCaptureDevice(monikerStringcolor);
                    colorDevices.Add(videoCaptureColor);
                }
            }
        }
     
        void OnNewFrameColor(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {

            if (colorPendingForSave)
            {
                bitmapColorForSave = (Bitmap)eventArgs.Frame.Clone();
                bitmapColorForSave = bitmapColorForSave.ToGrayscale();
                //bitmapColorForSave.Palette = ImageExtensions.GrayScalePalette;
                //bitmapColorForSave.ConvertPixelFormat(System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                colorPendingForSave = false;
                return;
            }
            //bitmapColor = (Bitmap)eventArgs.Frame;

            switch (this.DisplayType)
            {

                case DisplayType.Color:
                    {

                        bitmapColor = (Bitmap)eventArgs.Frame.Clone();
                        this.colorFrameSize = bitmapColor.Size;
                        this.pictureBoxColor.Image = bitmapColor;

                        break;
                    }
                case DisplayType.Depth:
                    {

                        break;
                    }
                case DisplayType.IR:
                    {

                        break;
                    }
                case DisplayType.OpenGL:
                    {

                        break;
                    }
            }

        }
        private Bitmap GenerateDepthImage(Bitmap capturedBitmap)
        {
            bitmapDepth = (Bitmap)capturedBitmap.Clone();
            this.depthFrameSize = bitmapDepth.Size;
            GenerateDepthMetaData(bitmapDepth);

            Bitmap bitmapDepthTest = new Bitmap(this.depthFrameSize.Width, this.depthFrameSize.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            bitmapDepthTest.Update_Gray(this.depthForImage);
            bitmapDepthTest.Palette = ImageExtensions.GrayScalePalette;

            return bitmapDepthTest;
        }
        private Bitmap GenerateIRImage(Bitmap capturedBitmap)
        {
            bitmapIR = (Bitmap)capturedBitmap.Clone();
            GenerateDepthMetaData(bitmapIR);
            Bitmap bitmapIRTest = new Bitmap(this.depthFrameSize.Width, this.depthFrameSize.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            bitmapIRTest.Palette = ImageExtensions.GrayScalePalette;

            bitmapIRTest.Update_Gray(this.infrared);

            return bitmapIRTest;
        }
        void OnNewFrameDepth(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
           
            if (depthPendingForSave)
            {

                bitmapDepthSave = GenerateDepthImage((Bitmap)eventArgs.Frame);
                bitmapIRSave = GenerateIRImage((Bitmap)eventArgs.Frame);
                this.depthPendingForSave = false;
                return;

            }
            VideoCaptureDevice videosource = sender as VideoCaptureDevice;
            if (videosource.SourceType == VideoSourceType.Depth)
            {
                Bitmap bm = (Bitmap)eventArgs.Frame.Clone();
                this.depthFrameSize = bm.Size;
                GenerateDepthMetaData(bm);
            }

            switch (this.DisplayType)
            {
                    
                case DisplayType.Color:
                    {

                        break;
                    }
                case DisplayType.Depth:
                    {
                        VideoCaptureDevice source = sender as VideoCaptureDevice;
                        if (source.SourceType == VideoSourceType.Depth)
                        {

                            this.pictureBoxDepth.Image = GenerateDepthImage((Bitmap)eventArgs.Frame); 
                           
                        }
                        break;
                    }
                case DisplayType.IR:
                    {
                        VideoCaptureDevice source = sender as VideoCaptureDevice;
                        if (source.SourceType == VideoSourceType.Infrared)
                        {

                            pictureBoxIR.Image = GenerateIRImage((Bitmap)eventArgs.Frame);

                         
                        }
                        break;
                    }
                case DisplayType.OpenGL:
                    {
                        if (this.openGLPart.ShowingIn3DControl)
                        {
                            openGLCounter++;
                            if (openGLCounter == this.openGLRefreshAt)
                            {
                                openGLCounter = 0;
                                VideoCaptureDevice source = sender as VideoCaptureDevice;
                                if (source.SourceType == VideoSourceType.Depth)
                                {
                                    Bitmap bm = (Bitmap)eventArgs.Frame.Clone();
                                    this.depthFrameSize = bm.Size;
                                    GenerateDepthMetaData(bm);
                                  
                                    
                                    try
                                    {
                                        
                                        this.parentControl.BeginInvoke(new UpdateShowFormOpenGLDelegate(UpdateOpenGLControl));
                                    }

                                    catch
                                    {
                                        System.Diagnostics.Debug.WriteLine("Error in Begin Invoke");
                                    }
                                }
                               
                            }
                        }
                        
                     
                   
                        break;
                    }
            }
          

        }
        public override PointCloudRenderable ToPointCloudRenderable(bool resizeTo1)
        {
            PointCloudRenderable pcr = new PointCloudRenderable();

            this.pointCloudBase = PointCloud.FromListVector3(this.DepthMetaData.Vectors);
            //SetDefaultColorForDepth();

            if (this.pointCloudBase != null && this.pointCloudBase.Vectors.Length > 0)
            {
                if (resizeTo1)
                    pointCloudBase.ResizeTo1();
                pcr.PointCloud = pointCloudBase;
                return pcr;
            }
            return null;

        }
        private void SetDefaultColorForDepth()
        {
            if (this.pointCloudBase.Colors == null && this.pointCloudBase.Colors.GetLength(0) != this.pointCloudBase.Vectors.Length)
            {
                this.pointCloudBase.Colors = new Vector3[this.pointCloudBase.Colors.GetLength(0)];

            }
            float deltaZ = this.pointCloudBase.BoundingBoxMaxFloat;
            float deltaSmall = deltaZ / 6f;

            for ( int i = 0; i < this.pointCloudBase.Vectors.Length; i++ )
            {
                float depthVal = Convert.ToUInt16(this.pointCloudBase.Vectors[i].Z);
                
                float r= 0 , g = 0, b = 0;

                if(depthVal >= deltaZ)
                {
                    r = 1f;
                    g = (depthVal - deltaZ) / deltaSmall;
                    b = (depthVal - deltaZ) / deltaSmall;

                    break;
                }
                int iVal = 1;
                if (depthVal >= (deltaZ - iVal * deltaSmall) && depthVal < deltaZ - (iVal -1) * deltaSmall)
                {
                    r = 1;
                    g = (depthVal - iVal * deltaZ) / deltaSmall;
                    b = 0;

                }
                iVal = 2;
                if (depthVal >= (deltaZ - iVal * deltaSmall) && depthVal < deltaZ - (iVal - 1) * deltaSmall)
                {
                    r = (depthVal - iVal * deltaZ) / deltaSmall;
                    g = 1;
                    b = 0;

                }
                iVal = 3;// green to cyan
                if (depthVal >= (deltaZ - iVal * deltaSmall) && depthVal < deltaZ - (iVal - 1) * deltaSmall)
                {
                    r = 0;
                    g = 1;
                    b = (depthVal - iVal * deltaZ) / deltaSmall; 

                }
                iVal = 4;// cyan to blue
                if (depthVal >= (deltaZ - iVal * deltaSmall) && depthVal < deltaZ - (iVal - 1) * deltaSmall)
                {
                    r = 0;
                    g = 1;
                    b = (depthVal - iVal * deltaZ) / deltaSmall;

                }
                iVal = 4;// // blue to black
                if (depthVal >= (deltaZ - iVal * deltaSmall) && depthVal < deltaZ - (iVal - 1) * deltaSmall)
                {
                    r = 0;
                    g = 0;
                    b = (depthVal - iVal * deltaZ) / deltaSmall;

                }
              
                Vector3 c = new Vector3(r, g, b);
                this.pointCloudBase.Colors[i] = c;
               

            }

         
        }
        private void UnusedGenerate_Depth(Bitmap bm)
        {

            this.DepthMetaData.Vectors = new List<Vector3>();
            LockBitmap lockBitmap = new LockBitmap(bm);
            lockBitmap.LockBits();
            byte[] pixelData = lockBitmap.Pixels;

            DepthMetaData = new DepthMetaData();
            DepthMetaData.FrameData = new ushort[pixelData.GetLength(0)];


            //depthArr = new uint[bm.Width, bm.Height];

            for (int iy = 0; iy < bm.Height; iy++)
            {
                int step24 = bm.Width * 3 * iy;
                for (int ix = 0; ix < bm.Width; ix++)
                {
                    int pixel24 = step24 + 3 * ix;
                    ushort depth = BitConverter.ToUInt16(pixelData, pixel24);

                    int depthIndex = (iy * DepthMetaData.XDepthMaxRealSense) + ix;
                    if (depth != 0)
                    {
                        
                        Vector3 v = ConvertDepthTomm(ix, iy, depth);
                        this.DepthMetaData.Vectors.Add(v);
                        DepthMetaData.FrameData[depthIndex] = Convert.ToUInt16(v.Z);
                       
                    }

                   
                }
            }
            lockBitmap.UnlockBits();
            //---------------
            
           

            
        }
    
        private void GenerateDepthMetaData(Bitmap bm)
        {
            DepthMetaData = new DepthMetaData();
            this.DepthMetaData.Vectors = new List<Vector3>();
           
          
            LockBitmap lockBitmap = new LockBitmap(bm);
            lockBitmap.LockBits();
            byte[] pixelData = lockBitmap.Pixels;
            
            infrared = new byte[bm.Width * bm.Height];
            depthForImage = new byte[bm.Width * bm.Height];
            int indexInfrared = 0;

            List<byte[]> listPixels = new List<byte[]>();

            DepthMetaData.FrameData = new ushort[DepthMetaData.XDepthMaxRealSense * DepthMetaData.YDepthMaxRealSense];


            for (int iy = 0; iy < bm.Height; iy++)
            {
                int step24 = bm.Width * 3 * iy;
                for (int ix = 0; ix < bm.Width; ix++)
                {
                    int pixel24 = step24 + 3 * ix;
                    ushort depth = BitConverter.ToUInt16(pixelData, pixel24);
                    
                    try
                    {
                       
                        byte[] pixel = new byte[3];
                        pixel[0] = pixelData[pixel24];
                        pixel[1] = pixelData[pixel24 + 1];
                        pixel[2] = pixelData[pixel24 + 2];

                        depth = BitConverter.ToUInt16(pixel, 0);
                        listPixels.Add(pixel);

                        infrared[indexInfrared] = pixelData[pixel24 + 2];
                      
                        
                        if (depth != 0)
                        {
                            //Vector3 vTest = new Vector3(ix, iy, depth);
                            
                            Vector3 v = ConvertDepthTomm(ix, -iy, depth);

                            this.DepthMetaData.Vectors.Add(v);
                            depthForImage[indexInfrared] = (byte)v.Z;
                            int depthIndex = (iy * DepthMetaData.XDepthMaxRealSense) + ix;
                            DepthMetaData.FrameData[depthIndex] = Convert.ToUInt16(v.Z);
                       

                        }
                    }
                    catch (Exception err)
                    {
                        System.Windows.Forms.MessageBox.Show("Error converting to depth: " + err.Message);
                    }
                    indexInfrared++;

                
                }
            }

            //UtilsPointCloudIO.Write_OBJ(listPixels, pathModels, "RawData_" + DateTime.Now.Millisecond.ToString() + PointCloudScannerSettings.FileNameDepthOBJ);
            //UtilsPointCloudIO.Write_OBJ(depthVectors, pathModels, "Depth_" + DateTime.Now.Millisecond.ToString() + PointCloudScannerSettings.FileNameDepthOBJ);

            if (PointCloudScannerSettings.InterpolateFrames)
            {
                CalculateInterpolatedPixels();
            }



            lockBitmap.UnlockBits();
        }
        private void CalculateInterpolatedPixels()
        {
            iFrameInterpolation++;
            HelperInterpolation_Iteration1(DepthMetaData.XDepthMaxRealSense, DepthMetaData.YDepthMaxRealSense);


          

            numberOfCutPointsArray[iFrameInterpolation - 1] = numberOfCutPoints;
            interpolationList.Add(this.DepthMetaData.FrameData);

            if (iFrameInterpolation == this.numberOfMaxFramesInterpolated)
            {
                HelperInterpolationCalculateEnd(DepthMetaData.XDepthMaxRealSense, DepthMetaData.YDepthMaxRealSense);
                //if (PointCloudScannerSettings.EntropyImage)
                //    ShowpictureBoxEntropy();

                if (bStopAfterFrameInterpolation)
                    StopScanner();
            }

        }
        private Vector3 ConvertDepthTomm(int ix, int iy, int iz)
        {
            float f = Convert.ToSingle(iz) / 31.25f + 0.5f;

            uint depth = Convert.ToUInt16(f); // convert to mm

            Vector3 v = new Vector3();

            if (depth != 0)
            {
                float x = depth * (DepthConversionMatrix.M11 * ix + DepthConversionMatrix.M13);
                float y = depth * (DepthConversionMatrix.M22 * iy + DepthConversionMatrix.M23);
               
                v = new Vector3(x, y, depth);
                
                
            }
            return v;


        }

        private void timer_Tick(object sender, EventArgs e)
        {
            IVideoSource videoSource = videoSourcePlayerDepth.VideoSource;

            int framesReceived = 0;


            // get number of frames for the last second
            if (videoSource != null)
            {
                framesReceived = videoSource.FramesReceived;
            }


            if (stopWatch == null)
            {
                stopWatch = new Stopwatch();
                stopWatch.Start();
            }
            else
            {
                stopWatch.Stop();

                float fps = 1000.0f * framesReceived / stopWatch.ElapsedMilliseconds;

                cameraFpsLabel.Text = fps.ToString("F2") + " fps";

                stopWatch.Reset();
                stopWatch.Start();
            }
        }
        private void SetFrameSize(VideoCaptureDevice videoSource, Size size)
        {

            //Überprüfen ob die Aufnahmequelle eine Liste mit möglichen Aufnahme-Auflösungen mitliefert.
            //System.Diagnostics.Debug.WriteLine("Resolution before: " + videoSource.VideoResolution.FrameSize.Width.ToString());
            if (videoSource.VideoCapabilities.Length > 0)
            {
                int desiredResolutionIndex = -1;
                //Das Profil mit der Auflösung size suchen
                for (int i = 0; i < videoSource.VideoCapabilities.Length; i++)
                {
                    if (videoSource.VideoCapabilities[i].FrameSize.Width == size.Width && videoSource.VideoCapabilities[i].FrameSize.Height == size.Height)
                    {
                        desiredResolutionIndex = i;
                        break;
                    }
                }
                if (desiredResolutionIndex == -1)
                {
                    desiredResolutionIndex = videoSource.VideoCapabilities.GetLength(0) - 1;
                }

                //Dem Webcam Objekt ermittelte Auflösung übergeben
                videoSource.VideoResolution = videoSource.VideoCapabilities[desiredResolutionIndex];
                System.Diagnostics.Debug.WriteLine("Video Source Resolution : " + videoSource.VideoResolution.FrameSize.Width.ToString() + " x " + videoSource.VideoResolution.FrameSize.Height.ToString());
            }
        }
        // On times tick - collect statistics

        public int ScannerID
        {
            get
            {
                return scannerID;
            }
            set
            {
                scannerID = value;
            
            }
        }
        //public VideoSourcePlayer VideoPlayer
        //{
        //    get
        //    {
        //        return this.videoSourcePlayerDepth;
        //    }
        //    set
        //    {
        //        this.videoSourcePlayerDepth = value;
        //    }
        //}
       

        private bool SaveBitmaps()
        {
            //prepareSave = true;
            colorPendingForSave = depthPendingForSave = true;
            System.Threading.Thread.Sleep(100);
            for (int i = 0; i < 100; i++)
            {
                if (this.colorPendingForSave || depthPendingForSave)
                {
                    System.Threading.Thread.Sleep(100);

                }
                else
                {
                    try
                    {
                        StopScanner();
                        this.bitmapColorForSave.SaveImage(PathModels, ImageExtensions.DateTimeString() + "Color_" + this.ScannerID.ToString() + "_" , false);

                        //this.bitmapDepthTest.SaveImage(pathModels, "DepthTest_", true);
                        this.bitmapDepthSave.SaveImage(PathModels, ImageExtensions.DateTimeString() + "Depth_" + this.ScannerID.ToString() + "_", false);
                        this.bitmapIRSave.SaveImage(PathModels, ImageExtensions.DateTimeString() + "IR_" + this.ScannerID.ToString() + "_", false);
                        //this.bitmapIRTest.SaveImage(pathModels, "IR_test_" , true);
                        return true;


                    }
                    catch (Exception err)
                    {
                        System.Windows.Forms.MessageBox.Show("Error saving bitmaps " + err.Message);
                        return false;
                    }
                }
            }
            return false;

        }

        public override bool SavePointCloud(string fileName)
        {
            if (this.DepthMetaData.Vectors != null)
            {
                //PointCloud pcTest = PointCloud.FromVector3List(this.depthVectors);

                PointCloud pc = PointCloud.FromListVector3(this.DepthMetaData.Vectors);
                if(pc != null)
                    UtilsPointCloudIO.ToObjFile_ColorInVertex(pc, PathModels, fileName);
                    //UtilsPointCloudIO.ToObjFile(pc, PathModels, ImageExtensions.DateTimeString() + "PointCloud_" + this.scannerID.ToString() + ".obj");
                //UtilsPointCloudIO.Write_OBJ_Test(pc, pcTest, pathModels, "ObjTest" + this.RealSenseCameraNumber.ToString() + "_" + DateTime.Now.ToFileTimeUtc() + ".obj");

                

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing to save");
            }
            return true;
        }
        public void ConfigurationDialog()
        {
            VideoCaptureDeviceForm vcd = new VideoCaptureDeviceForm();
            vcd.ShowDialog();
        }
        public void DisplayProperties(IntPtr hwnd)
        {

            //depthDevices[0].DisplayPropertyPage(hwnd);
            //depthDevices[0].DisplayCrossbarPropertyPage(hwnd);

            VideoCaptureDevice deD = DevicesDepth[0];
           
            System.Reflection.PropertyInfo[] infos = deD.GetType().GetProperties();


        }

        private void UpdateOpenGLControl()
        {
            try
            {
                //this.parentControl.UpdateOpenGLFramesPerSecond();
                PointCloudRenderable pcr = this.ToPointCloudRenderable(true);
                if (pcr != null)
                    this.openGLControl.GLrender.ReplaceRenderableObject(pcr, true);

            }
            catch (Exception err)
            {
                //System.Windows.Forms.MessageBox.Show("Error in Update OpenGL window" + err.Message);
                System.Diagnostics.Debug.WriteLine("Error in Update OpenGL window: " + err.Message);
            }

        }
        private void SetConversionMatrices()
        {
            DepthConversionMatrix = new Matrix3();
            //DepthConversionMatrix.M11 = 0.001f;
            //DepthConversionMatrix.M22 = 0.001f;

            DepthConversionMatrix.M11 = 0.002270445f;
            DepthConversionMatrix.M13 = -0.72654252f;

            DepthConversionMatrix.M22 = 0.002169029f;
            DepthConversionMatrix.M23 = -0.520567051f;

            DepthConversionMatrix.M33 = 1;


            ColorConversionMatrix = new Matrix3();
            ColorConversionMatrix.M11 = 0.003042459f;
            ColorConversionMatrix.M13 = -0.001661511f;
            ColorConversionMatrix.M22 = 0.001775862f;
            ColorConversionMatrix.M23 = -00293146f;
            ColorConversionMatrix.M33 = 1;
            

        }
        //public void Start3DShow()
        //{

        //    this.ShowingIn3DControl = true;
        //    if (this.openGLControl.GLrender.GLContextInitialized)
        //        Show3DInControl(true);
        //}
        //private void Show3DInControl(bool show)
        //{
        //    if (show)
        //    {
        //        ShowingIn3DControl = true;
        //        this.parentControl.BeginInvoke(new UpdateShowFormOpenGLDelegate(UpdateOpenGLControl));

        //    }
        //    else
        //    {
        //        ShowingIn3DControl = false;


        //    }


        //}

    }
}
