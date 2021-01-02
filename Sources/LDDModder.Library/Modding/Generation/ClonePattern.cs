using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding
{
    public abstract class ClonePattern : PartElement
    {
        private int _Repetitions;
        
        public ObservableCollection<PartElement> Elements { get; set; }
        public ObservableCollection<int> SkippedInstances { get; set; }

        public int Repetitions
        {
            get => _Repetitions;
            set => SetPropertyValue(ref _Repetitions, value);
        }

        protected ClonePattern()
        {
            Elements = new ObservableCollection<PartElement>();
            SkippedInstances = new ObservableCollection<int>();
        }

        public abstract ItemTransform ApplyTransform(ItemTransform transform, int instance);

        public PartElement GetClonedElement(PartElement element, int instance)
        {
            if (instance == 0)
                return element;

            var trans = ApplyTransform((element as IPhysicalElement).Transform, instance);
            var newElem = (element as IClonableElement).Clone();
            (newElem as IPhysicalElement).Transform = trans;
            return null;
        }

        public IEnumerable<ItemTransform> GetClonedTransforms(ItemTransform baseTransform)
        {
            for (int i = 1; i < Repetitions; i++)
            {
                if (SkippedInstances.Contains(i))
                    break;

                foreach (var elem in Elements)
                    yield return ApplyTransform(baseTransform, i);
            }
        }

        public IEnumerable<PartElement> GenerateClones()
        {
            for (int i = 1; i < Repetitions; i++)
            {
                if (SkippedInstances.Contains(i))
                    break;

                foreach(var elem in Elements)
                    yield return GetClonedElement(elem, i);
            }
        }
    }
}
