using LDDModder.LDD.Meshes;
using LDDModder.Serialization;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    public class BoundingBox : ChangeTrackingObject, IXmlObject
    {
        private Vector3d _Min;
        private Vector3d _Max;

        public Vector3d Min
        {
            get => _Min;
            set => SetPropertyValue(ref _Min, value);
        }

        public Vector3d Max
        {
            get => _Max;
            set => SetPropertyValue(ref _Max, value);
        }

        public Vector3d Size => Max - Min;

        public Vector3d Center => Min + (Size / 2d);

        public bool IsEmpty => Size == Vector3d.Zero;

        public double MinX
        {
            get => _Min.X;
            set => _Min.X = value;
        }

        public double MinY
        {
            get => _Min.Y;
            set => _Min.Y = value;
        }

        public double MinZ
        {
            get => _Min.Z;
            set => _Min.Z = value;
        }

        public double MaxX
        {
            get => _Max.X;
            set => _Max.X = value;
        }

        public double MaxY
        {
            get => _Max.Y;
            set => _Max.Y = value;
        }

        public double MaxZ
        {
            get => _Max.Z;
            set => _Max.Z = value;
        }

        public BoundingBox()
        {
        }

        public BoundingBox(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            _Min = new Vector3d(minX, minY, minZ);
            _Max = new Vector3d(maxX, maxY, maxZ);
        }

        public BoundingBox(Vector3 min, Vector3 max)
        {
            _Min = (Vector3d)min;
            _Max = (Vector3d)max;
        }

        public BoundingBox(Vector3d min, Vector3d max)
        {
            _Min = min;
            _Max = max;
        }

        public BoundingBox Clone()
        {
            return new BoundingBox(Min, Max);
        }

        public static BoundingBox FromVertices(IEnumerable<Vertex> vertices)
        {
            //var minX = vertices.Select(x => x.Position.X).Min();
            //var minY = vertices.Select(x => x.Position.Y).Min();
            //var minZ = vertices.Select(x => x.Position.Z).Min();
            //var maxX = vertices.Select(x => x.Position.X).Max();
            //var maxY = vertices.Select(x => x.Position.Y).Max();
            //var maxZ = vertices.Select(x => x.Position.Z).Max();

            float minX = 99999999;
            float minY = 99999999;
            float minZ = 99999999;
            float maxX = -99999999;
            float maxY = -99999999;
            float maxZ = -99999999;

            foreach (var vert in vertices)
            {
                minX = vert.Position.X < minX ? vert.Position.X : minX;
                minY = vert.Position.Y < minY ? vert.Position.Y : minY;
                minZ = vert.Position.Z < minZ ? vert.Position.Z : minZ;
                maxX = vert.Position.X > maxX ? vert.Position.X : maxX;
                maxY = vert.Position.Y > maxY ? vert.Position.Y : maxY;
                maxZ = vert.Position.Z > maxZ ? vert.Position.Z : maxZ;
            }

            return new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
        }

        public static BoundingBox FromVertices(IEnumerable<Vector3> vertices)
        {
            //var minX = vertices.Select(x => x.X).Min();
            //var minY = vertices.Select(x => x.Y).Min();
            //var minZ = vertices.Select(x => x.Z).Min();
            //var maxX = vertices.Select(x => x.X).Max();
            //var maxY = vertices.Select(x => x.Y).Max();
            //var maxZ = vertices.Select(x => x.Z).Max();

            float minX = 99999999;
            float minY = 99999999;
            float minZ = 99999999;
            float maxX = -99999999;
            float maxY = -99999999;
            float maxZ = -99999999;

            foreach (var vert in vertices)
            {
                minX = vert.X < minX ? vert.X : minX;
                minY = vert.Y < minY ? vert.Y : minY;
                minZ = vert.Z < minZ ? vert.Z : minZ;
                maxX = vert.X > maxX ? vert.X : maxX;
                maxY = vert.Y > maxY ? vert.Y : maxY;
                maxZ = vert.Z > maxZ ? vert.Z : maxZ;
            }

            return new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
        }

        public override bool Equals(object obj)
        {
            return obj is BoundingBox box &&
                   Min.Equals(box.Min) &&
                   Max.Equals(box.Max);
        }

        //public static bool operator ==(BoundingBox left, BoundingBox right)
        //{
        //    if (left is null || right is null)
        //        return left is null && right is null;

        //    return left.Min == right.Min && left.Max == right.Max;
        //}

        //public static bool operator !=(BoundingBox left, BoundingBox right)
        //{
        //    return !(left == right);
        //}

        public override int GetHashCode()
        {
            var hashCode = 1537547080;
            hashCode = hashCode * -1521134295 + Min.GetHashCode();
            hashCode = hashCode * -1521134295 + Max.GetHashCode();
            return hashCode;
        }

        public XElement SerializeToXml()
        {
            return SerializeToXml("AABB");
        }

        public XElement SerializeToXml(string elementName)
        {
            var elem = new XElement(elementName);
            elem.AddNumberAttribute("MinX", Min.X);
            elem.AddNumberAttribute("MinY", Min.Y);
            elem.AddNumberAttribute("MinZ", Min.Z);

            elem.AddNumberAttribute("MaxX", Max.X);
            elem.AddNumberAttribute("MaxY", Max.Y);
            elem.AddNumberAttribute("MaxZ", Max.Z);
            return elem;
        }

        public void LoadFromXml(XElement element)
        {
            element.TryReadAttribute("MinX", out double minX);
            element.TryReadAttribute("MinY", out double minY);
            element.TryReadAttribute("MinZ", out double minZ);

            Min = new Vector3d(minX, minY, minZ);

            element.TryReadAttribute("MaxX", out double maxX);
            element.TryReadAttribute("MaxY", out double maxY);
            element.TryReadAttribute("MaxZ", out double maxZ);

            Max = new Vector3d(maxX, maxY, maxZ);
        }

        
    }
}
