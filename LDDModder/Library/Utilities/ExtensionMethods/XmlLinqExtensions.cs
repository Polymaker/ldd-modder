using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace System.Xml.Linq
{
    public static class XmlLinqExtensions
    {
        public static XElement SortAttributes(this XElement element, Func<XAttribute, int> predicate)
        {
            var elemAttrs = element.Attributes().ToArray();
            element.RemoveAttributes();
            elemAttrs = elemAttrs.OrderBy(x => predicate(x)).ToArray();
            element.Add(elemAttrs);
            return element;
        }

        public static int GetDepth(this XElement element)
        {
            int depth = 0;
            while((element = element.Parent) != null)
                depth++;
            return depth;
        }

        public static int GetIntAttribute(this XElement element, XName attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return int.Parse(attribute.Value);
        }

        public static bool ContainsElement(this XElement element, XName childElementName)
        {
            return element.Element(childElementName) != null;
        }

        public static bool ContainsAttribute(this XElement element, XName attributeName)
        {
            return element.Attribute(attributeName) != null;
        }
    }
}
