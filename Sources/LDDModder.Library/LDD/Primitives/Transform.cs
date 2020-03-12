using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives
{
    public class Transform : ChangeTrackingObject
    {
        private Vector3d _Axis;
        private double _Angle;
        private Vector3d _Translation;

        public static readonly string[] AttributeNames = new string[] { "angle", "ax", "ay", "az", "tx", "ty", "tz" };

        public double Angle
        {
            get => _Angle;
            set => SetPropertyValue(ref _Angle, value);
        }

        public Vector3d Axis
        {
            get => _Axis;
            set => SetPropertyValue(ref _Axis, value);
        }

        public Vector3d Translation
        {
            get => _Translation;
            set => SetPropertyValue(ref _Translation, value);
        }

        public double Ax { get => Axis.X; set => Axis = new Vector3d(value, Axis.Y, Axis.Z); }
        public double Ay { get => Axis.Y; set => Axis = new Vector3d(Axis.X, value, Axis.Z); }
        public double Az { get => Axis.Z; set => Axis = new Vector3d(Axis.X, Axis.Y, value); }

        public double Tx { get => Translation.X; set => Translation = new Vector3d(value, Translation.Y, Translation.Z); }
        public double Ty { get => Translation.Y; set => Translation = new Vector3d(Translation.X, value, Translation.Z); }
        public double Tz { get => Translation.Z; set => Translation = new Vector3d(Translation.X, Translation.Y, value); }

        public Transform()
        {
            Axis = Vector3d.UnitY;
        }

        public Transform(double angle, Vector3d axis, Vector3d translation)
        {
            Angle = angle;
            Axis = axis;
            Translation = translation;
        }

        public Transform(float angle, Vector3 axis, Vector3 translation)
        {
            Angle = angle;
            Axis = (Vector3d)axis;
            Translation = (Vector3d)translation;
        }

        public Transform(double angle, 
            double ax, double ay, double az, 
            double tx, double ty, double tz)
        {
            Angle = angle;
            Axis = new Vector3d(ax, ay, az);
            Translation = new Vector3d(tx, ty, tz);
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
                Angle = element.ReadAttribute("angle", 0d),
                Axis = new Vector3d(
                    element.ReadAttribute("ax", 0d),
                    element.ReadAttribute("ay", 0d),
                    element.ReadAttribute("az", 0d)),
                Translation = new Vector3d(
                    element.ReadAttribute("tx", 0d),
                    element.ReadAttribute("ty", 0d),
                    element.ReadAttribute("tz", 0d)),
            };
        }

        public Matrix4 ToMatrix4()
        {
            return (Matrix4)ToMatrix4d();
        }

        public Matrix4d ToMatrix4d()
        {
            var rot = Matrix4d.FromAngleAxis(Angle * (Math.PI / 180d), Axis.Normalized());
            var trans = Matrix4d.FromTranslation(Translation);
            return rot * trans;
        }

        public static Transform FromMatrix(Matrix4 matrix)
        {
            return FromMatrix((Matrix4d)matrix);
        }

        public static Transform FromMatrix(Matrix4d matrix)
        {
            var rot = matrix.ExtractRotation();
            rot.ToAxisAngle(out Vector3d axis, out double angle);
            angle *= 180d / Math.PI;
            return new Transform(Math.Round(angle, 4), 
                axis.Rounded(), 
                matrix.ExtractTranslation().Rounded());
        }

        //public Vector3 GetPosition()
        //{
        //    return ToMatrix4().TransformPosition(Vector3.Zero);
        //}
    }
}
