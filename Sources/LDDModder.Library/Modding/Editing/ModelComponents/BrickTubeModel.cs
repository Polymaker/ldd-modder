using LDDModder.LDD.Meshes;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Diagnostics;

namespace LDDModder.Modding.Editing
{
    public class BrickTubeModel : PartCullingModel
    {
        public override ModelComponentType ComponentType => ModelComponentType.BrickTube;

        private StudReference _TubeStud;
        
        public StudReference TubeStud
        {
            get => _TubeStud;
            set => SetPropertyValue(ref _TubeStud, value);
        }

        public ElementCollection<StudReference> AdjacentStuds { get; set; }

        public BrickTubeModel()
        {
            AdjacentStuds = new ElementCollection<StudReference>(this);
        }

        internal override void LoadCullingInformation(MeshCulling culling)
        {
            base.LoadCullingInformation(culling);

            TubeStud = ReferencedStuds.FirstOrDefault();

            //var referencedStuds = ConvertFromRefs(culling.Studs).ToList();
            //TubeStud = referencedStuds.FirstOrDefault();

            //if (!referencedStuds.Any())
            //    Debug.WriteLine("Tube culling does not reference a stud!");
            //else if (referencedStuds.Count > 1)
            //    Debug.WriteLine("Tube culling references more than one stud!");

            var adjacentRefs = ConvertFromRefs(culling.AdjacentStuds).ToList();
            AdjacentStuds.AddRange(adjacentRefs);

            if (!adjacentRefs.Any())
                Debug.WriteLine("Tube culling does not reference any adjacent studs!");
        }

        internal override void FillCullingInformation(MeshCulling culling)
        {
            base.FillCullingInformation(culling);
            //if (TubeStud != null)
            //    culling.Studs.Add(ConvertToRef(TubeStud));

            foreach (var connGroup in AdjacentStuds.GroupBy(x => x.ConnectionID))
                culling.AdjacentStuds.Add(ConvertToRef(connGroup));
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();

            
            //if (TubeStud != null)
            //    elem.Add(TubeStud.SerializeToXml2());

            if (AdjacentStuds.Any())
            {
                elem.Add(new XComment("The following 4 studs are adjacent to the tube"));
                var studsElem = elem.AddElement(nameof(AdjacentStuds));
                foreach (var stud in AdjacentStuds)
                    studsElem.Add(stud.SerializeToXml());
            }
            
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            //LEGACY
            if (element.HasElement(StudReference.NODE_NAME, out XElement tubeStudElem))
            {
                var studRef = StudReference.FromXml(tubeStudElem);
                if (!string.IsNullOrEmpty(LegacyConnectionID))
                    studRef.ConnectionID = LegacyConnectionID;
                ReferencedStuds.Add(studRef);
                //TubeStud = StudReference.FromXml(tubeStudElem);
                //if (!string.IsNullOrEmpty(LegacyConnectionID))
                //    TubeStud.ConnectionID = LegacyConnectionID;
            }

            TubeStud = ReferencedStuds.FirstOrDefault();

            AdjacentStuds.Clear();
            if (element.HasElement(nameof(AdjacentStuds), out XElement adjStudsElem))
            {
                foreach (var adjStudElem in adjStudsElem.Elements(StudReference.NODE_NAME))
                {
                    var studRef = StudReference.FromXml(adjStudElem);
                    if (!string.IsNullOrEmpty(LegacyConnectionID))
                        studRef.ConnectionID = LegacyConnectionID;
                    AdjacentStuds.Add(studRef);
                }
            }
        }

        #endregion

        public void AutoGenerateAdjacentStuds()
        {
            var tubeStud = ReferencedStuds.FirstOrDefault();
            if (tubeStud != null && tubeStud.FieldNode != null)
            {
                AdjacentStuds.Clear();
                var custom2DField = tubeStud.Connector;

                int posX = tubeStud.FieldNode.X;
                int posY = tubeStud.FieldNode.Y;
                int[] offsets = new int[] { -1, 1 };

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        var adjField = custom2DField.GetNode(
                            posX + offsets[j], 
                            posY + offsets[i]);

                        if (adjField != null)
                        {
                            var studRef = new StudReference(tubeStud.ConnectionID, adjField.Index, 1, 0);
                            AdjacentStuds.Add(studRef);
                        }
                    }
                }
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

            if (!ReferencedStuds.Any())
                AddMessage("MODEL_STUDS_NOT_DEFINED", ValidationLevel.Warning);

            if (ReferencedStuds.Count > 1)
                AddMessage("MODEL_MORE_THAN_ONE_STUD", ValidationLevel.Warning);
            //else if (!AdjacentStuds.Any())
            //    AddMessage("MODEL_ADJ_STUDS_NOT_DEFINED", ValidationLevel.Warning);//TODO: implement message

            var connIDs = ReferencedStuds.Concat(AdjacentStuds).Select(x => x.ConnectionID).Distinct();
            if (connIDs.Count() > 1)
                AddMessage("MODEL_MORE_THAN_ONE_STUD_CONNECTOR", ValidationLevel.Warning);

            if (ReferencedStuds.Any())
                messages.AddRange(ReferencedStuds.SelectMany(x => x.ValidateElement()));

            if (AdjacentStuds.Any())
                messages.AddRange(AdjacentStuds.SelectMany(x => x.ValidateElement()));

            return messages;
        }
    }
}
