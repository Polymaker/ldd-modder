using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.ProjectHandling
{
    public class ModelElementExtension : IElementExtender/* : INotifyPropertyChanged*/
    {
        public ProjectManager Manager { get; internal set; }

        public PartElement Element { get; }

        private bool _IsHidden;
        private bool _IsVisible;
        private bool visbilityDirty;

        public bool IsHidden
        {
            get => _IsHidden;
            set
            {
                if (_IsHidden != value)
                {
                    _IsHidden = value;
                    CalculateVisibility();
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                if (visbilityDirty)
                    CalculateVisibility();
                return _IsVisible;
            }
            //protected set => _IsVisible = value;
        }

        public event EventHandler VisibilityChanged;

        internal ModelElementExtension(ProjectManager manager, PartElement element)
        {
            Manager = manager;
            Element = element;
            _IsVisible = true;
            visbilityDirty = true;
        }

        public ModelElementExtension(PartElement element)
        {
            Element = element;
            _IsVisible = true;
            visbilityDirty = true;
        }

        internal void AssignManager(ProjectManager manager)
        {
            Manager = manager;
            visbilityDirty = true;
        }

        public void FlagVisibilityDirty()
        {
            visbilityDirty = true;
        }

        public void CalculateVisibility()
        {
            bool isParentVisible = true;

            if (Element.Parent != null)
            {
                var parentExt = Element.Parent.GetExtension<ModelElementExtension>();//  Manager.GetExtension(Element.Parent);
                if (parentExt.visbilityDirty)
                {
                    parentExt.CalculateVisibility();
                    return;
                }
                isParentVisible = parentExt?.IsVisible ?? true;
            }

            if (Manager != null)
            {
                if (Element is PartSurface)
                    isParentVisible &= Manager.ShowPartModels;

                if (Element is PartCollision)
                    isParentVisible &= Manager.ShowCollisions;

                if (Element is PartConnection)
                    isParentVisible &= Manager.ShowConnections;
            }


            if (Element.Parent is FemaleStudModel femaleStudModel)
            {
                var parentExt = Element.Parent.GetExtension<FemaleStudModelExtension>();
                if (parentExt.ShowAlternateModels)
                    isParentVisible &= femaleStudModel.ReplacementMeshes.Contains(Element);
                else
                    isParentVisible &= femaleStudModel.Meshes.Contains(Element);
            }

            CalculateVisibility(isParentVisible);
        }

        public virtual bool IsHiddenByParent()
        {
            if (Element.Parent != null)
            {
                var parentExt = Element.Parent.GetExtension<ModelElementExtension>();
                if (parentExt?.IsVisible == false)
                    return true;
            }

            if (Manager != null)
            {
                if (Element is PartSurface)
                    return !Manager.ShowPartModels;
                else if (Element is PartCollision)
                    return !Manager.ShowCollisions;
                else if (Element is PartConnection)
                    return !Manager.ShowConnections;
            }

            if (Element.Parent is FemaleStudModel femaleStudModel)
            {
                var parentExt = Element.Parent.GetExtension<FemaleStudModelExtension>();
                bool isAlternateMesh = femaleStudModel.ReplacementMeshes.Contains(Element);

                if (isAlternateMesh != parentExt.ShowAlternateModels)
                    return true;
            }

            return false;
        }

        protected void CalculateVisibility(bool parentIsVisible)
        {
            bool wasVisible = _IsVisible;

            _IsVisible = parentIsVisible && !IsHidden;
            visbilityDirty = false;

            foreach (var elem in Element.GetAllChilds())
                PropagateVisibility(elem, _IsVisible);

            if (_IsVisible != wasVisible)
                VisibilityChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void PropagateVisibility(PartElement element, bool parentIsVisible)
        {
            var elemExt = element.GetExtension<ModelElementExtension>();

            if (elemExt == null)
            {
                foreach(var childElem in element.GetAllChilds())
                    PropagateVisibility(childElem, parentIsVisible);
            }
            else
            {
                elemExt.CalculateVisibility(parentIsVisible);
            }
        }
    }
}
