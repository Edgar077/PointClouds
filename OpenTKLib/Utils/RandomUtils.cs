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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Windows.Media;
using OpenTK;

namespace OpenTKExtension
{
    public class RandomUtils
    {
        /// <summary>
        /// Generates random indices 
        /// </summary>
        /// <param name="MaxIndex"></param>
        /// <param name="numIndices"></param>
        /// <returns></returns>
        public static List<int> UniqueRandomIndices(int numIndices, int MaxIndex)
        {
            List<int> indices = new List<int>();
            try
            {
                //generate Number unique random indices from 0 to MAX


                
                //cannot generate more unique numbers than than the size of the set we are sampling
                if (numIndices > MaxIndex )
                {
                    MessageBox.Show("SW Call error for UniqueRandomIndices");
                    return indices;
                }
                Random rnd = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < 100000; i++)
                {
                    double newRnd = Convert.ToSingle(rnd.NextDouble() * (MaxIndex - 1) -0.5);
                    int newIndex = Convert.ToInt32(newRnd);
                    if (newIndex < 0)
                        newIndex = 0;
                    if (newIndex == MaxIndex)
                        newIndex = MaxIndex - 1;

                    if (!indices.Contains(newIndex))
                        indices.Add(newIndex);

                    if (indices.Count == numIndices)
                        return indices;

                }
                MessageBox.Show("No random Indices are found - please check routine UniqueRandomIndices");
                return indices;
            }
            catch(Exception err)
            {
                MessageBox.Show("Error in UniqueRandomIndices " + err.Message);
                return indices;
            }

            
        }
        public static PointCloud ExtractPoints(PointCloud points, List<int> indices)
        {

            try
            {


                List<Vector3> output = new List<Vector3>();

                for (int i = 0; i < indices.Count; i++)
                {
                    int indexPoint = indices[i];
                    Vector3 p = points.Vectors[indexPoint];
                    output.Add(p);
                }
                return PointCloud.FromListVector3(output);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error in RandomUtils.ExtractPoints " + err.Message);
                return null;
            }
        }
    

      

        /// <summary>
        /// Generates random indices 
        /// </summary>
        /// <param name="MaxIndex"></param>
        /// <param name="numIndices"></param>
        /// <returns></returns>
        public static PointCloud GetRandomPoints(int numPoints, PointCloud pointsInput)
        {

            List<int> randomIndices = RandomUtils.UniqueRandomIndices(numPoints, Convert.ToInt32(pointsInput.Count));

            PointCloud output = RandomUtils.ExtractPoints(pointsInput, randomIndices);

            return output;
        }


    }
}
