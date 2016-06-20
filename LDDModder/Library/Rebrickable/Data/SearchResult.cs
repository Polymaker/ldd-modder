using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.Rebrickable.Data
{
    [XmlRoot("results")]
    public class SearchResult
    {
        [XmlElement("set")]
        public List<Set> SetsAndMOCs { get; set; }

        [XmlElement("part")]
        public List<Part> Parts { get; set; }

        [XmlRoot("set")]
        public class Set
        {
            [XmlElement("set_id")]
            public string SetId { get; set; }
            [XmlElement("year")]
            public string Year { get; set; }
            [XmlElement("pieces")]
            public int Pieces { get; set; }
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

            [XmlIgnore]
            public bool IsMOC
            {
                get { return SetId.StartsWith("MOC"); }
            }
        }

        [XmlRoot("part")]
        public class Part
        {
            /// <summary>
            /// Part ID.
            /// </summary>
            [XmlElement("part_id")]
            public string PartID { get; set; }
            /// <summary>
            /// Part description.
            /// </summary>
            [XmlElement("descr")]
            public string Description { get; set; }
            /// <summary>
            /// Part type description.
            /// </summary>
            [XmlElement("part_type")]
            public string PartType { get; set; }
            /// <summary>
            /// Part type ID.
            /// </summary>
            [XmlElement("part_type_id")]
            public int PartTypeID { get; set; }
            /// <summary>
            /// Number of times this part appears in all sets.
            /// </summary>
            [XmlElement("num_parts")]
            public int TotalParts { get; set; }
            /// <summary>
            /// Number of sets this part appears in.
            /// </summary>
            [XmlElement("num_sets")]
            public int TotalSets { get; set; }
            /// <summary>
            /// Number of colors this part appears in.
            /// </summary>
            [XmlElement("num_colors")]
            public int TotalColors { get; set; }
            /// <summary>
            /// First year this part appeared.
            /// </summary>
            [XmlElement("min_year")]
            public int MinYear { get; set; }
            /// <summary>
            /// Last year this part appeared.
            /// </summary>
            [XmlElement("max_year")]
            public int MaxYear { get; set; }
            /// <summary>
            /// Rebrickable URL for the part.
            /// </summary>
            [XmlElement("url")]
            public string PartUrl { get; set; }
            /// <summary>
            /// URL of the part image.
            /// </summary>
            [XmlElement("img_url")]
            public string ImageUrl { get; set; }
        }
    }
}