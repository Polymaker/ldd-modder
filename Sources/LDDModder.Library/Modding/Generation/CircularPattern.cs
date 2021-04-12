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
    public class CircularPattern : RepetitionPattern
    {
        private Vector3 _Origin;
        private Vector3 _Axis;

        public Vector3 Origin
        {
            get => _Origin;
            set => SetPropertyValue(ref _Origin, value);
        }

        public Vector3 Axis
        {
            get => _Axis;
            set => SetPropertyValue(ref _Axis, value);
        }

        public float Angle { get; set; }

        public bool EqualSpacing { get; set; }

        public override ClonePatternType Type => ClonePatternType.Circular;

        public CircularPattern()
        {
            Angle = 360f;
            EqualSpacing = true;
            Axis = Vector3.UnitZ;
        }

        public override ItemTransform ApplyTransform(ItemTransform transform, int instance)
        {
            if (instance == 0 || Repetitions == 0)
                return transform.Clone();

            var baseTrans = transform.ToMatrixD();

            var axisMat = Matrix4d.FromDirection((Vector3d)Axis, Vector3d.UnitZ);
            var originMat = Matrix4d.FromTranslation((Vector3d)Origin);

            var patternMat = axisMat * originMat;
            var itemRelativeMat = baseTrans * patternMat.Inverted();

            float angleInc = EqualSpacing ? (Angle / (Repetitions + 1)) : Angle;
            angleInc = MathHelper.ToRadian(angleInc);
            var rotMatrix = Matrix4d.FromAngleAxis(angleInc * instance, Vector3d.UnitZ);

            return ItemTransform.FromMatrix(itemRelativeMat * rotMatrix * patternMat);
        }

        public override Matrix4d GetPatternMatrix()
        {
            var axisMat = Matrix4d.FromDirection((Vector3d)Axis, Vector3d.UnitZ);
            var originMat = Matrix4d.FromTranslation((Vector3d)Origin);
            return axisMat * originMat;
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            elem.Add(XmlHelper.ToXmlAttribute(Axis, nameof(Axis)));
            elem.Add(XmlHelper.ToXmlAttribute(Origin, nameof(Origin)));
            elem.WriteAttribute(nameof(Angle), Angle);
            elem.WriteAttribute(nameof(EqualSpacing), EqualSpacing);
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            if (element.HasAttribute(nameof(Axis), out XAttribute axisAttr))
                Axis = XmlHelper.ParseVector3Attribute(axisAttr);

            if (element.HasAttribute(nameof(Origin), out XAttribute originAttr))
                Origin = XmlHelper.ParseVector3Attribute(originAttr);

            Angle = element.ReadAttribute(nameof(Angle), 360f);
            EqualSpacing = element.ReadAttribute(nameof(EqualSpacing), false);
        }
    }
}
