using System;
using System.Globalization;

namespace LDDModder.BrickEditor.Utilities
{
    public static class NumberHelper
    {
        public static bool SmartTryParse(string s, out double result)
        {
            return SmartTryParse(s, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool SmartTryParse(string s, IFormatProvider provider, out double result)
        {
            result = 0;
            var numberFormat = NumberFormatInfo.GetInstance(provider);

            if ((s.Contains(".") || s.Contains(",")) && !s.Contains(numberFormat.NumberDecimalSeparator))
            {
                var other = numberFormat.NumberDecimalSeparator == "," ? "." : ",";
                if (double.TryParse(s.Replace(other, numberFormat.NumberDecimalSeparator), NumberStyles.Number, provider, out result))
                    return true;
            }

            return double.TryParse(s, NumberStyles.Number, provider, out result);
        }
    }
}
