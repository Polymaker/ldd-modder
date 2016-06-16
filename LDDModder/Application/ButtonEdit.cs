using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace LDDModder
{
    [Designer(typeof(ButtonEditDesigner)), DefaultEvent("ButtonClicked")]
    public partial class ButtonEdit : UserControl
    {
        public new event EventHandler TextChanged;
        public event EventHandler ButtonClicked;
        private bool _UseReadOnlyAppearance = true;
        private bool _ReadOnly;
        private string TmpBtnText = "Button";
        

        [Browsable(true)]
        public override string Text
        {
            get
            {
                if (textBox1 != null && textBox1.IsHandleCreated)
                    return textBox1.Text;
                return string.Empty;
                //return base.Text;
            }

            set
            {
                if (textBox1 != null && textBox1.IsHandleCreated)
                    textBox1.Text = value;
                //base.Text = value;
            }
        }

        [DefaultValue("Button")]
        public string ButtonText
        {
            get
            {
                //if (button1 != null && button1.IsHandleCreated)
                //    return button1.Text;
                return TmpBtnText;
            }
            set
            {
                TmpBtnText = value;
                if (button1 != null && button1.IsHandleCreated)
                    button1.Text = value;
            }
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                if (_ReadOnly == value)
                    return;
                _ReadOnly = value;
                OnReadOnlyChanged();
            }
        }

        [DefaultValue(true)]
        public bool UseReadOnlyAppearance
        {
            get { return _UseReadOnlyAppearance; }
            set
            {
                if (_UseReadOnlyAppearance == value)
                    return;
                _UseReadOnlyAppearance = value;
                OnUseReadOnlyAppearanceChanged();
                
            }
        }
        

        public ButtonEdit()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ButtonClicked(this, EventArgs.Empty);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!string.IsNullOrEmpty(TmpBtnText))
                button1.Text = TmpBtnText;
            //textBox1.Text = base.Text;

            PerformControlsLayout();
            textBox1.ReadOnly = ReadOnly;
            textBox1.TextChanged += TextChanged;
            button1.Click += Button1_Click;
            button1.TextChanged += Button1_TextChanged;
            SetTextboxBackColor();
        }

        

        private void Button1_TextChanged(object sender, EventArgs e)
        {
            PerformControlsLayout();
        }

        private void PerformControlsLayout()
        {
            textBox1.Location = new Point(2, (Height - textBox1.Height) / 2);
            button1.MaximumSize = new Size(200, Height - 2);
            button1.Location = new Point(Width - button1.Width - 1, 1);
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            textBox1.Width = button1.Left - textBox1.Left - 2;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            textBox1.Width = button1.Left - textBox1.Left - 2;
        }

        protected void OnUseReadOnlyAppearanceChanged()
        {
            if (IsHandleCreated && ReadOnly)
            {
                Invalidate();
                SetTextboxBackColor();
            }
        }

        protected void OnReadOnlyChanged()
        {
            if (textBox1 == null || !textBox1.IsHandleCreated)
                return;
            textBox1.ReadOnly = ReadOnly;
            SetTextboxBackColor();
        }

        private void SetTextboxBackColor()
        {
            if (ReadOnly && !UseReadOnlyAppearance)
                textBox1.BackColor = SystemColors.Window;
            else
                textBox1.ResetBackColor();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (specified.HasFlag(BoundsSpecified.Height))
            {
                height = textBox1.Height + 8;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {


            if (TextBoxRenderer.IsSupported)
            {
                if(ReadOnly)
                    TextBoxRenderer.DrawTextBox(e.Graphics, ClientRectangle, TextBoxState.Readonly);
                else if(!Enabled)
                    TextBoxRenderer.DrawTextBox(e.Graphics, ClientRectangle, TextBoxState.Disabled);
                else
                    TextBoxRenderer.DrawTextBox(e.Graphics, ClientRectangle, TextBoxState.Normal);

            }
            else
                base.OnPaintBackground(e);

        }

       
    }

    internal class ButtonEditDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                return base.SelectionRules ^ SelectionRules.TopSizeable ^ SelectionRules.BottomSizeable;
            }
        }
    }
}
