using LDDModder.Serialization;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
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
    public class ItemTransform : IXmlSerializable
    {
        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }

        public ItemTransform()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
        }

        public ItemTransform(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public static ItemTransform FromMatrix(Matrix4 matrix)
        {
            var rot = matrix.ExtractRotation();

            return new ItemTransform(matrix.ExtractTranslation(), Quaternion.ToEuler(rot) * (180f / (float)Math.PI));
        }

        public static ItemTransform FromLDD(LDD.Primitives.Transform transform)
        {
            var trans = FromMatrix(transform.ToMatrix4());
            trans.Position = trans.Position.Rounded();
            trans.Rotation = trans.Rotation.Rounded();
            return trans;
        }

        public Matrix4 ToMatrix()
        {
            var quat = Quaternion.FromEuler(Rotation * ((float)Math.PI / 180f));
            quat.ToAxisAngle(out Vector3 axis, out float angle);
            var rot = Matrix4.FromAngleAxis(angle, axis);
            var trans = Matrix4.FromTranslation(Position);
            return rot * trans;
        }

        public LDD.Primitives.Transform ToLDD()
        {
            return LDD.Primitives.Transform.FromMatrix(ToMatrix());
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
                Position = new Vector3(
                    element.ReadAttribute("X", 0f),
                    element.ReadAttribute("Y", 0f),
                    element.ReadAttribute("Z", 0f)),

                Rotation = new Vector3(
                    element.ReadAttribute("Pitch", 0f),
                    element.ReadAttribute("Yaw", 0f),
                    element.ReadAttribute("Roll", 0f))
            };
            return trans;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            float x = float.Parse(reader.GetAttribute("X"), CultureInfo.InvariantCulture);
            float y = float.Parse(reader.GetAttribute("Y"), CultureInfo.InvariantCulture);
            float z = float.Parse(reader.GetAttribute("Z"), CultureInfo.InvariantCulture);
            Position = new Vector3(x, y, z);
            x = float.Parse(reader.GetAttribute("Pitch"), CultureInfo.InvariantCulture);
            y = float.Parse(reader.GetAttribute("Yaw"), CultureInfo.InvariantCulture);
            z = float.Parse(reader.GetAttribute("Roll"), CultureInfo.InvariantCulture);
            Rotation = new Vector3(x, y, z);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("X", Position.X.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Y", Position.Y.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Z", Position.Z.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Pitch", Rotation.X.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Yaw", Rotation.Y.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Roll", Rotation.Z.ToString(CultureInfo.InvariantCulture));
        }
    }
}
