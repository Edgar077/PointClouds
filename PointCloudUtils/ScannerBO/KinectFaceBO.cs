//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//using OpenTKExtension;
//using OpenTK;
//using System.Drawing;
//using System.Diagnostics;
//using Microsoft.Kinect;
//using System.Windows.Forms;
//using Microsoft.Kinect.Face;

//namespace PointCloudUtils
//{
//    public partial class KinectFaceBO : ScannerBase
//    {

        
//        Control parent;
//        System.Windows.Forms.PictureBox pictureBoxColor;
//        System.Windows.Forms.PictureBox pictureBoxDepth;
//        System.Windows.Forms.PictureBox pictureBoxIR;
     
//        System.Windows.Forms.Label cameraFpsLabel;
//        OGLControl openGLControl;
//        Label labelRefreshRateOpenGL;
//        Label labelFramesPerSecond;
//        Label labelDepth1;

//        PointCloud PointCloud;
      
//        string lastFileOpened;

//       // int openGLCounter;

//        private delegate void DispatcherCallback();
//        private Image ImageScreenshot;
      

//        //--------
        
//        #region public properties

//        public ColorMetaData ColorMetaData { get; set; }
//        public BodyMetaData BodyMetaData { get; set; }

//        #endregion


//        private System.Collections.ArrayList framesScanner;
//        private System.Collections.ArrayList framesOpenGL;

        

//        #region Kinect stuff
//        // open the reader for the body frames
//        //this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
//        KinectSensor scannerSensor;

//        MultiSourceFrameReader scannerReader;
//        private BodyFrameReader bodyFrameReader = null;
//        // 1) Specify a face frame source and a face frame reader
//        //FaceFrameSource _faceSource = null;
//        //FaceFrameReader _faceReader = null;

//        private HighDefinitionFaceFrameSource _faceSource = null;
//        private HighDefinitionFaceFrameReader _faceReader = null;

//        private FaceAlignment _faceAlignment = null;
//        private FaceModel _faceModel = null;

//        private Body[] bodies = null;

        
       
//        public KinectSkeleton kinectSkeleton = new KinectSkeleton();
     
//        CoordinateMapper coordinateMapper = null;

//        #endregion

//        #region Face

//        List<Vector3> pointsFace = new List<Vector3>();
//        #endregion

//        Ellipse ellipseFace;
//        float faceX = 0.1f;
//        float faceY = 0.15f;
//        public KinectFaceBO()
//        {
//            InitBitmaps();
           

//        }

//        public KinectFaceBO(System.Windows.Forms.Control myParentControl, System.Windows.Forms.PictureBox myPictureBoxColor, System.Windows.Forms.PictureBox myPictureBoxDepth, System.Windows.Forms.PictureBox mypictureBoxIR, OGLControl myOpenGLControl,
//            System.Windows.Forms.PictureBox mypictureBoxEntropy, System.Windows.Forms.PictureBox mypictureBoxPolygon, 
//            System.Windows.Forms.Label mycameraFpsLabel, Label mylabelRefreshRateOpenGL, Label mylabelDepth1):this()
//        {
//            openGLPart = new OpenGLPart(this, myParentControl, myOpenGLControl);

//            parent = myParentControl;
//            pictureBoxDepth = myPictureBoxDepth;
//            pictureBoxColor = myPictureBoxColor;
//            pictureBoxIR = mypictureBoxIR;
//            cameraFpsLabel = mycameraFpsLabel;
//            openGLControl = myOpenGLControl;
//            this.pictureBoxEntropy = mypictureBoxEntropy;
//            this.pictureBoxPolygon = mypictureBoxPolygon;

//            labelFramesPerSecond = mycameraFpsLabel;
//            labelRefreshRateOpenGL = mylabelRefreshRateOpenGL;
//            labelDepth1 = mylabelDepth1;

//            InitPictureBoxesForUI();
//            try
//            {
              
