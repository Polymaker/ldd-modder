using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.Editing
{
    public class PartNode
    {
        public string ID { get; set; }

        public PartNode Parent { get; set; }

        public RootNode Root
        {
            get
            {
                if (Parent == null)
                    return null;
                if (Parent is RootNode root)
                    return root;
                return Parent.Root;
            }
        }

        public PartProject Project => Root?.Project;

        public PartNodeCollection Nodes { get; }

        public PartNode()
        {
            Nodes = new PartNodeCollection(this);
        }

        public PartNode(string iD)
        {
            ID = iD;
            Nodes = new PartNodeCollection(this);
        }

        public virtual string GetName()
        {
            return ID;
        }

        public void GenerateID()
        {
            ID = Guid.NewGuid().ToString("N").Substring(0, 10);
        }

        public void Add(PartNode node)
        {
            Nodes.Add(node);
        }

        public static T Create<T>() where T : PartNode
        {
            PartNode node = Activator.CreateInstance<T>();
            node.GenerateID();
            return (T)node;
        }

        public virtual XElement SerializeToXml()
        {
            return new XElement("Node", new XAttribute("ID", ID));
        }

        public virtual XElement SerializeHierarchy()
        {
            var root = SerializeToXml();
            foreach(var child in Nodes)
                root.Add(child.SerializeHierarchy());
            return root;
        }
    }
}
