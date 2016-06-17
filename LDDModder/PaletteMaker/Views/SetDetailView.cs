using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;

namespace LDDModder.PaletteMaker.Views
{
    [Designer(typeof(SetDetailViewDesigner))]
    public partial class SetDetailView : UserControl
    {
        private bool _ShowGridBorders = true;

        [DefaultValue(true)]
        public bool ShowGridBorders
        {
            get { return _ShowGridBorders; }
            set
            {
                if (_ShowGridBorders == value)
                    return;
                _ShowGridBorders = value;
                if (tlpSetDetails.IsHandleCreated)
                {
                    tlpSetDetails.CellBorderStyle = !value ? TableLayoutPanelCellBorderStyle.None : TableLayoutPanelCellBorderStyle.Single;
                    AdjustLayout();
                }
            }
        }

        public SetDetailView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tlpSetDetails.CellBorderStyle = !ShowGridBorders ? TableLayoutPanelCellBorderStyle.None : TableLayoutPanelCellBorderStyle.Single;
            AdjustLayout();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            AdjustLayout();
        }

        private void AdjustLayout()
        {
            foreach (var txtBox in tlpSetDetails.Controls.OfType<TextBox>())
                txtBox.BorderStyle = BorderStyle.None;

            int maxH = Math.Max(lblSetName.Height + lblSetName.Margin.Vertical, txtSetID.Height + txtSetID.Margin.Vertical);

            for (int i = 0; i < tlpSetDetails.RowCount; i++)
            {
                tlpSetDetails.RowStyles[i].SizeType = SizeType.Absolute;
                tlpSetDetails.RowStyles[i].Height = maxH;
            }
            pbxSetPicture.Left = tlpSetDetails.Right + 3;
            pbxSetPicture.Top = tlpSetDetails.Top;
            pbxSetPicture.Size = new Size(tlpSetDetails.Height, tlpSetDetails.Height);
            SetBounds(0, 0, 1, 1, BoundsSpecified.Size);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            height = tlpSetDetails.Height + tlpSetDetails.Margin.Vertical;
            width = pbxSetPicture.Right + 3;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        public void FillSetDetails(Rebrickable.SetInfo setInfo)
        {
            if (setInfo == null)
            {
                txtSetID.Text = txtSetName.Text = txtSetTheme.Text = txtSetYear.Text = txtSetPieces.Text = string.Empty;
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
    }

    internal class SetDetailViewDesigner : ControlDesigner
    {

        public override SelectionRules SelectionRules
        {
            get
            {
                return base.SelectionRules 
                    ^ SelectionRules.AllSizeable;
            }
        }

        private string[] propertiesToHide = {"AutoSize", "AutoSizeMode" };

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            foreach (string propname in propertiesToHide)
            {
                var prop = (PropertyDescriptor)properties[propname];
                if (prop != null)
                {
                    AttributeCollection runtimeAttributes =
                                               prop.Attributes;

                    Attribute[] attrs =
                       new Attribute[runtimeAttributes.Count + 1];
                    runtimeAttributes.CopyTo(attrs, 0);
                    attrs[runtimeAttributes.Count] =
                                    new BrowsableAttribute(false);
                    prop =
                     TypeDescriptor.CreateProperty(this.GetType(),
                                 propname, prop.PropertyType, attrs);
                    properties[propname] = prop;
                }
            }
        }
    }
}