//            }
//            catch (Exception err)
//            {
//                System.Windows.Forms.MessageBox.Show("SW Error initializing Scanner : " + err.Message);
//            }
//            this.ellipseFace = new Ellipse(faceX, faceY, 0.6f);
//        }
//        private void InitScanner()
//        {
//            this.framesScanner = new System.Collections.ArrayList();
//            framesOpenGL = new System.Collections.ArrayList();
//            PathModels = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Models\\";

//            PointCloudScannerSettings.CutFrames = true;//this setting is obsolete by using this project (relevant only for the WPF


        
//        }
//        public override bool StartScanner()
//        {
//            InitScanner();
//            scannerSensor = KinectSensor.GetDefault();
//            //scannerSensor.DepthFrameSource
//            if (scannerSensor != null)
//            {
              
//                if (PointCloudScannerSettings.ShowSkeleton)
//                {
//                    this.bodyFrameReader = this.scannerSensor.BodyFrameSource.OpenReader();
//                    this.bodyFrameReader.FrameArrived += this.BodyReader_FrameArrived;
//                }

//                if (PointCloudScannerSettings.ShowFace)
//                {
//                    //_faceSource = new HighDefinitionFaceFrameSource(_sensor);
//                    //_faceReader = _faceSource.OpenReader();
//                    //_faceReader.FrameArrived += FaceReader_FrameArrived;
//                    //_faceModel = new FaceModel();
//                    //_faceAlignment = new FaceAlignment();


//                    _faceSource = new HighDefinitionFaceFrameSource(scannerSensor);
//                    //_faceSource = new FaceFrameSource(scannerSensor, 0, FaceFrameFeatures.BoundingBoxInColorSpace |
//                    //                                      FaceFrameFeatures.FaceEngagement |
//                    //                                      FaceFrameFeatures.Glasses |
//                    //                                      FaceFrameFeatures.Happy |
//                    //                                      FaceFrameFeatures.LeftEyeClosed |
//                    //                                      FaceFrameFeatures.MouthOpen |
//                    //                                      FaceFrameFeatures.PointsInColorSpace |
//                    //                                      FaceFrameFeatures.RightEyeClosed);
//                    _faceReader = _faceSource.OpenReader();
//                   _faceReader.FrameArrived += FaceReader_FrameArrived;
//                    _faceModel = new FaceModel();
//                    _faceAlignment = new FaceAlignment();

//                }
//                // 2) Initialize the face source with the desired features

              
//                this.ImageScreenshot = new Bitmap(this.scannerSensor.DepthFrameSource.FrameDescription.Width, this.scannerSensor.DepthFrameSource.FrameDescription.Height, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale);

//                this.coordinateMapper = this.scannerSensor.CoordinateMapper;
              
//                bStopAfterFrameInterpolation = false;
//                InitPictureBoxesForUI();

//                scannerSensor.Open();
               
            
//            }
//            else
//            {
//                return false;
//            }
            
//            return true;


//        }
//        private void FaceReader_FrameArrived(object sender, HighDefinitionFaceFrameArrivedEventArgs e)
//        {
//            using (var frame = e.FrameReference.AcquireFrame())
//            {
//                if (frame != null && frame.IsFaceTracked)
//                {
//                    frame.GetAndRefreshFaceAlignmentResult(_faceAlignment);
//                    UpdateFacePoints();
//                }
//            }
//        }
//        private void UpdateFacePoints()
//        {
//            if (_faceModel == null) return;

//            var vertices = _faceModel.CalculateVerticesForAlignment(_faceAlignment);
//            pointsFace = new List<Vector3>();
//            if (vertices.Count > 0)
//            {
//                //if (pointsFace.Count == 0)
//                //{
//                //    for (int index = 0; index < vertices.Count; index++)
//                //    {
//                //        //Ellipse ellipse = new Ellipse
//                //        //{
//                //        //    Width = 2.0,
//                //        //    Height = 2.0,
//                //        //    Fill = new SolidColorBrush(Colors.Blue)
//                //        //};
//                //        pointsFace.Add(new Vector3(vertices[index].X, vertices[index].Y, vertices[index].Z));

//                //        //_points.Add(ellipse);
//                //    }

//                //    //foreach (Vector3 v in _points)
//                //    //{
//                //    //    canvas.Children.Add(ellipse);
//                //    //}
//                //}

