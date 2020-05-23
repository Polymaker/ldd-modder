using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LifExtractor.Models
{
    class LifFileInfo : LifItemInfo<LifFile.FileEntry>
    {
        public LifFile.FileEntry File => Entry as LifFile.FileEntry;

        public string FileType { get; set; }

        public long Size => File.FileSize;

        public DateTime CreatedDate => File.CreatedDate;

        public LifFileInfo(LifFile.FileEntry file) : base(file)
        {
            FileType = Path.GetExtension(file.Name).ToUpper();
            ItemImageKey = FileType;
        }

    }
}
