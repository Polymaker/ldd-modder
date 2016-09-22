using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LDDModder.Display.Utilities
{
    [Designer(typeof(LocalizableStringsDesigner))]
    public partial class LocalizableStrings : Component
    {
        private List<StringEntry> _Entries;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<StringEntry> Entries
        {
            get { return _Entries; }
        }

        public LocalizableStrings()
        {
            _Entries = new List<StringEntry>();
            InitializeComponent();
        }

        public LocalizableStrings(IContainer container)
        {
            _Entries = new List<StringEntry>();
            container.Add(this);
            InitializeComponent();
        }
    }
}
