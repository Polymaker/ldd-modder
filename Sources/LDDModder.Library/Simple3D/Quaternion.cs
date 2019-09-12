using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Quaternion
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vector3 Xyz
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public float LengthSquared => X * X + Y * Y + Z * Z + W * W;

        public float Length => (float)Math.Sqrt(LengthSquared);

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Quaternion(float pitch, float yaw, float roll)
        {
            float cp = (float)Math.Cos(pitch * 0.5f);
            float sp = (float)Math.Sin(pitch * 0.5f);

            float cy = (float)Math.Cos(yaw * 0.5f);
            float sy = (float)Math.Sin(yaw * 0.5f);

            float cr = (float)Math.Cos(roll * 0.5f);
            float sr = (float)Math.Sin(roll * 0.5f);

            W = cr * cp * cy + sr * sp * sy;
            X = sr * cp * cy - cr * sp * sy;
            Y = cr * sp * cy + sr * cp * sy;
            Z = cr * cp * sy - sr * sp * cy;
        }


        public void Normalize()
        {
            float scale = 1f / Length;
            Xyz *= scale;
            W *= scale;
        }

        public void ToAxisAngle(out Vector3 axis, out float angle)
        {
            Quaternion q = this;
            if (Math.Abs(q.W) > 1f)
            {
                q.Normalize();
            }
            angle = 2f * (float)Math.Acos((double)q.W);
            float den = (float)Math.Sqrt(1.0 - (double)(q.W * q.W));
            if (den > 0.0001f)
            {
                axis = q.Xyz / den;
            }
            else
            {
                axis = Vector3.UnitX;
            }
        }

        public static Quaternion FromEuler(Vector3 angles)
        {
            return new Quaternion(angles.X, angles.Y, angles.Z);
        }

        public static Vector3 ToEuler(Quaternion quat)
        {

            Vector3 pitchYawRoll = new Vector3();

            // roll (x-axis rotation)
            float sinr_cosp = +2.0f * (quat.W * quat.X + quat.Y * quat.Z);
            float cosr_cosp = +1.0f - 2.0f * (quat.X * quat.X + quat.Y * quat.Y);
            pitchYawRoll.Z = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            float sinp = 2.0f * (quat.W * quat.Y - quat.Z * quat.X);
            
            if (Math.Abs(sinp) >= 1)
                pitchYawRoll.X = (float)((Math.PI / 2f) * Math.Sign(sinp)); // use 90 degrees if out of range
            else
                pitchYawRoll.X = (float)Math.Asin(sinp);

            // yaw (z-axis rotation)
            float siny_cosp = 2.0f * (quat.W * quat.Z + quat.X * quat.Y);
            float cosy_cosp = 1.0f - 2.0f * (quat.Y * quat.Y + quat.Z * quat.Z);
            pitchYawRoll.Y = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return pitchYawRoll;
            /*
            //don't ask me why, but after a million tries, doing this will return the result I expected, and can convert back and forth from euler to quat
            quat = new Quaternion(-quat.Z, -quat.Y, -quat.X, quat.W);

            float sqw = quat.W * quat.W;
            float sqx = quat.X * quat.X;
            float sqy = quat.Y * quat.Y;
            float sqz = quat.Z * quat.Z;

            // If quaternion is normalised the unit is one, otherwise it is the correction factor
            float unit = sqx + sqy + sqz + sqw;
            float test = quat.X * quat.Y + quat.Z * quat.W;

            if (test > 0.499f * unit)
            {
                // Singularity at north pole
                pitchYawRoll.Y = 2f * (float)Math.Atan2(quat.X, quat.W);  // Yaw
                pitchYawRoll.X = (float)Math.PI * 0.5f;                         // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else if (test < -0.499f * unit)
            {
                // Singularity at south pole
                pitchYawRoll.Y = -2f * (float)Math.Atan2(quat.X, quat.W); // Yaw
                pitchYawRoll.X = -(float)Math.PI * 0.5f;                        // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }

            pitchYawRoll.Y = (float)Math.Atan2(2 * quat.Y * quat.W - 2 * quat.X * quat.Z, sqx - sqy - sqz + sqw);       // Yaw
            pitchYawRoll.X = (float)Math.Asin(2 * test / unit);                                             // Pitch
            pitchYawRoll.Z = (float)Math.Atan2(2 * quat.X * quat.W - 2 * quat.Y * quat.Z, -sqx + sqy - sqz + sqw);      // Roll

            return pitchYawRoll;*/
        }

        public override string ToString()
        {
            return $"[{X};{Y};{Z};{W}]";
        }
    }
}
