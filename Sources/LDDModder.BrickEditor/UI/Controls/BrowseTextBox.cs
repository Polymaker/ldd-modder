using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Windows.Forms.Design.Behavior;

namespace LDDModder.BrickEditor.UI.Controls
{
    [Designer(typeof(BrowseTextBoxDesigner))]
    public partial class BrowseTextBox : UserControl
    {
        public event EventHandler BrowseButtonClicked;
        public event EventHandler ValueChanged;

        private bool _ReadOnly;
        private string _Value;
        private string _ButtonText;
        private int _ButtonWidth;
        private bool _AutoSizeButton;
        private bool _UseReadOnlyColor;

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => _ReadOnly;
            set
            {
                if (_ReadOnly != value)
                {
                    _ReadOnly = value;
                    ValueTextBox.ReadOnly = value;
                    ValueTextBox.BackColor = (_ReadOnly && UseReadOnlyColor) ? SystemColors.Control : SystemColors.Window;
                }
            }
        }

        [DefaultValue(false)]
        public bool UseReadOnlyColor
        {
            get => _UseReadOnlyColor;
            set
            {
                if (_UseReadOnlyColor != value)
                {
                    _UseReadOnlyColor = value;
                    ValueTextBox.BackColor = (ReadOnly && _UseReadOnlyColor) ? SystemColors.Control : SystemColors.Window;
                }
            }
        }

        public string Value
        {
            get => _Value;
            set
            {
                if (_Value != value)
                {
                    ValueTextBox.Text = value;
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public string ButtonText
        {
            get => _ButtonText;
            set
            {
                if (_ButtonText != value)
                {
                    BrowseButton.Text = value;
                    _ButtonText = value;
                    RecalculateButtonWidth();
                }
            }
        }

        [DefaultValue(75)]
        public int ButtonWidth
        {
            get => _ButtonWidth;
            set
            {
                if (!AutoSizeButton && _ButtonWidth != value && value > 3)
                {
                    _ButtonWidth = value;
                    RepositionControls();
                }
            }
        }

        [DefaultValue(false)]
        public bool AutoSizeButton
        {
            get => _AutoSizeButton;
            set
            {
                if (_AutoSizeButton != value)
                {
                    _AutoSizeButton = value;
                    RecalculateButtonWidth();
                }
            }
        }

        [DefaultValue(BorderStyle.Fixed3D)]
        public new BorderStyle BorderStyle
        {
            get => ValueTextBox.BorderStyle;
            set => ValueTextBox.BorderStyle = value;
        } 

        public BrowseTextBox()
        {
            InitializeComponent();
            _ButtonText = "Browse";
            _Value = string.Empty;
            _ButtonWidth = 75;
            _AutoSizeButton = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (AutoSizeButton)
                RecalculateButtonWidth();

            RepositionControls();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (specified.HasFlag(BoundsSpecified.Height))
            {
                height = ValueTextBox.Height;
            }
            
            base.SetBoundsCore(x, y, width, height, specified);

            if (specified.HasFlag(BoundsSpecified.Height) || specified.HasFlag(BoundsSpecified.Width))
            {
                RepositionControls();
            }
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            RepositionControls();
        }

        private void RepositionControls()
        {
            //var clientRect = ClientRectangle;
            ValueTextBox.Width = (Width - ButtonWidth) + 3;
            BrowseButton.Width = ButtonWidth;
            BrowseButton.Height = ValueTextBox.Height + 2;
            BrowseButton.Left = ValueTextBox.Right - 2;
            Height = ValueTextBox.Height;
        }

        private int CalculateButtonWidth()
        {
            //using(var g = BrowseButton.CreateGraphics())
            //{
            //    var textSize = g.MeasureString(ButtonText, Font);
            //    return (int)textSize.Width + 8;
            //}
            BrowseButton.SuspendLayout();
            var currentBounds = BrowseButton.Bounds;
            BrowseButton.AutoSize = true;
            BrowseButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            var prefSize = BrowseButton.GetPreferredSize(new Size(1000, Height));
            BrowseButton.AutoSize = false;
            BrowseButton.AutoSizeMode = AutoSizeMode.GrowOnly;
            BrowseButton.Bounds = currentBounds;
            BrowseButton.ResumeLayout();
            return prefSize.Width;
        }

        private void RecalculateButtonWidth()
        {
            if (AutoSizeButton)
            {
                int oldWidth = _ButtonWidth;
                _ButtonWidth = CalculateButtonWidth();
                if (oldWidth != _ButtonWidth)
                    RepositionControls();
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            BrowseButtonClicked?.Invoke(this, e);
        }

        private void ValueTextBox_Validated(object sender, EventArgs e)
        {
            _Value = ValueTextBox.Text;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ValueTextBox_Validating(object sender, CancelEventArgs e)
        {
            OnValidating(e);
        }

        protected bool ShouldSerializeButtonWidth()
        {
            return !AutoSizeButton;
        }

        private void Controls_FontChanged(object sender, EventArgs e)
        {
            if (AutoSizeButton)
                RecalculateButtonWidth();
            RepositionControls();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            RepositionControls();
        }
    }

    internal class BrowseTextBoxDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                selectionRules |= SelectionRules.AllSizeable;
                selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
                return selectionRules;
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            string[] propsToRemove = new string[] { "AutoSize", "AutoSizeMode" };

            for (int i = 0; i < propsToRemove.Length; i++)
            {
                if (properties.Contains(propsToRemove[i]))
                    properties.Remove(propsToRemove[i]);
            }
        }

        public override IList SnapLines
        {
            get
            {
                ArrayList arrayList = base.SnapLines as ArrayList;
                int textBaseline = GetTextBaseline(Control, ContentAlignment.TopLeft);
                BorderStyle borderStyle = BorderStyle.Fixed3D;
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["BorderStyle"];
                if (propertyDescriptor != null)
                {
                    borderStyle = (BorderStyle)propertyDescriptor.GetValue(base.Component);
                }
                switch (borderStyle)
                {
                    case BorderStyle.None:
                        break;
                    case BorderStyle.FixedSingle:
                        textBaseline += 2;
                        break;
                    case BorderStyle.Fixed3D:
                        textBaseline += 3;
                        break;
                }
                arrayList.Add(new SnapLine(SnapLineType.Baseline, textBaseline, SnapLinePriority.Medium));
                return arrayList;
            }
        }
        private static System.Reflection.MethodInfo GetTextBaselineMethod;

        private static int GetTextBaseline(Control ctrl, ContentAlignment alignment)
        {
            if (GetTextBaselineMethod == null)
            {
                var designerClass = Type.GetType("System.Windows.Forms.Design.DesignerUtils, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                if (designerClass != null)
                    GetTextBaselineMethod = designerClass.GetMethod("GetTextBaseline");

            }

            if (GetTextBaselineMethod != null)
                return (int)GetTextBaselineMethod.Invoke(null, new object[] { ctrl, alignment });
            return 0;
        }
    }
}
