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
            if (culling.Studs.Count == 1)
            {
                TubeStud = new StudReference(culling.Studs[0]);
            }
            else
                Debug.WriteLine("Tube culling does not reference a stud!");

            if (culling.AdjacentStuds.Any())
            {
                foreach (var fIdx in culling.AdjacentStuds[0].FieldIndices)
                {
                    var studRef = new StudReference(fIdx.Index, fIdx.Value2, fIdx.Value4);
                    AdjacentStuds.Add(studRef);
                }
            }
        }

        internal override void FillCullingInformation(MeshCulling culling)
        {
            culling.Studs.Add(GetFieldReference(TubeStud));
            culling.AdjacentStuds.Add(GetFieldReference(AdjacentStuds));
        }

        #region Xml Serialization

        public override XElement SerializeToXml()
        {
            var elem = base.SerializeToXml();

            if (TubeStud != null)
                elem.Add(TubeStud.SerializeToXml2());

            if (AdjacentStuds.Any())
            {
                elem.Add(new XComment("The following 4 studs are adjacent to the tube"));
                var studsElem = elem.AddElement(nameof(AdjacentStuds));
                foreach (var stud in AdjacentStuds)
                    studsElem.Add(stud.SerializeToXml2());
            }
            
            return elem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            if (element.HasElement(StudReference.NODE_NAME, out XElement tubeStudElem))
                TubeStud = StudReference.FromXml(tubeStudElem);

            if (element.HasElement(nameof(AdjacentStuds), out XElement adjStudsElem))
            {
                foreach (var adjStudElem in adjStudsElem.Elements(StudReference.NODE_NAME))
                    AdjacentStuds.Add(StudReference.FromXml(adjStudElem));
            }
        }

        #endregion

        public void AutoGenerateAdjacentStuds()
        {
            if (TubeStud != null && TubeStud.FieldNode != null)
            {
                AdjacentStuds.Clear();
                var custom2DField = TubeStud.Connector;

                int posX = TubeStud.FieldNode.X;
                int posY = TubeStud.FieldNode.Y;
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
                            var studRef = new StudReference(adjField.Index, 1, 0);
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

            if (!AdjacentStuds.Any() || TubeStud == null)
                AddMessage("MODEL_STUDS_NOT_DEFINED", ValidationLevel.Warning);

            if (TubeStud != null)
                messages.AddRange(TubeStud.ValidateElement());

            if (AdjacentStuds.Any())
                messages.AddRange(AdjacentStuds.SelectMany(x => x.ValidateElement()));

            return messages;
        }
    }
}
