using System;
using System.ComponentModel;
using System.Diagnostics;

namespace LDDModder.LDD.Primitives.Connectors
{

    public class Custom2DFieldNode : ChangeTrackingObject
    {
        private int[] _Values;

        public int[] Values
        {
            get => _Values;
            set
            {
                if (value != null && value.Length == 3)
                {
                    SetPropertyValue(ref _Values, value);
                }
            }
        }

        public int Value1
        {
            get => _Values[0];
            set => SetPropertyValue(ref _Values, 0, value, nameof(Values));
        }

        public int Value2
        {
            get => _Values[1];
            set => SetPropertyValue(ref _Values, 1, value, nameof(Values));
        }

        public int Value3
        {
            get => _Values[2];
            set => SetPropertyValue(ref _Values, 2, value, nameof(Values));
        }

        public int X { get; }
        public int Y { get; }

        public int Index { get; }

        public int this[int index]
        {
            get
            {
                switch (index)
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

        internal Custom2DFieldNode()
        {
            _Values = new int[3];
        }

        public Custom2DFieldNode(int x, int y, int index)
        {
            X = x;
            Y = y;
            Index = index;
            _Values = new int[3];
        }

        public static bool Parse(string nodeID, out int[] values)
        {
            values = new int[3];

            string[] strValues = nodeID.Trim().Split(':');

            if (strValues.Length < 2 || strValues.Length > 3)
                return false;

            for (int i = 0; i < 3; i++)
            {
                if (i < strValues.Length)
                {
                    if (int.TryParse(strValues[i], out int val))
                        values[i] = val;
                    else
                        return false;
                }
                else
                    values[i] = -1;
            }
            return true;
        }

        public void Parse(string nodeID)
        {
            string[] strValues = nodeID.Trim().Split(':');
            var values = new int[3];
            for (int i = 0; i < 3; i++)
            {
                if (i < strValues.Length)
                {
                    if (int.TryParse(strValues[i], out int val))
                        values[i] = val;
                    else
                        Trace.WriteLine($"Invalid value {i + 1}: {strValues[i]}");
                }
                else
                    values[i] = -1;
            }

            if (strValues.Length < 2 || strValues.Length > 3)
                Trace.WriteLine("Invalid Custom2DField node");

            Values = values;
        }

        public override string ToString()
        {
            if (Value3 == -1)
                return $"{Value1}:{Value2}";
            return $"{Value1}:{Value2}:{Value3}";
        }

        public Custom2DFieldNode Clone()
        {
            return new Custom2DFieldNode(X, Y, Index)
            {
                Values = Values
            };
        }
    }

}
