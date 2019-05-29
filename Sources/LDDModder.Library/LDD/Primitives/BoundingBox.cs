using LDDModder.LDD.Meshes;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    public class BoundingBox
    {
        [XmlAttribute("minX")]
        public float MinX { get; set; }
        [XmlAttribute("minY")]
        public float MinY { get; set; }
        [XmlAttribute("minZ")]
        public float MinZ { get; set; }
        [XmlAttribute("maxX")]
        public float MaxX { get; set; }
        [XmlAttribute("maxY")]
        public float MaxY { get; set; }
        [XmlAttribute("maxZ")]
        public float MaxZ { get; set; }

        [XmlIgnore]
        public float SizeX => MaxX - MinX;
        [XmlIgnore]
        public float SizeY => MaxY - MinY;
        [XmlIgnore]
        public float SizeZ => MaxZ - MinZ;

        [XmlIgnore]
        public Tuple<float, float, float> Center
        {
            get
            {
                return new Tuple<float, float, float>((SizeX / 2f) + MinX, (SizeY / 2f) + MinY, (SizeZ / 2f) + MinZ);
            }
        }

        public BoundingBox()
        {
        }

        public BoundingBox(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
        }

        public static BoundingBox FromVertices(IEnumerable<Vertex> vertices)
        {
            var minX = vertices.Select(x => x.Position.X).Min();
            var minY = vertices.Select(x => x.Position.Y).Min();
            var minZ = vertices.Select(x => x.Position.Z).Min();
            var maxX = vertices.Select(x => x.Position.X).Max();
            var maxY = vertices.Select(x => x.Position.Y).Max();
            var maxZ = vertices.Select(x => x.Position.Z).Max();

            return new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
        }

        public static BoundingBox FromVertices(IEnumerable<Vector3> vertices)
        {
            var minX = vertices.Select(x => x.X).Min();
            var minY = vertices.Select(x => x.Y).Min();
            var minZ = vertices.Select(x => x.Z).Min();
            var maxX = vertices.Select(x => x.X).Max();
            var maxY = vertices.Select(x => x.Y).Max();
            var maxZ = vertices.Select(x => x.Z).Max();

            return new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
        }
    }
}
