using LDDModder.Modding;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Models
{
    public class ClonePatternModel : ModelBase
    {
        public ClonePattern ClonePattern { get; set; }
        public List<PartElementModel> ElementModels { get; set; }

        public bool ElementModelsDirty { get; private set; }

        public ClonePatternModel(ClonePattern clonePattern)
        {
            ClonePattern = clonePattern;
            ClonePattern.PropertyChanged += ClonePattern_PropertyChanged;
            ElementModels = new List<PartElementModel>();
            ClonePattern.Elements.CollectionChanged += Elements_CollectionChanged;
            ElementModelsDirty = true;
            InitTranform();
        }

        private void ClonePattern_PropertyChanged(object sender, ElementValueChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LinearPattern.Direction) ||
                e.PropertyName == nameof(CircularPattern.Axis) ||
                e.PropertyName == nameof(CircularPattern.Origin))
            {
                InitTranform();
            }
        }

        private void InitTranform()
        {
            if (ClonePattern is CircularPattern circularPattern)
            {
                Transform = Matrix4.LookAt(Vector3.Zero, circularPattern.Axis.ToGL(), Vector3.UnitY);
                Transform = Transform * Matrix4.CreateTranslation(circularPattern.Origin.ToGL());
            }
            else if (ClonePattern is LinearPattern linearPattern)
            {
                var dir = linearPattern.Direction.ToGL();
                var upVec = Vector3.UnitZ;
                var angleDiff = Vector3.CalculateAngle(dir, upVec);
                
                if (float.IsNaN(angleDiff) || Math.Abs(angleDiff) <= 0.001f)
                {
                    upVec = Vector3.UnitY;
                }

                var omega = (float)Math.Acos(Vector3.Dot(upVec, dir));
                
                Transform = Matrix4.CreateFromAxisAngle(Vector3.Cross(upVec, dir), omega);

                if (ElementModels.Any())
                {
                    Transform *= Matrix4.CreateTranslation(ElementModels[0].Origin);
                }
            }
        }

        private void Elements_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ElementModelsDirty = true;
        }

        public void SetElementModels(IEnumerable<PartElementModel> models)
        {
            ElementModels = models.ToList();
            ElementModelsDirty = false;
            InitTranform();
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            distance = float.NaN;
            return false;
        }

        public override void RenderModel(Camera camera, MeshRenderMode mode = MeshRenderMode.Solid)
        {

            if (ClonePattern?.Repetitions > 1)
            {
                if (ClonePattern is LinearPattern linearPattern)
                {
                    float lineLength = (linearPattern.Repetitions - 1) * linearPattern.Offset;
                    //var dir = linearPattern.Direction.ToGL();
                    RenderHelper.DrawLine(Transform, new Vector4(1f, 1f, 0f, 1f), Vector3.Zero, Vector3.UnitZ * lineLength, 1.5f);
                }

                if (ElementModels == null || ElementModels.Count == 0)
                    return;

                foreach (var elem in ClonePattern.Elements)
                {
                    var elemModel = ElementModels.FirstOrDefault(x => x.Element == elem);
                    if (elemModel != null && elemModel.Visible)
                    {
                        var originalTrans = elemModel.Transform;
                        var baseTransform = ItemTransform.FromMatrix(elemModel.Transform.ToLDD());
                        bool isSelected = elemModel.IsSelected;

                        for (int i = 1; i < ClonePattern.Repetitions; i++)
                        {
                            if (ClonePattern.SkippedInstances.Contains(i))
                                continue;

                            var trans = ClonePattern.ApplyTransform(baseTransform, i).ToMatrix().ToGL();
                            //elemModel.SetTransform(trans, false);
                            elemModel.IsSelected = false;
                            elemModel.SetTemporaryTransform(trans);
                            elemModel.RenderModel(camera, mode);
                            elemModel.SetTemporaryTransform(null);
                        }
                        elemModel.IsSelected = isSelected;
                        //elemModel.SetTransform(originalTrans, false);
                    }
                }
                
            }
        }
    }
}
