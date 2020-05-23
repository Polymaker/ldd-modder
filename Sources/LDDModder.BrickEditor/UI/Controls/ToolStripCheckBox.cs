using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LDDModder.BrickEditor.UI.Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripCheckBox : ToolStripControlHost
    {

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckBox CheckBox => Control as CheckBox;

        [DefaultValue(false)]
        [Category("Appearance")]
        public bool Checked { get => CheckBox.Checked; set => CheckBox.Checked = value; }

        [DefaultValue(CheckState.Unchecked)]
        [Category("Appearance")]
        public CheckState CheckState { get => CheckBox.CheckState; set => CheckBox.CheckState = value; }

        [DefaultValue(ContentAlignment.MiddleLeft)]
        [Category("Appearance")]
        [Localizable(true)]
        public ContentAlignment CheckAlign { get => CheckBox.CheckAlign; set => CheckBox.CheckAlign = value; }

        private Color _BackColor;

        public override Color BackColor 
        { 
            get => _BackColor; 
            set => _BackColor = value; 
        }

        public ToolStripCheckBox() : base(new CheckBox())
        {
            CheckBox.BackColor = Color.Transparent;
            _BackColor = SystemColors.Control;
        }

        protected bool ShouldSerializeBackColor()
        {
            return _BackColor != SystemColors.Control;
        }

        public override void ResetBackColor()
        {
            _BackColor = SystemColors.Control;
        }
    }
}
