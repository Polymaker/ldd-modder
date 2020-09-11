using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives.Connectors;

namespace LDDModder.Modding.Editing
{
    public abstract class PartCullingModel : SurfaceComponent
    {
        //[System.Obsolete]
        //public string ConnectionID { get; set; }

        public string LegacyConnectionID { get; private set; }

        public ElementCollection<StudReference> ReferencedStuds { get; private set; }

        //[System.Obsolete]
        //internal int ConnectionIndex { get; set; } = -1;

        public PartCullingModel()
        {
            ReferencedStuds = new ElementCollection<StudReference>(this);
        }

        internal override void LoadCullingInformation(MeshCulling culling)
        {
            ReferencedStuds.Clear();
            ReferencedStuds.AddRange(ConvertFromRefs(culling.Studs));
            //var connectorRef = culling.Studs.FirstOrDefault() ?? culling.AdjacentStuds.FirstOrDefault();
            //ConnectionIndex = connectorRef != null ? connectorRef.ConnectorIndex : -1;
        }


        internal override void FillCullingInformation(MeshCulling culling)
        {
            base.FillCullingInformation(culling);
            culling.Studs.AddRange(ReferencedStuds.Select(x => ConvertToRef(x)));
        }
        //[System.Obsolete]
        //public PartConnection GetLinkedConnection()
        //{
        //    if (Project != null)
        //        return Project.Connections.FirstOrDefault(x => x.ID == ConnectionID);
        //    return null;
        //}
        //[System.Obsolete]
        //public Custom2DFieldConnector GetCustom2DField()
        //{
        //    return GetLinkedConnection()?.GetConnector<Custom2DFieldConnector>();
        //}

        public virtual IEnumerable<StudReference> GetStudReferences()
        {
            return OwnedElements.OfType<StudReference>()
                .Concat(Collections.SelectMany(x => x.GetElements()).OfType<StudReference>()).Distinct();
        }

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();
            //if (!string.IsNullOrEmpty(ConnectionID))
            //    elem.Add(new XAttribute(nameof(ConnectionID), ConnectionID));


            var studsElem = elem.AddElement(nameof(ReferencedStuds));
            foreach (var stud in ReferencedStuds)
                studsElem.Add(stud.SerializeToXml());
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            LegacyConnectionID = element.ReadAttribute("ConnectionID", string.Empty);

            ReferencedStuds.Clear();

            if (element.HasElement(nameof(ReferencedStuds), out XElement studsElem))
            {
                foreach (var studElem in studsElem.Elements(StudReference.NODE_NAME))
                {
                    var studRef = StudReference.FromXml(studElem);
                    if (!string.IsNullOrEmpty(LegacyConnectionID))
                        studRef.ConnectionID = LegacyConnectionID;
                    ReferencedStuds.Add(studRef);
                }
            }


        }

        protected Custom2DFieldReference ConvertToRef(StudReference studReference)
        {
            var fieldRef = new Custom2DFieldReference(studReference.ConnectionIndex);
            fieldRef.FieldIndices.Add(new Custom2DFieldIndex()
            {
                Index = studReference.FieldIndex,
                Value2 = studReference.Value1,
                Value4 = studReference.Value2
            });
            return fieldRef;
        }

        protected Custom2DFieldReference ConvertToRef(IEnumerable<StudReference> studReferences)
        {
            var fieldRef = new Custom2DFieldReference(studReferences.FirstOrDefault().ConnectionIndex);
            fieldRef.FieldIndices.AddRange(studReferences.Select(x => new Custom2DFieldIndex()
            {
                Index = x.FieldIndex,
                Value2 = x.Value1,
                Value4 = x.Value2
            }));
            return fieldRef;
        }

        protected static IEnumerable<StudReference> ConvertFromRefs(IEnumerable<Custom2DFieldReference> connectionReferences)
        {
            foreach (var connRef in connectionReferences)
            {
                foreach (var fieldRef in connRef.FieldIndices)
                    yield return new StudReference(connRef.ConnectorIndex, fieldRef.Index, fieldRef.Value2, fieldRef.Value4);
            }
        }

        public override List<ValidationMessage> ValidateElement()
        {
            var messages = base.ValidateElement();

            void AddMessage(string code, ValidationLevel level, params object[] args)
            {
                messages.Add(new ValidationMessage(this, code, level)
                {
                    MessageArguments = args
                });
            }

            //if (GetStudReferences().Any() && GetCustom2DField() == null)
            //    AddMessage("STUD_CONNECTION_NOT_DEFINED", ValidationLevel.Error);

            return messages;
        }
    }
}
