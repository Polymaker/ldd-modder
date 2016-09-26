using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.LDD.Primitives
{
    public class ConnectorInfo
    {
        private ConnectivityType _Type;
        private int _SubType;
        private string _Name;
        private bool _IsMale;
        private static List<ConnectorInfo> _Connectors;

        public ConnectivityType Type
        {
            get { return _Type; }
        }

        public int SubType
        {
            get { return _SubType; }
        }

        public string Name
        {
            get { return _Name; }
        }

        public bool IsMale
        {
            get { return _IsMale; }
        }

        public bool IsFemale
        {
            get { return !IsMale; }
        }

        public ConnectorInfo(ConnectivityType type, int subType, string name, bool isMale)
        {
            _Type = type;
            _SubType = subType;
            _Name = name;
            _IsMale = isMale;
        }

        public static List<ConnectorInfo> Connectors
        {
            get { return _Connectors; }
        }

        static ConnectorInfo()
        {
            _Connectors = new List<ConnectorInfo>();
            _Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 2, "Pin", false));
            _Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 3, "Pin", true));//male pin connector (thick, can't fit a bar inside)
            _Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 19, "Pin", true));//male pin connector (thin, can fit a bar inside)

            _Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 4, "Cross axle", false));
            _Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 5, "Cross axle", true));

            _Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 6, "Bar", false));
            _Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 7, "Bar", true));

            _Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 8, "Pin hole", false));//pin & cross axle hole, can connect stud (a Custom2DField is still needed)
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Hinge, 28, "?", ?));//this hinge subtype may be related to axel 8, as seen in part 11295

            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 12, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 13, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 14, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 15, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 17, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Axel, 21, "?", ?));

            _Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 2, "Tow ball", false));
            _Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 3, "Tow ball", true));

            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 4 , "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 5 , "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 6 , "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 7 , "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 8 , "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 9 , "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 10, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 11, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 12, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 13, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 14, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 15, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 16, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 17, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 18, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 19, "?", ?));
            //_Connectors.Add(new ConnectorInfo(ConnectivityType.Ball, 999000, "?", ?));
        }
    }
}
