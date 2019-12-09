using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class CollectionChangeItemInfo
    {
        public int OldIndex { get; set; }
        public int NewIndex { get; set; }
        public PartElement Element { get; set; }

        public CollectionChangeItemInfo(PartElement element, int oldIndex, int newIndex)
        {
            Element = element;
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }
    }
}
