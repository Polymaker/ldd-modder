using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.PaletteMaker.Rebrickable
{
    [XmlRoot("results")]
    public class SearchResult
    {
        [XmlElement("set")]
        public List<Set> SetsFound { get; set; }

        [XmlRoot("set")]
        public class Set
        {
            [XmlElement("set_id")]
            public string SetID { get; set; }
            [XmlElement("year")]
            public string Year { get; set; }
            [XmlElement("pieces")]
            public string Pieces { get; set; }
            [XmlElement("theme1")]
            public string Theme1 { get; set; }
            [XmlElement("theme2")]
            public string Theme2 { get; set; }
            [XmlElement("theme3")]
            public string Theme3 { get; set; }
            [XmlElement("accessory")]
            public int Accessory { get; set; }
            [XmlElement("kit")]
            public int Kit { get; set; }
            [XmlElement("descr")]
            public string Description { get; set; }
            [XmlElement("url")]
            public string SetUrl { get; set; }
            [XmlElement("img_tn")]
            public string ImageUrlTiny { get; set; }
            [XmlElement("img_sm")]
            public string ImageUrlSmall { get; set; }
            [XmlElement("img_big")]
            public string ImageUrlBig { get; set; }
        }
    }

    public enum SearchType
    {
        Set = 'S',
        Part = 'P',
        MOC = 'M'
    }

    public class SearchParameters : ApiFunctionParameters
    {
        [ApiParameter("query")]
        public string Query { get; set; }

        [ApiParameter("type")]
        public SearchType Type { get; set; }

        public SearchParameters(string query, SearchType type)
        {
            Query = query;
            Type = type;
        }

        protected override string GetParamValue(string paramName)
        {
            if (paramName == "Type")
                return ((char)Type).ToString();
            return base.GetParamValue(paramName);
        }
    }
}
