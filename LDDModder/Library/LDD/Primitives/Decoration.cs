using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Primitives
{
    [XmlRoot("Decoration")]
    public class Decoration
    {
        private string _LookUpString;
        private int[] _LookupTable;
        [XmlAttribute("faces")]
        public int Faces { get; set; }

        [XmlAttribute("subMaterialRedirectLookupTable")]
        
        public string LookUpString
        {
            get { return _LookUpString; }
            set
            {
                if (_LookUpString == value)
                    return;
                _LookUpString = value;
                ParseLookupTable();
            }
        }
        

        [XmlIgnore]
        public int[] LookupTable
        {
            get { return _LookupTable; }
            set
            {
                if (value == _LookupTable)
                    return;
                if (value == null)
                {
                    _LookUpString = string.Empty;
                    _LookupTable = new int[0];
                }
                else
                {
                    _LookupTable = value;
                    Faces = value.Length;
                    _LookUpString = LookupTable.Select(x => x.ToString()).Aggregate((i, j) => i + "," + j);
                }
            }
        }

        private void ParseLookupTable()
        {
            if (string.IsNullOrEmpty(LookUpString))
            {
                _LookupTable = new int[0];
                return;
            }
            var values = LookUpString.Split(',');
            _LookupTable = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
                _LookupTable[i] = int.Parse(values[i]);
        }

        public Decoration()
        {
            _LookupTable = new int[0];
            Faces = 0;
            LookUpString = String.Empty;
        }
    }
}
