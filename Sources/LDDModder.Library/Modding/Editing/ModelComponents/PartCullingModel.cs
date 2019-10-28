using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    public abstract class PartCullingModel : SurfaceComponent
    {
        [XmlAttribute]
        public string ConnectionID { get; set; }

        [XmlIgnore]
        internal int ConnectionIndex { get; set; } = -1;

        [XmlIgnore]
        protected ComponentCollection<StudReference> StudReferences { get; set; }

        public PartCullingModel()
        {
            StudReferences = new ComponentCollection<StudReference>(this);
        }

        public PartConnection GetLinkedConnection()
        {
            if (Project != null)
                return Project.Connections.FirstOrDefault(x => x.RefID == ConnectionID);
            return null;
        }

        public virtual IEnumerable<StudReference> GetStudReferences()
        {
            return StudReferences;
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            if (!string.IsNullOrEmpty(ConnectionID))
                elem.Add(new XAttribute("ConnectionID", ConnectionID));
            return elem;
        }
    }
}
