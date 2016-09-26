using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class NumberExtensions
    {

        public static bool IsCloseTo(this float value, float number, float error)
        {
            return Math.Abs(value - number) < Math.Abs(error);
        }

        public static bool IsCloseTo(this double value, double number, double error)
        {
            return Math.Abs(value - number) < Math.Abs(error);
        }
    }
}
