using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace NLinear
{
    public static class NumericConverter<T>
    {
        static Func<float, T> compiledTofloatExpression;

        static Func<T, float> compiledFromfloatExpression;

        static void CompileConvertTofloatExpression()
        {
            ParameterExpression parameter1 = Expression.Parameter(typeof(float), "d");

            Expression convert = Expression.Convert(
                            parameter1,
                            typeof(T)
                        );

            compiledTofloatExpression = Expression.Lambda<Func<float, T>>(convert, parameter1).Compile();
        }

        static void CompileConvertFromfloatExpression()
        {
            ParameterExpression parameter1 = Expression.Parameter(typeof(T), "d");

            Expression convert = Expression.Convert(
                            parameter1,
                            typeof(float)
                        );

            compiledFromfloatExpression = Expression.Lambda<Func<T, float>>(convert, parameter1).Compile();
        }

        static NumericConverter()
        {
            CompileConvertTofloatExpression();

            CompileConvertFromfloatExpression();
        }

        static public float ToFloat(T value)
        {
            return compiledFromfloatExpression(value);
        }

        static public T Fromfloat(float d)
        {
            return compiledTofloatExpression(d);
        }
    }
}
