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

        private bool ChangingTransform;

        public ConnectionModel(PartConnection connection) : base (connection)
        {
            Connection = connection;
            var baseTransform = connection.Transform.ToMatrix().ToGL();
            SetTransform(baseTransform, false);
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

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == nameof(PartConnection.Transform) && !ChangingTransform)
            {
                var baseTransform = Connection.Transform.ToMatrix().ToGL();
                SetTransform(baseTransform, true);
            }
        }

        public override void RenderModel(Camera camera)
        {
            base.RenderModel(camera);

            switch (Connection.ConnectorType)
            {
                case LDD.Primitives.Connectors.ConnectorType.Axel:
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
            //if (IsSelected)
            //{
            //    RenderHelper.EnableStencilTest();
            //    RenderHelper.EnableStencilMask();
            //    RenderHelper.DrawGizmoAxes(Transform, 0.5f, 2f);
            //    RenderHelper.ApplyStencilMask();
            //    RenderHelper.DrawGizmoAxes(Transform, 0.5f, new Vector4(1f), 3.5f);
            //    RenderHelper.RemoveStencilMask();
            //    RenderHelper.DisableStencilTest();
            //}
            //else
            //{
            //    RenderHelper.DrawGizmoAxes(Transform, 0.5f, 2f);
            //}
            RenderHelper.DrawGizmoAxes(Transform, 0.5f, 2f);
        }

        private void RenderCustom2DField(Custom2DFieldConnector connector)
        {
            RenderHelper.DrawRectangle(Transform, 
                new Vector2(connector.StudWidth * 0.8f, connector.StudHeight * 0.8f), 
                new Vector4(0, 0, 0, 1), 3f);
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            distance = float.NaN;
            return false;
        }
    }
}
