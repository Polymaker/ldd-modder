using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Models.Project
{
    public abstract class ElementExtention : INotifyPropertyChanged
    {
        public ProjectManager Manager { get; }

        public PartElement Element { get; }

        private bool _IsHidden;
        private bool _IsVisible;

        public bool IsHidden
        {
            get => _IsHidden;
            set
            {
                if (_IsHidden != value)
                {
                    _IsHidden = value;
                }
            }
        }

        public bool IsVisible
        {
            get => _IsVisible;
        }

        protected ElementExtention(ProjectManager manager, PartElement element)
        {
            Manager = manager;
            Element = element;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void Initialize()
        {

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
