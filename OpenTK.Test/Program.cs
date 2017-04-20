

using System;
using System.Windows.Forms;

using OpenTKExtension;
using grendgine_collada;

namespace OpenTKTest
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run((Form)new CharacterCreatorTest());
            //string path = AppDomain.CurrentDomain.BaseDirectory + GLSettings.PathPointClouds + "Collada\\testLinq.dae";

            //string path = AppDomain.CurrentDomain.BaseDirectory + GLSettings.PathPointClouds + "Collada\\cube.dae";
            //string path = AppDomain.CurrentDomain.BaseDirectory + GLSettings.PathPointClouds + "Collada\\crate.dae";
            //string fileName = AppDomain.CurrentDomain.BaseDirectory+ GLSettings.PathPointClouds + "Collada\\duck_triangulate.dae";
            // string fileName = AppDomain.CurrentDomain.BaseDirectory + @"A:\Archiv\3D\_Jeannie\Models\MilaPartyBlack_blender.dae";


            //Application.Run((Form)new Form3D());
            TestForm tf = new TestForm();
            //tf.ShowPointClouds(cm.PointClouds);
            Application.Run((Form)tf);
        }
        public static void ColladaTest()
        {
            string fileName = @"A:\Archiv\3D\_Jeannie\Models\2017.01.28\MilaPartyBlack_blender.dae";


            Grendgine_Collada.ReduceMesh(fileName, 0.1f);

            Grendgine_Collada col = Grendgine_Collada.Load_File(fileName);
            string path = OpenTKExtension.IOUtils.ExtractDirectory(fileName);


            Grendgine_Collada.Save(path + "new.dae", col);

         

            //OpenTKExtension.Collada.ColladaPointCloud cm = OpenTKExtension.Collada.ColladaLoader.LoadFile(path);
            TestForm tf = new TestForm();
            //tf.ShowPointClouds(cm.PointClouds);
            Application.Run((Form)tf);

        }
    }
}
