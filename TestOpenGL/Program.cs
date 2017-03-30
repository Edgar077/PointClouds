using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTKExtension;
namespace TestOpenGL
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run((Form)new Form3D());
            TestForm tf = new TestForm();
            //tf.ShowPointClouds(cm.PointClouds);
            Application.Run((Form)tf);

        }
    }
}
