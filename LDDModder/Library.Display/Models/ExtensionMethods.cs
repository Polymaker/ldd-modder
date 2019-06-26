using LDDModder.LDD.Primitives;
using OpenTK;
using Poly3D.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Display.Models
{
    public static class ExtensionMethods
    {

        public static Vector3 GetTranslation(this Transform transform)
        {
            return new Vector3((float)transform.Tx, (float)transform.Ty, (float)transform.Tz);
        }

        public static Vector3 GetRotationAxis(this Transform transform)
        {
            return new Vector3((float)transform.Ax, (float)transform.Ay, (float)transform.Az);
        }

        public static Rotation GetRotation(this Transform transform)
        {
            //Quaternion.FromAxisAngle(transform.GetRotationAxis(), )
            return Rotation.FromAxisAngle(transform.GetRotationAxis(), Angle.FromDegrees((float)transform.Angle));
        }

        public static ComplexTransform Get3dTransform(this Transform transform)
        {
            return new ComplexTransform(GetTranslation(transform), GetRotation(transform));
        }

        public static Transform ToLddTransform(this ComplexTransform transform)
        {
            transform.Rotation.Quaternion.ToAxisAngle(out Vector3 rotAxis, out float rotAmt);
            
            return new Transform(
                Math.Round(Angle.FromRadians(rotAmt).Degrees, 6), 
                Math.Round(rotAxis.X                        , 6),
                Math.Round(rotAxis.Y                        , 6), 
                Math.Round(rotAxis.Z                        , 6), 
                Math.Round(transform.Translation.X          , 6), 
                Math.Round(transform.Translation.Y          , 6), 
                Math.Round(transform.Translation.Z          , 6));
        }

        public static ComplexTransform Get3dTransform(this Connectivity connection)
        {
            return Get3dTransform(connection.Transform);
        }

        public static ComplexTransform Get3dTransform(this Collision collision)
        {
            return Get3dTransform(collision.Transform);
        }

        public static ComplexTransform Get3dTransform(this FlexBone bone)
        {
            return Get3dTransform(bone.Transform);
        }

        public static Transform ToLddTransform(this Poly3D.Engine.Transform transform)
        {
            var trans = transform.WorldPosition;
            Vector3 rotAxis;
            float rotAngle;
            transform.WorldRotation.Quaternion.ToAxisAngle(out rotAxis, out rotAngle);
            return new Transform(rotAngle, rotAxis.X, rotAxis.Y, rotAxis.Z, trans.X, trans.Y, trans.Z);
        }

    }
}
