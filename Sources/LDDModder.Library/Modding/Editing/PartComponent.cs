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

        [XmlIgnore]
        public string Comments
        {
            get => _Comments;
            set => SetPropertyValue(ref _Comments, value);
        }

        public PropertyCollection Properties { get; }

        internal bool IsLoading => Project?.IsLoading ?? false;

        internal PartProject _Project;

        [XmlIgnore]
        public PartProject Project => _Project ?? Parent?.Project;

        [XmlIgnore]
        public PartComponent Parent { get; internal set; }

        public PartComponent()
        {
            Properties = new PropertyCollection(this);
            DefineProperties();
        }

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
            {
                elem.Add(new XComment(Comments));
                //elem.Add(XmlHelper.ToXml(() => Comments));
            }

            return elem;
        }

        protected internal virtual void LoadFromXml(XElement element)
        {
            
            if (element.Attribute("RefID") != null)
                RefID = element.Attribute("RefID").Value;

            var commentElem = element.Elements().FirstOrDefault(x => x.NodeType == System.Xml.XmlNodeType.Comment);
            if (commentElem != null)
                Comments = commentElem.Value;
            else if (element.Element("Comments") != null)
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
    
        protected virtual void DefineProperties()
        {

        }
    }
}
