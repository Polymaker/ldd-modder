using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class Custom2DFieldConnector : Connector, IEnumerable<Custom2DFieldConnector.FieldNode>, INotifyPropertyChanged
    {
        private FieldNode[,] NodeArray;
        private int _Width;
        private int _Height;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler NodeValueChanged;

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
                    OnPropertyChanged(nameof(Width));
                }
            }
        }

        public int ArrayWidth => Width + 1;

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
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        public int ArrayHeight => Height + 1;

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
                int y = (int)Math.Floor(index / (Width + 1d));
                return NodeArray[x, y];
            }
        }

        public Custom2DFieldConnector()
        {
            _Width = 2;
            _Height = 2;
            Initialize2DArray();
        }

        public FieldNode GetNode(int x, int y)
        {
            if (x >= 0 && x <= Width && y >= 0 && y <= Height)
                return NodeArray[x, y];
            return null;
        }

        public FieldNode GetNode(int index)
        {
            if (index >= 0 && index < (Width + 1) * (Height + 1))
                return this[index];
            return null;
        }

        private void Initialize2DArray()
        {
            var oldValues = NodeArray;

            if (NodeArray != null)
                DettachNodeEvents(NodeArray);

            NodeArray = new FieldNode[Width + 1, Height + 1];
            int nodeIndex = 0;

            for (int y = 0; y < Height + 1; y++)
            {
                for (int x = 0; x < Width + 1; x++)
                {
                    NodeArray[x, y] = new FieldNode(x, y, nodeIndex++);
                }
            }

            if (oldValues != null)
            {
                int oldW = oldValues.GetLength(0);
                int oldH = oldValues.GetLength(1);

                for (int x = 0; x < Math.Min(oldW, Width + 1); x++)
                    for (int y = 0; y < Math.Min(oldH, Height + 1); y++)
                        NodeArray[x, y].Values = oldValues[x, y].Values;
            }

            AttachNodeEvents(NodeArray);
        }

        private void AttachNodeEvents(FieldNode[,] nodeArray)
        {
            int width = nodeArray.GetLength(0);
            int height = nodeArray.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodeArray[x, y].PropertyChanged += NodeArray_PropertyChanged;
                }
            }
        }


        private void DettachNodeEvents(FieldNode[,] nodeArray)
        {
            int width = nodeArray.GetLength(0);
            int height = nodeArray.GetLength(1);
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    nodeArray[x, y].PropertyChanged -= NodeArray_PropertyChanged;
                }
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

        public static bool TryParseNode(string nodeValue)
        {
            string[] values = nodeValue.Trim().Split(':');

            if (values.Length < 2 || values.Length > 3)
                return false;

            if (!int.TryParse(values[0], out _))
                return false;
            if (!int.TryParse(values[1], out _))
                return false;
            if (values.Length == 3)
            {
                if (values.Length == 3 && !int.TryParse(values[2], out _))
                    return false;
            }

            return true;
        }

        public IEnumerator<FieldNode> GetEnumerator()
        {
            return NodeArray.Cast<FieldNode>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnNodePropertyChanged(FieldNode node, string propertyName)
        {
            NodeValueChanged?.Invoke(node, new PropertyChangedEventArgs(propertyName));
        }

        private void NodeArray_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnNodePropertyChanged(sender as FieldNode, e.PropertyName);
        }

        public class FieldNode : INotifyPropertyChanged
        {
            private int[] _Values;

            public int Value1
            {
                get => _Values[0];
                set
                {
                    if (_Values[0] != value)
                    {
                        _Values[0] = value;
                        OnPropertyChanged(nameof(Value1));
                    }
                }
            }

            public int Value2
            {
                get => _Values[1];
                set
                {
                    if (_Values[1] != value)
                    {
                        _Values[1] = value;
                        OnPropertyChanged(nameof(Value2));
                    }
                }
            }

            public int Value3
            {
                get => _Values[2];
                set
                {
                    if (_Values[2] != value)
                    {
                        _Values[2] = value;
                        OnPropertyChanged(nameof(Value3));
                    }
                }
            }

            public Tuple<int,int,int> Values
            {
                get => new Tuple<int, int, int>(Value1, Value2, Value3);
                set
                {
                    _Values[0] = value.Item1;
                    _Values[1] = value.Item2;
                    _Values[2] = value.Item3;
                    OnPropertyChanged(nameof(Values));
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

            public event PropertyChangedEventHandler PropertyChanged;

            internal FieldNode()
            {
                _Values = new int[3];
            }

            public FieldNode(int x, int y, int index)
            {
                X = x;
                Y = y;
                Index = index;
                _Values = new int[3];
            }

            public void Parse(string value)
            {
                string[] values = value.Trim().Split(':');
                if (value.Length > 0 )
                {
                    
                }

                if (value.Length > 0)
                {
                    if (int.TryParse(values[0], out int v0))
                        _Values[0] = v0;
                    else
                        Trace.WriteLine("Invalid value");
                }

                for (int i = 0; i < 3; i++)
                {
                    if (i < values.Length)
                    {
                        if (int.TryParse(values[i], out int val))
                            _Values[i] = val;
                        else
                            Trace.WriteLine($"Invalid value {i + 1}: {values[i]}");
                    }
                    else
                        _Values[i] = -1;
                }

                if (values.Length < 2 || values.Length > 3)
                    Trace.WriteLine("Invalid Custom2DField node");
            }

            public override string ToString()
            {
                if (Value3 == -1)
                    return $"{Value1}:{Value2}";
                return $"{Value1}:{Value2}:{Value3}";
            }

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
