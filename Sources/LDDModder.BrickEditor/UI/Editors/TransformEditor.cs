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

        [Browsable(false), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ItemTransform Value
        {
            get => _Value;
            set => SetCurrentValue(value);
        }

        public event EventHandler ValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public TransformEditor()
        {
            InitializeComponent();
            FlagManager = new FlagManager();
            _Value = new ItemTransform();
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
    }
}
