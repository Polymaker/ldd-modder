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
        /// Width = S * 2, where S: Stud count
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
        /// Height = S * 2, where S: Stud count
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

        public FieldNode this[int x, int y]
        {
            get => NodeArray[x, y];
        }

        public FieldNode this[int index]
        {
            get
            {
                int x = index % (Width + 1);
                int y = (int)Math.Floor(index / (Height + 1d));
                return NodeArray[x, y];
            }
        }

        public Custom2DFieldConnector()
        {
            _Width = 2;
            _Height = 2;
            Initialize2DArray();
        }

        private void Initialize2DArray()
        {
            var oldValues = NodeArray;

            NodeArray = new FieldNode[Width + 1, Height + 1];
            int nodeIndex = 0;

            for (int y = 0; y < Height + 1; y++)
                for (int x = 0; x < Width + 1; x++)
                    NodeArray[x, y] = new FieldNode(x, y, nodeIndex++);

            if (oldValues != null)
            {
                int oldW = oldValues.GetLength(0);
                int oldH = oldValues.GetLength(1);
                for (int x = 0; x < Math.Min(oldW, Width + 1); x++)
                    for (int y = 0; y < Math.Min(oldH, Height + 1); y++)
                        NodeArray[x, y].Values = oldValues[x, y].Values;
            }
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            _Width = element.ReadAttribute<int>("width");
            _Height = element.ReadAttribute<int>("height");
            NodeArray = null;
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
            string content = string.Empty;
            for (int y = 0; y <= Height; y++)
            {
                string line = Environment.NewLine;
                for (int x = 0; x <= Width; x++)
                    line += NodeArray[x, y].ToString() + ",";
                content += line;
            }
            content = content.TrimEnd(',') + Environment.NewLine;
            element.Value = content;
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

            public Tuple<int,int,int> Values
            {
                get => new Tuple<int, int, int>(Value1, Value2, Value3);
                set
                {
                    Value1 = value.Item1;
                    Value2 = value.Item2;
                    Value3 = value.Item3;
                }
            }

            public int X { get; }
            public int Y { get; }

            public int Index { get; }

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

            public FieldNode()
            {
            }

            public FieldNode(int x, int y, int index)
            {
                X = x;
                Y = y;
                Index = index;
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
