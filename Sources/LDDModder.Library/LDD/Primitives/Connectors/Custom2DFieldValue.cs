using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Primitives.Connectors
{
    public struct Custom2DFieldValue
    {
        public int Value1;

        public int Value2;

        public int Value3;

        public int[] Values
        {
            get => new int[] { Value1, Value2, Value3 };
            set 
            {
                Value1 = value[0];
                Value2 = value[1];
                Value3 = value[2];
            }
        }

        public int this[int i]
        {
            get => Values[i];
            set
            {
                switch(i)
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
                }
            }
        }

        //public int[] Values;

        //public int Value1 { get => Values[0]; set => Values[0] = value; }

        //public int Value2 { get => Values[1]; set => Values[1] = value; }

        //public int Value3 { get => Values[2]; set => Values[2] = value; }

        public Custom2DFieldValue(int[] values)
        {
            Value1 = values[0];
            Value2 = values[1];
            Value3 = values[2];
        }

        public Custom2DFieldValue(int value1, int value2, int value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public override string ToString()
        {
            if (Value3 == -1)
                return $"{Value1}:{Value2}";
            return $"{Value1}:{Value2}:{Value3}";
        }

        public override bool Equals(object obj)
        {
            return obj is Custom2DFieldValue value &&
                   Value1 == value.Value1 &&
                   Value2 == value.Value2 &&
                   Value3 == value.Value3;
        }

        public static bool operator ==(Custom2DFieldValue v1, Custom2DFieldValue v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Custom2DFieldValue v1, Custom2DFieldValue v2)
        {
            return !v1.Equals(v2);
        }

        public override int GetHashCode()
        {
            var hashCode = 29335732;
            hashCode = hashCode * -1521134295 + Value1.GetHashCode();
            hashCode = hashCode * -1521134295 + Value2.GetHashCode();
            hashCode = hashCode * -1521134295 + Value3.GetHashCode();
            return hashCode;
        }

        public static bool Validate(string values)
        {
            return TryParse(values, out _);
        }


        public static bool TryParse(string values, out Custom2DFieldValue value)
        {
            value = new Custom2DFieldValue();

            string[] strValues = values.Trim().Split(':');

            if (strValues.Length < 2 || strValues.Length > 3)
                return false;

            for (int i = 0; i < 3; i++)
            {
                if (i < strValues.Length)
                {
                    if (int.TryParse(strValues[i], out int val))
                        value[i] = val;
                    else
                        return false;
                }
                else
                    value[i] = -1;
            }
            return true;
        }
    }
}
