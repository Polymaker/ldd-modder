using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LifExtractor.Models
{
    abstract class LifItemInfo<T> : ILifItemInfo where T : LifFile.LifEntry
    {
        public T Entry { get; private set; }

        public virtual string Name => Entry.Name;

        public string LifName { get; set; }

        public string Description { get; set; }

        public string ItemImageKey { get; set; }

        public string FullName => Entry.Parent == null ? Name : LifName + "\\" + Entry.FullName;

        LifFile.LifEntry ILifItemInfo.Entry => Entry;

        protected LifItemInfo(T entry)
        {
            Entry = entry;
            LifName = "LIF";
            Description = string.Empty;
        }
    }
}
