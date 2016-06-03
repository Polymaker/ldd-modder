using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.LDD.General
{
    public class VersionInfo
    {
        public int Major { get; set; }
        public int Minor { get; set; }


        public VersionInfo()
        {
            Major = 0;
            Minor = 0;
        }

        public VersionInfo(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
    }
}
