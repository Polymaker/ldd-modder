using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class Vertex/* : IEqualityComparer<Vertex>*/
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 TexCoord { get; set; }

        public List<BoneWeight> BoneWeights { get; }

        public Vertex()
        {
            BoneWeights = new List<BoneWeight>();
        }

        public Vertex(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
            TexCoord = Vector2.Empty;
            BoneWeights = new List<BoneWeight>();
        }

        public Vertex(float px, float py, float pz, float nx, float ny, float nz)
        {
            Position = new Vector3(px, py, pz);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = Vector2.Empty;
            BoneWeights = new List<BoneWeight>();
        }

        public Vertex(Vector3 position, Vector3 normal, Vector2 texCoord) : this(position, normal)
        {
            TexCoord = texCoord;
            BoneWeights = new List<BoneWeight>();
        }

        //public override bool Equals(object obj)
        //{
        //    return obj is Vertex vertex &&
        //           Position.Equals(vertex.Position) &&
        //           Normal.Equals(vertex.Normal) &&
        //           TexCoord.Equals(vertex.TexCoord);
        //}

        public bool Equals(Vertex vertex)
        {
            return Position.Equals(vertex.Position) &&
                   Normal.Equals(vertex.Normal) &&
                   TexCoord.Equals(vertex.TexCoord);
        }

        public bool Equals(Vertex vertex, bool checkBones)
        {
            if (checkBones)
            {
                if (BoneWeights.Count != vertex.BoneWeights.Count)
                    return false;

                if (!BoneWeights.SequenceEqual(vertex.BoneWeights))
                    return false;
            }

            return Position.Equals(vertex.Position) &&
                   Normal.Equals(vertex.Normal) &&
                   TexCoord.Equals(vertex.TexCoord);
        }

        public int GetHash()
        {
            var hashCode = 550714527;
            hashCode = hashCode * -1521134295 + Position.Rounded(4).GetHashCode();
            hashCode = hashCode * -1521134295 + Normal.Rounded(4).GetHashCode();
            hashCode = hashCode * -1521134295 + TexCoord.Rounded(4).GetHashCode();
            return hashCode;
        }

        public Vertex Clone()
        {
            var v = new Vertex(Position, Normal, TexCoord);
            v.BoneWeights.AddRange(BoneWeights);
            return v;
        }

        public override string ToString()
        {
            return $"{Position.Rounded()}";
        }

        //public bool Equals(Vertex x, Vertex y)
        //{
        //    return x.Equals(y);
        //}

        //public int GetHashCode(Vertex obj)
        //{
        //    return obj.GetHash();
        //}
    }
}
