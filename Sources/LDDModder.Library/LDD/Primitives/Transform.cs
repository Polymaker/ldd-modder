using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives
{
    public class Transform
    {
        public float Angle { get; set; }
        public Vector3 Axis { get; set; }
        public Vector3 Translation { get; set; }

        public float Ax { get => Axis.X; set => Axis = new Vector3(value, Axis.Y, Axis.Z); }
        public float Ay { get => Axis.Y; set => Axis = new Vector3(Axis.X, value, Axis.Z); }
        public float Az { get => Axis.Z; set => Axis = new Vector3(Axis.X, Axis.Y, value); }

        public float Tx { get => Translation.X; set => Translation = new Vector3(value, Translation.Y, Translation.Z); }
        public float Ty { get => Translation.Y; set => Translation = new Vector3(Translation.X, value, Translation.Z); }
        public float Tz { get => Translation.Z; set => Translation = new Vector3(Translation.X, Translation.Y, value); }

        public Transform() { }

        public Transform(float angle, Vector3 axis, Vector3 translation)
        {
            Angle = angle;
            Axis = axis;
            Translation = translation;
        }

        public Transform(float angle, float ax, float ay, float az, float tx, float ty, float tz)
        {
            Angle = angle;
            Axis = new Vector3(ax, ay, az);
            Translation = new Vector3(tx, ty, tz);
        }

        public XAttribute[] ToXmlAttributes()
        {
            return new XAttribute[]
            {
                new XAttribute("angle", Angle.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute("ax", Axis.X.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute("ay", Axis.Y.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute("az", Axis.Z.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute("tx", Translation.X.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute("ty", Translation.Y.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute("tz", Translation.Z.ToString(NumberFormatInfo.InvariantInfo))
            };
        }

        public static Transform FromElementAttributes(XElement element)
        {
            return new Transform
            {
                Angle = element.ReadAttribute<float>("angle"),
                Axis = new Vector3(
                    element.ReadAttribute<float>("ax"),
                    element.ReadAttribute<float>("ay"),
                    element.ReadAttribute<float>("az")),
                Translation = new Vector3(
                    element.ReadAttribute<float>("tx"),
                    element.ReadAttribute<float>("ty"),
                    element.ReadAttribute<float>("tz")),
            };
        }

        public Matrix4 ToMatrix4()
        {
            var rot = Matrix4.FromAngleAxis(Angle * ((float)Math.PI / 180f), Axis);
            var trans = Matrix4.FromTranslation(Translation);
            return rot * trans;
        }

        public Vector3 GetPosition()
        {
            return ToMatrix4() * Vector3.Zero;
        }
    }
}
