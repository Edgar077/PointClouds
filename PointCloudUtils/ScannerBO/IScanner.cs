using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKExtension;

namespace PointCloudUtils
{
    public interface IScanner
    {
        bool StartScanner();
        void StopScanner();
        bool SavePointCloud(string fileName);
        bool SavePointCloudDefault();
        
        //bool SavePointCloud(string path, string fileName);
        //bool SavePointCloud(string fileName);
        bool SaveAll();
        void ShowPointCloud();
        PointCloudRenderable ToPointCloudRenderable(bool resizeTo1);
        OpenGLPart OpenGLPart { get; }
        int OpenGLRefreshAt { get; set; }

    }
}
