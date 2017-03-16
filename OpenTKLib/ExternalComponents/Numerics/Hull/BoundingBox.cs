using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKExtension;

namespace NLinear
{
     public struct BoundingBox<T> //: IEquatable<BoundingBox<T>> 
        where T : IEquatable<T>

     {
         Numeric<T> X1, Y1, Z1, X2, Y2, Z2;
         public MinStruct<Numeric<T>> Min;
         public MaxStruct<Numeric<T>> Max;


         public BoundingBox(Numeric<T> x1, Numeric<T> y1, Numeric<T> z1, Numeric<T> x2, Numeric<T> y2, Numeric<T> z2)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.Z1 = z1;
            this.X2 = x2;
            this.Y2 = y2;
            this.Z2 = z2;

            Min = new MinStruct<Numeric<T>>(x1, y1, z1, x2, y2, z2);
            Max = new MaxStruct<Numeric<T>>(x1, y1, z1, x2, y2, z2);


        }
       

         //public bool Equals(BoundingBox<T> other)
         //{
         //    if(X1 == other.X1 && )
         //    return Equals(this, other);
         //}
        

         
        // public T Min
        //float x1 = Counter.Min.X;
        //float y1 = Counter.Min.Y;
        //float z1 = Counter.Min.Z;
        //float x2 = Counter.Max.X;
        //float y2 = Counter.Max.Y;
        //float z2 = Counter.Max.Z;

    }
     public struct MinStruct<T> //: IEquatable<MinStruct<T>> 
        where T : IEquatable<T>
    {
        //Numeric<T> X1, Y1, Z1, X2, Y2, Z2;
        public Numeric<T> X, Y, Z;
        public MinStruct(Numeric<T> x1, Numeric<T> y1, Numeric<T> z1, Numeric<T> x2, Numeric<T> y2, Numeric<T> z2)
        {
            X = x2;
            if(x1 < x2)
                X = x1;
            Y = y2;
            if(y1 < y2)
                Y = y1;
            Z = z2;
            if(z1 < z2)
                Z = z1;

         

        }
    }
     public struct MaxStruct<T> //: IEquatable<MaxStruct<T>>
        where T : IEquatable<T>
     {
         //Numeric<T> X1, Y1, Z1, X2, Y2, Z2;
         public Numeric<T> X, Y, Z;
         public MaxStruct(Numeric<T> x1, Numeric<T> y1, Numeric<T> z1, Numeric<T> x2, Numeric<T> y2, Numeric<T> z2)
         {
             X = x2;
             if (x1 > x2)
                 X = x1;
             Y = y2;
             if (y1 > y2)
                 Y = y1;
             Z = z2;
             if (z1 > z2)
                 Z = z1;


         }
     }

}
