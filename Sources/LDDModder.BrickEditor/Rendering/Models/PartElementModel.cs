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

            if (Element is IPhysicalElement physicalElement)
            {
                Matrix4 transCopy = Transform;
                transCopy.ClearScale();
                IsApplyingTransform = true;
                physicalElement.Transform = ItemTransform.FromMatrix(transCopy.ToLDD());
                IsApplyingTransform = false;
            }
        }

        private void Element_PropertyChanged(object sender, ElementValueChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IPhysicalElement.Transform) && !IsApplyingTransform)
            {
                var baseTransform = (Element as IPhysicalElement).Transform.ToMatrix().ToGL();
                SetTransform(baseTransform, true);
            }

            OnElementPropertyChanged(e);
        }

        protected virtual void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {

        }
    }
}