//                for (int index = 0; index < vertices.Count; index++)
//                {
//                    pointsFace.Add(new Vector3(vertices[index].X, vertices[index].Y, vertices[index].Z));
//                    CameraSpacePoint vertice = vertices[index];
//                    DepthSpacePoint point = this.coordinateMapper.MapCameraPointToDepthSpace(vertice);

//                    if (float.IsInfinity(point.X) || float.IsInfinity(point.Y)) 
//                        return;


//                    //Ellipse ellipse = _points[index];

//                    //Canvas.SetLeft(ellipse, point.X);
//                    //Canvas.SetTop(ellipse, point.Y);

//                }
//            }
//        }
//        //void FaceReader_FrameArrived(object sender, FaceFrameArrivedEventArgs e)
//        //{
//        //    using (var frame = e.FrameReference.AcquireFrame())
//        //    {
//        //        if (frame != null)
//        //        {
//        //            if (frame != null && frame.IsFaceTracked)
//        //            {

//        //                frame.GetAndRefreshFaceAlignmentResult(_faceAlignment);
//        //                UpdateFacePoints();
//        //            }

//        //            // 4) Get the face frame result
//        //            FaceFrameResult result = frame.FaceFrameResult;

//        //            if (result != null)
//        //            {
//        //                FaceFrame ff = frame as FaceFrame;
                        
                       
//        //                // 5) Do magic!

//        //                // Get the face points, mapped in the color space.
//        //                var eyeLeft = result.FacePointsInColorSpace[FacePointType.EyeLeft];
//        //                var eyeRight = result.FacePointsInColorSpace[FacePointType.EyeRight];
//        //                var nose = result.FacePointsInColorSpace[FacePointType.Nose];
//        //                var mouthLeft = result.FacePointsInColorSpace[FacePointType.MouthCornerLeft];
//        //                var mouthRight = result.FacePointsInColorSpace[FacePointType.MouthCornerRight];

//        //                var eyeLeftClosed = result.FaceProperties[FaceProperty.LeftEyeClosed];
//        //                var eyeRightClosed = result.FaceProperties[FaceProperty.RightEyeClosed];
//        //                var mouthOpen = result.FaceProperties[FaceProperty.MouthOpen];

//        //            //    // Position the canvas UI elements
//        //            //    Canvas.SetLeft(ellipseEyeLeft, eyeLeft.X - ellipseEyeLeft.Width / 2.0);
//        //            //    Canvas.SetTop(ellipseEyeLeft, eyeLeft.Y - ellipseEyeLeft.Height / 2.0);

//        //            //    Canvas.SetLeft(ellipseEyeRight, eyeRight.X - ellipseEyeRight.Width / 2.0);
//        //            //    Canvas.SetTop(ellipseEyeRight, eyeRight.Y - ellipseEyeRight.Height / 2.0);

//        //            //    Canvas.SetLeft(ellipseNose, nose.X - ellipseNose.Width / 2.0);
//        //            //    Canvas.SetTop(ellipseNose, nose.Y - ellipseNose.Height / 2.0);

//        //            //    Canvas.SetLeft(ellipseMouth, ((mouthRight.X + mouthLeft.X) / 2.0) - ellipseMouth.Width / 2.0);
//        //            //    Canvas.SetTop(ellipseMouth, ((mouthRight.Y + mouthLeft.Y) / 2.0) - ellipseMouth.Height / 2.0);
//        //            //    ellipseMouth.Width = Math.Abs(mouthRight.X - mouthLeft.X);

//        //            //    // Display or hide the ellipses
//        //            //    if (eyeLeftClosed == DetectionResult.Yes || eyeLeftClosed == DetectionResult.Maybe)
//        //            //    {
//        //            //        ellipseEyeLeft.Visibility = Visibility.Collapsed;
//        //            //    }
//        //            //    else
//        //            //    {
//        //            //        ellipseEyeLeft.Visibility = Visibility.Visible;
//        //            //    }

