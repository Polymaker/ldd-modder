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
    public abstract class PartElement
    {

        [XmlAttribute]
        public string ID { get; set; }

        private string _Comments;
        private string _Name;

        [XmlAttribute]
        public string Name
        {
            get => _Name;
            set => SetPropertyValue(ref _Name, value);
        }

        [XmlElement]
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
        public PartElement Parent { get; internal set; }

        public PartElement()
        {
            Properties = new PropertyCollection(this);
            //DefineProperties();
        }

        public virtual XElement SerializeToXml()
        {
            return SerializeToXmlBase(GetType().Name);
        }

        protected XElement SerializeToXmlBase(string elementName)
        {
            var elem = new XElement(elementName);

            if (!string.IsNullOrEmpty(ID))
                elem.Add(new XAttribute("ID", ID));

            if (!string.IsNullOrEmpty(Name))
                elem.Add(new XAttribute("Name", Name));

            if (!string.IsNullOrEmpty(Comments))
                elem.Add(new XElement("Comments", Comments));

            return elem;
        }

        protected internal virtual void LoadFromXml(XElement element)
        {
            if (element.HasAttribute("ID", out XAttribute idAttr))
                ID = idAttr.Value;

            if (element.HasAttribute("Name", out XAttribute nameAttr))
                Name = nameAttr.Value;

            if (element.HasElement("Comments", out XElement commentElem))
                Comments = commentElem.Value;
        }

        protected bool SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                if (Project != null && !IsLoading)
                {
                    Project.OnElementPropertyChanged(
                        new PropertyChangedEventArgs(this, propertyName, property, value));
                }
                property = value;
                return true;
            }
            return false;
        }



        protected virtual IEnumerable<PartElement> GetAllChilds()
        {
            return Enumerable.Empty<PartElement>();
        }

        public virtual IEnumerable<PartElement> GetChildsHierarchy()
        {
            foreach(var child in GetAllChilds())
            {
                yield return child;
                foreach (var subChild in child.GetChildsHierarchy())
                    yield return subChild;
            }
        }

        

        public Type GetElementType()
        {
            if (this is PartSurface)
                return typeof(PartSurface);
            else if (this is SurfaceComponent)
                return typeof(SurfaceComponent);
            else if (this is ModelMesh)
                return typeof(ModelMesh);
            else if (this is PartCollision)
                return typeof(PartCollision);
            else if (this is PartConnection)
                return typeof(PartConnection);
            else if (this is StudReference)
                return typeof(StudReference);
            return typeof(PartElement);
        }

        //protected virtual void DefineProperties()
        //{

        //}
    }
}
