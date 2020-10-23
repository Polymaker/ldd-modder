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

        public static float ToRadian(float degrees)
        {
            return degrees * (float)Math.PI / 180f;
        }

        public static float Map(float value, float in1, float in2, float out1, float out2)
        {
            float t = (value - in1) / (in2 - in1);
            return out1 + ((out2 - out1) * t);
        }
    }
}
