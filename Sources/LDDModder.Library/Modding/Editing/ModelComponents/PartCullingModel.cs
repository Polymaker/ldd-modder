using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using LDDModder.LDD.Meshes;

namespace LDDModder.Modding.Editing
{
    public abstract class PartCullingModel : SurfaceComponent
    {
        [XmlAttribute]
        public string ConnectionID { get; set; }

        [XmlIgnore]
        internal int ConnectionIndex { get; set; } = -1;

        public PartCullingModel()
        {
        }

        internal override void LoadCullingInformation(MeshCulling culling)
        {
            var connectorRef = culling.Studs.FirstOrDefault() ?? culling.AdjacentStuds.FirstOrDefault();
            ConnectionIndex = connectorRef != null ? connectorRef.ConnectorIndex : -1;
        }

        public PartConnection GetLinkedConnection()
        {
            if (Project != null)
                return Project.Connections.FirstOrDefault(x => x.ID == ConnectionID);
            return null;
        }

        public virtual IEnumerable<StudReference> GetStudReferences()
        {
            return OwnedElements.OfType<StudReference>()
                .Concat(Collections.SelectMany(x => x.GetElements()).OfType<StudReference>());
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            if (!string.IsNullOrEmpty(ConnectionID))
                elem.Add(new XAttribute(nameof(ConnectionID), ConnectionID));
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            ConnectionID = element.ReadAttribute(nameof(ConnectionID), string.Empty);
        }

        protected Custom2DFieldReference GetFieldReference(StudReference studReference)
        {
            var fieldRef = new Custom2DFieldReference(ConnectionIndex);
            fieldRef.FieldIndices.Add(new Custom2DFieldIndex()
            {
                Index = studReference.FieldIndex,
                Value2 = studReference.Value1,
                Value4 = studReference.Value2
            });
            return fieldRef;
        }

        protected Custom2DFieldReference GetFieldReference(IEnumerable<StudReference> studReferences)
        {
            var fieldRef = new Custom2DFieldReference(ConnectionIndex);
            fieldRef.FieldIndices.AddRange(studReferences.Select(x => new Custom2DFieldIndex()
            {
                Index = x.FieldIndex,
                Value2 = x.Value1,
                Value4 = x.Value2
            }));
            return fieldRef;
        }
    }
}
