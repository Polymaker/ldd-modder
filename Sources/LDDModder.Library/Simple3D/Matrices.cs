using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Matrix3
    {
        public Vector3 Row0;
        public Vector3 Row1;
        public Vector3 Row2;

        public Vector3 Column0 => new Vector3(Row0.X, Row1.X, Row2.X);
        public Vector3 Column1 => new Vector3(Row0.Y, Row1.Y, Row2.Y);
        public Vector3 Column2 => new Vector3(Row0.Z, Row1.Z, Row2.Z);

        public float M11
        {
            get => Row0.X;
            set => Row0.X = value;
        }

        public float M12
        {
            get => Row0.Y;
            set => Row0.Y = value;
        }

        public float M13
        {
            get => Row0.Z;
            set => Row0.Z = value;
        }

        public float M21
        {
            get => Row1.X;
            set => Row1.X = value;
        }

        public float M22
        {
            get => Row1.Y;
            set => Row1.Y = value;
        }

        public float M23
        {
            get => Row1.Z;
            set => Row1.Z = value;
        }

        public float M31
        {
            get => Row2.X;
            set => Row2.X = value;
        }

        public float M32
        {
            get => Row2.Y;
            set => Row2.Y = value;
        }

        public float M33
        {
            get => Row2.Z;
            set => Row2.Z = value;
        }

        public float this[int x, int y]
        {
            get
            {
                Vector3 row;
                switch (y)
                {
                    case 0:
                        row = Row0;
                        break;
                    case 1:
                        row = Row1;
                        break;
                    case 2:
                        row = Row2;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                switch (x)
                {
                    case 0:
                        return row.X;
                    case 1:
                        return row.Y;
                    case 2:
                        return row.Z;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                Vector3 row;
                switch (y)
                {
                    case 0:
                        row = Row0;
                        break;
                    case 1:
                        row = Row1;
                        break;
                    case 2:
                        row = Row2;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                switch (x)
                {
                    case 0:
                        row.X = value;
                        break;
                    case 1:
                        row.Y = value;
                        break;
                    case 2:
                        row.Z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                switch (y)
                {
                    case 0:
                        Row0 = row;
                        break;
                    case 1:
                        Row1 = row;
                        break;
                    case 2:
                        Row2 = row;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public float this[int index]
        {
            get
            {
                int x = index % 3;
                int y = (int)Math.Floor(index / 3d);
                return this[x, y];
            }
            set
            {
                int x = index % 3;
                int y = (int)Math.Floor(index / 3d);
                this[x, y] = value;
            }
        }
    }
}
