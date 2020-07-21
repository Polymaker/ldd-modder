using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
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

        public bool IsSelected { get; set; }

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
                ModelExtension.VisibilityChanged += Extender_VisibilityChanged;
            }
        }

        private void Extender_VisibilityChanged(object sender, EventArgs e)
        {
            var extender = Element.GetExtension<ModelElementExtension>();
            Visible = extender.IsVisible;
        }

        protected override bool GetVisibleCore()
        {
            var extender = Element.GetExtension<ModelElementExtension>();
            return extender?.IsVisible ?? base.GetVisibleCore();
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
                physicalElement.Transform = ItemTransform.FromMatrix(transform.ToLDD());
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
                var baseTransform = physicalElement.Transform.ToMatrix().ToGL();
                return baseTransform;
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
    }
}