//        //            //    if (eyeRightClosed == DetectionResult.Yes || eyeRightClosed == DetectionResult.Maybe)
//        //            //    {
//        //            //        ellipseEyeRight.Visibility = Visibility.Collapsed;
//        //            //    }
//        //            //    else
//        //            //    {
//        //            //        ellipseEyeRight.Visibility = Visibility.Visible;
//        //            //    }

//        //            //    if (mouthOpen == DetectionResult.Yes || mouthOpen == DetectionResult.Maybe)
//        //            //    {
//        //            //        ellipseMouth.Height = 50.0;
//        //            //    }
//        //            //    else
//        //            //    {
//        //            //        ellipseMouth.Height = 20.0;
//        //            //    }
//        //            }
//        //        }
//        //    }
//        //}
//        public override void StopScanner()
//        {
           
//            if (scannerReader != null)
//            {
//                scannerReader.Dispose();
//                scannerReader = null;
//            }

//            if (scannerSensor != null)
//            {
//                scannerSensor.Close();
//                scannerSensor = null;

//            }
//            if (bodyFrameReader != null)
//            {
//                this.bodyFrameReader.Dispose();
//                bodyFrameReader = null;
//            }
//            if (_faceReader != null)
//            {
//                _faceReader.Dispose();
//                _faceReader = null;
//            }

//            if (_faceSource != null)
//            {
//                _faceSource = null;
//            }
//            if (_faceModel != null)
//            {
//                _faceModel.Dispose();
//                _faceModel = null;
//            }
//        }
//        public override void ShowPointCloud()
//        {
//            this.StopScanner();
//            if (SaveAll())
//            {
//                OpenTKExtension.TestForm fb = new TestForm();

//                PointCloud pc = MetaDataBase.ToPointCloud(this.ColorMetaData, this.DepthMetaData, this.BodyMetaData, this.coordinateMapper);
//                if (pc == null || pc.Count == 0)
//                {
//                    System.Windows.Forms.MessageBox.Show("Not available point Cloud - please scan first");
//                }
//                else
//                {
//                    fb.Show();
//                    fb.UpdatePointCloud(pc);
//                }
//            }
//            else
//            {
//                System.Windows.Forms.MessageBox.Show("Not available point Cloud - please scan first");
//            }
//        }
     
//        public bool ShowDepth_ColorImage(PointCloud pc)
//        {
            
//            if (pc != null)
//            {

//                this.SetMetaData(pc);
//                ShowScreenshotsFromMetaData();


//            }
//            return true;
//        }
//        public override PointCloudRenderable ToPointCloudRenderable(bool resizeTo1)
//        {
//            if (this.ColorMetaData != null && this.DepthMetaData != null)
//            {
//                PointCloud pc = MetaDataBase.ToPointCloud(this.ColorMetaData, this.DepthMetaData, this.BodyMetaData, this.coordinateMapper);

//                PointCloud pgl = pc;
//                PointCloudRenderable pcr = new PointCloudRenderable();
//                pcr.PointCloud = pgl;

//                return pcr;
//            }
//            return null;
//        }
    
//        private void InitPictureBoxesForUI()
//        {

//            this.pictureBoxDepth.Image = bitmapDepth;
//            this.pictureBoxIR.Image = bitmapIR;

//            this.pictureBoxColor.Image = bitmapColor;
//            this.pictureBoxEntropy.Image = bitmapEntropy;
//            this.pictureBoxPolygon.Image = bitmapPolygon;

//            //Edgar untested!!
//            //bitmapDepth.ConvertToGrayScale();
//        }
//        private void InitBitmaps()
//        {
//            //var dest = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
//            bitmapColor = new Bitmap(ColorMetaData.XColorMaxKinect, ColorMetaData.YColorMaxKinect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
//            bitmapDepth = new Bitmap(DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
//            bitmapIR = new Bitmap(DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
//            bitmapEntropy = new Bitmap(DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
//            bitmapPolygon = new Bitmap(DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            
//            bitmapIR.ChangeToGrayScale();
//            bitmapDepth.ChangeToGrayScale();
          
           



//        }
//        private void SetResizepictureBoxColor(Bitmap bmpInitial, PictureBox pictBox)
//        {

