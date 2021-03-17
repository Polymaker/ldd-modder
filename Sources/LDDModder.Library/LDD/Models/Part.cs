using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public class Part : ModelItem
    {
        public int DesignID { get; set; }

        public List<int> Decorations { get; set; }
        public List<int> Materials { get; set; }

        //public List<Bone> Bones { get; set; }

        public Bone Bone { get; set; }

        public Part()
        {
            //Bones = new List<Bone>();
            Materials = new List<int>();
            Decorations = new List<int>();
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            DesignID = element.ReadAttribute("designID", 0);
            Materials.Clear();
            Decorations.Clear();

            if (element.HasAttribute("materials", out XAttribute matAttr))
            {
                var materials = matAttr.Value.Split(',');
                for (int i = 0; i < materials.Length; i++)
                {
                    if (int.TryParse(materials[i], out int matID))
                        Materials.Add(matID);
                }
            }

            if (element.HasAttribute("decoration", out XAttribute decAttr))
            {
                var decorations = decAttr.Value.Split(',');
                for (int i = 0; i < decorations.Length; i++)
                {
                    if (int.TryParse(decorations[i], out int decID))
                        Decorations.Add(decID);
                }
            }

            Bone = null;
            if (element.HasElement("Bone", out XElement boneElem))
            {
                Bone = new Bone();
                Bone.LoadFromXml(boneElem);
            }
        }

        protected override void SerializeElement(XElement element)
        {
            base.SerializeElement(element);
            element.WriteAttribute("designID", DesignID);
            if (Materials.Count > 0)
                element.WriteAttribute("materials", string.Join(",", Materials));
            if (Decorations.Count > 0)
                element.WriteAttribute("decoration", string.Join(",", Decorations));
            if (Bone != null)
                element.Add(Bone.SerializeToXml());
        }
    }
}
