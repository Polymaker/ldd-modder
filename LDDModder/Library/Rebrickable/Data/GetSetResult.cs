using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.Rebrickable.Data
{
    [XmlRoot("set")]
    public class GetSetResult
    {
        /// <summary>
        /// Set ID.
        /// </summary>
        [XmlElement("set_id")]
        public string SetId { get; set; }
        /// <summary>
        /// Set type (set, moc, kit).
        /// </summary>
        [XmlElement("type")]
        public string Type { get; set; }
        /// <summary>
        /// Number of parts in the set.
        /// </summary>
        [XmlElement("pieces")]
        public string Pieces { get; set; }
        /// <summary>
        /// Set description.
        /// </summary>
        [XmlElement("descr")]
        public string Description { get; set; }
        /// <summary>
        /// Set theme level 1.
        /// </summary>
        [XmlElement("theme")]
        public string Theme { get; set; }
        /// <summary>
        /// Year the set was released.
        /// </summary>
        [XmlElement("year")]
        public string Year { get; set; }
        /// <summary>
        /// Image URL - tiny.
        /// </summary>
        [XmlElement("img_tn")]
        public string ImageUrlTiny { get; set; }
        /// <summary>
        /// Image URL - small.
        /// </summary>
        [XmlElement("img_sm")]
        public string ImageUrlSmall { get; set; }
        /// <summary>
        /// Image URL - big.
        /// </summary>
        [XmlElement("img_big")]
        public string ImageUrlBig { get; set; }
    }
}
