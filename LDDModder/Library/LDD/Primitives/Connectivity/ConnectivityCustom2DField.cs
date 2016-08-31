using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Custom2DField")]
    public class ConnectivityCustom2DField : Connectivity
    {
        internal static string[] AttributeOrder = new string[] { "type", "width", "height", "angle", "ax", "ay", "az", "tx", "ty", "tz" };

        private ConnectionItem[,] _Mapping;

        public override ConnectivityType Type
        {
            get { return ConnectivityType.Custom2DField; }
        }

        /// <summary>
        /// Width = S * 2, where S: Stud length
        /// </summary>
        [XmlAttribute("width")]
        public int Width { get; set; }

        /// <summary>
        /// Height = S * 2, where S: Stud length
        /// </summary>
        [XmlAttribute("height")]
        public int Height { get; set; }

        //TODO: change type for class to parse custom data
        [XmlText]
        public string ConnectivityData { get; set; }

        [XmlIgnore]
        public bool IsMaleConnection { get { return SubType == 23; } }

        [XmlIgnore]
        public bool IsFemaleConnection { get { return SubType == 22; } }

        /// <summary>
        /// For some (still unknown) reasons, the width and heigth of the array is +1 from the specified value (Width/Height properties)
        /// </summary>
        [XmlIgnore]
        public ConnectionItem[,] Mapping
        {
            get { return _Mapping; }
        }

        public ConnectivityCustom2DField()
        {
            _Mapping = null;
            Width = 0;
            Height = 0;
            ConnectivityData = String.Empty;
        }

        protected override void OnDeserialized()
        {
            base.OnDeserialized();
            TrimText();
            ParseData();
        }

        private void TrimText()
        {
            ConnectivityData = ConnectivityData.Split(new string[] { "\n" }, StringSplitOptions.None).Select(s => s.TrimStart()).Aggregate((i, j) => i + "\n" + j);
            ConnectivityData = ConnectivityData.Trim();
        }

        public void ParseData()
        {
            _Mapping = new ConnectionItem[Width + 1, Height + 1];

            var cleanedString = ConnectivityData
                .Replace("\n", string.Empty)
                .Replace(" ", string.Empty);

            var itemsStr = cleanedString.Split(',');
            int i = 0;
            for (int y = 0; y <= Height; y++)
            {
                for (int x = 0; x <= Width; x++)
                {
                    _Mapping[x, y] = ParseItem(itemsStr[i++].Trim());
                }
            }
        }

        private static ConnectionItem ParseItem(string value)
        {
            var numbers = value.Split(':');
            if (numbers.Length == 3)
                return new ConnectionItem(int.Parse(numbers[0]), int.Parse(numbers[1]), int.Parse(numbers[2]));
            else if (numbers.Length == 2)
                return new ConnectionItem(int.Parse(numbers[0]), int.Parse(numbers[1]));
            return null;
        }

        public class ConnectionItem
        {
            public int Value1 { get; set; }
            public int Value2 { get; set; }
            public int Value3 { get; set; }//almost always 1

            public ConnectionItem(int value1, int value2, int value3)
            {
                Value1 = value1;
                Value2 = value2;
                Value3 = value3;
            }

            public ConnectionItem(int value1, int value2)
            {
                Value1 = value1;
                Value2 = value2;
                Value3 = 0;
            }

            public ConnectionItem()
            {
                Value1 = 0;
                Value2 = 0;
                Value3 = 0;
            }

            public override string ToString()
            {
                return string.Format("{0}:{1}:{2}", Value1, Value2, Value3);
            }
        }
    }
}
