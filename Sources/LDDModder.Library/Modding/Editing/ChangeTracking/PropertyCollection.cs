using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    public class PropertyCollection : IList<PartProperty>//, IXmlSerializable
    {
        public PartElement Owner { get; }

        public int Count => ((IList<PartProperty>)Properties).Count;

        public bool IsReadOnly => ((IList<PartProperty>)Properties).IsReadOnly;

        public PartProperty this[int index] { get => ((IList<PartProperty>)Properties)[index]; set => ((IList<PartProperty>)Properties)[index] = value; }

        private List<PartProperty> Properties;

        public PropertyCollection()
        {
            Properties = new List<PartProperty>();
        }

        public PropertyCollection(PartElement owner)
        {
            Owner = owner;
            Properties = new List<PartProperty>();
        }


        public PartProperty<T> DefineProperty<T>(string name)
        {
            if (!Properties.Any(x => x.Name == name))
            {
                var prop = new PartProperty<T>(this, name);
                Properties.Add(prop);
                return prop;
            }
            return null;
        }

        public PartProperty DefineProperty(string name, Type valueType)
        {
            if (!Properties.Any(x => x.Name == name))
            {
                var prop = new PartProperty(this, name, valueType);
                Properties.Add(prop);
                return prop;
            }
            return null;
        }

        public int IndexOf(PartProperty item)
        {
            return ((IList<PartProperty>)Properties).IndexOf(item);
        }

        public void Insert(int index, PartProperty item)
        {
            ((IList<PartProperty>)Properties).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<PartProperty>)Properties).RemoveAt(index);
        }

        public void Add(PartProperty item)
        {
            ((IList<PartProperty>)Properties).Add(item);
        }

        public void Clear()
        {
            ((IList<PartProperty>)Properties).Clear();
        }

        public bool Contains(PartProperty item)
        {
            return ((IList<PartProperty>)Properties).Contains(item);
        }

        public void CopyTo(PartProperty[] array, int arrayIndex)
        {
            ((IList<PartProperty>)Properties).CopyTo(array, arrayIndex);
        }

        public bool Remove(PartProperty item)
        {
            return ((IList<PartProperty>)Properties).Remove(item);
        }

        public IEnumerator<PartProperty> GetEnumerator()
        {
            return ((IList<PartProperty>)Properties).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<PartProperty>)Properties).GetEnumerator();
        }

        public delegate void CustomSerializeXmlDelegate(string propertyName, XElement element);

        public event CustomSerializeXmlDelegate CustomSerializeXml;
    }
}
