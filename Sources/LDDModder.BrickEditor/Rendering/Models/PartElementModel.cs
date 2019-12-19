using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public abstract class PartElementModel : ModelBase
    {
        public PartElement Element { get; set; }

        public bool IsSelected { get; set; }

        protected bool IsApplyingTransform { get; set; }

        protected PartElementModel(PartElement element)
        {
            Element = element;
            Element.PropertyChanged += Element_PropertyChanged;

            var extender = element.GetExtension<ModelElementExtension>();

            if (extender != null)
            {
                Visible = extender.IsVisible;
                extender.VisibilityChanged += Extender_VisibilityChanged;
            }
        }

        private void Extender_VisibilityChanged(object sender, EventArgs e)
        {
            var extender = Element.GetExtension<ModelElementExtension>();
            Visible = extender.IsVisible;
        }

        protected override void OnTransformChanged()
        {
            base.OnTransformChanged();

            Matrix4 transCopy = Transform;
            transCopy.ClearScale();

            IsApplyingTransform = true;
            ApplyTransformToElement(transCopy);
            IsApplyingTransform = false;
            
        }

        protected virtual void ApplyTransformToElement(Matrix4 transform)
        {
            if (Element is IPhysicalElement physicalElement)
                physicalElement.Transform = ItemTransform.FromMatrix(transform.ToLDD());
        }

        protected void SetTransformFromElement()
        {
            SetTransform(GetElementTransform(), false);
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
            if (e.PropertyName == nameof(IPhysicalElement.Transform) && !IsApplyingTransform)
                SetTransformFromElement();

            OnElementPropertyChanged(e);
        }

        protected virtual void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {

        }
    }
}
