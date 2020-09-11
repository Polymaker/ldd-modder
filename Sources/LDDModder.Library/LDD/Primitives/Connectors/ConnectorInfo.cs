using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Primitives.Connectors
{
    public class ConnectorInfo
    {
        public ConnectorType Type { get; set; }
        public int SubType { get; set; }
        public string Description { get; set; }
        public List<int> ConnectsWith { get; set; } = new List<int>();

        public static string MALE_LABEL = "Male";
        public static string FEMALE_LABEL = "Female";

        public string SubTypeDisplayText
        {
            get
            {
                if (string.IsNullOrEmpty(Description))
                {
                    if (SubType % 2 == 0)
                        return $"{SubType} ({FEMALE_LABEL})";
                    else
                        return $"{SubType} ({MALE_LABEL})";
                    //return SubType.ToString();
                }
                return $"{SubType} - {Description}";
            }
        }
    }
}