//            Bitmap bmpResult = new Bitmap(pictBox.Width, pictBox.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
//            using (var gr = Graphics.FromImage(bmpResult))
//            {
//                gr.DrawImage(bmpInitial, new Rectangle(Point.Empty, bmpResult.Size));
//            }


//            pictBox.Image = bmpResult;

//        }

   
    
//        private void ProcessBodyFrame(MultiSourceFrame multiSourceFrame)
//        {
//            // Body
//            using (var bodyIndexFrame = multiSourceFrame.BodyIndexFrameReference.AcquireFrame())
//            {
//                BodyIndexFrame frameBodyIndex = bodyIndexFrame;
//                if (bodyIndexFrame != null && this.DepthMetaData != null)
//                {
//                    this.BodyMetaData = new BodyMetaData(frameBodyIndex);
//                    //ProcessDepthColorIR(multiSourceFrame);

//                }
//            }
//        }
    

//        private void UpdateScannerFramesPerSecond()
//        {
//            framesScanner.Add(DateTime.Now);
//            if (framesScanner.Count < 10)
//                return;

//            //int frameRate = frameListTimes.Count;
//            TimeSpan ts = (DateTime)framesScanner[9] - (DateTime)framesScanner[0];
//            if (ts.TotalSeconds > 0)
//            {
//                int frameRate = Convert.ToInt32(10 / ts.TotalSeconds);

//                this.labelFramesPerSecond.Text = frameRate.ToString() + " f/s";
//                this.framesScanner = new System.Collections.ArrayList();
//            }

//        }
  


    
//        private void ShowScreenshotsFromMetaData()
//        {

//            if (this.DepthMetaData != null)
//            {

//                bitmapDepth = bitmapDepth.Update_Gray(this.DepthMetaData.Pixels);
//                this.pictureBoxDepth.Image = bitmapDepth;
//                this.pictureBoxDepth.Refresh();

//                this.ImageScreenshot = bitmapDepth;

//            }
//            if (this.ColorMetaData != null)
//            {
//                if (PointCloud != null)
//                {
//                    //Bitmap bitmapColorSmall = this.ColorMetaData.FromPointCloud_ToBitmap(PointCloud);
//                    //this.pictureBoxColor.Image = bitmapColorSmall;
//                    this.pictureBoxColor.Invalidate();

//                    this.ImageScreenshot = bitmapDepth;
//                }
//                else
//                {
//                    System.Windows.Forms.MessageBox.Show("No point cloud for showing color Image");
//                }

//                //bitmapColor.Update_Color(ColorMetaData.Pixels);
//                //this.pictureBoxColor.Invalidate(false);
               
//            }

//            if (this.IRMetadData != null)
//            {
//                this.bitmapIR.Update_Gray(IRMetadData.FrameData);
//                this.pictureBoxIR.Refresh();
//            }
           
//        }


//        private void SHowInterpolatedFrame()
//        {
//            Bitmap bm = this.pictureBoxEntropy.Image as Bitmap;
//            this.pictureBoxEntropy.Image = bm.Update_Gray(DepthMetaData.Pixels);

//        }
//        public override bool SaveAll()
//        {
//            StopScanner();
//            if (this.DepthMetaData != null)
//            {

//                SavePointCloud_BW_Interpolated();
//                SaveImage_BW_Interpolated();
//                SaveImage_Color();

               
//                SavePointCloud_CurrentTimeName();

//                if (PointCloudScannerSettings.ShowSkeleton)
//                {
//                    string fileName = PointCloudScannerSettings.FileNameOBJ.Remove(PointCloudScannerSettings.FileNameOBJ.Length - 4, 4);
//                    this.kinectSkeleton.ToJsonFile(fileName);
//                }
//                if (PointCloudScannerSettings.ShowFace)
//                {
//                    string fileName = PointCloudScannerSettings.FileNameOBJ.Remove(PointCloudScannerSettings.FileNameOBJ.Length - 4, 4);
//                    //this.kinectSkeleton.SaveSkeleton(fileName);
//                }
//                //SaveEntropyImage(pc);


