//from WavefrontOBJViewer.NET, David Jeske
//http://www.codeproject.com/Articles/798054/SimpleScene-d-scene-manager-in-Csharp-and-OpenTK

using System;


namespace OpenTKExtension
{
    public class FramesPerSecond
    {
        int FPS_frames;
        double FPS_time;
        DateTime oldTime;
        double _framesPerSecond;
        public double AvgFramesPerSecond
        {
            get { return _framesPerSecond; }
        }

      

        public FramesPerSecond()
        {
        }

        public void newFrame()
        {
            DateTime now = DateTime.Now;

            if (oldTime == default(DateTime))
            {
                oldTime = now;
                return;
            }
            TimeSpan span = now - oldTime;


            FPS_frames++;
            FPS_time += span.TotalSeconds;
            if (FPS_time > 2.0)
            {
                _framesPerSecond = (double)FPS_frames / FPS_time;
                FPS_frames = 0;
                FPS_time = 0.0;
            }
            oldTime = now;

        }
       
    }
}

