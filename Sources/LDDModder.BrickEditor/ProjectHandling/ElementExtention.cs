using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.ProjectHandling
{
    public class ElementExtention/* : INotifyPropertyChanged*/
    {
        public ProjectManager Manager { get; }

        public PartElement Element { get; }

        private bool _IsHidden;

        public bool IsHidden
        {
            get => _IsHidden;
            set
            {
                if (_IsHidden != value)
                {
                    _IsHidden = value;
                    CalculateVisibillity();
                }
            }
        }

        public bool IsVisible { get; private set; }

        public event EventHandler VisibillityChanged;

        internal ElementExtention(ProjectManager manager, PartElement element)
        {
            Manager = manager;
            Element = element;
            IsVisible = true;
        }

        public void CalculateVisibillity()
        {
            bool isParentVisible = true;

            if (Element.Parent != null)
            {
                var parentExt = Manager.GetExtension(Element.Parent);
                isParentVisible = parentExt?.IsVisible ?? true;
            }

            if (Element is PartSurface)
                isParentVisible &= Manager.ShowModels;

            if (Element is PartCollision)
                isParentVisible &= Manager.ShowCollisions;

            if (Element is PartConnection)
                isParentVisible &= Manager.ShowConnections;

            CalculateVisibillity(isParentVisible);
        }

        protected void CalculateVisibillity(bool parentIsVisible)
        {
            bool wasVisible = IsVisible;
            IsVisible = parentIsVisible && !IsHidden;

            foreach (var elem in Element.GetAllChilds())
            {
                if (!(elem is IPhysicalElement))
                    continue;
                PropagateVisibillity(elem, IsVisible);
            }

            if (IsVisible != wasVisible)
                VisibillityChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void PropagateVisibillity(PartElement element, bool parentIsVisible)
        {
            var elemExt = Manager.GetExtension(element);
            elemExt?.CalculateVisibillity(parentIsVisible);
        }
    }
}
