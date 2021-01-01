using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class AboutWindow : Form
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var myAssem = Assembly.GetExecutingAssembly();
            var copyrightAttr = myAssem.GetCustomAttribute<AssemblyCopyrightAttribute>();
            CopyrightLabel.Text = copyrightAttr.Copyright;
            AppVersionLabel.Text = $"v{Application.ProductVersion}";

            FillExternalLibraries();
        }

        class LibInfo
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Version { get; set; }
            public string Copyright { get; set; }
            public string License { get; set; }
            public string Websiste { get; set; }
        }

        private LibInfo GetLibInfo(Assembly assembly, string license = null, string website = null)
        {
            string Coalesce(string str1, string str2)
            {
                if (string.IsNullOrEmpty(str1))
                    return str2;
                return str1;
            }

            var titleAttr = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            var descAttr = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            var copyrightAttr = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            //var versionAttr = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
            var assemName = assembly.GetName();

            var info = new LibInfo()
            {
                Name = Path.GetFileName(assembly.Location),
                Title = Coalesce(titleAttr?.Title, assemName.Name),
                Copyright = copyrightAttr?.Copyright,
                Description = descAttr?.Description,
                Version = assemName.Version.ToString(),
                License = license,
                Websiste = website
            };

            if (!string.IsNullOrEmpty(info.Copyright) &&
                !info.Copyright.ToLower().Contains("copyright"))
            {
                info.Copyright = "Copyright © " + info.Copyright.Replace("©", string.Empty).Replace("  ", " ");
            }

            return info;
        }

        private void FillExternalLibraries()
        {
            var libraries = new List<LibInfo>();

            libraries.Add(GetLibInfo(typeof(Newtonsoft.Json.JsonConverter).Assembly,
                "MIT", "https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md"));

            libraries.Add(GetLibInfo(typeof(WeifenLuo.WinFormsUI.Docking.DockPanel).Assembly,
                "MIT", "https://github.com/dockpanelsuite/dockpanelsuite/blob/master/license.txt"));

            libraries.Add(GetLibInfo(typeof(ICSharpCode.SharpZipLib.Zip.ZipFile).Assembly,
                "MIT", "https://github.com/icsharpcode/SharpZipLib/blob/master/LICENSE.txt"));

            libraries.Add(GetLibInfo(typeof(OpenTK.Vector3).Assembly,
                "MIT", "https://github.com/opentk/opentk/blob/master/License.txt"));

            libraries.Add(GetLibInfo(typeof(BrightIdeasSoftware.ObjectListView).Assembly,
                "GNU GPL", "https://sourceforge.net/p/objectlistview/code/HEAD/tree/cs/trunk/COPYING"));

            libraries.Add(GetLibInfo(typeof(Assimp.Vector3D).Assembly,
                "MIT", "https://bitbucket.org/Starnick/assimpnet/src/master/License.txt"));

            libraries.Add(GetLibInfo(typeof(QuickFont.QFont).Assembly,
               "MIT", "https://github.com/opcon/QuickFont/blob/master/License.txt"));

            libraries.Add(GetLibInfo(typeof(ObjectTK.GLObject).Assembly,
               "MIT", "https://github.com/Polymaker/ObjectTK/blob/master/LICENSE"));

            foreach (var libInfo in libraries)
            {
                ExternalInfoTextbox.AppendText($"{libInfo.Title} version {libInfo.Version}\r\n");

                if (!string.IsNullOrEmpty(libInfo.Copyright))
                    ExternalInfoTextbox.AppendText($"{libInfo.Copyright}\r\n");

                if (!string.IsNullOrEmpty(libInfo.License))
                {
                    ExternalInfoTextbox.AppendText($"Licensed under ");

                    var licenseLink = new LinkLabel()
                    {
                        Text = libInfo.License,
                        AutoSize = true
                    };
                    licenseLink.Links.Add(new LinkLabel.Link() { LinkData = libInfo.Websiste });

                    licenseLink.Location = ExternalInfoTextbox.GetPositionFromCharIndex(ExternalInfoTextbox.TextLength);
                    licenseLink.Tag = ExternalInfoTextbox.TextLength;
                    licenseLink.LinkClicked += LicenseLink_LinkClicked;

                    ExternalInfoTextbox.Controls.Add(licenseLink);
                    ExternalInfoTextbox.AppendText("\r\n");
                }

                ExternalInfoTextbox.AppendText("\r\n");
            }
        }

        #region License Box Handling

        private void LicenseLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void ExternalInfoTextbox_ScrollOrResized(object sender, EventArgs e)
        {
            AdjustLinkLabels();
        }

        private void AdjustLinkLabels()
        {
            foreach (var linkLabel in ExternalInfoTextbox.Controls.OfType<LinkLabel>())
            {
                int charPos = (int)linkLabel.Tag;
                linkLabel.Location = ExternalInfoTextbox.GetPositionFromCharIndex(charPos);
            }
        }

        #endregion
    }
}
