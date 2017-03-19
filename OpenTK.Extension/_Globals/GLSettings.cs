// Pogramming by
//     Douglas Andrade ( http://www.cmsoft.com.br, email: cmsoft@cmsoft.com.br)
//               Implementation of most of the functionality
//     Edgar Maass: (email: maass@logisel.de)
//               Code adaption, changed to user control
//
//Software used: 
//    OpenGL : http://www.opengl.org
//    OpenTK : http://www.opentk.com
//
// DISCLAIMER: Users rely upon this software at their own risk, and assume the responsibility for the results. Should this software or program prove defective, 
// users assume the cost of all losses, including, but not limited to, any necessary servicing, repair or correction. In no event shall the developers or any person 
// be liable for any loss, expense or damage, of any type or nature arising out of the use of, or inability to use this software or program, including, but not
// limited to, claims, suits or causes of action involving alleged infringement of copyrights, patents, trademarks, trade secrets, or unfair competition. 
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKExtension
{
    public static class GLSettings
    {
        //public static string Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string Path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location);

        public static string PathModels = "\\Models";
        public static string PathCharacters = "\\Characters";

        public static bool ShowAxes;
        
        public static bool ShowGrid;
        public static bool ShowCameraFOV;
        public static bool ShowNormals;
        public static float PointSize = 1;
        public static float PointSizeAxis = 1;
        public static string ViewMode;
        public static System.Drawing.Color BackColor;
        public static System.Drawing.Color ColorModelCurrent;

        public static System.Drawing.Color ColorModels;

        public static bool PointCloudCentered = true;
        public static bool BoundingBoxLeftStartsAt000 = false;

        public static bool ShowAxesLabels = true;
        public static bool ShowModelAxes = true;

        public static int Height ;
        public static int Width ;
        public static bool Fill;
        public static bool IsInitializedFromSettings = false;

        public static string FileNamePointCloudLast1;
        public static string FileNamePointCloudLast2;
        public static string FileNamePointCloudLast3;
        public static string FileNamePointCloudLast4;

        public static bool ShowPointCloudAsTexture;


        public static void InitFromSettings()
        {
            IsInitializedFromSettings = true;

            ShowAxes = OpenTKExtension.Properties.Settings.Default.ShowAxes;
            ShowGrid = OpenTKExtension.Properties.Settings.Default.ShowGrid;
            ShowCameraFOV = OpenTKExtension.Properties.Settings.Default.ShowCameraFOV;

            PointSize = OpenTKExtension.Properties.Settings.Default.PointSize;
            PointSizeAxis = OpenTKExtension.Properties.Settings.Default.PointSizeAxis;
            ViewMode = OpenTKExtension.Properties.Settings.Default.ViewMode;
            BackColor = OpenTKExtension.Properties.Settings.Default.BackColor;
            ColorModels = OpenTKExtension.Properties.Settings.Default.ColorModels;
            PointCloudCentered = OpenTKExtension.Properties.Settings.Default.PointCloudCentered;
            BoundingBoxLeftStartsAt000 = OpenTKExtension.Properties.Settings.Default.BoundingBoxLeftStartsAt000;
            ShowAxesLabels = OpenTKExtension.Properties.Settings.Default.ShowAxesLabels;
            ShowModelAxes = OpenTKExtension.Properties.Settings.Default.ShowModelAxes;
            Height = OpenTKExtension.Properties.Settings.Default.Height;
            Width = OpenTKExtension.Properties.Settings.Default.Width;
            Fill = OpenTKExtension.Properties.Settings.Default.Fill;

            FileNamePointCloudLast1 = OpenTKExtension.Properties.Settings.Default.FilePointCloudLast1;
            FileNamePointCloudLast2 = OpenTKExtension.Properties.Settings.Default.FilePointCloudLast2;
            FileNamePointCloudLast3 = OpenTKExtension.Properties.Settings.Default.FilePointCloudLast3;
            FileNamePointCloudLast4 = OpenTKExtension.Properties.Settings.Default.FilePointCloudLast4;
            ShowPointCloudAsTexture = OpenTKExtension.Properties.Settings.Default.ShowPointCloudAsTexture;
            if (FileNamePointCloudLast1 == string.Empty)
                setFileNames();

        }
        public static void SaveSettings()
        {
            OpenTKExtension.Properties.Settings.Default.ShowAxes = ShowAxes;
            OpenTKExtension.Properties.Settings.Default.ShowGrid = ShowGrid;
            OpenTKExtension.Properties.Settings.Default.ShowCameraFOV = ShowCameraFOV;

            OpenTKExtension.Properties.Settings.Default.PointSize = PointSize ;
            OpenTKExtension.Properties.Settings.Default.PointSizeAxis = PointSizeAxis ;
            OpenTKExtension.Properties.Settings.Default.ViewMode = ViewMode ;
            OpenTKExtension.Properties.Settings.Default.BackColor = BackColor;
            OpenTKExtension.Properties.Settings.Default.ColorModels = ColorModels;

            OpenTKExtension.Properties.Settings.Default.PointCloudCentered = PointCloudCentered;
            OpenTKExtension.Properties.Settings.Default.BoundingBoxLeftStartsAt000 = BoundingBoxLeftStartsAt000;
            
            OpenTKExtension.Properties.Settings.Default.ShowAxesLabels = ShowAxesLabels;
            OpenTKExtension.Properties.Settings.Default.ShowModelAxes = ShowModelAxes;

            OpenTKExtension.Properties.Settings.Default.Height = Height;
            OpenTKExtension.Properties.Settings.Default.Width = Width;
            OpenTKExtension.Properties.Settings.Default.Fill = Fill;
           
           
            OpenTKExtension.Properties.Settings.Default.FilePointCloudLast1 = FileNamePointCloudLast1;
            OpenTKExtension.Properties.Settings.Default.FilePointCloudLast2 = FileNamePointCloudLast2;
            OpenTKExtension.Properties.Settings.Default.FilePointCloudLast3 = FileNamePointCloudLast3;
            OpenTKExtension.Properties.Settings.Default.FilePointCloudLast4 = FileNamePointCloudLast4;
            OpenTKExtension.Properties.Settings.Default.ShowPointCloudAsTexture = ShowPointCloudAsTexture ;


            OpenTKExtension.Properties.Settings.Default.Save();

        }
        public static void SetDefaultSettings()
        {
            ShowAxes = true;
            ShowGrid = false;
            ShowCameraFOV = false;
            ShowNormals = false;
            PointSize = 1;
            PointSizeAxis = 1;
            BackColor = System.Drawing.Color.DarkSlateBlue;
            BoundingBoxLeftStartsAt000 = false;
            ColorModels = System.Drawing.Color.White;
            PointCloudCentered = true;
            ShowAxesLabels = false;
            ShowModelAxes = true;
            Height = 600;
            Width = 1000;
            Fill = false;
            ShowPointCloudAsTexture = false;


            setFileNames();

            SaveSettings();
            //InitFromSettings();

        }
        private static void setFileNames()
        {
            FileNamePointCloudLast1 = "PointCloudLast1.obj";
            FileNamePointCloudLast2 = "PointCloudLast2.obj";
            FileNamePointCloudLast3 = "PointCloudLast3.obj";
            FileNamePointCloudLast4 = "PointCloudLast4.obj";

        }
        public static bool DesignMode
        {
            get { return (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv"); }
        }
        
    }
   
}
