using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PointCloudUtils;

namespace ScannerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members

       

        

        #endregion

        #region Constructor

        public MainWindow()
        {
            
            InitializeComponent();
            this.PointCloudUC.InitFromSettings();
            EventManager.RegisterClassHandler(typeof(Window),Keyboard.KeyUpEvent, new KeyEventHandler(keyUp), true);
            //ReadSettings();
        }
        private void keyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.NumPad0)
            {
                this.PointCloudUC.SaveDepthPoints();
                //MessageBox.Show("YAY!!!");
            }
        }
        #endregion

     

        #region Event handlers

        private void Window_Loaded(object sender, EventArgs e)
        {
           //ConnectScanner()


        }
        private void Window_Closed(object sender, EventArgs e)
        {

            this.PointCloudUC.ScannerClose();
            PointCloudScannerSettings.SaveSettings();
            
        }

        private void ConnectScanner()
        {
            if (this.PointCloudUC != null)
            {
                PointCloudUC.ScannerConnect();

            }
        }
      

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            PointCloudScannerSettings.ScannerMode = ScannerMode.Color;
        }

        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            PointCloudScannerSettings.ScannerMode = ScannerMode.Depth;
        }

        private void RemoveBackground_Click(object sender, RoutedEventArgs e)
        {
            PointCloudScannerSettings.BackgroundRemoved = !PointCloudScannerSettings.BackgroundRemoved;
        }

       
        private void Color_Depth_Click(object sender, RoutedEventArgs e)
        {
            PointCloudScannerSettings.ScannerMode = ScannerMode.Color_Depth;

        }
         private void ConnectScanner_Click(object sender, RoutedEventArgs e)
        {
            ConnectScanner();
        }
         private void OpenSavedDepthData_Click(object sender, RoutedEventArgs e)
        {
            this.PointCloudUC.OpenSavedDepthData();
            this.PointCloudUC.SetDepthBitmap();
            
        }
        

        #endregion

         private void Window_Closed_1(object sender, EventArgs e)
         {

         }

     

      

    }

    
}
