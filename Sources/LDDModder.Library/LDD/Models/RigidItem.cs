using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public class RigidItem : TransformableModelItem
    {
        public override string NodeName => "Rigid";

        public List<int> BoneRefs { get; set; }

        public RigidItem()
        {
            BoneRefs = new List<int>();
        }

        protected override void SerializeElement(XElement element)
        {
            base.SerializeElement(element);
            if (BoneRefs.Count > 0)
                element.WriteAttribute("boneRefs", string.Join(",", BoneRefs));
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            BoneRefs.Clear();

            if (element.HasAttribute("boneRefs", out XAttribute boneRefsAttr))
            {
                var boneRefs = boneRefsAttr.Value.Split(',');
                for (int i = 0; i < boneRefs.Length; i++)
                {
                    if (int.TryParse(boneRefs[i], out int boneID))
                        BoneRefs.Add(boneID);
                }
            }
        }
    }
}
