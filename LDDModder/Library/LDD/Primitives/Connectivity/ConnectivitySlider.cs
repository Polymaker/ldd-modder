using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Slider")]
    public class ConnectivitySlider : Connectivity
    {
        internal static string[] AttributeOrder = new string[] { "type", "length", "cylindrical", "tag", "startCapped", "endCapped", "spring", "angle", "ax", "ay", "az", "tx", "ty", "tz" };

        public override ConnectivityType Type
        {
            get { return ConnectivityType.Slider; }
        }

        [XmlAttribute("length")]
        public double Length { get; set; }
        [XmlIgnore]
        public bool StartCapped { get; set; }
        [XmlIgnore]
        public bool EndCapped { get; set; }
        [XmlIgnore]
        public bool Cylindrical { get; set; }
        [XmlAttribute("tag")]
        public string Tag { get; set; }
        //TODO: change type for class/struct to represent 3D point/vector (string value = 3 float separated by commas)
        [XmlAttribute("spring")]
        public string SpringStr { get; set; }
        //spring="0,1,0.10000000000000001"

        #region int (0,1) <-> bool trick properties

        [XmlAttribute("startCapped")]
        [EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int StartCappedInt
        {
            get { return Convert.ToInt32(StartCapped); }
            set { StartCapped = Convert.ToBoolean(value); }
        }

        [XmlAttribute("endCapped")]
        [EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int EndCappedInt
        {
            get { return Convert.ToInt32(EndCapped); }
            set { EndCapped = Convert.ToBoolean(value); }
        }

        [XmlAttribute("cylindrical")]
        [EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int CylindricalInt
        {
            get { return Convert.ToInt32(Cylindrical); }
            set { Cylindrical = Convert.ToBoolean(value); }
        }

        #endregion

        #region ShouldSerialize

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeTag()
        {
            return !string.IsNullOrEmpty(Tag);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeSpringStr()
        {
            return !string.IsNullOrEmpty(SpringStr);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeCylindricalInt()
        {
            return Cylindrical;
        }

        #endregion
    }
}
