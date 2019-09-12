using LDDModder.LDD.Primitives.Connectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class StudConnection : PartConnector<Custom2DFieldConnector>
    {
        public string RefID { get; set; }

    }
}
