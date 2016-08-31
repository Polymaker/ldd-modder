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
        public static Vector3 GetTranslation(this Collision collision)
        {
            return new Vector3((float)collision.Tx, (float)collision.Ty, (float)collision.Tz);
        }

        public static Vector3 GetTranslation(this Connectivity connection)
        {
            return new Vector3((float)connection.Tx, (float)connection.Ty, (float)connection.Tz);
        }

        public static Vector3 GetRotationAxis(this Collision collision)
        {
            return new Vector3((float)collision.Ax, (float)collision.Ay, (float)collision.Az);
        }

        public static Vector3 GetRotationAxis(this Connectivity connection)
        {
            return new Vector3((float)connection.Ax, (float)connection.Ay, (float)connection.Az);
        }

        public static Rotation GetRotation(this Collision collision)
        {
            return Rotation.FromAxisAngle(collision.GetRotationAxis(), Angle.FromDegrees((float)collision.Angle));
        }

        public static Rotation GetRotation(this Connectivity connection)
        {
            return Rotation.FromAxisAngle(connection.GetRotationAxis(), Angle.FromDegrees((float)connection.Angle));
        }
    }
}
