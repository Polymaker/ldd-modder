using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;

namespace LDDModder.Display.Utilities
{
    internal class LocalizableStringsDesigner : ComponentDesigner
    {
        private DesignerActionListCollection _ActionList;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_ActionList == null)
                {
                    _ActionList = new DesignerActionListCollection();
                    _ActionList.Add(new LocalizableStringsActions(this));
                }
                return _ActionList;
            }
        }
    }

    internal class LocalizableStringsActions : DesignerActionList
    {
        private ComponentDesigner _Designer;

        public LocalizableStringsActions(ComponentDesigner designer)
            : base(designer.Component)
        {
            _Designer = designer;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            return new DesignerActionItemCollection
            {
                new DesignerActionMethodItem(this, "InvokeItemsDialog", "Edit strings...", "Properties", "Edit localizable strings.", true),
            };
        }

        public void InvokeItemsDialog()
        {
            ShowCollectionEditor(_Designer, base.Component, "Entries");
        }

        private static MethodInfo EditValueMI;

        private static void ShowCollectionEditor(ComponentDesigner designer, object objectToChange, string propName)
        {
            if (EditValueMI == null)
            {
                var systemDesignAssem = Assembly.GetAssembly(typeof(ComponentDesigner));
                var editorServiceType = systemDesignAssem.GetType("System.Windows.Forms.Design.EditorServiceContext");
                EditValueMI = editorServiceType.GetMethod("EditValue", BindingFlags.Static | BindingFlags.Public);
            }
            if (EditValueMI != null)
            {
                EditValueMI.Invoke(null, new object[] { designer, objectToChange, propName });
            }
        }
    }
}
