using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLinear;

namespace OpenTKExtension
{
  
    public class Polyline<T> : IList<Vector3<T>> //IEquatable<Polyline<T>>
        where T : IEquatable<T>
    {
        private List<Vector3<T>> list;
        public Polyline()
        { 
            list = new List<Vector3<T>>();
        }

        
        public void Add(Vector3<T> element)
        {
            list.Add(element);
        }
        public int IndexOf(Vector3<T> element)
        {
            return list.IndexOf(element);

        }
        public void CopyTo(Vector3<T>[] element, int i)
        {
            list.CopyTo(element, i);

        }
        public bool Remove(Vector3<T> element)
        {
            return list.Remove(element);

        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        public IEnumerator<Vector3<T>> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
            //throw new NotImplementedException();
        }
                   
        public void Insert(int ind, Vector3<T> element)
        {
            list.Insert(ind, element);
            
        }
        public void RemoveAt(int ind)
        {
            list.RemoveAt(ind);


        }
        public void Clear()
        {
            list.Clear();


        }
        public bool Contains(Vector3<T> element)
        {
            return list.Contains(element);


        }
        public int Count
        {
            get

            {
                return list.Count;
            }
         
        }
        public Vector3<T> this[int i] 
        {
            get
            {
                return list[i];
            }
            set
            {
                list[i] = value;
            }

        }
    }
}
