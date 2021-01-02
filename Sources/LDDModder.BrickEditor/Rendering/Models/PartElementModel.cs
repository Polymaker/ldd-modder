using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding;
using LDDModder.Modding;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public abstract class PartElementModel : ModelBase
    {
        public PartElement Element { get; set; }

        public ModelElementExtension ModelExtension { get; private set; }

        public bool IsHidden => ModelExtension?.IsHidden ?? false;

        protected bool IsApplyingTransform { get; set; }

        protected bool IsUpdatingTransform { get; set; }

        protected PartElementModel(PartElement element)
        {
            Element = element;
            Element.PropertyChanged += Element_PropertyChanged;

            ModelExtension = element.GetExtension<ModelElementExtension>();

            if (ModelExtension != null)
            {
                Visible = ModelExtension.IsVisible;
                ModelExtension.VisibileChanged += Extender_VisibileChanged;
            }

            Element.ParentChanging += Element_ParentChanging;
            Element.ParentChanged += Element_ParentChanged;

            Element_ParentChanged(null, EventArgs.Empty);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (ModelExtension != null)
            {
                ModelExtension.VisibileChanged -= Extender_VisibileChanged;
            }
        }

        private void Element_ParentChanging(object sender, EventArgs e)
        {
            if (Element.Parent is IPhysicalElement parentElem)
                parentElem.TranformChanged -= ParentElem_TranformChanged;
        }

        private void Element_ParentChanged(object sender, EventArgs e)
        {
            if (Element.Parent is IPhysicalElement parentElem)
                parentElem.TranformChanged += ParentElem_TranformChanged;

            SetTransformFromElement();
        }

        private void ParentElem_TranformChanged(object sender, EventArgs e)
        {
            SetTransformFromElement();
        }

        private void Extender_VisibileChanged(object sender, EventArgs e)
        {
            var extender = Element.GetExtension<ModelElementExtension>();
            Visible = extender.IsVisible;
        }

        protected override bool? VisibleOverride()
        {
            var extender = Element?.GetExtension<ModelElementExtension>();
            if (extender != null)
                return extender.IsVisible;

            return null;
        }

        protected override void OnTransformChanged()
        {
            base.OnTransformChanged();

            if (!IsUpdatingTransform)
            {
                Matrix4 transCopy = Transform;
                transCopy.ClearScale();
                ApplyTransformToElement(transCopy);
            }
        }

        protected virtual void ApplyTransformToElement(Matrix4 transform)
        {
            IsApplyingTransform = true;

            if (Element is IPhysicalElement physicalElement)
            {
                if (Element.Parent is IPhysicalElement parentElem)
                {
                    var parentTrans = parentElem.Transform.ToMatrixD().ToGL();
                    var localTrans = transform.ToMatrix4d() * parentTrans.Inverted();
                    physicalElement.Transform = ItemTransform.FromMatrix(localTrans.ToLDD());
                }
                else
                    physicalElement.Transform = ItemTransform.FromMatrix(transform.ToLDD());

            }
            IsApplyingTransform = false;
        }

        protected void SetTransformFromElement()
        {
            IsUpdatingTransform = true;
            SetTransform(GetElementTransform(), true);
            IsUpdatingTransform = false;
        }

        protected virtual Matrix4 GetElementTransform()
        {
            if (Element is IPhysicalElement physicalElement)
            {
                var baseTransform = physicalElement.Transform.ToMatrixD().ToGL();

                if (Element.Parent is IPhysicalElement parentElem)
                {
                    var parentTransform = parentElem.Transform.ToMatrixD().ToGL();
                    baseTransform = baseTransform * parentTransform;
                }
                
                return baseTransform.ToMatrix4();
            }
            return Matrix4.Identity;
        }

        private void Element_PropertyChanged(object sender, ElementValueChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IPhysicalElement.Transform))
            {
                if (!IsApplyingTransform)
                    SetTransformFromElement();
            }

            OnElementPropertyChanged(e);
        }

        protected virtual void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {

        }

        public void RenderTransformed(Matrix4 transform, Camera camera, MeshRenderMode mode = MeshRenderMode.Solid)
        {
            var currentTransform = Transform;
            SetTransform(transform, false);
            RenderModel(camera, mode);
            SetTransform(currentTransform, false);
        }
    }
}
