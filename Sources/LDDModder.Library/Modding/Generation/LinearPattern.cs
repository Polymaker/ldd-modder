using LDDModder.Serialization;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding
{
    public class LinearPattern : RepetitionPattern
    {
        //public Vector3 Origin { get; set; }
        private Vector3 _Direction;
        private float _Offset;

        public Vector3 Direction
        {
            get => _Direction;
            set => SetPropertyValue(ref _Direction, value);
        }

        public float Offset
        {
            get => _Offset;
            set => SetPropertyValue(ref _Offset, value);
        }

        public override ClonePatternType Type => ClonePatternType.Linear;

        public LinearPattern()
        {
            Direction = Vector3.UnitX;
            Offset = 1;
        }

        public override ItemTransform ApplyTransform(ItemTransform transform, int instance)
        {
            if (instance == 0)
                return transform.Clone();

            var baseTrans = transform.ToMatrixD();
            var translation = Matrix4d.FromTranslation((Vector3d)Direction * (Offset * instance));
            var final = baseTrans * translation;
            return ItemTransform.FromMatrix(final);
        }

        public override XElement SerializeToXml()
        {
            var baseElem = base.SerializeToXml();
            //baseElem.AddElement(nameof(Direction), XmlHelper.ToXmlAttributes(Direction));
            baseElem.Add(XmlHelper.ToXmlAttribute(Direction, nameof(Direction)));
            //baseElem.WriteAttribute(nameof(Direction), Direction);
            baseElem.WriteAttribute(nameof(Offset), Offset);
            return baseElem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            if (element.HasAttribute(nameof(Direction), out XAttribute dirAttr))
                Direction = XmlHelper.ParseVector3Attribute(dirAttr);

            Offset = element.ReadAttribute(nameof(Offset), 0f);
            
        }

        public override Matrix4d GetPatternMatrix()
        {
            return Matrix4d.FromDirection((Vector3d)Direction, Vector3d.UnitZ);
        }
    }
}
