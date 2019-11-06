using System;
using System.ComponentModel;

namespace LDDModder.BrickEditor.Localization
{
    [ToolboxItem(false), DesignTimeVisible(false), Serializable]
    public partial class LocalizableString : Component
    {
        [Localizable(true), Category("Design")]
        public string Text { get; set; }

        public LocalizableString()
        {
            InitializeComponent();
        }

        public LocalizableString(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public static implicit operator string(LocalizableString message)
        {
            return message.Text;
        }
    }
}
