using LDDModder.Serialization;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [XmlRoot("Transform")]
    public class ItemTransform : ChangeTrackingObject
    {
        private Vector3d _Position;
        private Vector3d _Rotation;

        public Vector3d Position
        {
            get => _Position;
            set => SetPropertyValue(ref _Position, value);
        }

        public Vector3d Rotation
        {
            get => _Rotation;
            set => SetPropertyValue(ref _Rotation, value);
        }

        public bool IsEmpty => Position == Vector3d.Zero && Rotation == Vector3d.Zero;

        public ItemTransform()
        {
            Position = Vector3d.Zero;
            Rotation = Vector3d.Zero;
        }

        public ItemTransform(Vector3d position, Vector3d rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public bool Equals(ItemTransform item)
        {
            return Position.Equals(item.Position) && Rotation.Equals(item.Rotation);
        }

        public ItemTransform Clone()
        {
            return new ItemTransform(Position, Rotation);
        }

        public static ItemTransform FromMatrix(Matrix4 matrix)
        {
            return FromMatrix((Matrix4d)matrix);
        }

        public static ItemTransform FromMatrix(Matrix4d matrix)
        {
            var rot = matrix.ExtractRotation();

            return new ItemTransform(
                matrix.ExtractTranslation(),
                Quaterniond.ToEuler(rot) * (180d / Math.PI));
        }

        public static ItemTransform FromLDD(LDD.Primitives.Transform transform)
        {
            var trans = FromMatrix(transform.ToMatrix4d());
            trans.Position = trans.Position.Rounded();
            trans.Rotation = trans.Rotation.Rounded();
            return trans;
        }

        public Matrix4 ToMatrix()
        {
            return (Matrix4)ToMatrixD();
        }

        public Matrix4d ToMatrixD()
        {
            var quat = Quaterniond.FromEuler((Vector3d)Rotation * (Math.PI / 180d));
            quat.ToAxisAngle(out Vector3d axis, out double angle);
            var rot = Matrix4d.FromAngleAxis(angle, axis);
            var trans = Matrix4d.FromTranslation((Vector3d)Position);
            return rot * trans;
        }

        public LDD.Primitives.Transform ToLDD()
        {
            return LDD.Primitives.Transform.FromMatrix(ToMatrixD());
        }

        public XElement GetLddXml()
        {
            var lddTrans = ToLDD();
            return new XElement("Transform", lddTrans.ToXmlAttributes());
        }

        public XElement SerializeToXml(string elementName = "Transform")
        {
            var elem = new XElement(elementName);
            elem.AddNumberAttribute("X", Position.X);
            elem.AddNumberAttribute("Y", Position.Y);
            elem.AddNumberAttribute("Z", Position.Z);
            elem.AddNumberAttribute("Pitch", Rotation.X);
            elem.AddNumberAttribute("Yaw", Rotation.Y);
            elem.AddNumberAttribute("Roll", Rotation.Z);
            return elem;
        }

        public static ItemTransform FromXml(XElement element)
        {
            var trans = new ItemTransform
            {
                Position = new Vector3d(
                    element.ReadAttribute("X", 0d),
                    element.ReadAttribute("Y", 0d),
                    element.ReadAttribute("Z", 0d)),

                Rotation = new Vector3d(
                    element.ReadAttribute("Pitch", 0d),
                    element.ReadAttribute("Yaw", 0d),
                    element.ReadAttribute("Roll", 0d))
            };
            return trans;
        }
    }
}
