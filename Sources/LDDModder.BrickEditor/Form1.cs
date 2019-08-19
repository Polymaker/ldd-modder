using Assimp;
using LDDModder.LDD.Data;
using LDDModder.LDD.Files;
using LDDModder.LDD.Files.MeshStructures;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LDDModder.BrickEditor
{
    public partial class Form1 : Form
    {
        private string LddDbDirectory;
        private string LddMeshDirectory;
        private string PrimitiveDirectory;
        public Form1()
        {
            InitializeComponent();
            //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            LddDbDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\");
            LddMeshDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\LOD0\");
            PrimitiveDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\");
        }
    }
}
