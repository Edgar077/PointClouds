using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Extension;

namespace OpenTK.Examples
{
    static class Program
    {
        /// <summary>
        /// Entry point of this example.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (OpenTKForm example = new OpenTKForm())
            {
                //Utilities.SetWindowTitle(example);
                example.ShowDialog();
            }
        }
    }
}
