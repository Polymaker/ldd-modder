using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.PaletteMaker.Rebrickable
{
    [XmlRoot("root"), Serializable]
    public class PartInfo
    {
        [XmlElement("part_id")]
        public string PartId { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("year1")]
        public string Year1 { get; set; }

        [XmlElement("year2")]
        public string Year2 { get; set; }

        [XmlElement("category")]
        public string Category { get; set; }

        [XmlElement("part_type_id")]
        public string PartTypeId { get; set; }

        [XmlArray("rebrickable_part_ids"), XmlArrayItem("part_id")]
        public string[] RebrickablePartIds { get; set; }

        [XmlArray("colors"), XmlArrayItem("color")]
        public ColorInfo[] Colors { get; set; }

        [XmlElement("external_part_ids")]
        public ExternalPartInfo ExternalParts { get; set; }

        [XmlArray("related_parts"), XmlArrayItem("related_to")]
        public RelatedPart[] RelatedParts { get; set; }

        [XmlElement("part_url")]
        public string PartUrl { get; set; }

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
            /// Total number of the part in this color from all sets
            /// (eg: set #1 qty=1, set #2 qty=2, total = 3)
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
            public string[] LDrawIds { get; set; }

            [XmlElement("lego_design_id")]
            public string[] LegoDesignIds { get; set; }

            [XmlArray("lego_element_ids"), XmlArrayItem("element")]
            public LegoElement[] LegoElements { get; set; }

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
                LegoElements = new LegoElement[0];
                BrickLinkId = String.Empty;
                LDrawIds = new string[0];
                LegoDesignIds = new string[0];
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
                return (Year1 == "0" || string.IsNullOrEmpty(Year1)) && RebrickablePartIds != null && RebrickablePartIds.Length > 0;
            }
        }

        public PartInfo()
        {
            PartId = String.Empty;
            Name = String.Empty;
            Year1 = String.Empty;
            Year2 = String.Empty;
            Category = String.Empty;
            PartTypeId = String.Empty;
            RebrickablePartIds = new string[0];
            ExternalParts = new ExternalPartInfo();
            RelatedParts = new RelatedPart[0];
        }
    }

    public class GetPartParameters : ApiFunctionParameters
    {
        [ApiParameter("part_id")]
        public string PartId { get; set; }
        [ApiParameter("inc_rels")]
        public bool IncludeRelationships { get; set; }
        [ApiParameter("inc_ext")]
        public bool IncludeExternalIDs { get; set; }
        [ApiParameter("inc_colors")]
        public bool IncludePartColors { get; set; }


        public GetPartParameters(string partId)
        {
            PartId = partId;
            IncludeRelationships = false;
            IncludeExternalIDs = false;
            IncludePartColors = false;
        }

        public GetPartParameters(string partId, bool includeRelationships, bool includeExternalIDs, bool includePartColors)
        {
            PartId = partId;
            IncludeRelationships = includeRelationships;
            IncludeExternalIDs = includeExternalIDs;
            IncludePartColors = includePartColors;
        }

        public static implicit operator GetPartParameters(string partId)
        {
            return new GetPartParameters(partId);
        }
    }
}
