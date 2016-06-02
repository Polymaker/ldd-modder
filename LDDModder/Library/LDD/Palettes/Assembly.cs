﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    public class Assembly : Brick
    {
        [XmlElement("Part")]
        public List<Part> Parts { get; set; }
    }
}
