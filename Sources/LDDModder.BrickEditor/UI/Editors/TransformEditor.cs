using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;
using System.Diagnostics;
using LDDModder.Simple3D;

namespace LDDModder.BrickEditor.UI.Controls
{
    [DefaultEvent("ValueChanged")]
    public partial class TransformEditor : UserControl, INotifyPropertyChanged
    {
        private ItemTransform _Value;
        private FlagManager FlagManager;
        private IPhysicalElement _PhysicalElement;
        //private int _MinLabelWidth;
        private int _TextBoxesWidth;

        private EditLayout _ViewLayout;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IPhysicalElement PhysicalElement
        {
            get => _PhysicalElement;
            set
            {
                if (value != _PhysicalElement)
                    BindPhysicalElement(value);
            }
        }

        [Browsable(false), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ItemTransform Value
        {
            get => _Value;
            set => SetCurrentValue(value);
        }

        [DefaultValue(EditLayout.Horizontal)]
        public EditLayout ViewLayout
        {
            get => _ViewLayout;
            set
            {
                if (_ViewLayout != value)
                {
                    _ViewLayout = value;
                    ApplyViewLayout();
                }
            }
        }

        [DefaultValue(70)]
        public int TextBoxesWidth
        {
            get => _TextBoxesWidth;
            set
            {
                value = Math.Max(20, value);
                if (_TextBoxesWidth != value)
                {
                    _TextBoxesWidth = value;
                    AdjustWidths();
                }
            }
        }

        public event EventHandler ValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public TransformEditor()
        {
            InitializeComponent();
            FlagManager = new FlagManager();
            _Value = new ItemTransform();
            _TextBoxesWidth = 70;
            _ViewLayout = EditLayout.Horizontal;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FillValues();
        }

        private void SetCurrentValue(ItemTransform value)
        {
            if (FlagManager.IsSet("OnValueChanged"))
                return;

            var notNullValue = value ?? new ItemTransform();

            if (_Value != notNullValue)
            {
                _Value = notNullValue;
                FillValues();
                OnValueChanged();
            }
        }

        public void BindPhysicalElement(IPhysicalElement value)
        {
            if (_PhysicalElement != null)
            {
                _PhysicalElement.TranformChanged -= PhysicalElement_TranformChanged;

                DataBindings.Clear();
                _PhysicalElement = null;
            }

            _PhysicalElement = value;

            if (value != null)
            {
                DataBindings.Add(new Binding("Value",
                    value, nameof(value.Transform),
                    false, DataSourceUpdateMode.OnPropertyChanged));
                _PhysicalElement.TranformChanged += PhysicalElement_TranformChanged;


            }
            else
                SetCurrentValue(null);
        }

        private void PhysicalElement_TranformChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet("OnValueChanged"))
                return;

            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(ReadCurrentValue));
            else
                ReadCurrentValue();
            
        }

        private void ReadCurrentValue()
        {
            foreach (var db in DataBindings.OfType<Binding>())
            {
                if (db.PropertyName == "Value")
                {
                    db.ReadValue();
                    break;
                }
            }
        }

        private void FillValues()
        {
            using (FlagManager.UseFlag("FillValues"))
            {
                PosX_TextBox.Value = Value.Position.X;
                PosY_TextBox.Value = Value.Position.Y;
                PosZ_TextBox.Value = Value.Position.Z;

                RotX_TextBox.Value = Value.Rotation.X;
                RotY_TextBox.Value = Value.Rotation.Y;
                RotZ_TextBox.Value = Value.Rotation.Z;
            }
        }

        private void SetValueFromControls()
        {
            var pos = new Vector3d()
            {
                X = PosX_TextBox.Value,
                Y = PosY_TextBox.Value,
                Z = PosZ_TextBox.Value
            };

            var rot = new Vector3d()
            {
                X = RotX_TextBox.Value,
                Y = RotY_TextBox.Value,
                Z = RotZ_TextBox.Value
            };

            _Value = new ItemTransform(pos, rot);

            OnValueChanged();
        }

        protected void OnValueChanged()
        {
            using (FlagManager.UseFlag("OnValueChanged"))
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        private void PositionValues_ValueChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("FillValues"))
                SetValueFromControls();
        }

        private void RotationValues_ValueChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("FillValues"))
                SetValueFromControls();
        }

        public enum EditLayout
        {
            Horizontal,
            Vertical
        }

        public void ApplyViewLayout()
        {

            SuspendLayout();

            LayoutGrid.RowStyles.Clear();
            LayoutGrid.ColumnStyles.Clear();
            LayoutGrid.Controls.Clear();

            void AddControl(Control ctrl, int x, int y, int colSpan = 0)
            {
                LayoutGrid.Controls.Add(ctrl);
                LayoutGrid.SetCellPosition(ctrl, new TableLayoutPanelCellPosition(x, y));
                if (colSpan > 0)
                    LayoutGrid.SetColumnSpan(ctrl, colSpan);
            }

            var positionLabels = new Control[] { PosX_Label, PosY_Label, PosZ_Label };
            var positionTextboxes = new Control[] { PosX_TextBox, PosY_TextBox, PosZ_TextBox };
            //var rotationLabels = new Control[] { RotX_Label, RotY_Label, RotZ_Label };
            var rotationTextboxes = new Control[] { RotX_TextBox, RotY_TextBox, RotZ_TextBox };

            if (ViewLayout == EditLayout.Vertical)
            {
                for (int i = 0; i < 4; i++)
                    LayoutGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                LayoutGrid.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                LayoutGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, TextBoxesWidth + 6));
                LayoutGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, TextBoxesWidth + 6));

                AddControl(PositionLabel, 1, 0);
                PositionLabel.Anchor = AnchorStyles.None;
                AddControl(RotationLabel, 2, 0);
                RotationLabel.Anchor = AnchorStyles.None;

                for (int i = 0; i < 3; i++)
                {
                    var ctrlMargin = new Padding(3, 3, 3, 3);

                    switch (i)
                    {
                        case 0: ctrlMargin.Bottom = 2; break;
                        case 1: ctrlMargin.Top = 1; ctrlMargin.Bottom = 1; break;
                        case 2: ctrlMargin.Top = 2; break;
                    }

                    AddControl(positionLabels[i], 0, 1 + i, 0);
                    positionLabels[i].Margin = ctrlMargin;

                    AddControl(positionTextboxes[i], 1, 1 + i, 0);
                    positionTextboxes[i].Margin = ctrlMargin;

                    AddControl(rotationTextboxes[i], 2, 1 + i, 0);
                    rotationTextboxes[i].Margin = ctrlMargin;
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                    LayoutGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                LayoutGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 83));
                for (int i = 0; i < 3; i++)
                    LayoutGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, TextBoxesWidth + 2));

                AddControl(PositionLabel, 0, 1);
                PositionLabel.Anchor = AnchorStyles.Left;
                AddControl(RotationLabel, 0, 2);
                RotationLabel.Anchor = AnchorStyles.Left;

                for (int i = 0; i < 3; i++)
                {
                    var ctrlMargin = new Padding(0, 3, 0, 3);

                    switch(i)
                    {
                        case 0: ctrlMargin.Right = 2; break;
                        case 1: ctrlMargin.Right = 1; ctrlMargin.Left = 1; break;
                        case 2: ctrlMargin.Left = 2; break;
                    }

                    AddControl(positionLabels[i], 1 + i, 0, 0);
                    positionLabels[i].Margin = ctrlMargin;

                    AddControl(positionTextboxes[i], 1 + i, 1, 0);
                    positionTextboxes[i].Margin = ctrlMargin;

                    AddControl(rotationTextboxes[i], 1 + i, 2, 0);
                    rotationTextboxes[i].Margin = ctrlMargin;
                }
            }

            ResumeLayout(true);
        }

        private void AdjustWidths()
        {
            if (ViewLayout == EditLayout.Horizontal)
            {
                for (int i = 0; i < 3; i++)
                    LayoutGrid.ColumnStyles[1 + i].Width = TextBoxesWidth + 2;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                    LayoutGrid.ColumnStyles[1 + i].Width = TextBoxesWidth + 6;
            }
        }

    }
}
