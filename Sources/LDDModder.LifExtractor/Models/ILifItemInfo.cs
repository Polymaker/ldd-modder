using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LifExtractor.Models
{
    interface ILifItemInfo
    {
        LifFile.LifEntry Entry { get; }
        string Name { get; }

        string FullName { get; }

        string Description { get; }

        string ItemImageKey { get; }
    }
}
