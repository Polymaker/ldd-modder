using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding
{
    public abstract class ClonePattern : PartElement
    {
        public const string NODE_NAME = "ClonePattern";

        public abstract ClonePatternType Type { get; }
        public ElementReferenceCollection Elements { get; set; }

        public virtual int NumberOfInstances => 1;
        
        protected ClonePattern()
        {
            Elements = new ElementReferenceCollection(this);
            TrackCollectionChanges(Elements);
        }

        public IEnumerable<PartElement> GetElements()
        {
            return Elements.Select(r => r.Element);
        }

        public abstract ItemTransform ApplyTransform(ItemTransform transform, int instance);

        public abstract Simple3D.Matrix4d GetPatternMatrix();

        public PartElement GetClonedElement(PartElement element, int instance)
        {
            if (instance == 0)
                return element;

            if (!(element is IClonableElement))
                return null;

            var trans = ApplyTransform((element as IPhysicalElement).Transform, instance);
            var newElem = (element as IClonableElement).Clone();
            if (newElem is IPhysicalElement physElem)
                physElem.Transform = trans;
            return newElem;
        }

        public virtual IEnumerable<PartElement> GetAllClonedElements()
        {
            foreach (var elemRef in Elements)
            {
                foreach (var clone in GetElementClones(elemRef.Element))
                    yield return clone;
            }
        }

        public virtual IEnumerable<PartElement> GetElementClones(PartElement element)
        {
            for (int i = 0; i < NumberOfInstances; i++)
                yield return GetClonedElement(element, i + 1);
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);

            elem.WriteAttribute(nameof(Type), Type);

            elem.Add(Elements.Serialize(nameof(Elements)));

            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            

            Elements.Clear();
            if (element.HasElement(nameof(Elements), out XElement elemsElem))
                Elements.LoadFromXml(elemsElem);
        }

        public static ClonePattern FromXml(XElement element)
        {
            ClonePattern patttern = null;
            var patternType = element.ReadAttribute<ClonePatternType>("Type");
            switch (patternType)
            {
                case ClonePatternType.Mirror:
                    patttern = new MirrorPattern();
                    break;
                case ClonePatternType.Linear:
                    patttern = new LinearPattern();
                    break;
                case ClonePatternType.Circular:
                    patttern = new CircularPattern();
                    break;
            }

            if (patttern != null)
                patttern.LoadFromXml(element);

            return patttern;
        }
    }
}
