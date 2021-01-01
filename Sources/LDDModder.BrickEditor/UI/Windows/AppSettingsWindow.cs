using BrightIdeasSoftware;
using LDDModder.BrickEditor.Settings;
using LDDModder.BrickEditor.UI.Controls;
using LDDModder.BrickEditor.UI.Settings;
using LDDModder.LDD.Files;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class AppSettingsWindow : Form
    {
        private FlagManager FlagManager;
        private List<SettingsTabNode> SettingsTabPanels;
        private SettingsTabNode CurrentTab;

        public SettingTab StartupTab { get; set; }

        public enum SettingTab
        {
            LddEnvironment,
            EditorSettings,
            LayoutSettings
            //ProjectSettings
        }

        private class SettingsTabNode
        {
            public SettingTab Tab { get; set; }
            public string Text { get; set; }
            public SettingsPanelBase Panel { get; set; }

            public SettingsTabNode()
            {
            }

            public SettingsTabNode(SettingTab tab, string text)
            {
                Tab = tab;
                Text = text;
            }

            public SettingsTabNode(SettingTab tab, SettingsPanelBase panel)
            {
                Tab = tab;
                Panel = panel;
                Text = panel.Title;
            }
        }

        

        public AppSettingsWindow()
        {
            InitializeComponent();
            Icon = Properties.Resources.BrickStudioIcon;
            StartupTab = SettingTab.LddEnvironment;
            FlagManager = new FlagManager();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;
            
            InitializeSettingPanels();
            InitializeSettingsList();

            LoadSettings();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
              ShowSettingTab(StartupTab);
        }

        private void InitializeSettingsList()
        {
            SettingsTabPanels = new List<SettingsTabNode>()
            {
                new SettingsTabNode(SettingTab.LddEnvironment, lddSettingsPanel1),
                new SettingsTabNode(SettingTab.EditorSettings, editorSettingsPanel1),
                new SettingsTabNode(SettingTab.LayoutSettings, displaySettingsPanel1)
            };

            CategoryListView.Columns.Add(new OLVColumn()
            {
                FillsFreeSpace = true,
                AspectGetter = (item) => (item as SettingsTabNode).Text,
                Groupable = false
            });
            CategoryListView.SetObjects(SettingsTabPanels);
            CategoryListView.SelectedIndex = 0;


        }

        private void InitializeSettingPanels()
        {
            foreach (var panel in splitContainer1.Panel2.Controls.OfType<SettingsPanelBase>())
            {
                panel.Location = new Point(3, 3);
                //panel.Width = splitContainer1.Panel2.Width - 6;
                //panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                panel.Dock = DockStyle.Top;
                panel.AutoSizeMode = AutoSizeMode.GrowOnly;
                panel.AutoSize = true;
                panel.Visible = false;
            }
        }

        private bool CanChangeTab()
        {
            if (CurrentTab?.Panel != null)
            {
                if (CurrentTab.Panel.HasSettingsChanged)
                {
                    //TODO: ask confirmation
                    return false;
                }
            }

            return true;
        }

        public bool ShowSettingTab(SettingTab tab)
        {
            using (FlagManager.UseFlag(nameof(ShowSettingTab)))
            {
                if (CurrentTab != null)
                {
                    if (CurrentTab.Panel?.HasSettingsChanged ?? false)
                        return false;

                    if (CurrentTab.Panel != null)
                        CurrentTab.Panel.Visible = false;
                }

                CurrentTab = SettingsTabPanels.FirstOrDefault(x => x.Tab == tab);
                if (CurrentTab != null)
                {
                    if (CurrentTab.Panel != null)
                        CurrentTab.Panel.Visible = true;
                    SelectTabInList(CurrentTab);
                    return true;
                }
            }
            return false;
        }

        private void LoadSettings()
        {
            foreach (var panel in splitContainer1.Panel2.Controls.OfType<SettingsPanelBase>())
                panel.FillSettings(SettingsManager.Current);
        }


        #region Settings Category List

        private void SelectTabInList(SettingsTabNode tabNode)
        {
            using (FlagManager.UseFlag(nameof(SelectTabInList)))
            {
                CategoryListView.SelectedObject = tabNode;
            }
        }

        private void CategoryListView_SelectionChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet(nameof(SelectTabInList)))
                return;

            if (CategoryListView.SelectedObject is SettingsTabNode tabNode)
            {
                if (!ShowSettingTab(tabNode.Tab) && CurrentTab != null)
                    SelectTabInList(CurrentTab);
            }
            else if (CurrentTab != null)
                SelectTabInList(CurrentTab);
        }

        #endregion


        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (CurrentTab?.Panel != null)
            {
                if (CurrentTab.Panel.ValidateSettings())
                {
                    SettingsManager.LoadSettings();
                    CurrentTab.Panel.ApplySettings(SettingsManager.Current);
                    SettingsManager.SaveSettings();
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (CurrentTab?.Panel != null)
            {
                if (CurrentTab.Panel.HasSettingsChanged)
                {
                   //TODO: ask confirmation
                }
                DialogResult = DialogResult.OK;
            }
        }
    }
}