//                return true;
//            }
//            else
//            {
//                System.Windows.Forms.MessageBox.Show("Nothing to save - please scan first");
//                return false;
//            }
            

//        }
      

//        private void CalculateInterpolatedPixels()
//        {
//            iFrameInterpolation++;
//            HelperInterpolation_Iteration1(DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);


//            //if (PointCloudScannerSettings.BackgroundRemoved)
//            //{
//            //    if (BodyMetaData != null)
//            //    {
//            //        int numberOfPixRemoved = backgroundRemovalTool.DepthFrameData_RemoveBackground(this.DepthMetaData, this.BodyMetaData);
//            //        if (numberOfPixRemoved == -1) // the method returns an error ...
//            //            return;
//            //        numberOfPixelsRemovedArray[iFrameInterpolation - 1] = numberOfPixRemoved;
//            //    }

//            //}

//            numberOfCutPointsArray[iFrameInterpolation - 1] = numberOfCutPoints;
//            interpolationList.Add(this.DepthMetaData.FrameData);

//            if (iFrameInterpolation == this.numberOfMaxFramesInterpolated)
//            {
//                HelperInterpolationCalculateEnd(DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
//                if (PointCloudScannerSettings.EntropyImage)
//                    ShowpictureBoxEntropy();

//                if (PointCloudScannerSettings.SaveAndStop)
//                {
//                    SaveAndStop();
//                }

//                if (bStopAfterFrameInterpolation)
//                    StopScanner();
//            }
            
//        }

//        protected void SaveAndStop()
//        {
//            if (depthQuality > PointCloudScannerSettings.SaveImageIfQualityIsBetterThan)
//            {
//                SavePointCloud_BW_Interpolated();
//                if (PointCloudScannerSettings.ScannerMode == PointCloudUtils.ScannerMode.Color_Depth || PointCloudScannerSettings.ScannerMode == PointCloudUtils.ScannerMode.Color_Depth_3DDisplay)
//                {
//                    SavePointCloud_CurrentTimeName();
//                    SaveEntropyImage();
                   
//                }
//                PointCloudScannerSettings.SaveAndStop = false;
//                StopScanner();

//            }

//        }
  
      
      
       
    
//        //private void Helper_SetLabelContent(int[] statisticValuesSum)
//        //{
//        //    labelDepth1.Text = statisticValuesSum[1].ToString() + " %";
//        //    labelDepth2.Text = statisticValuesSum[2].ToString() + " %";
//        //    labelDepth3.Text = statisticValuesSum[3].ToString() + " %";
//        //    labelDepth4.Text = statisticValuesSum[4].ToString() + " %";
//        //    labelDepth0.Text = statisticValuesSum[0].ToString() + " %";

//        //    Helper_SetLabelBackground(statisticValuesSum[1]);
//        //}
    
      
//        private void Helper_SetLabelBackground(int intVal)
//        {

//            if (intVal < 20)
//            {
//                labelDepth1.BackColor = Color.Red;
//                this.pictureBoxPolygon.BackColor = Color.Red;

//            }
//            else if (intVal < 30)
//            {
//                labelDepth1.BackColor = Color.Yellow;
//                pictureBoxPolygon.BackColor = Color.Yellow;
//            }
//            else if (intVal < 40)
//            {
//                labelDepth1.BackColor = Color.GreenYellow;
//                this.pictureBoxPolygon.BackColor = Color.GreenYellow;
//            }
//            else if (intVal < 50)
//            {
//                labelDepth1.BackColor = Color.LightGreen;
//                this.pictureBoxPolygon.BackColor = Color.LightGreen;
//            }
//            else
//            {
//                labelDepth1.BackColor = Color.Green;
//                this.pictureBoxPolygon.BackColor = Color.Green;
//            }

//        }

       
//        public void OpenSavedDepthData()
//        {
//            List<OpenTK.Vector3> listPoints = UtilsPointCloudIO.Read_XYZ_Vectors(PathModels, GLSettings.FileNamePointCloudLast1);
//            this.DepthMetaData = new DepthMetaData(listPoints, false);



