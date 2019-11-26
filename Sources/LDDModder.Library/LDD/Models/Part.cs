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
        public string DesignID { get; set; }

        public string Decoration { get; set; }

        public List<int> Materials { get; set; }

        //public List<Bone> Bones { get; set; }

        public Bone Bone { get; set; }

        public Part()
        {
            //Bones = new List<Bone>();
            Materials = new List<int>();
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            DesignID = element.ReadAttribute("designID", string.Empty);

            Materials.Clear();

            if (element.HasAttribute("materials", out XAttribute matAttr))
            {
                var materials = matAttr.Value.Split(',');
                for (int i = 0; i < materials.Length; i++)
                {
                    if (int.TryParse(materials[i], out int matID))
                        Materials.Add(matID);
                }
            }

            Bone = null;
            if (element.HasElement("Bone", out XElement boneElem))
            {
                Bone = new Bone();
                Bone.LoadFromXml(boneElem);
            }
        }
    }
}
