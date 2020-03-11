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

        private Custom2DFieldConnector.FieldNode SelectedNode;

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

            for (int x = 0; x <= StudConnector.ArrayWidth; x++)
            {
                var colRect = new RectangleF(cellWidth * x, 0, cellWidth, cellHeight * StudConnector.ArrayHeight);
                
                if ((x % 2) == 0)
                {
                    using (var brush = new SolidBrush(evenColor))
                        g.FillRectangle(brush, colRect);
                }

                g.DrawLine(Pens.Black, cellWidth * x, 0, cellWidth * x, 
                    cellHeight * StudConnector.ArrayHeight);
            }

            for (int y = 0; y <= StudConnector.ArrayHeight; y++)
            {
                var rowRect = new RectangleF(0, cellHeight * y, cellWidth * StudConnector.ArrayWidth, cellHeight);

                if ((y % 2) == 0)
                {
                    using (var brush = new SolidBrush(evenColor))
                        g.FillRectangle(brush, rowRect);
                }

                g.DrawLine(Pens.Black, 0, cellHeight * y, 
                    cellWidth * StudConnector.ArrayWidth, cellHeight * y);
            }

            var sf = new StringFormat() 
            { 
                LineAlignment = StringAlignment.Center, 
                Alignment = StringAlignment.Center 
            };

            for (int y = 0; y < StudConnector.ArrayHeight; y++)
            {
                for (int x = 0; x < StudConnector.ArrayWidth; x++)
                {
                    var cellRect = new RectangleF(
                        cellWidth * x, cellHeight * y, 
                        cellWidth, cellHeight);

                    g.DrawString(StudConnector[x, y].ToString(), Font, Brushes.Black, cellRect, sf);
                }
            }

            sf.Dispose();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                var clickedNode = GetNodeFromPosition(e.Location);
                if (clickedNode != null)
                    BeginEditNode(clickedNode);
            }
        }




        #region Edit ComboBox Handling

        private bool IsEditingNode;
        private bool NodeValueChanged;
        private bool LoadingCombobox;

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
        }

        

        private void BeginEditNode(Custom2DFieldConnector.FieldNode node)
        {
            
            if (IsEditingNode)
            {
                this.Focus();
                if (NodeValueChanged)
                    return;
            }

            SelectedNode = null;
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
            if (!Custom2DFieldConnector.TryParseNode(EditCombo.Text))
            {
                e.Cancel = true;
            }
        }

        private void EditCombo_Validated(object sender, EventArgs e)
        {
            if (IsEditingNode && SelectedNode != null)
            {
                SelectedNode.Parse(EditCombo.Text);
                NodeValueChanged = false;
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


        private Custom2DFieldConnector.FieldNode GetNodeFromPosition(Point point)
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
