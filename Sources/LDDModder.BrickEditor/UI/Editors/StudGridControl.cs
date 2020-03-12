using LDDModder.LDD.Primitives.Connectors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Editors
{
    public partial class StudGridControl : Control
    {
        private Custom2DFieldConnector _StudConnector;
        private Size GridCellSize;
        private ComboBox EditCombo;

        private Tuple<int, int> SelectionStart;
        private Tuple<int, int> SelectionEnd;
        private Tuple<int, int> FocusedCell;

        private Size SelectionSize
        {
            get
            {
                if (SelectionStart != null)
                {
                    var end = SelectionEnd ?? SelectionStart;
                    int minX = Math.Min(SelectionStart.Item1, end.Item1);
                    int maxX = Math.Max(SelectionStart.Item1, end.Item1);
                    int minY = Math.Min(SelectionStart.Item2, end.Item2);
                    int maxY = Math.Max(SelectionStart.Item2, end.Item2);
                    return new Size((maxX - minX) + 1, (maxY - minY) + 1);
                }

                return Size.Empty;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Custom2DFieldConnector StudConnector
        {
            get => _StudConnector;
            set
            {
                if (_StudConnector != value)
                    BindConnector(value);
            }
        }

        private Custom2DFieldNode SelectedNode;

        public StudGridControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw | 
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.Selectable, true);

            InitEditCombo();


            BindConnector(null);
        }

        class NodeTypeInfo
        {
            public string ID { get; set; }
            public int Affinity { get; set; }

            public NodeTypeInfo(string iD, int affinity)
            {
                ID = iD;
                Affinity = affinity;
            }
        }

        protected void BindConnector(Custom2DFieldConnector connector)
        {
            if (_StudConnector != null)
            {
                _StudConnector.PropertyChanged -= StudConnector_PropertyChanged;
                _StudConnector.NodeValueChanged -= StudConnector_NodeValueChanged;
            }

            _StudConnector = connector ?? new Custom2DFieldConnector();

            _StudConnector.PropertyChanged += StudConnector_PropertyChanged;
            _StudConnector.NodeValueChanged += StudConnector_NodeValueChanged;

            UpdateControlSize();
            Invalidate();
        }

        private void StudConnector_NodeValueChanged(object sender, PropertyChangedEventArgs e)
        {
            Invalidate();
        }

        private void StudConnector_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateControlSize();
        }

        private void UpdateControlSize()
        {
            if (StudConnector == null)
                return;

            var cellSize = TextRenderer.MeasureText("15:4:96", Font);

            cellSize.Width += 12;
            cellSize.Height += 12;
            GridCellSize = cellSize;

            Width = (cellSize.Width * StudConnector.ArrayWidth) + 1;
            Height = (cellSize.Height * StudConnector.ArrayHeight) + 1;

        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (StudConnector != null)
            {
                width = (GridCellSize.Width * StudConnector.ArrayWidth) + 1;
                height = (GridCellSize.Height * StudConnector.ArrayHeight) + 1;
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        #region Drawing

        protected override void OnPaint(PaintEventArgs pe)
        {
            var g = pe.Graphics;
            g.Clear(Color.White);

            if (StudConnector == null)
                return;

            float studWidth = (Width - 1) / (float)StudConnector.StudWidth;
            float studHeight = (Height - 1) / (float)StudConnector.StudHeight;
            float cellWidth = (Width - 1) / (float)StudConnector.ArrayWidth;
            float cellHeight = (Height - 1) / (float)StudConnector.ArrayHeight;

            //for (int x = 0; x <= StudConnector.StudWidth; x++)
            //{
            //    g.DrawLine(Pens.Gray, studWidth * x, 0, studWidth * x,
            //        studHeight * StudConnector.StudHeight);
            //}

            //for (int y = 0; y <= StudConnector.StudHeight; y++)
            //{
            //    g.DrawLine(Pens.Gray, 0, studHeight * y,
            //        studWidth * StudConnector.StudWidth, studHeight * y);
            //}

            var evenColor = Color.FromArgb(80, 180, 180, 180);

            var columnRects = new List<RectangleF>();
            var rowRects = new List<RectangleF>();

            for (int x = 0; x <= StudConnector.ArrayWidth; x++)
            {
                var colRect = new RectangleF(cellWidth * x, 0, cellWidth, cellHeight * StudConnector.ArrayHeight);
                columnRects.Add(colRect);
            }

            for (int y = 0; y <= StudConnector.ArrayHeight; y++)
            {
                var rowRect = new RectangleF(0, cellHeight * y, cellWidth * StudConnector.ArrayWidth, cellHeight);
                rowRects.Add(rowRect);
            }

            void DrawBackground(List<RectangleF> rects)
            {
                for (int i = 0; i < rects.Count; i++)
                {
                    var colRowRect = rects[i];

                    if ((i % 2) == 0)
                    {
                        using (var brush = new SolidBrush(evenColor))
                            g.FillRectangle(brush, colRowRect);
                    }
                }
            }

            void DrawBorders(List<RectangleF> rects)
            {
                for (int i = 0; i < rects.Count; i++)
                {
                    var colRowRect = rects[i];
                    g.DrawRectangle(Pens.Black, colRowRect.X, colRowRect.Y, colRowRect.Width, colRowRect.Height);
                }
            }

            DrawBackground(columnRects);
            DrawBackground(rowRects);

            var selectionColor = Color.FromArgb(50, 180, 180, 200);
            if (ContainsFocus || Focused)
                selectionColor = Color.FromArgb(100, 180, 180, 255);

            var selectionBrush = new SolidBrush(selectionColor);

            for (int y = 0; y < StudConnector.ArrayHeight; y++)
            {
                for (int x = 0; x < StudConnector.ArrayWidth; x++)
                {
                    var cellRect = new RectangleF(
                        cellWidth * x, cellHeight * y,
                        cellWidth, cellHeight);

                    if (IsInSelection(x, y))
                        g.FillRectangle(selectionBrush, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }
            selectionBrush.Dispose();

            DrawBorders(columnRects);
            DrawBorders(rowRects);

            //for (int x = 0; x <= StudConnector.ArrayWidth; x++)
            //{
            //    var colRect = columnRects[x];

            //    if ((x % 2) == 0)
            //    {
            //        using (var brush = new SolidBrush(evenColor))
            //            g.FillRectangle(brush, colRect);
            //    }

            //    g.DrawLine(Pens.Black, cellWidth * x, 0, cellWidth * x,
            //        cellHeight * StudConnector.ArrayHeight);
            //}

            //for (int y = 0; y <= StudConnector.ArrayHeight; y++)
            //{
            //    var rowRect = rowRects[y];

            //    if ((y % 2) == 0)
            //    {
            //        using (var brush = new SolidBrush(evenColor))
            //            g.FillRectangle(brush, rowRect);
            //    }

            //    g.DrawLine(Pens.Black, 0, cellHeight * y,
            //        cellWidth * StudConnector.ArrayWidth, cellHeight * y);
            //}


            var sf = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            Pen focusedBorderPen = SystemPens.Highlight;

            //if (!(Focused || ContainsFocus))
            //{
            //    focusedBorderPen = SystemPens.
            //}

            for (int y = 0; y < StudConnector.ArrayHeight; y++)
            {
                for (int x = 0; x < StudConnector.ArrayWidth; x++)
                {
                    var cellRect = new RectangleF(
                        cellWidth * x, cellHeight * y,
                        cellWidth, cellHeight);

                    if (IsInSelection(x,y))
                    {

                        g.DrawRectangle(focusedBorderPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }

                    g.DrawString(StudConnector[x, y].ToString(), Font, Brushes.Black, cellRect, sf);
                }
            }

            sf.Dispose();
        }

        #endregion

        #region Selection

        private bool IsSelectingRange;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (EditCombo.Visible)
                    FinishEditNode();

                var clickedNode = GetNodeFromPosition(e.Location);
                if (clickedNode != null)
                {
                    IsSelectingRange = true;
                    FocusedCell = GetCellAddressFromPosition(e.Location);
                    SelectionStart = FocusedCell;
                    SelectionEnd = null;
                    Invalidate();
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && IsSelectingRange)
            {
                
                IsSelectingRange = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SelectionEnd = null;
                Invalidate();
                var curNode = GetNodeFromPosition(e.Location);
                if (curNode != null)
                {
                    BeginEditNode(curNode);
                }
            }
            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsSelectingRange)
            {
                var curCell = GetCellAddressFromPosition(e.Location);

                var end = SelectionEnd ?? SelectionStart;
                if (!end.Equals(curCell))
                {
                    SelectionEnd = curCell;
                    FocusedCell = curCell;
                    Invalidate();
                }
            }
            base.OnMouseMove(e);
        }

        public bool IsInSelection(int x, int y)
        {
            if (SelectionStart != null)
            {
                var end = SelectionEnd ?? SelectionStart;

                int minX = Math.Min(SelectionStart.Item1, end.Item1);
                int maxX = Math.Max(SelectionStart.Item1, end.Item1);
                int minY = Math.Min(SelectionStart.Item2, end.Item2);
                int maxY = Math.Max(SelectionStart.Item2, end.Item2);

                return x >= minX && x <= maxX && y >= minY && y <= maxY;
            }

            return false;
        }

        #endregion

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {

            }
            else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                string clipContent = Clipboard.GetText();
                if (!string.IsNullOrEmpty(clipContent))
                {
                    var lines = clipContent.Split('\r', '\n').ToList();
                    lines.RemoveAll(x => string.IsNullOrWhiteSpace(x?.Trim()));
                    

                    for (int i = 0; i < lines.Count; i++)
                    {
                        var rowValues = lines[i].Trim().Split(',', ';', '\t');
                    }
                }
            }

            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                if (!IsEditingNode && FocusedCell != null)
                {
                    var focusedNode = StudConnector.GetNode(FocusedCell.Item1, FocusedCell.Item2);
                    if (focusedNode != null)
                    {
                        if (BeginEditNode(focusedNode))
                        {
                            EditCombo.SelectAll();
                            EditCombo.SelectedText = ((char)e.KeyCode).ToString();
                        }
                    }
                }
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                if (!IsEditingNode && FocusedCell != null)
                {
                    var focusedNode = StudConnector.GetNode(FocusedCell.Item1, FocusedCell.Item2);
                    if (focusedNode != null)
                        BeginEditNode(focusedNode);
                }
            }

            if (e.KeyCode == Keys.Escape && IsEditingNode)
            {
                CancelEditNode();
            }

            base.OnKeyDown(e);
        }

        #region Edit ComboBox Handling

        private bool IsEditingNode;
        private bool NodeValueChanged;
        private bool LoadingCombobox;
        private bool IsCancelingEdit;

        private void InitEditCombo()
        {
            EditCombo = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                Visible = false,
                FlatStyle = FlatStyle.Flat,
                AutoSize = false
            };

            Controls.Add(EditCombo);

            var nodeTypes = new List<NodeTypeInfo>()
            {
                new NodeTypeInfo("0:4", 0),
                new NodeTypeInfo("1:4", 0),
                new NodeTypeInfo("2:4", 0),
                new NodeTypeInfo("3:4", 0),
                new NodeTypeInfo("4:4", 0),
                new NodeTypeInfo("5:4", 1),
                new NodeTypeInfo("7:4", 1),
                new NodeTypeInfo("8:4", 1),
                new NodeTypeInfo("9:4", 1),
                new NodeTypeInfo("10:4", 1),
                new NodeTypeInfo("11:4", 1),
                new NodeTypeInfo("12:4", 1),
                new NodeTypeInfo("13:4", 1),
                new NodeTypeInfo("14:4", 1),
                new NodeTypeInfo("15:4", 1),
                new NodeTypeInfo("16:4", 1),
                new NodeTypeInfo("17:4", 1),
                new NodeTypeInfo("18:1", 0),
                new NodeTypeInfo("18:2", 0),
                new NodeTypeInfo("18:3", 0),
                new NodeTypeInfo("18:4", 0),
                new NodeTypeInfo("19:4", 0),
                new NodeTypeInfo("21:4", 1),
                new NodeTypeInfo("22:1", 1),
                new NodeTypeInfo("22:2", 1),
                new NodeTypeInfo("22:3", 1),
                new NodeTypeInfo("23:4", 2),
                new NodeTypeInfo("24:4", 1),
                new NodeTypeInfo("25:4", 0),
                new NodeTypeInfo("26:4", 0),
                new NodeTypeInfo("27:4", 1),
                new NodeTypeInfo("28:4", 1),
                new NodeTypeInfo("29:0", 2),
            };

            EditCombo.DataSource = nodeTypes;
            EditCombo.DisplayMember = "ID";
            EditCombo.Validating += EditCombo_Validating;
            EditCombo.Validated += EditCombo_Validated;
            EditCombo.LostFocus += EditCombo_LostFocus;
            EditCombo.TextChanged += EditCombo_TextChanged;
            EditCombo.SelectedValueChanged += EditCombo_SelectedValueChanged;
            EditCombo.KeyDown += EditCombo_KeyDown;
        }

        private bool BeginEditNode(Custom2DFieldNode node)
        {
            if (IsEditingNode && !FinishEditNode())
                return false;

            SelectedNode = null;
            SelectionEnd = null;
            SelectionStart = new Tuple<int, int>(node.X, node.Y);
            FocusedCell = new Tuple<int, int>(node.X, node.Y);
            var rectangle = GetCellRect(node.X, node.Y);
            LoadingCombobox = true;
            EditCombo.Visible = true;
            
            EditCombo.Width = rectangle.Width - 1;
            EditCombo.Left = rectangle.X + 1;
            EditCombo.Top = rectangle.Y + (rectangle.Height - EditCombo.Height) / 2;

            EditCombo.SelectedIndex = -1;
            EditCombo.Text = node.ToString();
            EditCombo.Focus();
            EditCombo.SelectionStart = 0;
            EditCombo.SelectionLength = 0;

            LoadingCombobox = false;

            SelectedNode = node;
            IsEditingNode = true;
            Invalidate();

            return true;
        }

        private void CancelEditNode()
        {
            if (IsEditingNode || EditCombo.Visible)
            {
                IsCancelingEdit = true;
                Focus();
                EditCombo.Hide();
                IsCancelingEdit = false;
                IsEditingNode = false;
                SelectedNode = null;
                NodeValueChanged = false;
            }
        }

        private bool FinishEditNode()
        {
            if (IsEditingNode || EditCombo.Visible)
            {
                if (!EditCombo.Focused)
                    EditCombo.Focus();
                this.Focus();
                if (NodeValueChanged)
                    return false;
            }

            if (EditCombo.Visible)
            {
                EditCombo.Visible = false;
            }
            return true;
        }

        private void EditCombo_TextChanged(object sender, EventArgs e)
        {
            if (IsEditingNode && !LoadingCombobox)
            {
                NodeValueChanged = true;
            }
        }

        private void EditCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            if (IsEditingNode && !LoadingCombobox)
            {
                NodeValueChanged = true;
            }
        }

        private void EditCombo_Validating(object sender, CancelEventArgs e)
        {
            if (IsCancelingEdit)
                return;

            if (!Custom2DFieldConnector.TryParseNode(EditCombo.Text))
            {
                e.Cancel = true;
            }
        }

        private void EditCombo_Validated(object sender, EventArgs e)
        {
            if (IsEditingNode && SelectedNode != null && !IsCancelingEdit)
            {
                SelectedNode.Parse(EditCombo.Text);
                NodeValueChanged = false;
            }
        }

        private void EditCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && IsEditingNode)
            {
                CancelEditNode();
            }
        }

        private void EditCombo_LostFocus(object sender, EventArgs e)
        {
            if (!NodeValueChanged && IsEditingNode)
            {
                SelectedNode = null;
                EditCombo.Visible = false;
                IsEditingNode = false;
                NodeValueChanged = false;
            }
        }

        #endregion

        private Tuple<int, int> GetCellAddressFromPosition(Point point)
        {
            int x = point.X / GridCellSize.Width;
            int y = point.Y / GridCellSize.Height;
            return new Tuple<int, int>(x, y);
        }


        private Custom2DFieldNode GetNodeFromPosition(Point point)
        {
            int x = point.X / GridCellSize.Width;
            int y = point.Y / GridCellSize.Height;

            return StudConnector.GetNode(x, y);
        }

        private Rectangle GetCellRect(int x, int y)
        {
            return new Rectangle(
                        GridCellSize.Width * x, GridCellSize.Height * y,
                        GridCellSize.Width, GridCellSize.Height);
        }
    }
}
