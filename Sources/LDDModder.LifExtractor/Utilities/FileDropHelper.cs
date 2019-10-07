using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.LifExtractor.Utilities
{
    class FileDropHelper : DataObject
    {
        public delegate object GetDataDelegate(string format, bool autoConvert);

        public FileDropHelper() : base() { }

        public FileDropHelper(string path) : base(DataFormats.FileDrop, new string[] { path }) { }

        public FileDropHelper(IEnumerable<string> paths) : base(DataFormats.FileDrop, paths.ToArray()) { }

        public GetDataDelegate GetDataOverride;

        public override object GetData(string format, bool autoConvert)
        {
            if (format == DataFormats.FileDrop && GetDataOverride != null)
                return GetDataOverride(format, autoConvert);
            return base.GetData(format, autoConvert);
        }

        public override StringCollection GetFileDropList()
        {
            return base.GetFileDropList();
        }

        public override bool GetDataPresent(string format, bool autoConvert)
        {
            return format == DataFormats.FileDrop;
        }

        public override string[] GetFormats()
        {
            return new string[] { DataFormats.FileDrop };
        }
    }
}
