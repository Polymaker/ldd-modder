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
    }
}
