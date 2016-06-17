using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.Rebrickable.Data
{
    [XmlRoot("root")]
    public class GetPartResult
    {
        /// <summary>
        /// Part ID.
        /// </summary>
        [XmlElement("part_id")]
        public string PartId { get; set; }
        /// <summary>
        /// Part Name.
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }
        /// <summary>
        /// Year the part first appeared in sets.
        /// </summary>
        [XmlElement("year1")]
        public string Year1 { get; set; }
        /// <summary>
        /// Year the part was last seen in sets.
        /// </summary>
        [XmlElement("year2")]
        public string Year2 { get; set; }
        /// <summary>
        /// Part category/type description.
        /// </summary>
        [XmlElement("category")]
        public string Category { get; set; }
        /// <summary>
        /// Part Type ID (matches get_part_types).
        /// </summary>
        [XmlElement("part_type_id")]
        public string PartTypeId { get; set; }
        /// <summary>
        /// If this field is returned, it indicates the equivalent Part ID(s) used by Rebrickable.
        /// </summary>
        [XmlArray("rebrickable_part_ids"), XmlArrayItem("part_id")]
        public List<string> RebrickablePartIds { get; set; }
        /// <summary>
        /// List of colors the part appears in.
        /// </summary>
        [XmlArray("colors"), XmlArrayItem("color")]
        public List<ColorInfo> Colors { get; set; }
        /// <summary>
        /// List of related Part IDs used by external systems.
        /// </summary>
        [XmlElement("external_part_ids")]
        public ExternalPartInfo ExternalParts { get; set; }
        /// <summary>
        /// List of part relationships such as mold variants, printed parts, etc.
        /// </summary>
        [XmlArray("related_parts"), XmlArrayItem("related_to")]
        public List<RelatedPart> RelatedParts { get; set; }
        /// <summary>
        /// URL to the Part Details page.
        /// </summary>
        [XmlElement("part_url")]
        public string PartUrl { get; set; }
        /// <summary>
        /// URL of the main part image (tries to use most common color).
        /// </summary>
        [XmlElement("part_img_url")]
        public string PartImgUrl { get; set; }

        [Serializable]
        public class ColorInfo
        {
            [XmlElement("rb_color_id")]
            public int RebrickableId { get; set; }
            [XmlElement("ldraw_color_id")]
            public int LDrawId { get; set; }
            [XmlElement("color_name")]
            public string Name { get; set; }
            /// <summary>
            /// Number of sets containing the part in this color
            /// </summary>
            [XmlElement("num_sets")]
            public int NumberOfSets { get; set; }
            /// <summary>
            /// Sum of the part in this color from all sets
            /// </summary>
            [XmlElement("num_parts")]
            public int NumberOfParts { get; set; }
        }

        [XmlRoot("external_part_ids"), Serializable]
        public class ExternalPartInfo
        {
            [XmlElement("bricklink")]
            public string BrickLinkId { get; set; }

            [XmlElement("ldraw")]
            public List<string> LDrawIds { get; set; }

            [XmlElement("lego_design_id")]
            public List<string> LegoDesignIds { get; set; }

            [XmlArray("lego_element_ids"), XmlArrayItem("element")]
            public List<LegoElement> LegoElements { get; set; }

            [Serializable]
            public class LegoElement
            {
                [XmlElement("color")]
                public int Color { get; set; }
                [XmlElement("element_id")]
                public string Id { get; set; }
            }

            public ExternalPartInfo()
            {
                LegoElements = new List<LegoElement>();
                BrickLinkId = String.Empty;
                LDrawIds = new List<string>();
                LegoDesignIds = new List<string>();
            }
        }

        [Serializable]
        public class RelatedPart
        {
            [XmlElement("part_id")]
            public string PartId { get; set; }
            [XmlElement("rel_type")]
            public string Type { get; set; }
        }

        [XmlIgnore]
        public bool RebrickableOnlyId
        {
            get
            {
                return (Year1 == "0" || string.IsNullOrEmpty(Year1)) && RebrickablePartIds != null && RebrickablePartIds.Count > 0;
            }
        }

        public GetPartResult()
        {
            PartId = String.Empty;
            Name = String.Empty;
            Year1 = String.Empty;
            Year2 = String.Empty;
            Category = String.Empty;
            PartTypeId = String.Empty;
            RebrickablePartIds = new List<string>();
            Colors = new List<ColorInfo>();
            ExternalParts = new ExternalPartInfo();
            RelatedParts = new List<RelatedPart>();
        }
    }
}
