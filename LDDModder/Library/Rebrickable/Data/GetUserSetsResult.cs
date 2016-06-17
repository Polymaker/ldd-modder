using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.Rebrickable.Data
{
    [XmlRoot("sets")]
    public class GetUserSetsResult
    {
        [XmlElement("set")]
        public List<Set> Sets { get; set; }

        [XmlRoot("set")]
        public class Set
        {
            /// <summary>
            /// Set ID.
            /// </summary>
            [XmlElement("set_id")]
            public string SetID { get; set; }
            /// <summary>
            /// Number of sets the user owns.
            /// </summary>
            [XmlElement("qty")]
            public int Quantity { get; set; }
            /// <summary>
            /// Number of parts in the set.
            /// </summary>
            [XmlElement("pieces")]
            public int Pieces { get; set; }
            /// <summary>
            /// Set description.
            /// </summary>
            [XmlElement("descr")]
            public string Description { get; set; }
            /// <summary>
            /// Year the set was released.
            /// </summary>
            [XmlElement("year")]
            public string Year { get; set; }
            /// <summary>
            /// Set image URL - tiny
            /// </summary>
            [XmlElement("img_tn")]
            public string ImageUrlTiny { get; set; }
            /// <summary>
            /// Set image URL - small
            /// </summary>
            [XmlElement("img_sm")]
            public string ImageUrlSmall { get; set; }
            /// <summary>
            /// Set theme.
            /// </summary>
            [XmlElement("theme")]
            public string Theme { get; set; }

        }

        public GetUserSetsResult()
        {
            Sets = new List<Set>();
        }
    }
}
