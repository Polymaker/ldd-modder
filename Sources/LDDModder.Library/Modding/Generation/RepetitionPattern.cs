using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding
{
    public abstract class RepetitionPattern : ClonePattern
    {
        private int _Repetitions;

        public int Repetitions
        {
            get => _Repetitions;
            set => SetPropertyValue(ref _Repetitions, value);
        }

        public override int NumberOfInstances => Repetitions;

        public ChangeTrackingCollection<int> SkippedInstances { get; set; }

        protected RepetitionPattern()
        {
            SkippedInstances = new ChangeTrackingCollection<int>();
            TrackCollectionChanges(SkippedInstances);
            Repetitions = 1;
        }

        public override XElement SerializeToXml()
        {
            var baseElem = base.SerializeToXml();
            baseElem.WriteAttribute(nameof(Repetitions), Repetitions);
            if (SkippedInstances.Any())
                baseElem.AddElement(nameof(SkippedInstances), string.Join(",", SkippedInstances));
            return baseElem;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            Repetitions = element.ReadAttribute(nameof(Repetitions), 1);

            SkippedInstances.Clear();
            if (element.HasElement(nameof(SkippedInstances), out XElement skipElem))
                SkippedInstances.AddRange(StringUtils.ParseStringList<int>(skipElem.Value));
        }
    }
}
