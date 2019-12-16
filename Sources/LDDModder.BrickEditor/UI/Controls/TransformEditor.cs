using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;

namespace LDDModder.BrickEditor.UI.Controls
{
    public partial class TransformEditor : UserControl
    {
        private ItemTransform _Value;
        private FlagManager FlagManager;


        public event EventHandler ValueChanged;

        public TransformEditor()
        {
            InitializeComponent();
            FlagManager = new FlagManager();
        }

        private void FillValues()
        {

        }
    }
}
