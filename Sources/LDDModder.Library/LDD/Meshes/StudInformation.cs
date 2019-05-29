namespace LDDModder.LDD.Meshes
{
    public class StudInformation
    {
        /// <summary>
        /// Index of the related Custom2DField connector in the primitive XML file.
        /// </summary>
        public int ConnectorIndex { get; set; }

        public int Value2 { get; set; }
        public int Value3 { get; set; }
        public int Value4 { get; set; }
        public int Value5 { get; set; }
        public int Value6 { get; set; }

        public StudInformation()
        {
        }

        public StudInformation(int connectorIndex, int value2, int value3, int value4, int value5, int value6)
        {
            ConnectorIndex = connectorIndex;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
            Value5 = value5;
            Value6 = value6;
        }

        public StudInformation(int[] values)
        {
            ConnectorIndex = values[0];
            Value2 = values[1];
            Value3 = values[2];
            Value4 = values[3];
            Value5 = values[4];
            Value6 = values[5];
        }
    }
}
