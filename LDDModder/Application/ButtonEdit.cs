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
                _ReadOnly = value;
                if (textBox1 != null && textBox1.IsHandleCreated)
                    textBox1.ReadOnly = value;
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

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (specified.HasFlag(BoundsSpecified.Height))
            {
                height = textBox1.Height + 8;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            //if (Application.RenderWithVisualStyles)
            //{
            //    SetWindowTheme(Handle, "EXPLORER", null);
            //}
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            //ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle);

            //if (Application.RenderWithVisualStyles &&
            //    VisualStyleRenderer.IsElementDefined(VisualStyleElement.TextBox.TextEdit.Selected))
            //{
            //    //var renderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Selected);
            //    //renderer.DrawBackground(e.Graphics, this.ClientRectangle);
            //    DrawWindowBackground(Handle, e.Graphics, ClientRectangle, "EDIT", 1, 8);
            //}

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

        /*
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        private extern static int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, IntPtr pClipRect);

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr OpenThemeData(IntPtr hWnd, String classList);

        [DllImport("uxtheme.dll", ExactSpelling = true)]
        public extern static Int32 CloseThemeData(IntPtr hTheme);

        const int EP_BACKGROUNDWITHBORDER = 5;
        const int EBWBS_NORMAL = 1;

        public static void DrawWindowBackground(IntPtr hWnd, Graphics g, Rectangle bounds, string className, int elementId, int state = 0)
        {
            IntPtr theme = OpenThemeData(hWnd, className);
            if (theme != IntPtr.Zero)
            {
                IntPtr hdc = g.GetHdc();
                RECT area = new RECT(bounds);
                DrawThemeBackground(theme, hdc, elementId, state, ref area, IntPtr.Zero);
                g.ReleaseHdc();
                CloseThemeData(theme);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }
        */
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
