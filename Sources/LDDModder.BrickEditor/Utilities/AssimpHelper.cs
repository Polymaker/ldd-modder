using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimp
{
    public static class AssimpHelper
    {
        public static Node GetMeshNode(this Scene scene, Mesh mesh)
        {
            int index = scene.Meshes.IndexOf(mesh);
            if (index < 0)
                return null;
            return GetMeshNode(scene, index);
        }

        public static Node GetMeshNode(this Scene scene, int meshIndex)
        {
            Node FindNode(Node parent)
            {
                foreach (var n in parent.Children)
                {
                    if (n.MeshIndices.Contains(meshIndex))
                        return n;
                }

                foreach (var n in parent.Children)
                {
                    var res = FindNode(n);
                    if (res != null)
                        return res;
                }
                return null;
            }

            return FindNode(scene.RootNode);
        }

        public static IEnumerable<Node> GetNodeHierarchy(this Scene scene)
        {
            return GetNodeHierarchy(scene.RootNode, true);
        }

        public static IEnumerable<Node> GetNodeHierarchy(this Node node)
        {
            foreach (var child in node.Children)
                yield return child;
        }

        public static IEnumerable<Node> GetNodeHierarchy(this Node node, bool includeSelf)
        {
            if (includeSelf)
                yield return node;

            foreach (var child in node.Children)
            {
                foreach (var subChild in GetNodeHierarchy(child, true))
                    yield return subChild;
            }
        }

        public static Matrix4x4 GetFinalTransform(this Node node)
        {
            var parents = new List<Node>();
            var curNode = node;

            while(curNode != null)
            {
                parents.Add(curNode);
                curNode = curNode.Parent;
            }

            parents.Reverse();
            var trans = Matrix4x4.Identity;
            foreach (var n in parents)
                trans = n.Transform * trans;

            return trans;
        }

        public static Matrix4x4 GetGlobalTransform(this Node node)
        {
            var parentTrans = node.Parent != null ? node.Parent.Transform : Matrix4x4.Identity;
            var invTrans = node.Transform;
            invTrans.Inverse();
            return parentTrans * invTrans;
        }


    }
}
