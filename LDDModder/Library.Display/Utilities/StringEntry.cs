using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LDDModder.Display.Utilities
{
    [ToolboxItem(false), DesignTimeVisible(false), Serializable]
    public partial class StringEntry : Component
    {
        [Localizable(true)]
        public string Text { get; set; }

        public StringEntry()
        {
            InitializeComponent();
        }

        public StringEntry(IContainer container)
        {
            container.Add(this);
            
            InitializeComponent();
            
        }

        public static implicit operator string(StringEntry message)
        {
            return message.Text;
        }
    }
}
