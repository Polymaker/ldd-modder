using LDDModder.BrickEditor.Rendering.Models;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Modding.Editing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class ConnectionModel : PartElementModel
    {
        public PartConnection Connection { get; set; }

        public PartialModel RenderingModel { get; set; }

        public Matrix4 ModelTransform { get; set; }

        public Matrix4 ParentTransform { get; set; }

        private MaterialInfo ModelMaterial;

        private bool _DisplayInvertedGender;

        public bool DisplayInvertedGender
        {
            get => _DisplayInvertedGender;
            set
            {
                if (value != _DisplayInvertedGender)
                {
                    _DisplayInvertedGender = value;
                    UpdateRenderingModel();
                }
            }
        }

        public ConnectionModel(PartConnection connection) : base (connection)
        {
            Connection = connection;

            SetTransformFromElement();

            UpdateRenderingModel();
        }


        protected override Matrix4 GetElementTransform()
        {
            var baseTransform = Connection.Transform.ToMatrixD().ToGL();

            if (Connection.Parent is PartBone partBone)
            {
                ParentTransform = partBone.Transform.ToMatrix().ToGL();
                baseTransform = baseTransform * partBone.Transform.ToMatrixD().ToGL();
            }

            return baseTransform.ToMatrix4();
        }

        protected override void ApplyTransformToElement(Matrix4 transform)
        {
            if (Connection.Parent is PartBone partBone)
            {
                var parentTrans = partBone.Transform.ToMatrixD().ToGL();
                var localTrans = transform.ToMatrix4d() * parentTrans.Inverted();
                //transform = localTrans.ToMatrix4();
                Connection.Transform = ItemTransform.FromMatrix(localTrans.ToLDD());
            }
            else
                base.ApplyTransformToElement(transform);
        }

        protected override void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            switch (e.PropertyName)
            {
                case nameof(PartElement.ID):
                case nameof(PartElement.Name):
                case nameof(PartConnection.Transform):
                case nameof(PartConnection.Comments):
                    break;

                default:
                    UpdateRenderingModel();
                    break;
            }
        }

        private void UpdateRenderingModel()
        {
            RenderingModel = null;
            ModelTransform = Matrix4.Identity;
            BoundingBox = BBox.Empty;
            ModelMaterial = RenderHelper.ConnectionMaterial;

            if (Connection.SubType < 1000)
            {
                if (Connection.SubType % 2 == 1)
                    ModelMaterial = RenderHelper.MaleConnectorMaterial;
                else
                    ModelMaterial = RenderHelper.FemaleConnectorMaterial;
            }
            

            if (Connection.Connector is AxelConnector axelConnector)
            {
                CreateAxleModel(axelConnector);
            }
            else if (Connection.Connector is Custom2DFieldConnector custom2DField)
            {
                var studsSize = new Vector3(custom2DField.StudWidth * 0.8f, 0.2f, custom2DField.StudHeight * 0.8f);
                BoundingBox = BBox.FromCenterSize(studsSize * new Vector3(0.5f,0,0.5f), studsSize);
            }

            if (RenderingModel != null)
            {
                var vertices = RenderingModel.BoundingBox.GetCorners();
                vertices = vertices.Select(x => Vector3.TransformPosition(x, ModelTransform)).ToArray();
                BoundingBox = BBox.FromVertices(vertices);
            }
        }

        private void CreateAxleModel(AxelConnector axelConnector)
        {
            if (axelConnector.Length > 0)
            {
                int renderType = axelConnector.SubType;
                if (DisplayInvertedGender && renderType < 1000)
                    renderType += (renderType % 2 == 0) ? 1 : -1;

                switch (renderType)
                {
                    case 2:
                        RenderingModel = ModelManager.TechnicPinFemaleModel;
                        ModelTransform = Matrix4.CreateScale(1f, axelConnector.Length, 1f);
                        break;

                    case 3:
                        RenderingModel = ModelManager.CylinderModel;
                        ModelTransform = Matrix4.CreateScale(0.48f, axelConnector.Length, 0.48f);
                        break;

                    case 4:
                        RenderingModel = ModelManager.CrossAxleFemaleModel;
                        ModelTransform = Matrix4.CreateScale(1f, axelConnector.Length, 1f);
                        break;

                    case 5:
                        RenderingModel = ModelManager.CrossAxleMaleModel;
                        ModelTransform = Matrix4.CreateScale(1f, axelConnector.Length, 1f);
                        break;

                    case 6:
                        RenderingModel = ModelManager.BarFemaleModel;
                        ModelTransform = Matrix4.CreateScale(1f, axelConnector.Length, 1f);
                        break;

                    case 7:
                        RenderingModel = ModelManager.CylinderModel;
                        ModelTransform = Matrix4.CreateScale(0.32f, axelConnector.Length, 0.32f);
                        break;

                    case 15:
                        RenderingModel = ModelManager.CylinderModel;
                        ModelTransform = Matrix4.CreateScale(0.15f, axelConnector.Length, 0.15f);
                        break;
                }
                
            }
        }

        public override void RenderModel(Camera camera)
        {
            base.RenderModel(camera);

            switch (Connection.ConnectorType)
            {
                case ConnectorType.Axel:
                    RenderTechnicAxle(Connection.GetConnector<AxelConnector>());
                    break;
                case ConnectorType.Ball:
                    break;
                case ConnectorType.Custom2DField:
                    RenderCustom2DField(Connection.GetConnector<Custom2DFieldConnector>());
                    break;
                case ConnectorType.Fixed:
                    break;
                case ConnectorType.Gear:
                    break;
                case ConnectorType.Hinge:
                    break;
                case ConnectorType.Rail:
                    break;
                case ConnectorType.Slider:
                    break;
            }

            if (RenderingModel != null)
            {
                var finalTransform = ModelTransform * Transform;

                RenderHelper.RenderWithStencil(
                    () =>
                    {
                        RenderHelper.BeginDrawModel(RenderingModel, finalTransform, ModelMaterial);

                        RenderHelper.ModelShader.IsSelected.Set(IsSelected);
                        RenderingModel.DrawElements();

                        RenderHelper.EndDrawModel(RenderingModel);
                    },
                    () =>
                    {
                        var wireColor = IsSelected ? RenderHelper.SelectionOutlineColor : RenderHelper.WireframeColor;
                        RenderHelper.BeginDrawWireframe(RenderingModel.VertexBuffer, finalTransform, 
                            IsSelected ? 4f : 2f, wireColor);

                        RenderingModel.DrawElements();

                        RenderHelper.EndDrawWireframe(RenderingModel.VertexBuffer);
                    });
            }
            else
            {
                RenderHelper.DrawGizmoAxes(Transform, 0.5f, IsSelected);
            }
        }

        private void RenderCustom2DField(Custom2DFieldConnector connector)
        {
            RenderHelper.DrawStudConnector2(Transform, connector);

            var color = IsSelected ? new Vector4(1f) : new Vector4(0, 0, 0, 1);
            RenderHelper.DrawRectangle(Transform, 
                new Vector2(connector.StudWidth * 0.8f, connector.StudHeight * 0.8f),
                color, 3f);
        }

        private void RenderTechnicAxle(AxelConnector axel)
        {
            if (RenderingModel == null && axel.Length > 0)
            {
                RenderHelper.DrawLine(Transform, RenderHelper.DefaultAxisColors[1],
                    Vector3.Zero, Vector3.UnitY * axel.Length, 2f);
            }
        }

        public override bool RayIntersectsBoundingBox(Ray ray, out float distance)
        {
            if (BoundingBox.IsEmpty)
            {
                var bsphere = new BSphere(Origin, 0.5f);
                return Ray.IntersectsSphere(ray, bsphere, out distance);
            }

            return base.RayIntersectsBoundingBox(ray, out distance);
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            distance = float.NaN;

            if (RenderingModel != null)
            {
                var modelRay = Ray.Transform(ray, Transform.Inverted());
                return RenderingModel.RayIntersects(modelRay, ModelTransform, out distance);
                //return RayIntersectsBoundingBox(ray, out distance);
            }

            var localRay = Ray.Transform(ray, Transform.Inverted());

            if (Connection.ConnectorType == ConnectorType.Custom2DField)
            {
                var studConnector = Connection.GetConnector<Custom2DFieldConnector>();
                if (Ray.IntersectsPlane(localRay, new Plane(Vector3.Zero, Vector3.UnitY, 0f), out distance))
                {
                    var hitPos = localRay.Origin + localRay.Direction * distance;
                    if (hitPos.X < 0 || hitPos.X > studConnector.StudWidth * 0.8f
                        || hitPos.Z < 0 || hitPos.Z > studConnector.StudHeight * 0.8f)
                    {
                        return false;
                    }
                    return true;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                var axe = new Vector3(0.08f);
                axe[i] = 0.5f;
                var center = Vector3.Zero;
                center[i] = axe[i] / 2f;
                var axeBox = BBox.FromCenterSize(center, axe);
                if (Ray.IntersectsBox(localRay, axeBox, out float hitDist))
                    distance = float.IsNaN(distance) ? hitDist : Math.Min(hitDist, distance);
            }

            return !float.IsNaN(distance);

            //var bsphere = new BSphere(Vector3.Zero, 0.5f);
            //return Ray.IntersectsSphere(localRay, bsphere, out distance);

        }


    }
}
