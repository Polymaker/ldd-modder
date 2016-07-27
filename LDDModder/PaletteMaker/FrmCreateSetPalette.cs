using LDDModder.LDD.Palettes;
using LDDModder.Rebrickable;
using LDDModder.Rebrickable.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.PaletteMaker
{
    public partial class FrmCreateSetPalette : Form
    {
        private static Dictionary<int, string> RBPartTypes;
        private BindingList<BrickMappingItem> PartList;
        private List<PaletteItem> PaletteItems;
        private GetSetResult CurrentSetInfo;

        public FrmCreateSetPalette()
        {
            InitializeComponent();
            RBPartTypes = new Dictionary<int, string>();
            PartList = new BindingList<BrickMappingItem>();
            PaletteItems = new List<PaletteItem>();
            dataGridView1.AutoGenerateColumns = false;
            CurrentSetInfo = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FixSetDetailLayout();
            SendMessage(txtSearchSetID.Handle, 0x1501, 0, "Enter Set ID or name");
            pbxSetPicture.Select();
            Task.Factory.StartNew(() => LoadRBPartTypes());
            PaletteManager.LoadPalettes();
            FillUserPaletteList();
        }

        private void LoadRBPartTypes()
        {
            var partTypes = RebrickableAPI.GetPartTypes.Execute();
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

        private void ClearSetDetails()
        {
            txtSetID.Text = "NO SET FOUND";
            txtSetName.Text = txtSetTheme.Text = txtSetYear.Text = txtSetPieces.Text = string.Empty;
            pbxSetPicture.Image = null;
            CurrentSetInfo = null;
        }

        private void FillSetDetails(GetSetResult setInfo)
        {
            if (setInfo == null)
            {
                ClearSetDetails();
            }
            else
            {
                txtSetID.Text = setInfo.SetId;
                txtSetName.Text = setInfo.Description;
                txtSetTheme.Text = setInfo.Theme;
                txtSetYear.Text = setInfo.Year;
                txtSetPieces.Text = setInfo.Pieces.ToString();
                pbxSetPicture.ImageLocation = setInfo.ImageUrlSmall;
                CurrentSetInfo = setInfo;
            }
        }

        private void FillSetDetails(SearchResult.Set setInfo)
        {
            if (setInfo == null)
            {
                ClearSetDetails();
            }
            else
            {
                txtSetID.Text = setInfo.SetId;
                txtSetName.Text = setInfo.Description;
                txtSetTheme.Text = setInfo.Theme1;
                txtSetYear.Text = setInfo.Year;
                txtSetPieces.Text = setInfo.Pieces.ToString();
                pbxSetPicture.ImageLocation = setInfo.ImageUrlSmall;
                CurrentSetInfo = new GetSetResult()
                {
                    SetId = setInfo.SetId,
                    Description = setInfo.Description,
                    Theme = setInfo.Theme1,
                    Year = setInfo.Year,
                    Pieces = setInfo.Pieces,
                    ImageUrlSmall = setInfo.ImageUrlSmall,
                };
            }
        }

        private void FillPartGrid(GetSetPartsResult partsInfo)
        {
            if (partsInfo == null)
            {
                dataGridView1.DataSource = null;
            }
            else
            {
                PartList.Clear();
                PaletteItems.Clear();
                foreach (var setPart in partsInfo.Parts)
                {
                    PartList.Add(new BrickMappingItem(setPart));
                }

                dataGridView1.DataSource = PartList;
                Task.Factory.StartNew(() => MatchLddParts());
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

        private void MatchLddParts()
        {
            PaletteItems.Clear();
            foreach (var part in PartList)
            {
                var lddPart = PaletteBuilder.GetPaletteItem(part.RBPart);
                
                if (lddPart != null)
                {
                    PaletteItems.Add(lddPart);
                    part.LDD = lddPart.DesignID.ToString();
                }
            }
            BeginInvoke((Action)(() => dataGridView1.Update()));
        }

        private void btnSearchSet_Click(object sender, EventArgs e)
        {
            if (Regex.IsMatch(txtSearchSetID.Text, "\\d+(\\-\\d+)?"))
                FindSet(txtSearchSetID.Text);
            else if (!string.IsNullOrEmpty(txtSearchSetID.Text))
                SearchSet(txtSearchSetID.Text);
            else
                ClearSetDetails();
        }

        private void FindSet(string setNumber)
        {
            setNumber = setNumber.Trim();
            if (!setNumber.Contains('-'))
                setNumber += "-1";

            FillPartGrid(null);
            var setInfo = RebrickableAPI.GetSet.Execute(setNumber);

            FillSetDetails(setInfo);

            if (setInfo != null)
            {
                Task.Factory.StartNew(() =>
                {
                    var partsInfo = RebrickableAPI.GetSetParts.Execute(setNumber);
                    BeginInvoke(new Action<GetSetPartsResult>(FillPartGrid), partsInfo);
                });
            }

        }

        private void SearchSet(string query)
        {
            var result = Rebrickable.RebrickableAPI.Search.Execute(new SearchParameters(SearchType.Set, query));
            if (result.SetsAndMOCs.Count > 0)
            {
                FillSetDetails(result.SetsAndMOCs[0]);
            }
        }

        class BrickMappingItem
        {
            public GetSetPartsResult.Part RBPart { get; set; }
            public string PartID { get { return RBPart.PartId; } }
            public string PartType { get; set; }
            public string Name { get { return RBPart.Name; } }
            public string Color { get { return RBPart.ColorName; } }
            public string ElementID { get { return RBPart.ElementId; } }
            public int Quantity { get { return RBPart.Quantity; } }
            public string LDD { get; set; }

            public BrickMappingItem(GetSetPartsResult.Part rBPart)
            {
                RBPart = rBPart;
                PartType = RBPartTypes[rBPart.PartTypeId];
                LDD = String.Empty;
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (PaletteItems.Count == 0)
                return;

            var shortSetNumber = CurrentSetInfo.SetId.Substring(0, CurrentSetInfo.SetId.IndexOf('-'));
            var paletteInfo = new Bag(shortSetNumber + " " + CurrentSetInfo.Description, true);

            var setPalette = new Palette();
            setPalette.Items.AddRange(PaletteItems);
            PaletteManager.SavePalette(new PaletteFile(paletteInfo, setPalette));
        }
    }
}
