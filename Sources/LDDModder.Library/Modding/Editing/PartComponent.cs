using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    public abstract class PartComponent
    {
        [XmlAttribute]
        public string RefID { get; set; }

        private string _Comments;

        [XmlElement]
        public string Comments
        {
            get => _Comments;
            set => SetPropertyValue(ref _Comments, value);
        }

        internal bool IsLoading => Project?.IsLoading ?? false;

        internal PartProject _Project;

        public PartProject Project => _Project ?? Parent?.Project;

        public PartComponent Parent { get; internal set; }


        public virtual XElement SerializeToXml()
        {
            return SerializeToXmlBase(GetType().Name);
        }

        protected XElement SerializeToXmlBase(string elementName)
        {
            var elem = new XElement(elementName);

            if (!string.IsNullOrEmpty(RefID))
                elem.Add(new XAttribute("RefID", RefID));

            if (!string.IsNullOrEmpty(Comments))
                elem.Add(XmlHelper.ToXml(() => Comments));

            return elem;
        }

        protected internal virtual void LoadFromXml(XElement element)
        {
            if (element.Attribute("RefID") != null)
                RefID = element.Attribute("RefID").Value;

            if (element.Element("Comments") != null)
                Comments = element.Element("Comments").Value;
        }

        protected bool SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                if (Project != null && !IsLoading)
                {
                    Project.OnComponentPropertyChanged(
                        new PropertyChangedEventArgs(this, propertyName, property, value));
                }
                property = value;
                return true;
            }
            return false;
        }
    }
}
