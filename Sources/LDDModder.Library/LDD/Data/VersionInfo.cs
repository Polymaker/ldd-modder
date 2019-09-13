using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Data
{
    public class VersionInfo
    {
        public int Major { get; set; }
        public int Minor { get; set; }

        public VersionInfo()
        {
        }

        public VersionInfo(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }

        public XElement ToXmlElement(string elementName = "Version")
        {
            return new XElement(elementName, new XAttribute("Major", Major), new XAttribute("Minor", Minor));
        }
    }
}
