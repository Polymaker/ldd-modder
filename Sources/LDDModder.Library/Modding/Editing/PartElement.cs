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
        private string _Comments;
        private string _Name;
        internal PartProject _Project;

        [XmlAttribute]
        public string ID { get; set; }
        
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

        internal List<PartElement> OwnedElements { get; }

        public IList<PartElement> ChildElements => OwnedElements.AsReadOnly();

        internal List<IElementCollection> Collections { get; }

        public IList<IElementCollection> ElementCollections => Collections.AsReadOnly();

        internal bool IsLoading => Project?.IsLoading ?? false;

        [XmlIgnore]
        public PartProject Project => _Project ?? Parent?.Project;

        [XmlIgnore]
        public PartElement Parent { get; internal set; }

        public event EventHandler<ElementValueChangedEventArgs> PropertyChanged;

        public PartElement()
        {
            Collections = new List<IElementCollection>();
            OwnedElements = new List<PartElement>();
        }

        internal void AssignParent(PartElement parent)
        {
            Parent = parent;
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
                if (property is PartElement oldElem)
                {
                    oldElem.AssignParent(null);
                    OwnedElements.Remove(oldElem);
                }
                if (value is PartElement newElem)
                {
                    newElem.AssignParent(this);
                    OwnedElements.Add(newElem);
                }

                object oldValue = property;
                property = value;

                if (Project != null && !IsLoading)
                {
                    var args = new ElementValueChangedEventArgs(this, propertyName, oldValue, value);
                    PropertyChanged?.Invoke(this, args);
                    Project.OnElementPropertyChanged(args);
                }
                
                return true;
            }
            return false;
        }


        public virtual IEnumerable<PartElement> GetAllChilds()
        {
            return OwnedElements.Concat(Collections.SelectMany(x => x.GetElements()));
        }

        public virtual IEnumerable<PartElement> GetChildsHierarchy(bool includeSelf = false)
        {
            if (includeSelf)
                yield return this;
            foreach(var child in GetAllChilds())
            {
                yield return child;
                foreach (var subChild in child.GetChildsHierarchy())
                    yield return subChild;
            }
        }

        
        public virtual bool TryRemove()
        {
            if (Project == null && Parent != null)
                return true;//parent was removed

            if (Parent != null)
            {
                foreach(var col in Parent.Collections)
                {
                    if (col.Contains(this))
                    {
                        col.Remove(this);
                        return true;
                    }
                }
            }
            else if (Project != null)
            {
                if (Project.Collisions.Contains(this))
                {
                    Project.Collisions.Remove(this);
                    return true;
                }
                else if (Project.Connections.Contains(this))
                {
                    Project.Connections.Remove(this);
                    return true;
                }
                else if (Project.Surfaces.Contains(this))
                {
                    Project.Surfaces.Remove(this);
                    return true;
                }
                else if (Project.Bones.Contains(this))
                {
                    Project.Bones.Remove(this);
                    return true;
                }
            }
            return false;
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

        public string GetFullElementType()
        {
            if (this is PartConnection conn)
                return "Connection" + conn.ConnectorType.ToString();
            return GetType().Name;
        }
    }
}
