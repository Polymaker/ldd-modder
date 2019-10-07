using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LifExtractor.Models
{
    class LifFolderInfo : ILifItemInfo
    {
        public LifFile.FolderEntry Folder { get; }

        public string Name => Folder.Name;

        public string Description => string.Empty;

        public string ItemImageKey => "folder";

        public LifFolderInfo(LifFile.FolderEntry folder)
        {
            Folder = folder;
        }
    }
}
