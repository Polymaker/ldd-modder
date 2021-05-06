using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding
{
    public class ElementReferenceCollection : ChangeTrackingCollection<ElementReference>
    {
        public PartElement Owner { get; }

        public List<Type> SupportedTypes { get; set; }

        public ElementReferenceCollection(PartElement owner)
        {
            Owner = owner;
            SupportedTypes = new List<Type>();
        }

        public void Add(PartElement element)
        {
            if (SupportedTypes.Count > 0)
            {
                var elemType = element.GetType();
                if (!SupportedTypes.Any(t => t.IsAssignableFrom(elemType)))
                    throw new NotSupportedException($"The element type {elemType.Name} is not supported in this collection");
            }

            if (element is ElementReference elemRef)
                base.Add(elemRef);
            else if (!Contains(element))
                base.Add(new ElementReference(element));
        }

        protected override void BeforeRemoveItem(ElementReference item)
        {
            base.BeforeRemoveItem(item);
            item.BeginChangeParent(Owner);
        }

        protected override void AfterRemoveItem(ElementReference item)
        {
            base.AfterRemoveItem(item);
            item.AssignParent(null);
        }

        protected override void BeforeAddItem(ElementReference item)
        {
            base.BeforeAddItem(item);
            item.BeginChangeParent(Owner);
        }

        protected override void AfterAddItem(ElementReference item)
        {
            base.AfterAddItem(item);
            item.AssignParent(Owner);
        }

        public void Remove(PartElement element)
        {
            if (element is ElementReference elemRef)
                base.Remove(elemRef);
            else
            {
                var existingRef = this.FirstOrDefault(r => r.ElementID == element.ID);
                if (existingRef != null)
                    base.Remove(existingRef);
            }
        }

        public void AddRange(IEnumerable<PartElement> elements)
        {
            foreach(var elem in elements)
                Add(elem);
        }

        public bool Contains(PartElement element)
        {
            if (element is ElementReference elemRef)
                return base.Contains(elemRef);
            return this.Any(r => r.ElementID == element.ID);
        }

        public XElement Serialize(string nodeName)
        {
            var elem = new XElement(nodeName);

            foreach (var elemRef in this)
                elem.Add(elemRef.SerializeToXml());

            return elem;
        }

        public void LoadFromXml(XElement element)
        {
            foreach (var refElem in element.Elements(ElementReference.NODE_NAME))
            {
                var elemRef = new ElementReference();
                elemRef.LoadFromXml(refElem);
                Add(elemRef);
            }
        }

        public IEnumerable<T> OfType<T>() where T : PartElement
        {
            return this.Select(x => x.Element).OfType<T>();
        }
    }
}
