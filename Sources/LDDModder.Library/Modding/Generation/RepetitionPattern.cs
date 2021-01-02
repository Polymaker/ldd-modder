using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding
{
    public class RepetitionPattern : ChangeTrackingObject
    {
        private int _Repetitions;

        public ChangeTrackingCollection<int> SkippedInstances { get; private set; }

        public int Repetitions
        {
            get => _Repetitions;
            set => SetPropertyValue(ref _Repetitions, value);
        }

        public RepetitionPattern()
        {
            SkippedInstances = new ChangeTrackingCollection<int>(this);
            //AttachCollection(SkippedInstances);
        }

        protected override void OnPropertyValueChanged(PropertyValueChangedEventArgs args)
        {
            base.OnPropertyValueChanged(args);
            
        }
    }
}
