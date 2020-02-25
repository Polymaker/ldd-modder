using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Quaterniond
    {
        public static readonly Quaterniond Identity = new Quaterniond(0d, 0d, 0d, 1d);

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public Vector3d Xyz
        {
            get => new Vector3d(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public double LengthSquared => X * X + Y * Y + Z * Z + W * W;

        public double Length => Math.Sqrt(LengthSquared);

        public Quaterniond(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Quaterniond(double pitch, double yaw, double roll)
        {
            yaw *= 0.5;
            pitch *= 0.5;
            roll *= 0.5;
            double num = Math.Cos(yaw);
            double num2 = Math.Cos(pitch);
            double num3 = Math.Cos(roll);
            double num4 = Math.Sin(yaw);
            double num5 = Math.Sin(pitch);
            double num6 = Math.Sin(roll);
            W = num * num2 * num3 - num4 * num5 * num6;
            X = num4 * num5 * num3 + num * num2 * num6;
            Y = num4 * num2 * num3 + num * num5 * num6;
            Z = num * num5 * num3 - num4 * num2 * num6;
        }

        public void Normalize()
        {
            double scale = 1d / Length;
            Xyz *= scale;
            W *= scale;
        }

        public static Quaterniond operator *(Quaterniond left, Quaterniond right)
        {
            double w = left.W * right.W - Vector3d.Dot(left.Xyz, right.Xyz);
            var xyz = right.W * left.Xyz + left.W * right.Xyz + Vector3d.Cross(left.Xyz, right.Xyz);
            return new Quaterniond(xyz.X, xyz.Y, xyz.Z, w);
        }

        public void ToAxisAngle(out Vector3d axis, out double angle)
        {
            Quaterniond q = this;
            if (Math.Abs(q.W) > 1d)
            {
                q.Normalize();
            }
            angle = 2d * Math.Acos(q.W);
            double den = Math.Sqrt(1.0 - (q.W * q.W));
            if (den > 0.0001d)
            {
                axis = q.Xyz / den;
            }
            else
            {
                axis = Vector3d.UnitX;
            }
        }

        public static Quaterniond FromAxisAngle(Vector3d axis, double angle)
        {
            if (axis.Length == 0d)
            {
                return Identity;
            }
            Quaterniond result = Identity;
            angle *= 0.5d;
            axis.Normalize();
            result.Xyz = axis * (double)Math.Sin((double)angle);
            result.W = (double)Math.Cos((double)angle);
            result.Normalize();
            return result;
        }

        public static Quaterniond FromEuler(double x, double y, double z)
        {
            return FromEuler(new Vector3d(x, y, z));
        }

        public static Quaterniond FromEuler(Vector3d angles)
        {
            var roll = FromAxisAngle(Vector3d.UnitZ * -1d, angles.Z);
            var pitch = FromAxisAngle(Vector3d.UnitX * -1d, angles.X);
            var yaw = FromAxisAngle(Vector3d.UnitY * -1d, angles.Y);
            return yaw * pitch * roll;
            //return new Quaternion(angles.X, angles.Y, angles.Z);
        }

        public static Vector3d ToEuler(Quaterniond quat)
        {
            Vector3d pitchYawRoll = new Vector3d();

            //don't ask me why, but after a million tries, doing this will return the result I expect, and can convert back and forth from euler to quat
            quat = new Quaterniond(-quat.Z, -quat.Y, -quat.X, quat.W);

            double sqw = quat.W * quat.W;
            double sqx = quat.X * quat.X;
            double sqy = quat.Y * quat.Y;
            double sqz = quat.Z * quat.Z;

            // If quaternion is normalised the unit is one, otherwise it is the correction factor
            double unit = sqx + sqy + sqz + sqw;
            double test = quat.X * quat.Y + quat.Z * quat.W;

            if (test > 0.499d * unit)
            {
                // Singularity at north pole
                pitchYawRoll.Y = 2d * (double)Math.Atan2(quat.X, quat.W);  // Yaw
                pitchYawRoll.X = (double)Math.PI * 0.5d;                         // Pitch
                pitchYawRoll.Z = 0d;                                // Roll
                return pitchYawRoll;
            }
            else if (test < -0.499d * unit)
            {
                // Singularity at south pole
                pitchYawRoll.Y = -2d * (double)Math.Atan2(quat.X, quat.W); // Yaw
                pitchYawRoll.X = -(double)Math.PI * 0.5d;                        // Pitch
                pitchYawRoll.Z = 0d;                                // Roll
                return pitchYawRoll;
            }

            pitchYawRoll.Y = (double)Math.Atan2(2 * quat.Y * quat.W - 2 * quat.X * quat.Z, sqx - sqy - sqz + sqw);       // Yaw
            pitchYawRoll.X = (double)Math.Asin(2 * test / unit);                                             // Pitch
            pitchYawRoll.Z = (double)Math.Atan2(2 * quat.X * quat.W - 2 * quat.Y * quat.Z, -sqx + sqy - sqz + sqw);      // Roll

            return pitchYawRoll;
        }

        public override string ToString()
        {
            return $"[{X};{Y};{Z};{W}]";
        }
    }
}
