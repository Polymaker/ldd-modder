using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LifExtractor.Models
{
    interface ILifItemInfo
    {
        string Name { get; }

        string Description { get; }

        string ItemImageKey { get; }
    }
}