//        }
//        public void SetMetaData(PointCloud pointCloud)
//        {


//            PointCloud = pointCloud;

//            this.DepthMetaData = new DepthMetaData();
//            this.DepthMetaData = new DepthMetaData(pointCloud, false);
            

//            this.ColorMetaData = new ColorMetaData();
//            byte[] colorInfo = PointCloud.ToColorArrayBytes(pointCloud, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
//            this.ColorMetaData.SetColorPixels(colorInfo);

//            this.IRMetadData = null;
            

//        }
    

//        private void SaveImage_BW()
//        {
//            Image im = BitmapExtensions.FromByteArray_Gray(DepthMetaData.Pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
//            im.SaveImage(PathModels, "Depth", true);


//        }

//        public void SavePointCloud_BW()
//        {
//            if (this.DepthMetaData == null)
//            {
//                MessageBox.Show("No Depth Data to save - please capture, or open last saved depth data");
//                return;

//            }
//            //ushort[] rotatedPoints = DepthMetaData.RotateDepthFrame(this.DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);
//            List<Vector3> listPoints = DepthMetaData.CreateListPoints_Depth(this.DepthMetaData.FrameData, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
//            GLSettings.FileNamePointCloudLast1 = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "_PointCloud.xyz";
//            UtilsPointCloudIO.ToXYZFile(listPoints, GLSettings.FileNamePointCloudLast1, PathModels);
//        }

     
//        private void SaveImage_Color()
//        {
//            //write color Bitmap
//            if (this.ColorMetaData != null)
//            {
//                //ImageSource imSource = ImageSourceUtils.CreateImageSource(ColorMetaData.Pixels, ColorMetaData.XResDefault, ColorMetaData.YResDefault);
//                //System.Windows.Media.Imaging.WriteableBitmap bitmap = ColorMetaData.CreateWriteableBitmap(ColorMetaData.Pixels);

//                System.Drawing.Image im = this.bitmapColor.Update_Color(ColorMetaData.Pixels);

//                im.SaveImage(PathModels, "Color_", true);

//            }


//        }


    

 
//        private void LoadCore(string pathFile)
//        {


//            if (pathFile != null)
//            {
//                try
//                {
//                    using (System.IO.FileStream file = System.IO.File.OpenRead(pathFile))
//                    {
//                        lastFileOpened = pathFile;
//                    }
//                }
//                catch (Exception err)
//                {
//                    MessageBox.Show(
//                        String.Format("Unable to parse file:\r\n\r\n{0}",
//                        err.Message), "Error");
//                }
//            }
//        }

//        private void UpdateOpenGLControl()
//        {
//            try
//            {
//                this.openGLControl.GLrender.AdditionalObjectsToDraw.Clear();
//                //this is the most time consuming part - mapping the color frame to depth
//                PointCloudRenderable pcr = this.ToPointCloudRenderable(false);
//                //filter out only calibration model
//                if(PointCloudScannerSettings.ShowOnlyCalibrationModel)
//                {
//                    pcr.PointCloud = pcr.PointCloud.ExtractCalibrationObject();

//                }
//                //pcr.PointCloudOpenGL.ResizeTo1();
                
//                if (this.kinectSkeleton.BonesAsLines != null && this.kinectSkeleton.BonesAsLines.Count > 0)
//                {
//                    Skeleton sk = new Skeleton(kinectSkeleton.BonesAsLines);
//                    sk.InitializeGL();
//                    this.openGLControl.GLrender.AdditionalObjectsToDraw.Add(sk);
//                }
//                if (this.pointsFace != null && this.pointsFace.Count > 0)
//                {
//                    Face f = new Face(pointsFace);
//                    f.InitializeGL();
//                    this.openGLControl.GLrender.AdditionalObjectsToDraw.Add(f);
//                }
//                if (PointCloudScannerSettings.ShowFaceScanEllipse)
//                {
                    
//                    ellipseFace.InitializeGL();
//                    this.openGLControl.GLrender.AdditionalObjectsToDraw.Add(ellipseFace);
//                }
//                if (pcr != null && pcr.PointCloud.Vectors.Length > 0)
//                    this.openGLControl.GLrender.ReplaceRenderableObject(pcr, true);

