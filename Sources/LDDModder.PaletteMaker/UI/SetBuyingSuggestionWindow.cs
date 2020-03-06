﻿using LDDModder.LDD;
using LDDModder.PaletteMaker.Models.Rebrickable;
using LDDModder.PaletteMaker.Rebrickable;
using LDDModder.PaletteMaker.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.PaletteMaker.UI
{
    public partial class SetBuyingSuggestionWindow : Form
    {
        public SetBuyingSuggestionWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!SettingsManager.HasInitialized)
                SettingsManager.Initialize();

            LDDEnvironment.Initialize();
            RebrickableAPI.ApiKey = "aU49o5xulf";
            RebrickableAPI.InitializeClient();

            if (!SettingsManager.DatabaseExists())
            {
                using (var win = new DatabaseInitProgressWindow())
                {
                    win.StartPosition = FormStartPosition.CenterParent;
                    win.ShowDialog();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                var SetInfo = RebrickableAPI.GetSet("10019-1"); //10019-1
                var setParts = RebrickableAPI.GetSetParts(SetInfo.SetNum).ToList();
                BeginInvoke((Action)(() => LoadSetParts(setParts)));
                //PalatteGenerator.CreatePaletteFromSet(DBFilePath, setInfo);

            });
        }


        class PartMatchInfo
        {
            public string PartID { get; set; }

            public int ColorID { get; set; }

            public bool IsSpare { get; set; }

            public int SetQty { get; set; }

            public int NeededQty { get; set; }

            public double Percent => NeededQty > 0 ? SetQty / (double)NeededQty : 0d;

            public PartMatchInfo()
            {
            }

            public PartMatchInfo(Rebrickable.Models.SetPart setPart)
            {
                PartID = setPart.Part.PartNum;
                ColorID = setPart.Color?.Id ?? 0;
                NeededQty = setPart.Quantity;
                IsSpare = setPart.IsSpare;
            }
        }

        class SetPartMatchInfo
        {
            public RbSet Set { get; set; }

            public List<PartMatchInfo> MatchingParts { get; set; } = new List<PartMatchInfo>();

            public int MatchingPartCount => MatchingParts.Count(x => x.SetQty > 0);

            public double CompletionPercent
            {
                get
                {
                    var totalQty = MatchingParts.Sum(x => x.NeededQty);
                    var partPercents = MatchingParts.Sum(x => Math.Min(1d, x.Percent) * x.NeededQty);
                    return partPercents / (double)totalQty;
                }
            }

            public override string ToString()
            {
                return $"{Set.SetID}: {Set.Name} {MatchingPartCount}/{MatchingParts.Count} ({CompletionPercent:0.00%})";
            }
        }

        private void LoadSetParts(List<Rebrickable.Models.SetPart> parts)
        {
            var distinctPartIDs = parts.Select(x => x.Part.PartNum).Distinct().ToList();
            var matchingSetInfos = new List<SetPartMatchInfo>();

            using (var db = SettingsManager.GetDbContext())
            {
                var matchingSetIDs = db.RbSetParts.Where(x => distinctPartIDs.Contains(x.PartID))
                    .Select(x => x.SetID)
                    .Distinct().ToList();

                var matchingSets = db.RbSets.Where(x => 
                    matchingSetIDs.Contains(x.SetID) &&
                    x.Parts.Count(y => distinctPartIDs.Contains(y.PartID)) >= 3);

                foreach (var setinfo in matchingSets)
                {
                    var setMatchInfo = new SetPartMatchInfo() { Set = setinfo };
                    var setParts = setinfo.Parts.Where(x => distinctPartIDs.Contains(x.PartID)).ToList();

                    foreach (var neededPart in parts)
                    {
                        var partMatchInfo = new PartMatchInfo(neededPart);
                        var foundPart = setParts.FirstOrDefault(x =>
                            x.PartID == partMatchInfo.PartID &&
                            x.ColorID == partMatchInfo.ColorID);
                        if (foundPart != null)
                            partMatchInfo.SetQty += foundPart.Quantity;
                        setMatchInfo.MatchingParts.Add(partMatchInfo);
                    }

                    if (setMatchInfo.MatchingParts.Any(x => x.Percent > 0))
                        matchingSetInfos.Add(setMatchInfo);
                }

                var top10Sets = matchingSetInfos.OrderByDescending(x => x.CompletionPercent).Take(20).ToList();
                var top10Sets2 = matchingSetInfos.OrderByDescending(x => x.MatchingPartCount).Take(20).ToList();
            }
        }
    }
}
