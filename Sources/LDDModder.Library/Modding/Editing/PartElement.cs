using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Dictionary<Type, IElementExtender> _Extenders;

        [XmlAttribute]
        public string ID { get; set; }
        
        [XmlAttribute]
        public string Name
        {
            get => _Name;
            //set => SetPropertyValue(ref _Name, value);
            set
            {
                if (_Name != value)
                {
                    if (Project != null)
                        Project.RenameElement(this, value);
                    else
                        _Name = value;
                }
            }
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

        [XmlIgnore]
        public int HierarchyLevel => Parent == null ? 0 : Parent.HierarchyLevel + 1;

        public event EventHandler<ElementValueChangedEventArgs> PropertyChanged;

        public event PropertyValueChangedEventHandler ExtendedPropertyChanged;

        public PartElement()
        {
            Collections = new List<IElementCollection>();
            OwnedElements = new List<PartElement>();
            _Extenders = new Dictionary<Type, IElementExtender>();
        }

        #region Project & Parent Handling

        internal void AssignProject(PartProject project)
        {
            _Project = project;
            OnProjectAssigned();
        }

        protected virtual void OnProjectAssigned()
        {

        }

        internal void AssignParent(PartElement parent)
        {
            Parent = parent;
            OnParentAssigned();
        }

        protected virtual void OnParentAssigned()
        {

        }

        #endregion

        #region Xml

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
                InternalSetName(nameAttr.Value);

            if (element.HasElement("Comments", out XElement commentElem))
                Comments = commentElem.Value;
        }

        #endregion

        #region Property Assignation Handling

        protected bool SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!ChangeTrackingObject.AreEquals(property, value))
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

                var args = new ElementValueChangedEventArgs(this, propertyName, oldValue, value);
                RaisePropertyValueChanged(args);

                if (propertyName == nameof(Name) && oldValue != null)
                    OnAfterRename(oldValue as string, value as string);

                return true;
            }
            return false;
        }

        protected virtual void OnAfterRename(string oldName, string newName)
        {

        }

        internal void InternalSetName(string name, bool raiseValueChange = false)
        {
            if (raiseValueChange)
                SetPropertyValue(ref _Name, name, nameof(Name));
            else
                _Name = name;
        }

        internal void InternalSetID(string id)
        {
            ID = id;
        }

        internal void InternalSetNameAndID(string id, string name)
        {
            ID = id;
            _Name = name;
        }

        protected virtual void OnPropertyChanged(ElementValueChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        protected void RaisePropertyValueChanged(ElementValueChangedEventArgs args)
        {
            if (Project != null && !IsLoading)
            {
                OnPropertyChanged(args);
                Project.OnElementPropertyChanged(args);
            }
        }



        #endregion


        public T GetExtension<T>() where T : IElementExtender
        {
            if (_Extenders.TryGetValue(typeof(T), out IElementExtender extender))
                return (T)extender;

            if (_Extenders.Count > 0)
            {
                if (typeof(T).IsInterface)
                {
                    return (T)_Extenders.Values.FirstOrDefault(x => 
                        typeof(T).IsAssignableFrom(x.GetType()));
                }

                var test = _Extenders.FirstOrDefault(x => typeof(T).IsAssignableFrom(x.Value.GetType()));
                if (test.Value != null)
                    _Extenders.Add(typeof(T), test.Value);

                return (T)test.Value;
            }

            if (!typeof(T).IsInterface && ElementExtenderFactory.CanExtendElement(GetType(), typeof(T)))
            {
                extender = ElementExtenderFactory.CreateExtender(this, typeof(T));
                _Extenders.Add(typeof(T), extender);

                if (extender is INotifyPropertyValueChanged notifyProperty)
                    notifyProperty.PropertyValueChanged += Extender_PropertyValueChanged;

                return (T)extender;
            }

            return default(T);
        }

        private void Extender_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            ExtendedPropertyChanged?.Invoke(sender, e);
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

        public IElementCollection GetParentCollection()
        {
            if (Parent != null)
            {
                foreach (var collection in Parent.Collections)
                {
                    if (collection.Contains(this))
                        return collection;
                }
            }
            return null;
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

        public virtual List<ValidationMessage> ValidateElement()
        {
            return new List<ValidationMessage>();
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
            else if (this is ModelMeshReference)
                return typeof(ModelMeshReference);

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
