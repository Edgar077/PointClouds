using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PointCloudScanner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                MainForm newForm = new MainForm();
                //newForm.Text = "Point Cloud Scanner";
                Application.Run(newForm);
            }
            catch(Exception err)
            {
                System.Windows.MessageBox.Show("Error running app : " + err.Message);
            }
        }
    }
}
