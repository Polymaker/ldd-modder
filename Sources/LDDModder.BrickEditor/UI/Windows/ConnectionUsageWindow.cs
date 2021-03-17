using LDDModder.LDD;
using LDDModder.LDD.Primitives;
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
    public partial class ConnectionUsageWindow : Form
    {
        public ConnectionUsageWindow()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }

        class ConnUsageModel
        {
            public string ConnectionType { get; set; }
            public string SubType { get; set; }
            public int RefCount { get; set; }
            public string Parts { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FetchConnectionsUsage();
        }

        private void FetchConnectionsUsage()
        {
            var primitivesPath = LDDEnvironment.Current.GetAppDataSubDir("db\\Primitives");

            var connDic = new Dictionary<Tuple<int, int>, List<string>>();
            foreach(string xmlPath in Directory.GetFiles(primitivesPath, "*.xml"))
            {
                try
                {
                    var primitive = Primitive.Load(xmlPath);

                    foreach (var connGroup in primitive.Connectors.GroupBy(x => new Tuple<int,int> ((int)x.Type, x.SubType)))
                    {
                        if (!connDic.ContainsKey(connGroup.Key))
                            connDic.Add(connGroup.Key, new List<string>());

                        connDic[connGroup.Key].Add(primitive.ID.ToString());
                    }
                }
                catch (Exception ex)
                { 
                }
            }

            var usageList = new List<ConnUsageModel>();
            foreach (var key in connDic.Keys.OrderBy(x => x.Item1).ThenBy(x => x.Item2))
            {
                var connType = ((LDD.Primitives.Connectors.ConnectorType)key.Item1);
                usageList.Add(new ConnUsageModel
                {
                    ConnectionType = $"{connType}",
                    SubType = key.Item2.ToString(),
                    RefCount = connDic[key].Count,
                    Parts = string.Join(", ", connDic[key])
                });
            }
            dataGridView1.DataSource = usageList;

        }
    }
}
