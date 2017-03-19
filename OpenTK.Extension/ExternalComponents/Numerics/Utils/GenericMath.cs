using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLinear
{
    public class GenericMath
    {
        /// <summary>
        /// Calculate the square root using Bhaskara-Brouncker algorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="identity"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        static public T Sqrt<T>(T x, T additiveIdentity, int limit = 8)
            where T : IEquatable<T>
        {
            Numeric<T> n = x;
            Numeric<T> _1 = additiveIdentity;

            Numeric<T> an, bn;

            an = bn = _1;

            for (int i = 0; i < limit; i++)
            {
                Numeric<T> tmp = an;

                an = an + bn * n;
                bn = tmp + bn;
            }

            Numeric<T> div = an / bn;
            return div;
        }
       

        /// <summary>
        /// Multiply x by n
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="identity"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static public T Multiply<T>(T x, int n)
            where T : IEquatable<T>
        {
            Numeric<T> sum = Numeric<T>.Zero();

            for (int i = 0; i < n; i++)
            {
                sum = sum + x;
            }

            return sum;
        }

        /// <summary>
        /// Raise x to power n
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="multiplicativeIdentity"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static public T Power<T>(T x, T multiplicativeIdentity, int n)
            where T : IEquatable<T>
        {
            Numeric<T> pow = multiplicativeIdentity;

            for (int i = 0; i < n; i++)
            {
                pow = pow * x;
            }

            return pow;
        }
     

        static public T Sin<T>(T x, T multiplicativeIdentity, int limit = 8)
            where T : IEquatable<T>
        {
            Numeric<T> num = x;
            Numeric<T> sum = Numeric<T>.Zero();

            for (int i = 0; i < limit; i++)
            {
                float coef = Convert.ToSingle(Math.Pow(-1, i) / Factorial(2 * i + 1, 1) );
                Numeric<T> xi = Power((T)num, multiplicativeIdentity, 2 * i + 1);

                sum = sum + xi * coef;
            }

            return sum;
        }

        static public T Cos<T>(T x, T multiplicativeIdentity, int limit = 8)
            where T : IEquatable<T>
        {
            Numeric<T> num = x;
            Numeric<T> sum = Numeric<T>.Zero();

            for (int i = 0; i < limit; i++)
            {
                float coef =Convert.ToSingle( Math.Pow(-1, i) / Factorial(2 * i, 1));
                Numeric<T> xi = Power((T)num, multiplicativeIdentity, 2 * i);

                sum = sum + xi * coef;
            }

            return sum;
        }

        static public T Factorial<T>(T x, T additiveIdentity)
            where T : IEquatable<T>
        {
            if (x == Numeric<T>.Zero())
                return additiveIdentity;

            Numeric<T> num = x;
            Numeric<T> pow = additiveIdentity;

            while (num != (Numeric<T>)additiveIdentity)
            {
                pow = pow * num;
                num = num - additiveIdentity;
            }

            return pow;
        }

        public static T Max<T>(T x, T y)
        {
            return (Comparer<T>.Default.Compare(x, y) > 0) ? x : y;
        }
        static public T Abs<T>(T x)
            where T : IEquatable<T>
        {
            if (Comparer<T>.Default.Compare(x, default(T)) > 0)
                return x;

            Numeric<T> num = x;
            return -num;


        }

        public static T Min<T>(T x, T y)
        {
            return (Comparer<T>.Default.Compare(x, y) < 0) ? x : y;
        }
    }
}
