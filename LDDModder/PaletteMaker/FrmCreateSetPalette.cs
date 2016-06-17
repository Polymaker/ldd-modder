using LDDModder.LDD.Palettes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.PaletteMaker
{
    public partial class FrmCreateSetPalette : Form
    {
        private Dictionary<int, string> RBPartTypes;
        private BindingList<BrickMappingItem> PartList;

        public FrmCreateSetPalette()
        {
            InitializeComponent();
            RBPartTypes = new Dictionary<int, string>();
            PartList = new BindingList<BrickMappingItem>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FixSetDetailLayout();
            SendMessage(txtSearchSetID.Handle, 0x1501, 0, "Enter Set ID");
            pbxSetPicture.Select();
            Task.Factory.StartNew(() => LoadRBPartTypes());
            PaletteManager.LoadPalettes();
            FillUserPaletteList();
        }

        private void LoadRBPartTypes()
        {
            var partTypes = Rebrickable.RebrickableAPI.GetPartTypes.Execute();
            foreach (var partTypeElem in partTypes.PartTypes)
            {
                RBPartTypes.Add(partTypeElem.TypeID, partTypeElem.Description);
            }
        }

        private void FixSetDetailLayout()
        {
            listView1.Dock = DockStyle.Top;
            listView1.Height = 50;
            foreach (var txtBox in tlpSetDetails.Controls.OfType<TextBox>())
                txtBox.BorderStyle = BorderStyle.None;

            int maxH = Math.Max(lblSetName.Height + lblSetName.Margin.Vertical, txtSetID.Height + txtSetID.Margin.Vertical);

            for (int i = 0; i < tlpSetDetails.RowCount; i++)
            {
                tlpSetDetails.RowStyles[i].SizeType = SizeType.Absolute;
                tlpSetDetails.RowStyles[i].Height = maxH;
            }
            
            pbxSetPicture.Height = tlpSetDetails.Bottom - pbxSetPicture.Top;
            groupBox1.Height = pbxSetPicture.Top + pbxSetPicture.Height + 6;

            listView1.Dock = DockStyle.Fill;

        }

        private void FillSetDetails(Rebrickable.SetInfo setInfo)
        {
            if (setInfo == null)
            {
                txtSetID.Text = "NO SET FOUND";
                txtSetName.Text = txtSetTheme.Text = txtSetYear.Text = txtSetPieces.Text = string.Empty;
                pbxSetPicture.Image = null;
            }
            else
            {
                txtSetID.Text = setInfo.SetId;
                txtSetName.Text = setInfo.Description;
                txtSetTheme.Text = setInfo.Theme;
                txtSetYear.Text = setInfo.Year;
                txtSetPieces.Text = setInfo.Pieces;
                pbxSetPicture.ImageLocation = setInfo.ImageUrlSmall;
            }
        }

        private void FillPartGrid(Rebrickable.SetParts partsInfo)
        {
            if (partsInfo == null)
            {
                dataGridView1.DataSource = null;
            }
            else
            {
                PartList.Clear();
                foreach (var setPart in partsInfo.Parts)
                {
                    PartList.Add(new BrickMappingItem()
                    {
                        PartID = setPart.PartId,
                        PartType = RBPartTypes[setPart.PartTypeId],
                        Name = setPart.Name,
                        Color = setPart.ColorName,
                        ElementID = setPart.ElementId,
                        Quantity = setPart.Quantity,
                        LDD = string.Empty
                    });
                }

                dataGridView1.DataSource = PartList;
            }
        }


        private void FillUserPaletteList()
        {
            listView1.Items.Clear();
            foreach (var palette in PaletteManager.Palettes.Where(p => p.Type == PaletteType.User))
            {
                listView1.Items.Add(palette.Info.Name);
            }
        }

        private void btnSearchSet_Click(object sender, EventArgs e)
        {
            FindSet(txtSearchSetID.Text);
        }

        private void FindSet(string setNumber)
        {
            setNumber = setNumber.Trim();
            if (!setNumber.Contains('-'))
                setNumber += "-1";

            FillPartGrid(null);
            var setInfo = Rebrickable.RebrickableAPI.GetSet.Execute(setNumber);

            FillSetDetails(setInfo);

            if (setInfo != null)
            {
                Task.Factory.StartNew(() =>
                {
                    var partsInfo = Rebrickable.RebrickableAPI.GetSetParts.Execute(setNumber);
                    BeginInvoke(new Action<Rebrickable.SetParts>(FillPartGrid), partsInfo);
                });
            }

        }

        class BrickMappingItem
        {
            public string PartID { get; set; }
            public string PartType { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }
            public string ElementID { get; set; }
            public int Quantity { get; set; }
            public string LDD { get; set; }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg,
            int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        private void txtSearchSetID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearchSet.PerformClick();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = dataGridView1.Columns[e.ColumnIndex];

            var strSortOrder = GetSortOrder(e.ColumnIndex);
            var tmpList = PartList.ToList();

            tmpList.Sort(new DynamicComparer<BrickMappingItem>(newColumn.DataPropertyName, strSortOrder));

            PartList.Clear();
            tmpList.ForEach(i => PartList.Add(i));

            dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = strSortOrder;
        }

        private SortOrder GetSortOrder(int columnIndex)
        {
            if (dataGridView1.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ||
                dataGridView1.Columns[columnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending)
            {
                dataGridView1.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                return SortOrder.Ascending;
            }
            else
            {
                dataGridView1.Columns[columnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                return SortOrder.Descending;
            }
        }

        class DynamicComparer<T> : IComparer<T>
        {
            private string memberName = string.Empty; // specifies the member name to be sorted
            private SortOrder sortOrder = SortOrder.None; // Specifies the SortOrder.
            private MemberInfo memberInfo;
            private Type memberValueType;

            public DynamicComparer(string memberName, SortOrder sortOrder)
            {
                this.memberName = memberName;
                this.sortOrder = sortOrder;
                memberInfo = typeof(T).GetMember(memberName)[0];

                if (memberInfo is PropertyInfo)
                    memberValueType = (memberInfo as PropertyInfo).PropertyType;
                else if (memberInfo is FieldInfo)
                    memberValueType = (memberInfo as FieldInfo).FieldType;
            }

            public int Compare(T x, T y)
            {
                if (memberValueType == typeof(string))
                {
                    if(sortOrder == SortOrder.Ascending)
                        return string.Compare(GetValue(x) as string, GetValue(y) as string);
                    else
                        return string.Compare(GetValue(y) as string, GetValue(x) as string);
                }
                else if (memberValueType == typeof(int))
                {
                    if (sortOrder == SortOrder.Ascending)
                        return ((int)GetValue(x)).CompareTo((int)GetValue(y));
                    else
                        return ((int)GetValue(y)).CompareTo((int)GetValue(x));
                }
                return 0;
            }

            private object GetValue(T obj)
            {
                if (memberInfo is PropertyInfo)
                    return (memberInfo as PropertyInfo).GetValue(obj, null);
                else if (memberInfo is FieldInfo)
                    return (memberInfo as FieldInfo).GetValue(obj);
                return null;
            }
        }
    }
}
