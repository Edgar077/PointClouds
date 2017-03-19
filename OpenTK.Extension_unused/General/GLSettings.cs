
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK.Extension
{
    public static class GLSettings
    {
        public static bool ShowAxes;
        public static bool ShowGrid;
        public static bool ShowNormals;
        public static int PointSize = 1;
        public static int PointSizeAxis = 1;
        public static string ViewMode;
        public static System.Drawing.Color BackColor;
        public static System.Drawing.Color ColorModels;

        public static bool PointCloudCentered = true;
        public static bool ShowAxesLabels = true;
        public static bool ShowModelAxes = true;

        public static int Height;
        public static int Width;
        public static bool Fill;
        public static bool IsInitializedFromSettings = false;

        public static bool Lighting = false;
        public static bool NormalMapping = false;

        public static bool OpenGL_FaceCull = false;
        public static bool PointCloudResize;

        public static void InitFromSettings()
        {
            IsInitializedFromSettings = true;

            ShowAxes = OpenTK.Extension.Properties.Settings.Default.ShowAxes;
            ShowGrid = OpenTK.Extension.Properties.Settings.Default.ShowGrid;
            PointSize = OpenTK.Extension.Properties.Settings.Default.PointSize;
            PointSizeAxis = OpenTK.Extension.Properties.Settings.Default.PointSizeAxis;
            ViewMode = OpenTK.Extension.Properties.Settings.Default.ViewMode;
            BackColor = OpenTK.Extension.Properties.Settings.Default.BackColor;
            ColorModels = OpenTK.Extension.Properties.Settings.Default.ColorModels;
            PointCloudCentered = OpenTK.Extension.Properties.Settings.Default.PointCloudCentered;
            ShowAxesLabels = OpenTK.Extension.Properties.Settings.Default.ShowAxesLabels;
            ShowModelAxes = OpenTK.Extension.Properties.Settings.Default.ShowModelAxes;
            Height = OpenTK.Extension.Properties.Settings.Default.Height;
            Width = OpenTK.Extension.Properties.Settings.Default.Width;
            Fill = OpenTK.Extension.Properties.Settings.Default.Fill;
            Lighting = OpenTK.Extension.Properties.Settings.Default.Lighting;
            NormalMapping = OpenTK.Extension.Properties.Settings.Default.NormalMapping;
            OpenGL_FaceCull = OpenTK.Extension.Properties.Settings.Default.OpenGL_FaceCull;
            PointCloudResize = OpenTK.Extension.Properties.Settings.Default.PointCloudResize;
        }
        public static void SaveSettings()
        {
            OpenTK.Extension.Properties.Settings.Default.ShowAxes = ShowAxes;
            OpenTK.Extension.Properties.Settings.Default.ShowGrid = ShowGrid;
            OpenTK.Extension.Properties.Settings.Default.PointSize = PointSize;
            OpenTK.Extension.Properties.Settings.Default.PointSizeAxis = PointSizeAxis;
            OpenTK.Extension.Properties.Settings.Default.ViewMode = ViewMode;
            OpenTK.Extension.Properties.Settings.Default.BackColor = BackColor;
            OpenTK.Extension.Properties.Settings.Default.ColorModels = ColorModels;

            OpenTK.Extension.Properties.Settings.Default.PointCloudCentered = PointCloudCentered;
            OpenTK.Extension.Properties.Settings.Default.ShowAxesLabels = ShowAxesLabels;
            OpenTK.Extension.Properties.Settings.Default.ShowModelAxes = ShowModelAxes;

            OpenTK.Extension.Properties.Settings.Default.Height = Height;
            OpenTK.Extension.Properties.Settings.Default.Width = Width;
            OpenTK.Extension.Properties.Settings.Default.Fill = Fill;
            OpenTK.Extension.Properties.Settings.Default.Lighting = Lighting;
            OpenTK.Extension.Properties.Settings.Default.NormalMapping = NormalMapping;
            OpenTK.Extension.Properties.Settings.Default.OpenGL_FaceCull = OpenGL_FaceCull;
            OpenTK.Extension.Properties.Settings.Default.PointCloudResize = PointCloudResize;

            OpenTK.Extension.Properties.Settings.Default.Save();
          

        }
        public static void SetDefaultSettings()
        {
            ShowAxes = true;
            ShowGrid = false;
            ShowNormals = false;
            PointSize = 1;
            PointSizeAxis = 1;
            BackColor = System.Drawing.Color.DarkSlateBlue;
            ColorModels = System.Drawing.Color.White;
            PointCloudCentered = true;
            ShowAxesLabels = false;
            ShowModelAxes = true;
            Height = 600;
            Width = 1000;
            Fill = false;
            OpenGL_FaceCull = false;
            PointCloudResize = false;

            SaveSettings();
            InitFromSettings();

        }
        public static bool DesignMode
        {
            get { return (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv"); }
        }

    }

}
