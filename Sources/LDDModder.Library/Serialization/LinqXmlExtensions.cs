    using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Xml.Linq
{
    public static class LinqXmlExtensions
    {
        #region Attributes

        public static XAttribute GetAttribute(this XElement element, string name)
        {
            return element.Attributes().FirstOrDefault(x => x.Name.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool HasAttribute(this XElement element, string attributeName, out XAttribute attribute)
        {
            attribute = GetAttribute(element, attributeName);
            return attribute != null;
        }

        public static bool HasAttribute(this XElement element, string attributeName)
        {
            return HasAttribute(element, attributeName, out _);
        }

        public static bool TryReadAttribute(this XElement element, string attributeName, Type valueType, out object result)
        {
            result = null;

            var attr = GetAttribute(element, attributeName);
            if (attr == null)
                return false;

            if (valueType == typeof(byte) &&
                byte.TryParse(attr.Value, out byte byteVal))
            {
                result = byteVal;
                return true;
            }
            else if (valueType == typeof(short) &&
                short.TryParse(attr.Value, out short shortVal))
            {
                result = shortVal;
                return true;
            }
            else if (valueType == typeof(int) &&
                int.TryParse(attr.Value, out int intVal))
            {
                result = intVal;
                return true;
            }
            else if (valueType == typeof(long) &&
                long.TryParse(attr.Value, out long longVal))
            {
                result = longVal;
                return true;
            }
            else if (valueType == typeof(float) &&
                float.TryParse(attr.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatVal))
            {
                result = floatVal;
                return true;
            }
            else if (valueType == typeof(double) &&
                double.TryParse(attr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out double dblVal))
            {
                result = dblVal;
                return true;
            }
            else if (valueType == typeof(decimal) &&
                decimal.TryParse(attr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal decVal))
            {
                result = decVal;
                return true;
            }
            else if (valueType == typeof(string))
            {
                result = attr.Value;
                return true;
            }
            else if (valueType == typeof(bool))
            {
                switch (attr.Value.Trim().ToLower())
                {
                    case "1":
                    case "true":
                    case "yes":
                        result = true;
                        return true;
                    case "0":
                    case "false":
                    case "no":
                        result = false;
                        return true;
                }
            }
            else if (valueType.IsEnum)
            {
                if (int.TryParse(attr.Value, out int intEnumVal) &&
                    Enum.IsDefined(valueType, intEnumVal))
                {
                    result = Enum.ToObject(valueType, intEnumVal);
                    return true;
                }

                try
                {
                    result = Enum.Parse(valueType, attr.Value, true);
                    return true;
                }
                catch { }
            }

            return false;
        }

        public static bool TryReadAttribute<T>(this XElement element, string attributeName, out T result)
        {
            if (TryReadAttribute(element, attributeName, typeof(T), out object objResult))
            {
                result = (T)objResult;
                return true;
            }

            result = default(T);
            return false;
        }

        public static T ReadAttribute<T>(this XElement element, string attributeName)
        {
            if (TryReadAttribute(element, attributeName, out T result))
                return result;

            if (element.HasAttribute(attributeName))
                throw new InvalidCastException($"The value '{element.Attribute(attributeName).Value}' could not be converted to {typeof(T).Name}");

            throw new KeyNotFoundException($"The attribute '{attributeName}' was not found");
        }

        public static T ReadAttribute<T>(this XElement element, string attributeName, T defaultValue)
        {
            if (TryReadAttribute(element, attributeName, out T result))
                return result;
            return defaultValue;
        }

        public static void WriteAttribute(this XElement element, string attributeName, Type valueType, object value)
        {
            if (value != null)
            {
                element.Add(new XAttribute(attributeName,
                    string.Format(CultureInfo.InvariantCulture, "{0}", value))
                    );
            }

            //if (valueType == typeof(byte) ||
            //    valueType == typeof(short) ||
            //    valueType == typeof(int) ||
            //    valueType == typeof(long) ||
            //    valueType == typeof(float) ||
            //    valueType == typeof(double) ||
            //    valueType == typeof(decimal))
            //{
            //    element.Add(new XAttribute(attributeName, 
            //        string.Format(CultureInfo.InvariantCulture, "{0}", value))
            //        );
            //}
        }

        public static void WriteAttribute<T>(this XElement element, string attributeName, T value)
        {
            WriteAttribute(element, attributeName, typeof(T), value);
        }

        public static void WriteAttribute(this XElement element, string attributeName, bool value, BooleanXmlRepresentation representation)
        {
            string stringValue = string.Empty;
            switch (representation)
            {
                case BooleanXmlRepresentation.OneZero:
                    stringValue = value ? "1" : "0"; break;
                case BooleanXmlRepresentation.TrueFalse:
                    stringValue = value ? "true" : "false"; break;
                case BooleanXmlRepresentation.YesNo:
                    stringValue = value ? "yes" : "no"; break;
            }
            WriteAttribute(element, attributeName, typeof(bool), stringValue);
        }

        #endregion

        #region Get/Read Element

        public static XElement GetElement(this XElement element, string elementName)
        {
            return element.Elements()
                .FirstOrDefault(x =>
                x.Name.LocalName.Equals(elementName, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool HasElement(this XElement element, string elementName, out XElement childElement)
        {
            childElement = GetElement(element, elementName);
            return childElement != null;
        }

        public static bool HasElement(this XElement parentElem, string elementName)
        {
            return HasElement(parentElem, elementName, out _);
        }

        public static bool TryReadElement<T>(this XElement element, string elementName, out T result)
        {
            result = default;

            if (TryReadElement(element, elementName, typeof(T), out object objResult))
            {
                result = (T)objResult;
                return true;
            }

            return false;
        }

        public static bool TryReadElement(this XElement element, string elementName, Type valueType, out object result)
        {
            result = null;

            var elem = GetElement(element, elementName);
            if (elem == null)
                return false;

            if (valueType == typeof(int) &&
                int.TryParse(elem.Value, out int intVal))
            {
                result = intVal;
                return true;
            }
            else if (valueType == typeof(float) &&
                float.TryParse(elem.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatVal))
            {
                result = floatVal;
                return true;
            }
            else if (valueType == typeof(double) &&
                double.TryParse(elem.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out double dblVal))
            {
                result = dblVal;
                return true;
            }
            else if (valueType == typeof(decimal) &&
                decimal.TryParse(elem.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal decVal))
            {
                result = decVal;
                return true;
            }
            else if (valueType == typeof(string))
            {
                result = elem.Value;
                return true;
            }
            else if (valueType == typeof(bool))
            {
                switch (elem.Value.Trim().ToLower())
                {
                    case "1":
                    case "true":
                    case "yes":
                        result = true;
                        return true;
                    case "0":
                    case "false":
                    case "no":
                        result = false;
                        return true;
                }
            }
            else if (valueType.IsEnum)
            {
                if (int.TryParse(elem.Value, out int intEnumVal) &&
                    Enum.IsDefined(valueType, intEnumVal))
                {
                    result = Enum.ToObject(valueType, intEnumVal);
                    return true;
                }
                try
                {
                    result = Enum.Parse(valueType, elem.Value, true);
                    return true;
                }
                catch { }
            }

            return false;
        }

        public static T ReadElement<T>(this XElement element, string elementName, T defaultValue)
        {
            if (TryReadElement(element, elementName, out T result))
                return result;
            return defaultValue;
        }

        public static T ReadElementAttribute<T>(this XElement element, string elementName, string attributeName, T defaultValue)
        {
            var elem = GetElement(element, elementName);
            if (TryReadAttribute(elem, attributeName, out T result))
                return result;
            return defaultValue;
        }

        #endregion

        public static void AddNumberAttribute(this XElement element, string attributeName, int number)
        {
            element.Add(new XAttribute(attributeName, number.ToString(CultureInfo.InvariantCulture)));
        }

        public static void AddNumberAttribute(this XElement element, string attributeName, float number)
        {
            element.Add(new XAttribute(attributeName, number.ToString(CultureInfo.InvariantCulture)));
        }

        public static void AddNumberAttribute(this XElement element, string attributeName, double number)
        {
            element.Add(new XAttribute(attributeName, number.ToString(CultureInfo.InvariantCulture)));
        }

        public static XElement AddElement(this XElement element, string elementName)
        {
            var newElem = new XElement(elementName);
            element.Add(newElem);
            return newElem;
        }

        public static XElement AddElement(this XElement element, string elementName, params object[] content)
        {
            var newElem = new XElement(elementName, content);
            element.Add(newElem);
            return newElem;
        }

        public enum BooleanXmlRepresentation
        {
            TrueFalse,
            YesNo,
            OneZero
        }

        public static void AddBooleanAttribute(this XElement element, string attributeName, bool value, BooleanXmlRepresentation representation = BooleanXmlRepresentation.OneZero)
        {
            switch (representation)
            {
                default:
                case BooleanXmlRepresentation.OneZero:
                    element.Add(new XAttribute(attributeName, value ? "1" : "0"));
                    break;
                case BooleanXmlRepresentation.TrueFalse:
                    element.Add(new XAttribute(attributeName, value ? "true" : "false"));
                    break;
                case BooleanXmlRepresentation.YesNo:
                    element.Add(new XAttribute(attributeName, value ? "yes" : "no"));
                    break;
            }
        }

        public static bool TryGetIntAttribute(this XElement element,  string attributeName, out int value)
        {
            value = 0;
            var attr = GetAttribute(element, attributeName);
            return attr != null && int.TryParse(attr.Value, out value);
        }

        public static bool GetBoolAttribute(this XElement element, string attributeName, BooleanXmlRepresentation representation = BooleanXmlRepresentation.OneZero)
        {
            var attr = GetAttribute(element, attributeName);
            if (attr == null)
                return false;
            switch (representation)
            {
                default:
                case BooleanXmlRepresentation.OneZero:
                    return attr.Value == "1";
                case BooleanXmlRepresentation.TrueFalse:
                    return attr.Value.ToLower() == "true";
                case BooleanXmlRepresentation.YesNo:
                    return attr.Value.ToLower() == "yes";
            }
        }
    }
}
