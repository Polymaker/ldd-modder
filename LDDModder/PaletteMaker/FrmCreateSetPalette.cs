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
        private BindingList<BrickMappingItem> PartList;
        private List<PaletteItem> PaletteItems;
        public static Dictionary<int, Rebrickable.Models.Theme> Themes;
        private static Dictionary<int, string> PartCategories;
        private Rebrickable.Models.Set CurrentSetInfo;

        public FrmCreateSetPalette()
        {
            InitializeComponent();
            PartList = new BindingList<BrickMappingItem>();
            PaletteItems = new List<PaletteItem>();
            Themes = new Dictionary<int, Rebrickable.Models.Theme>();
            PartCategories = new Dictionary<int, string>();
            dataGridView1.AutoGenerateColumns = false;
            CurrentSetInfo = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FixSetDetailLayout();
            SendMessage(txtSearchSetID.Handle, 0x1501, 0, "Enter Set ID or name");
            pbxSetPicture.Select();
            Task.Factory.StartNew(() => LoadGeneralData());
            PaletteManager.LoadPalettes();
            FillUserPaletteList();
        }

        private void LoadGeneralData()
        {
            var partTypes = RebrickableAPIv3.PartCategories.GetCategories();
            foreach (var partTypeElem in partTypes.Results)
                PartCategories.Add(partTypeElem.ID, partTypeElem.Name);

            var setThemes = RebrickableAPIv3.Themes.GetAllThemes();
            foreach (var theme in setThemes.Results)
                Themes.Add(theme.Id, theme);
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

        private void FillSetDetails(Rebrickable.Models.Set setInfo)
        {
            if (setInfo == null)
            {
                ClearSetDetails();
            }
            else
            {
                var setTheme = Themes.ContainsKey(setInfo.ThemeID) ? Themes[setInfo.ThemeID] : null;
                var parentTheme = setTheme != null && setTheme.ParentId.HasValue && Themes.ContainsKey(setTheme.ParentId.Value) ? Themes[setTheme.ParentId.Value] : null;
                var themeName = setTheme?.Name ?? "Unknown";
                if (parentTheme != null)
                    themeName = parentTheme.Name + " - " + themeName;

                txtSetID.Text = setInfo.SetNumber;
                txtSetName.Text = setInfo.Name;
                txtSetTheme.Text = themeName;
                txtSetYear.Text = setInfo.Year.ToString();
                txtSetPieces.Text = setInfo.PartCount.ToString();
                pbxSetPicture.ImageLocation = setInfo.SetImageUrl;
                CurrentSetInfo = setInfo;
            }
        }
        private void FillPartGrid(Rebrickable.Models.ListResult<Rebrickable.Models.SetPart> partsInfo)
        {
            if (partsInfo == null)
            {
                dataGridView1.DataSource = null;
            }
            else
            {
                PartList.Clear();
                PaletteItems.Clear();
                foreach (var setPart in partsInfo.Results)
                {
                    PartList.Add(new BrickMappingItem(setPart));
                }

                dataGridView1.DataSource = PartList;
                Task.Factory.StartNew(() => MatchLddParts());
            }
        }
        //private void FillPartGrid(GetSetPartsResult partsInfo)
        //{
        //    if (partsInfo == null)
        //    {
        //        dataGridView1.DataSource = null;
        //    }
        //    else
        //    {
        //        PartList.Clear();
        //        PaletteItems.Clear();
        //        foreach (var setPart in partsInfo.Parts)
        //        {
        //            PartList.Add(new BrickMappingItem(setPart));
        //        }

        //        dataGridView1.DataSource = PartList;
        //        Task.Factory.StartNew(() => MatchLddParts());
        //    }
        //}

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
            var setInfo = RebrickableAPIv3.Sets.GetSet(setNumber);

            FillSetDetails(setInfo);

            if (setInfo != null)
            {
                Task.Factory.StartNew(() =>
                {
                    var partsInfo = RebrickableAPIv3.Sets.GetAllSetParts(setNumber);
                    BeginInvoke(new Action<Rebrickable.Models.ListResult<Rebrickable.Models.SetPart>>(FillPartGrid), partsInfo);
                });
            }

        }

        private void SearchSet(string query)
        {
            var result = Rebrickable.RebrickableAPIv3.Sets.GetSets(search: query);
            if (result.Results.Count > 0)
            {
                FillSetDetails(result.Results[0]);
            }
        }

        class BrickMappingItem
        {
            public Rebrickable.Models.SetPart RBPart { get; set; }
            public int PartID { get { return RBPart.Id; } }
            public string PartType { get; set; }
            public string Name { get { return RBPart.Part.Name; } }
            public string Color { get { return RBPart.Color.Name; } }
            public string ElementID { get { return RBPart.ElementId.ToString(); } }
            public int Quantity { get { return RBPart.Quantity; } }
            public string LDD { get; set; }

            //public BrickMappingItem(GetSetPartsResult.Part rBPart)
            //{
            //    RBPart = rBPart;
            //    PartType = PartCategories[rBPart.PartTypeId];
            //    LDD = String.Empty;
            //}

            public BrickMappingItem(Rebrickable.Models.SetPart rBPart)
            {
                RBPart = rBPart;
                PartType = PartCategories[rBPart.Part.PartCatId];
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

            var shortSetNumber = CurrentSetInfo.SetNumber.Substring(0, CurrentSetInfo.SetNumber.IndexOf('-'));
            var paletteInfo = new Bag(shortSetNumber + " " + CurrentSetInfo.Name, true);

            var setPalette = new Palette();
            setPalette.Items.AddRange(PaletteItems);
            PaletteManager.SavePalette(new PaletteFile(paletteInfo, setPalette));
        }
    }
}
