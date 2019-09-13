using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public struct ItemTransform
    {
        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }

        //public ItemTransform()
        //{
        //    Position = Vector3.Empty;
        //    Rotation = Vector3.Empty;
        //}

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

        public XElement SerializeToXml()
        {
            var elem = new XElement("Transform");
            elem.AddNumberAttribute("X", Position.X);
            elem.AddNumberAttribute("Y", Position.Y);
            elem.AddNumberAttribute("Z", Position.Z);
            elem.AddNumberAttribute("Pitch", Rotation.X);
            elem.AddNumberAttribute("Yaw", Rotation.Y);
            elem.AddNumberAttribute("Roll", Rotation.Z);
            return elem;
            //var posElem = new XElement("Position");
            //posElem.AddNumberAttribute("X", Position.X);
            //posElem.AddNumberAttribute("Y", Position.Y);
            //posElem.AddNumberAttribute("Z", Position.Z);
            //var rotElem = new XElement("Rotation");
            //rotElem.AddNumberAttribute("Pitch", Rotation.X);
            //rotElem.AddNumberAttribute("Yaw", Rotation.Y);
            //rotElem.AddNumberAttribute("Roll", Rotation.Z);

            //return new XElement("Transform", posElem, rotElem);
        }
    }
}
