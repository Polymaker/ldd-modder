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

        public string Description { get; set; }

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

        public virtual PartProject Project => Root?.Project;

        public PartNodeCollection Nodes { get; }

        public int Index => Parent != null ? Parent.Nodes.IndexOf(this) : -1;

        public PartNode()
        {
            Nodes = new PartNodeCollection(this);
        }

        public PartNode(string id)
        {
            ID = id;
            Nodes = new PartNodeCollection(this);
        }

        public virtual string GetDisplayName()
        {
            return Description;
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
            var elem = new XElement(GetType().Name, new XAttribute("ID", ID));
            if (!string.IsNullOrEmpty(Description))
                elem.Add(new XAttribute("Description", Description));
            return elem;
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
