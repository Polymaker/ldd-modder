using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder
{
    public static class MathHelper
    {
        public static double Clamp(double n, double min, double max)
        {
            return Math.Max(Math.Min(n, max), min);
        }

        public static float Clamp(float n, float min, float max)
        {
            return Math.Max(Math.Min(n, max), min);
        }

        public static float SetSign(float value, float sign)
        {
            return Math.Abs(value) * Math.Sign(sign);
        }

        public static float SetSign(float value, int sign)
        {
            return Math.Abs(value) * Math.Sign(sign);
        }
    }
}
