using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;


namespace NLinear
{
    /// <summary>
    /// Models numeric data types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Numeric<T> : IEquatable<Numeric<T>>
        where T : IEquatable<T>
    {
        #region Private

        T _value;

        #endregion

        #region Constructor

        public Numeric(T a)
        {
            _value = a;
        }

        #endregion

        #region Public attributes
        
        public T Value
        {
            get
            {
                return _value;
            }
        }

        #endregion

        #region implicit cast

        public static implicit operator T(Numeric<T> data)
        {
            return data.Value;
        }

        public static implicit operator Numeric<T>(T data)
        {
            return new Numeric<T>(data);
        }

        #endregion

        #region Equality

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Numeric<T>)
            {
                Numeric<T> other = (Numeric<T>)obj;
                return Equals(ref this, ref other);
            }
            return base.Equals(obj);
        }

        public bool Equals(Numeric<T> other)
        {
            return Equals(this, other);
        }

        public static bool operator ==(Numeric<T> x, Numeric<T> y)
        {
            return EqualityComparer<T>.Default.Equals(x.Value, y.Value);
        }

        public static bool operator !=(Numeric<T> x, Numeric<T> y)
        {
            return !(EqualityComparer<T>.Default.Equals(x.Value, y.Value));
        }

        private static bool Equals(ref Numeric<T> x, ref Numeric<T> y)
        {
            return EqualityComparer<T>.Default.Equals(x.Value, y.Value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        #endregion

        #region Numeric Methods/Operators

        public static Numeric<T> Zero()
        {
            return Operator<T>.Zero;
        }

        static public float ToFloat(T val)
        {
            return NumericConverter<T>.ToFloat(val);
        }

        static public T Fromfloat(float d)
        {
            return NumericConverter<T>.Fromfloat(d);
        }
        static public T FromFloat(float d)
        {
            return NumericConverter<T>.Fromfloat(d);
        }

        public static Numeric<T> operator +(Numeric<T> c1, Numeric<T> c2)
        {
            return new  Numeric<T>(Operator<T>.Add(c1.Value, c2.Value));
        }

        public static Numeric<T> operator -(Numeric<T> c1, Numeric<T> c2)
        {
            return new Numeric<T>(Operator<T>.Subtract(c1.Value, c2.Value));
        }

        public static Numeric<T> operator -(Numeric<T> c1)
        {
            return new Numeric<T>(Operator<T>.Negate(c1.Value));
        }

        public static Numeric<T> operator *(Numeric<T> c1, Numeric<T> c2)
        {
            return new Numeric<T>(Operator<T>.Multiply(c1.Value, c2.Value));
        }

        public static Numeric<T> operator *(Numeric<T> c1, float c)
        {
            return new Numeric<T>(Operator.MultiplyAlternative(c1.Value, c));
        }

        public static Numeric<T> operator /(Numeric<T> c1, Numeric<T> c2)
        {
            return new Numeric<T>(Operator<T>.Divide(c1.Value, c2.Value));
        }

        public static bool operator >(Numeric<T> c1, Numeric<T> c2)
        {
            return (Operator<T>.GreaterThan(c1.Value, c2.Value));
        }

        public static bool operator >=(Numeric<T> c1, Numeric<T> c2)
        {
            return (Operator<T>.GreaterThanOrEqual(c1.Value, c2.Value));
        }

        public static bool operator <(Numeric<T> c1, Numeric<T> c2)
        {
            return (Operator<T>.LessThan(c1.Value, c2.Value));
        }

        public static bool operator <=(Numeric<T> c1, Numeric<T> c2)
        {
            return (Operator<T>.LessThanOrEqual(c1.Value, c2.Value));
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        #endregion
    }
}
