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

        public static bool HasAttribute(this XElement element, string attributeName)
        {
            return element.Attribute(attributeName) != null; 
        }

        public static bool HasAttribute(this XElement element, string attributeName, out XAttribute attribute)
        {
            attribute = element.Attribute(attributeName);
            return attribute != null;
        }

        public static bool HasElement(this XElement baseElement, string elementName)
        {
            return HasElement(baseElement, elementName, out XElement _);
        }

        public static bool HasElement(this XElement baseElement, string elementName, out XElement element)
        {
            element = baseElement.Element(elementName);
            return element != null;
        }

        public static XElement AddElement(this XElement element, string elementName)
        {
            var newElem = new XElement(elementName);
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
            var attr = element.Attribute(attributeName);
            return attr != null && int.TryParse(attr.Value, out value);
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

        public static bool TryReadAttribute<T>(this XElement element, string attributeName, out T result)
        {
            result = default;
            var attr = element.Attribute(attributeName);
            if (attr == null)
                return false;

            if (typeof(T) == typeof(int) && 
                int.TryParse(attr.Value, out int intVal))
            {
                result = (T)(object)intVal;
                return true;
            }
            else if (typeof(T) == typeof(float) && 
                float.TryParse(attr.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatVal))
            {
                result = (T)(object)floatVal;
                return true;
            }
            else if (typeof(T) == typeof(double) &&
                double.TryParse(attr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out double dblVal))
            {
                result = (T)(object)dblVal;
                return true;
            }
            else if (typeof(T) == typeof(decimal) &&
                decimal.TryParse(attr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal decVal))
            {
                result = (T)(object)decVal;
                return true;
            }
            else if (typeof(T) == typeof(string))
            {
                result = (T)(object)attr.Value;
                return true;
            }
            else if(typeof(T) == typeof(bool))
            {
                switch (attr.Value.Trim().ToLower())
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

            return false;
        }

        public static bool GetBoolAttribute(this XElement element, string attributeName, BooleanXmlRepresentation representation = BooleanXmlRepresentation.OneZero)
        {
            var attr = element.Attribute(attributeName);

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
