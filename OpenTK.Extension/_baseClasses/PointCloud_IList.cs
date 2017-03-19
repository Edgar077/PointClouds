using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Globalization;
using System.Drawing;

namespace OpenTKExtension
{

    public partial class PointCloud : IList<Vertex> 
    {

       
        /// <summary>
        /// very costy 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            List<Vector3> listV = new List<Vector3>(this.Vectors);
            List<Vector3> listC = new List<Vector3>(this.Colors);
            List<uint> listi = new List<uint>(this.Indices);

            listV.RemoveAt(index);
            listC.RemoveAt(index);
            listi.RemoveAt(index);


            this.Vectors = listV.ToArray();
            this.Colors = listC.ToArray();
            this.Indices = listi.ToArray();

            
        }

        public Vertex this[int index]
        {
            get
            {
                Vertex ver;
                if(this.Colors != null && this.Indices != null)
                {
                    ver = new Vertex(this.Vectors[index], this.Colors[index], this.Indices[index]);
                }
                else 
                {
                    ver = new Vertex(this.Vectors[index]);
                }


                return ver;
                //throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
                //this[index] = value;
                
            }
        }
        public int IndexOf(Vertex item)
        {
            System.Windows.Forms.MessageBox.Show("SW Error - PointCloud method not implemented - IndexOf");
            throw new NotImplementedException();
        }

        public void Insert(int index, Vertex item)
        {
            System.Windows.Forms.MessageBox.Show("SW Error - PointCloud method not implemented - Insert");
            //throw new NotImplementedException();
        }

        
        public void Add(Vertex item)
        {
            this.AddVector(item.Vector);
            if(item.Color != Vector3.Zero)
                this.AddColor(item.Color);
            this.AddIndex(Convert.ToUInt32(item.Index));

           

        }

        public void Clear()
        {
            System.Windows.Forms.MessageBox.Show("SW Error - PointCloud method not implemented - Clear");
            //throw new NotImplementedException();
        }

        public bool Contains(Vertex item)
        {
            System.Windows.Forms.MessageBox.Show("SW Error - PointCloud method not implemented - Contains");
            return false;
            //throw new NotImplementedException();
        }

        public bool IsReadOnly
        {

            get
            {
                return false;
                //System.Windows.Forms.MessageBox.Show("SW Error - PointCloud method not implemented - IsReadOnly");
                //throw new NotImplementedException();
            }
        }

        public bool Remove(Vertex item)
        {
            System.Windows.Forms.MessageBox.Show("SW Error - PointCloud method not implemented - Remove");
            throw new NotImplementedException();
        }


        public IEnumerator<Vertex> GetEnumerator()
        {
            System.Windows.Forms.MessageBox.Show("SW Error - PointCloud method not implemented- GetEnumerator");
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            System.Windows.Forms.MessageBox.Show("SW Error - PointCloud method not implemented - GetEnumerator");
            throw new NotImplementedException();
        }
        public void CopyTo(Vertex[] array, int arrayIndex)
        {
            Vectors.CopyTo(array, arrayIndex);
            //throw new NotImplementedException();
        }

        public int Count
        {
            get { return this.Vectors.Length; }
        }

    
        public void AddVector(Vector3 v)
        {
            List<Vector3> list;
            if (this.Vectors != null)
                list = new List<Vector3>(this.Vectors);
            else
                list = new List<Vector3>();
            list.Add(v);
            Vectors = list.ToArray();
        }
        public void AddColor(Vector3 color)
        {
            List<Vector3> list;
            if (this.Colors != null)
                list = new List<Vector3>(this.Colors);
            else
                list = new List<Vector3>();
            list.Add(color);
            Colors = list.ToArray();
        }
        public void AddIndex(uint ind)
        {
            List<uint> list;
            if (this.Indices != null)
                list = new List<uint>(this.Indices);
            else
                list = new List<uint>();
            list.Add(ind);
            Indices = list.ToArray();
        }
    }
}
