using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class NumberExtensions
    {
        public static bool EqualOrClose(this float number, float other)
        {
            return EqualOrClose(number, other, float.Epsilon);
        }

        public static bool EqualOrClose(this float number, float other, float tolerence)
        {
            return Math.Abs(number - other) < tolerence;
        }

        public static bool EqualOrClose(this double number, double other)
        {
            return EqualOrClose(number, other, double.Epsilon);
        }

        public static bool EqualOrClose(this double number, double other, double tolerence)
        {
            return Math.Abs(number - other) < tolerence;
        }
    }
}