//            }
//            catch (Exception err)
//            {
//                //System.Windows.Forms.MessageBox.Show("Error in Update OpenGL window" + err.Message);
//                System.Diagnostics.Debug.WriteLine("Error in Update OpenGL window: " + err.Message);
//            }

//        }

//        public bool SavePointCloud_CurrentTimeName()
//        {
//            string fileID = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "_PointCloud";
//            SavePointCloud(fileID);
//            return true;

//        }
      
      
//        public override bool SavePointCloud(string fileID)
//        {

//            return SavePointCloud(PathModels, fileID);
           
//        }


//        public override bool SavePointCloud(string path, string fileName)
//        {

//            if (ColorMetaData == null || DepthMetaData == null)
//                return false;

//            List<int> listToBeRemoved = new List<int>();
//            PointCloud pc = MetaDataBase.ToPointCloud(this.ColorMetaData, this.DepthMetaData, this.BodyMetaData, this.coordinateMapper);
//            if (PointCloudScannerSettings.ShowFaceScanEllipse)
//            {
//                for (int i = pc.Count - 1; i >= 0; i--)
//                {
//                    if (pc.Vectors[i].X > 0 && pc.Vectors[i].X > faceX)
//                    {
//                        listToBeRemoved.Add(i);

//                    }
//                    else if (pc.Vectors[i].X < 0 && pc.Vectors[i].X < -faceX)
//                    {
//                        listToBeRemoved.Add(i);
                        
//                    }
//                    else if (pc.Vectors[i].Y > 0 && pc.Vectors[i].Y > faceY)
//                    {
//                        listToBeRemoved.Add(i);
                        
//                    }
//                    else if (pc.Vectors[i].Y < 0 && pc.Vectors[i].Y < -faceY)
//                    {
//                        listToBeRemoved.Add(i);
                        

//                    }

//                }
//            }

//            pc.RemovePoints(listToBeRemoved);
//            return UtilsPointCloudIO.ToObjFile(pc, path + "\\" + fileName);

//        }
     
//        private void SaveEntropyImage()
//        {
//            PointCloud pc = MetaDataBase.ToPointCloud(this.ColorMetaData, this.DepthMetaData, this.BodyMetaData, this.coordinateMapper);
            
//            System.Drawing.Image bitmapCustom = bitmapEntropy.UpdateFromPointCloud_Color(pc, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
//            bitmapCustom.SaveImage(PathModels, "PointCloud_", true);
//        }

//        /// <summary>
//        /// Handles the body frame data arriving from the sensor
//        /// </summary>
//        /// <param name="sender">object sending the event</param>
//        /// <param name="e">event arguments</param>
//        private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
//        {
//            bool dataReceived = false;

//            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
//            {
//                if (bodyFrame != null)
//                {
//                    if (this.bodies == null)
//                    {
//                        this.bodies = new Body[bodyFrame.BodyCount];
//                    }

//                    if (_faceSource != null && !_faceSource.IsTrackingIdValid)
//                    {
//                        //Body body = bodies.Where(b => b.IsTracked).FirstOrDefault();
//                        //if (body != null)
//                        //{
//                        //    // 4) Assign a tracking ID to the face source
//                        //    _faceSource.TrackingId = body.TrackingId;
//                        //}
//                    }
//                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
//                    // As long as those body objects are not disposed and not set to null in the array,
//                    // those body objects will be re-used.
//                    bodyFrame.GetAndRefreshBodyData(this.bodies);
//                    dataReceived = true;
//                }
//            }

//            if (dataReceived)
//            {

//                // Draw a transparent background to set the render size
//                //dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));
            
//                foreach (Body body in this.bodies)
//                {

//                    //Pen drawPen = this.bodyColors[penIndex++];

//                    if (body.IsTracked)
//                    {
                        
//                        this.kinectSkeleton.Update(body, this.coordinateMapper);
                        

//                    }

//                }
//            }


//        }
       

//    }
//}
