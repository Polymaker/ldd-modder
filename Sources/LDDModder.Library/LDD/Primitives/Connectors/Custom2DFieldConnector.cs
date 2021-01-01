using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public partial class Custom2DFieldConnector : Connector, IEnumerable<Custom2DFieldNode>
    {
        private Custom2DFieldNode[,] _NodeArray;
        private Custom2DFieldValue[,] _ValueArray;

        private int _Width;
        private int _Height;

        public event EventHandler SizeChanged;

        public override ConnectorType Type => ConnectorType.Custom2DField;

        /// <summary>
        /// Width = S * 2, where S: Stud count
        /// </summary>
        public int Width
        {
            get => _Width;
            set
            {
                Resize(value, Height);
                //if ((value % 2) == 0 && value > 0)
                //{
                //    if (SetPropertyValue(ref _Width, value))
                //        Initialize2DArray();
                //}
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
                Resize(Width, value);
                //if ((value % 2) == 0 && value > 0)
                //{
                //    if (SetPropertyValue(ref _Height, value))
                //        Initialize2DArray();
                //}
            }
        }

        public int ArrayHeight => Height + 1;

        public int StudHeight
        {
            get => Height / 2;
            set => Height = value * 2;
        }

        public Custom2DFieldNode this[int x, int y]
        {
            get => _NodeArray[x, y];
            set
            {
                if (value != null)
                {
                    var node = GetNode(x, y);
                    if (node != null)
                        node.Values = value.Values;
                }
            }
        }

        public Custom2DFieldNode this[int index]
        {
            get
            {
                var pos = IndexToPosition(index);
                return _NodeArray[pos.Item1, pos.Item2];
            }
            set
            {
                if (value != null)
                {
                    var pos = IndexToPosition(index);
                    _NodeArray[pos.Item1, pos.Item2].Values = value.Values;
                } 
            }
        }

        public Custom2DFieldNode[,] NodeArray
        {
            get => _NodeArray;
            //set => AssignArrayValues(value);
        }

        public Custom2DFieldValue[,] Values
        {
            get => _ValueArray;
            set => AssignArrayValues(value);
        }

        public Custom2DFieldConnector()
        {
            _Width = 2;
            _Height = 2;
            SubType = 23;
            Rebuild2DArray(false);
        }

        public Custom2DFieldConnector(int subType, int width, int height)
        {
            _Width = (int)(Math.Round(width / 2d) * 2);
            _Height = (int)(Math.Round(height / 2d) * 2);
            SubType = subType;
            Rebuild2DArray(false);
        }

        public void Resize(int newWidth, int newHeight)
        {
            if (newWidth % 2 != 0 || newHeight % 2 != 0)
            {
                newWidth = (int)(Math.Round(newWidth / 2d) * 2);
                newHeight = (int)(Math.Round(newHeight / 2d) * 2);
            }

            if (newWidth <= 0 || newHeight <= 0)
                return;

            if (newWidth == Width && newHeight == Height)
                return;

            _Width = newWidth;
            _Height = newHeight;

            Rebuild2DArray(true);
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }

        public int PositionToIndex(int x, int y)
        {
            return (y * ArrayWidth) + x;
        }

        public Tuple<int,int> IndexToPosition(int index)
        {
            int x = index % ArrayWidth;
            //int y = (int)Math.Floor(index / (double)ArrayWidth);
            int y = (index - x) / ArrayWidth;
            return new Tuple<int, int>(x, y);
        }

        public Custom2DFieldNode GetNode(int x, int y)
        {
            if (x >= 0 && x <= Width && y >= 0 && y <= Height)
                return _NodeArray[x, y];
            return null;
        }

        public Custom2DFieldNode GetNode(int index)
        {
            if (index >= 0 && index < (Width + 1) * (Height + 1))
                return this[index];
            return null;
        }

        public Custom2DFieldValue GetValue(int x, int y)
        {
            if (x >= 0 && x <= Width && y >= 0 && y <= Height)
                return _ValueArray[x, y];
            return new Custom2DFieldValue();
        }

        public Custom2DFieldValue GetValue(int index)
        {
            if (index >= 0 && index < (Width + 1) * (Height + 1))
            {
                var pos = IndexToPosition(index);
                return _ValueArray[pos.Item1, pos.Item2];
            }
            return new Custom2DFieldValue();
        }

        private void Rebuild2DArray(bool raiseChange)
        {
            var oldValues = _ValueArray;

            var newValues = new Custom2DFieldValue[Width + 1, Height + 1];

            for (int y = 0; y < Height + 1; y++)
            {
                for (int x = 0; x < Width + 1; x++)
                    newValues[x, y] = new Custom2DFieldValue(new int[] { 29, 0, 0 });
            }

            if (oldValues != null)
            {
                int oldW = oldValues.GetLength(0);
                int oldH = oldValues.GetLength(1);

                for (int x = 0; x < Math.Min(oldW, Width + 1); x++)
                {
                    for (int y = 0; y < Math.Min(oldH, Height + 1); y++)
                        newValues[x, y] = oldValues[x, y];
                }
            }

            if (raiseChange)
                OnPropertyValueChanging(new PropertyValueChangingEventArgs(nameof(Values), oldValues, newValues));

            _ValueArray = newValues;

            if (raiseChange)
                OnPropertyValueChanged(new PropertyValueChangedEventArgs(nameof(Values), oldValues, newValues));

            RebuildNodes();
        }

        private void RebuildNodes()
        {
            _NodeArray = new Custom2DFieldNode[Width + 1, Height + 1];

            for (int y = 0; y < Height + 1; y++)
            {
                for (int x = 0; x < Width + 1; x++)
                    _NodeArray[x, y] = new Custom2DFieldNode(this, x, y);
            }
        }

        public void AssignArrayValues(Custom2DFieldValue[,] valuesArray)
        {
            int newWidth = valuesArray.GetLength(0);
            int newHeight = valuesArray.GetLength(1);


            if (newWidth <= 0 || newHeight <= 0)
                return;
            
            bool sizeChanged = newWidth != ArrayWidth|| newHeight != ArrayHeight;
            bool hasChanges = sizeChanged;

            if (!hasChanges)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    for (int y = 0; y < newHeight; y++)
                    {
                        if (_ValueArray[x, y].Values != valuesArray[x, y].Values)
                        {
                            hasChanges = true;
                            break;
                        }
                    }
                }
            }

            if (!hasChanges)
                return;

            var oldValue = _ValueArray;

            OnPropertyValueChanging(new PropertyValueChangingEventArgs(nameof(Values), oldValue, valuesArray));

            _Width = newWidth - 1;
            _Height = newHeight - 1;
            _ValueArray = valuesArray;
            RebuildNodes();

            OnPropertyValueChanged(new PropertyValueChangedEventArgs(nameof(Values), oldValue, valuesArray));

            if (sizeChanged)
                SizeChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void InternalSetValue(int x, int y, Custom2DFieldValue value)
        {
            _ValueArray[x, y] = value;
        }

        public void SetValue(int x, int y, Custom2DFieldValue value)
        {
            if (_ValueArray[x, y] != value)
            {
                SetIndexedPropertyValue(ref _ValueArray, new int[] { x, y }, value, nameof(Values));
            }
            //var node = GetNode(x, y);
            //node.SetArrayValue(value);
        }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            _Width = element.ReadAttribute<int>("width");
            _Height = element.ReadAttribute<int>("height");

            Rebuild2DArray(false);
            int expectedValueCount = (Width + 1) * (Height + 1);

            var values = element.Value.Trim().Split(',').ToList();
            values.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            if (values.Count != expectedValueCount)
                Console.WriteLine("Unexpected number of values!");
            //if (values.Count != expectedValueCount)
            //    throw new Exception("Unexpected number of values!");
            
            for (int i = 0; i < expectedValueCount; i++)
            {
                int rowIdx = (int)Math.Floor(i / (double)(Width + 1));
                if (rowIdx > Height + 1)
                    break;

                if (Custom2DFieldValue.TryParse(values[i], out Custom2DFieldValue value))
                    _ValueArray[i % (Width + 1), rowIdx] = value;
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
                    line += _ValueArray[x, y].ToString() + ",";
                content += line;
            }
            content = content.TrimEnd(',') + Environment.NewLine;
            element.Value = content;
        }

        public IEnumerator<Custom2DFieldNode> GetEnumerator()
        {
            return _NodeArray.Cast<Custom2DFieldNode>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override Connector Clone()
        {
            var conn = new Custom2DFieldConnector(SubType, Width, Height)
            {
                _ValueArray = Values
            };
            conn.RebuildNodes();
            //conn.AssignArrayValues(Values);
            return conn;
        }
    }
}
