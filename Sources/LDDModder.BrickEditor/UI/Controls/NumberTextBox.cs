using LDDModder.BrickEditor.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LDDModder.BrickEditor.UI.Controls
{
    [Designer(typeof(NumberTextBoxDesigner)), DefaultEvent("ValueChanged")]
    public partial class NumberTextBox : TextBox
    {
        #region Fields

        private bool _AllowDecimals;
        private double _Value;
        private double _MinimumValue;
        private double _MaximumValue;
        private bool updatingText;
        private int _MinDisplayedDecimalPlaces;
        private int _MaxDisplayedDecimalPlaces;
        private bool _AutoSize;

        #endregion

        #region Properties

        [DefaultValue(0d), Bindable(true)]
        public double Value
        {
            get { return _Value; }
            set
            {
                if (value != _Value)
                {
                    if (value < MinimumValue || value > MaximumValue)
                        throw new ArgumentOutOfRangeException("Value");

                    _Value = value;
                    if (IsHandleCreated)
                        UpdateTextboxValue();
                    OnValueChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(0d)]
        public double MinimumValue
        {
            get { return _MinimumValue; }
            set
            {
                if (value != _MinimumValue)
                {
                    if (value > _MaximumValue)
                        _MaximumValue = value;
                    _MinimumValue = value;
                    Value = ConstraintValue(_Value);
                }
            }
        }

        [DefaultValue(100d)]
        public double MaximumValue
        {
            get { return _MaximumValue; }
            set
            {
                if (value != _MaximumValue)
                {
                    if (_MinimumValue > value)
                        _MinimumValue = value;
                    _MaximumValue = value;
                    Value = ConstraintValue(_Value);
                }
            }
        }

        [DefaultValue(true)]
        public bool AllowDecimals
        {
            get { return _AllowDecimals; }
            set
            {
                if (value != _AllowDecimals)
                {
                    _AllowDecimals = value;
                    if (!_AllowDecimals)
                        Value = Math.Round(Value);
                }
            }
        }

        [DefaultValue(0)]
        public int MinDisplayedDecimalPlaces
        {
            get { return _MinDisplayedDecimalPlaces; }
            set
            {
                if (value != _MinDisplayedDecimalPlaces)
                {
                    _MinDisplayedDecimalPlaces = Math.Max(0, Math.Min(value, MaxDisplayedDecimalPlaces));
                    UpdateTextboxValue();
                }
            }
        }

        [DefaultValue(5)]
        public int MaxDisplayedDecimalPlaces
        {
            get { return _MaxDisplayedDecimalPlaces; }
            set
            {
                if (value != _MaxDisplayedDecimalPlaces)
                {
                    _MaxDisplayedDecimalPlaces = Math.Max(0, value);
                    UpdateTextboxValue();
                }
            }
        }

        public bool IsEditing { get; private set; }

        [Browsable(true), DefaultValue(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize { get => _AutoSize; set => base.AutoSize = _AutoSize = value; }

        #endregion

        #region Events

        public event EventHandler BeginEdit;
        public event EventHandler EndEdit;
        public event EventHandler ValueChanged;

        #endregion

        public NumberTextBox()
        {
            Width = 100;
            InitializeComponent();
            _MaximumValue = 100;
            
            _AllowDecimals = true;
            _AutoSize = true;
            _MaxDisplayedDecimalPlaces = 5;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateTextboxValue();
        }

        private void UpdateTextboxValue()
        {
            updatingText = true;
            string formatString = "0";
            if (MaxDisplayedDecimalPlaces > 0)
            {
                string decimalsFormat = string.Empty.PadRight(MinDisplayedDecimalPlaces, '0');
                decimalsFormat = decimalsFormat.PadRight(MaxDisplayedDecimalPlaces, '#');
                formatString += "." + decimalsFormat;
            }
            base.Text = Value.ToString(formatString);
            updatingText = false;
        }

        private double ConstraintValue(double value)
        {
            if (value < MinimumValue)
                value = MinimumValue;
            if (value > MaximumValue)
                value = MaximumValue;
            return value;
        }

        #region MyRegion

        public void PerformEndEdit()
        {
            if (IsEditing)
            {
                IsEditing = false;
                OnEndEdit();

                if (NumberHelper.SmartTryParse(base.Text, out double value))
                {
                    if (AllowDecimals || value % 1d == 0d)
                        Value = ConstraintValue(value);
                    //success
                }
                else
                {
                    //fail
                }
                UpdateTextboxValue();
            }
        }

        public void CancelEdit()
        {
            if (IsEditing)
            {
                IsEditing = false;
                UpdateTextboxValue();
                OnEndEdit();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!updatingText)
            {
                if (!IsEditing)
                {
                    OnBeginEdit();
                    IsEditing = true;
                }
            }
            base.OnTextChanged(e);
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            if (e.Cancel)
                CancelEdit();
            else
            {
                if (!updatingText && IsEditing)
                {
                    if (NumberHelper.SmartTryParse(base.Text, out double value))
                    {
                        if (!AllowDecimals && value % 1d != 0)
                        {
                            System.Media.SystemSounds.Exclamation.Play();
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        e.Cancel = true;
                    }
                }
            }
        }

        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);
            PerformEndEdit();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && IsEditing)
            {
                if (GetContainerControl() is ContainerControl)
                    (GetContainerControl() as ContainerControl).ValidateChildren();
                else
                    PerformEndEdit();
                return true;
            }
            else if (keyData == Keys.Escape && IsEditing)
            {
                CancelEdit();
                return true;
            }

            if (ShortcutsEnabled && keyData.HasFlag(Keys.Y) && keyData.HasFlag(Keys.Control) && CanUndo)
            {
                Undo();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        protected void OnBeginEdit()
        {
            BeginEdit?.Invoke(this, EventArgs.Empty);
        }

        protected void OnEndEdit()
        {
            EndEdit?.Invoke(this, EventArgs.Empty);
        }

        protected void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        protected bool ShouldSerializeAutoSize()
        {
            return !AutoSize;
        }
    }

    internal class NumberTextBoxDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                object component = base.Component;
                selectionRules |= SelectionRules.AllSizeable;
                var autosizeProperty = TypeDescriptor.GetProperties(component)["AutoSize"];
                if (autosizeProperty != null)
                {
                    object value2 = autosizeProperty.GetValue(component);
                    if (value2 is bool && (bool)value2)
                    {
                        selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
                    }
                }
                return selectionRules;
            }
        }

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);
            var hiddenProperties = new string[]
            {
                "Text", "AutoCompleteCustomSource", "AutoCompleteMode", "AutoCompleteSource",
                "Lines", "CharacterCasing", "Multiline", "PasswordChar", "WordWrap",
                "MaxLength", "ScrollBars", "UseSystemPasswordChar"
            };
            foreach (var propName in hiddenProperties)
            {
                if (properties.Contains(propName))
                    properties.Remove(propName);
            }
        }
    }
}
