using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class Custom2DFieldConnector : Connector, IEnumerable<Custom2DFieldConnector.FieldNode>
    {
        private FieldNode[,] NodeArray;
        private int _Width;
        private int _Height;

        public override ConnectorType Type => ConnectorType.Custom2DField;

        /// <summary>
        /// Width = S * 2, where S: Stud length
        /// </summary>
        public int Width
        {
            get => _Width;
            set
            {
                if (value != _Width && value % 2 == 0 && value > 0)
                {
                    _Width = value;
                    Initialize2DArray();
                }
            }
        }

        public int StudWidth
        {
            get => Width / 2;
            set => Width = value * 2;
        }

        /// <summary>
        /// Height = S * 2, where S: Stud length
        /// </summary>
        public int Height
        {
            get => _Height;
            set
            {
                if (value != _Height && value % 2 == 0 && value > 0)
                {
                    _Height = value;
                    Initialize2DArray();
                }
            }
        }

        public int StudHeight
        {
            get => Height / 2;
            set => Height = value * 2;
        }

        public Custom2DFieldConnector()
        {
            _Width = 2;
            _Height = 2;
            Initialize2DArray();
        }

        private void Initialize2DArray()
        {
            NodeArray = new FieldNode[Width + 1, Height + 1];
            for (int x = 0; x <= Width; x++)
                for (int y = 0; y <= Height; y++)
                    NodeArray[x, y] = new FieldNode();
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            _Width = element.ReadAttribute<int>("width");
            _Height = element.ReadAttribute<int>("height");
            Initialize2DArray();

            var values = element.Value.Trim().Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                int rowIdx = (int)Math.Floor(i / (double)(Width + 1));
                NodeArray[i % (Width + 1), rowIdx].Parse(values[i]);
            }
        }

        protected override void SerializeBeforeTransform(XElement element)
        {
            element.Add(new XAttribute("width", Width));
            element.Add(new XAttribute("height", Height));
            var values = new List<string>();
            for (int y = 0; y <= Height; y++)
                for (int x = 0; x <= Width; x++)
                    values.Add(NodeArray[x, y].ToString());
            
            element.Value = string.Join(",", values);
        }

        public IEnumerator<FieldNode> GetEnumerator()
        {
            return NodeArray.Cast<FieldNode>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class FieldNode
        {
            public int Value1 { get; set; }
            public int Value2 { get; set; }
            public int Value3 { get; set; }

            public int this[int index]
            {
                get
                {
                    switch(index)
                    {
                        case 0:
                            return Value1;
                        case 1:
                            return Value2;
                        case 2:
                            return Value3;
                        default:
                            throw new IndexOutOfRangeException("index");
                    }
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            Value1 = value;
                            break;
                        case 1:
                            Value2 = value;
                            break;
                        case 2:
                            Value3 = value;
                            break;
                        default:
                            throw new IndexOutOfRangeException("index");
                    }
                }
            }

            public void Parse(string value)
            {
                string[] values = value.Trim().Split(':');
                if (values.Length == 3)
                {
                    Value1 = int.Parse(values[0]);
                    Value2 = int.Parse(values[1]);
                    Value3 = int.Parse(values[2]);
                }
                else if (values.Length == 2)
                {
                    Value1 = int.Parse(values[0]);
                    Value2 = int.Parse(values[1]);
                    Value3 = -1;
                }
                else
                    Trace.WriteLine("Invalid Custom2DField node");
            }

            public override string ToString()
            {
                if (Value3 == -1)
                    return $"{Value1}:{Value2}";
                return $"{Value1}:{Value2}:{Value3}";
            }
        }
    }
}
