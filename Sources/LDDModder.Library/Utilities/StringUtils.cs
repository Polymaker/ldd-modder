using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Utilities
{
    public static class StringUtils
    {
        public static string GenerateUUID(string uniqueID, int length = 16)
        {
            byte[] stringbytes = Encoding.UTF8.GetBytes(uniqueID);
            byte[] hashedBytes = new System.Security.Cryptography
                .SHA1CryptoServiceProvider()
                .ComputeHash(stringbytes);
            Array.Resize(ref hashedBytes, 16);
            var guid = new Guid(hashedBytes);
            if (length < 4)
                length = 4;
            if (length > 16)
                length = 16;
            return guid.ToString("N").Substring(0, length);
        }

        public static string GenerateUID(int length = 16)
        {
            var guid = Guid.NewGuid();
            if (length < 4)
                length = 4;
            if (length > 16)
                length = 16;
            return guid.ToString("N").Substring(0, length);
        }

        public static bool TryParse<T>(string stringValue, out T result)
        {
            result = default;

            var valueType = typeof(T);

            if (valueType == typeof(int) &&
                int.TryParse(stringValue, out int intVal))
            {
                result = (T)(object)intVal;
                return true;
            }
            else if (valueType == typeof(float) &&
                float.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatVal))
            {
                result = (T)(object)floatVal;
                return true;
            }
            else if (valueType == typeof(double) &&
                double.TryParse(stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out double dblVal))
            {
                result = (T)(object)dblVal;
                return true;
            }
            else if (valueType == typeof(decimal) &&
                decimal.TryParse(stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal decVal))
            {
                result = (T)(object)decVal;
                return true;
            }
            else if (valueType == typeof(string))
            {
                result = (T)(object)stringValue;
                return true;
            }
            else if (valueType == typeof(bool))
            {
                switch (stringValue.Trim().ToLower())
                {
                    case "1":
                    case "true":
                    case "yes":
                        result = (T)(object)true;
                        return true;
                    case "0":
                    case "false":
                    case "no":
                        result = (T)(object)false;
                        return true;
                }
            }
            else if (valueType.IsEnum)
            {
                if (int.TryParse(stringValue, out int intEnumVal) &&
                    Enum.IsDefined(valueType, intEnumVal))
                {
                    result = (T)(object)Enum.ToObject(valueType, intEnumVal);
                    return true;
                }
                try
                {
                    result = (T)(object)Enum.Parse(valueType, stringValue, true);
                    return true;
                }
                catch { }
            }

            return false;
        }

        public static List<T> ParseStringList<T>(string value, string separator = ",")
        {
            string[] values = value.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            var list = new List<T>();
            foreach(string strVal in values)
            {
                if (TryParse<T>(strVal, out T res))
                    list.Add(res);
            }
            return list;
        }

        public static bool EqualsIC(string text, string other)
        {
            return text.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
