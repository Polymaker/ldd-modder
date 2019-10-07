using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LifExtractor.Models
{
    class LifFileInfo : ILifItemInfo
    {
        public LifFile.FileEntry File { get; }

        public string Name => File.Name;

        public string FileType { get; set; }

        public string Description { get; set; }

        public long Size => File.FileSize;

        public DateTime CreatedDate => File.CreatedDate;

        public string ItemImageKey { get; set; }

        public LifFileInfo(LifFile.FileEntry file)
        {
            File = file;
            FileType = Path.GetExtension(file.Name).ToUpper();
            ItemImageKey = FileType;
        }
    }
}
