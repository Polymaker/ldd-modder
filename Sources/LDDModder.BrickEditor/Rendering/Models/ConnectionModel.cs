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

        private bool ChangingTransform;

        public ConnectionModel(PartConnection connection) : base (connection)
        {
            Connection = connection;
            var baseTransform = connection.Transform.ToMatrix().ToGL();
            SetTransform(baseTransform, false);
            

            UpdateRenderingModel();
        }

        protected override void OnTransformChanged()
        {
            base.OnTransformChanged();
            Matrix4 transCopy = Transform;
            transCopy.ClearScale();

            ChangingTransform = true;
            Connection.Transform = ItemTransform.FromMatrix(transCopy.ToLDD());
            ChangingTransform = false;
        }

        protected override void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == nameof(PartConnection.Transform) && !ChangingTransform)
            {
                var baseTransform = Connection.Transform.ToMatrix().ToGL();
                SetTransform(baseTransform, true);
            }
            else
            {
                UpdateRenderingModel();
            }
        }

        private void UpdateRenderingModel()
        {
            RenderingModel = null;
            ModelTransform = Matrix4.Identity;
            BoundingBox = BBox.FromCenterSize(Vector3.Zero, new Vector3(0.5f));

            if (Connection.Connector is AxelConnector axelConnector)
            {
                if (axelConnector.Length > 0)
                {
                    int renderType = axelConnector.SubType;
                    if (DisplayInvertedGender)
                        renderType += (renderType % 2 == 0) ? 1 : -1;

                    if (renderType == 3)
                    {
                        RenderingModel = ModelManager.CylinderModel;
                        ModelTransform = Matrix4.CreateScale(0.48f, axelConnector.Length, 0.48f);
                    }
                    else if (renderType == 4)
                    {
                        RenderingModel = ModelManager.CrossAxleFemaleModel;
                        ModelTransform = Matrix4.CreateScale(1f, axelConnector.Length, 1f);
                    }
                    else if (renderType == 5)
                    {
                        RenderingModel = ModelManager.CrossAxleMaleModel;
                        ModelTransform = Matrix4.CreateScale(1f, axelConnector.Length, 1f);
                    }
                    else if (renderType == 7)
                    {
                        RenderingModel = ModelManager.CylinderModel;
                        ModelTransform = Matrix4.CreateScale(0.32f, axelConnector.Length, 0.32f);
                    }
                    else if (renderType == 15)
                    {
                        RenderingModel = ModelManager.CylinderModel;
                        ModelTransform = Matrix4.CreateScale(0.15f, axelConnector.Length, 0.15f);
                    }
                }
            }

            if (RenderingModel != null)
            {
                var vertices = RenderingModel.BoundingBox.GetCorners();
                vertices = vertices.Select(x => Vector3.TransformPosition(x, ModelTransform)).ToArray();
                BoundingBox = BBox.FromVertices(vertices);
            }
        }

        public override void RenderModel(Camera camera)
        {
            base.RenderModel(camera);

            switch (Connection.ConnectorType)
            {
                case LDD.Primitives.Connectors.ConnectorType.Axel:
                    RenderTechnicAxle(Connection.GetConnector<AxelConnector>());
                    break;
                case LDD.Primitives.Connectors.ConnectorType.Ball:
                    break;
                case LDD.Primitives.Connectors.ConnectorType.Custom2DField:
                    RenderCustom2DField(Connection.GetConnector<Custom2DFieldConnector>());
                    break;
                case LDD.Primitives.Connectors.ConnectorType.Fixed:
                    break;
                case LDD.Primitives.Connectors.ConnectorType.Gear:
                    break;
                case LDD.Primitives.Connectors.ConnectorType.Hinge:
                    break;
                case LDD.Primitives.Connectors.ConnectorType.Rail:
                    break;
                case LDD.Primitives.Connectors.ConnectorType.Slider:
                    break;
            }

            if (RenderingModel != null)
            {
                RenderHelper.BeginDrawModel(RenderingModel, 
                    ModelTransform * Transform, RenderHelper.ConnectionMaterial);
                RenderHelper.ModelShader.IsSelected.Set(IsSelected);
                RenderingModel.DrawElements();
                RenderHelper.EndDrawModel(RenderingModel);
            }
            else
            {
                RenderHelper.DrawGizmoAxes(Transform, 0.5f, 2f);
            }
        }

        private void RenderCustom2DField(Custom2DFieldConnector connector)
        {
            RenderHelper.DrawRectangle(Transform, 
                new Vector2(connector.StudWidth * 0.8f, connector.StudHeight * 0.8f), 
                new Vector4(0, 0, 0, 1), 3f);
        }

        private void RenderTechnicAxle(AxelConnector axel)
        {
            if (RenderingModel == null && axel.Length > 0)
            {
                RenderHelper.DrawLine(Transform, RenderHelper.DefaultAxisColors[1],
                    Vector3.Zero, Vector3.UnitY * axel.Length, 2f);
            }
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            if (RenderingModel != null)
                return RayIntersectsBoundingBox(ray, out distance);

            var localRay = Ray.Transform(ray, Transform.Inverted());
            var bsphere = new BSphere(Vector3.Zero, 0.5f);
            return Ray.IntersectsSphere(localRay, bsphere, out distance);
        }
    }
}
