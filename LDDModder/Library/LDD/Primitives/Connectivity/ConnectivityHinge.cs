using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Hinge")]
    public class ConnectivityHinge : Connectivity
    {
        internal static string[] AttributeOrder = new string[] { "type", "oriented", "tag", "angle", "ax", "ay", "az", "tx", "ty", "tz", "LimMin", "LimMax", "FlipLimMin", "FlipLimMax" };

        [XmlIgnore]
        public bool Oriented { get; set; }

        [XmlAttribute("oriented"), EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int OrientedInt
        {
            get { return Convert.ToInt32(Oriented); }
            set { Oriented = Convert.ToBoolean(value); }
        }

        [XmlAttribute("LimMin")]
        public float LimitMin { get; set; }

        [XmlAttribute("LimMax")]
        public float LimitMax { get; set; }

        [XmlAttribute("FlipLimMin")]
        public float FlipLimitMin { get; set; }

        [XmlAttribute("FlipLimMax")]
        public float FlipLimitMax { get; set; }
        
        [XmlAttribute("tag")]
        public string Tag { get; set; }

        #region ShouldSerialize

        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        //public bool ShouldSerializeOrientedInt()
        //{
        //    return Oriented;
        //}

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeLimitMin()
        {
            return LimitMin != 0f;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeLimitMax()
        {
            return LimitMax != 0f;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeFlipLimitMin()
        {
            return FlipLimitMin != 0f;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeFlipLimitMax()
        {
            return FlipLimitMax != 0f;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeTag()
        {
            return !string.IsNullOrEmpty(Tag);
        }

        #endregion
    }
}
