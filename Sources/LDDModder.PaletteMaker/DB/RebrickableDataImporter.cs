using HtmlAgilityPack;
using LDDModder.LDD;
using LDDModder.LDD.Files;
using LDDModder.LDD.Primitives;
using LDDModder.PaletteMaker.Models;
using LDDModder.PaletteMaker.Models.LDD;
using LDDModder.PaletteMaker.Models.Rebrickable;
using LDDModder.PaletteMaker.Rebrickable;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.DB
{
    public class RebrickableDataImporter : DbInitializerModule
    {
        public string PartsCsvFile { get; set; }

        public string RelationshipsCsvFile { get; set; }

        public RebrickableDataImporter(SQLiteConnection connection, LDDEnvironment environment, CancellationToken cancellationToken) : base(connection, environment, cancellationToken)
        {

        }

        public void ImportAllData()
        {
            if (!IsCancellationRequested)
                ImportColors();

            if (!IsCancellationRequested)
                ImportCategories();

            if (!IsCancellationRequested)
                ImportThemes();

            if (!IsCancellationRequested)
                ImportPartsAndRelationships();
        }

        public void ImportColors()
        {
            using (var trans = Connection.BeginTransaction())
            using (var colorCmd = Connection.CreateCommand())
            using (var matchCmd = Connection.CreateCommand())
            {
                colorCmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<RbColorMatch>()}";
                colorCmd.ExecuteNonQuery();
                colorCmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<RbColor>()}";
                colorCmd.ExecuteNonQuery();
                colorCmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<RbColorMatch>()}'";
                colorCmd.ExecuteNonQuery();
                colorCmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<RbColor>()}'";
                colorCmd.ExecuteNonQuery();

                NotifyIndefiniteProgress("Downloading rebrickable colors...");

                var colors = RebrickableAPI.GetAllColors().ToList();

                NotifyTaskStart(colors.Count, "Importing colors...");

                DbHelper.InitializeInsertCommand<RbColor>(colorCmd, x =>
                new
                {
                    x.ID,
                    x.Name,
                    x.IsTransparent,
                    x.RgbHex
                });

                DbHelper.InitializeInsertCommand<RbColorMatch>(matchCmd, x =>
                new
                {
                    x.RbColorID,
                    x.Platform,
                    x.ColorID,
                    x.ColorName
                });

                void AddColorMatches(int colorID, PaletteMaker.Rebrickable.Models.ColorIds colorIds, string platform)
                {
                    int colorCount = colorIds?.ExtIds?.Count ?? 0;

                    for (int i = 0; i < colorCount; i++)
                    {
                        DbHelper.InsertWithParameters(matchCmd,
                            colorID,
                            platform,
                            colorIds.ExtIds[i],
                            colorIds.ExtDescrs[i][0]
                        );
                    }
                }

                foreach (var rbColor in colors)
                {
                    if (IsCancellationRequested)
                        break;

                    DbHelper.InsertWithParameters(colorCmd,
                        rbColor.Id,
                        rbColor.Name,
                        rbColor.IsTransparent,
                        rbColor.RgbHex
                    );

                    AddColorMatches(rbColor.Id, rbColor.ExternalColorIds.LEGO, "LEGO");
                    AddColorMatches(rbColor.Id, rbColor.ExternalColorIds.BrickLink, "BrickLink");

                    //progressTracker.Current++;
                    //progress?.Report(progressTracker);
                }

                trans.Commit();
            }
        }

        public void ImportCategories()
        {
            using (var trans = Connection.BeginTransaction())
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<RbCategory>()}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<RbCategory>()}'";
                cmd.ExecuteNonQuery();

                NotifyIndefiniteProgress("Downloading rebrickable categories...");
                var categories = RebrickableAPI.GetAllCategories().ToList();
                NotifyTaskStart(categories.Count, "Importing themes...");

                DbHelper.InitializeInsertCommand<RbCategory>(cmd, x => new { x.ID, x.Name });

                foreach (var rbCat in categories)
                {
                    if (IsCancellationRequested)
                        break;

                    DbHelper.InsertWithParameters(cmd, rbCat.Id, rbCat.Name);
                }

                trans.Commit();
            }
        }

        public void ImportThemes()
        {
            using (var trans = Connection.BeginTransaction())
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<RbTheme>()}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<RbTheme>()}'";
                cmd.ExecuteNonQuery();

                
                NotifyIndefiniteProgress("Downloading rebrickable themes...");
                var themes = RebrickableAPI.GetAllThemes().ToList();
                NotifyTaskStart(themes.Count, "Importing themes...");

                DbHelper.InitializeInsertCommand<RbTheme>(cmd, x => new { x.ID, x.Name, x.ParentThemeID });

                foreach (var rbTheme in themes)
                {
                    if (IsCancellationRequested)
                        break;

                    DbHelper.InsertWithParameters(cmd, rbTheme.Id, rbTheme.Name, rbTheme.ParentID);

                }

                trans.Commit();
            }
        }
    
        public void ImportPartsAndRelationships()
        {
            string tmpDownloadDir = null;

            try
            {
                if (string.IsNullOrEmpty(PartsCsvFile) || string.IsNullOrEmpty(RelationshipsCsvFile))
                {
                    NotifyIndefiniteProgress("Downloading rebrickable csv files...");
                    tmpDownloadDir = FileHelper.GetTempDirectory();
                    DownloadRebrickableCsvFiles(tmpDownloadDir);
                }

                if (!IsCancellationRequested)
                     ImportRebrickableParts();

                if (!IsCancellationRequested)
                    ImportRebrickableRelationships();
            }
            finally
            {
                if (!string.IsNullOrEmpty(tmpDownloadDir))
                    Task.Factory.StartNew(() => FileHelper.DeleteFileOrFolder(tmpDownloadDir, true, true));
            }
        }

        public static Regex IsPrintOrPatternRegex = new Regex("((?<=\\d)p|pr|pb|px|pat)\\d", RegexOptions.Compiled);
        public static Regex IsAssemblyRegex = new Regex("c\\d\\d[a-c]?$", RegexOptions.Compiled);

        private void ImportRebrickableParts()
        {
            if (!File.Exists(PartsCsvFile))
                return;

            var partCsv = IO.CsvFile.Read(PartsCsvFile, IO.CsvFile.Separator.Comma);
            partCsv.Rows[0].IsHeader = true;


            using (var trans = Connection.BeginTransaction())
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<RbPart>()}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<RbPart>()}'";
                cmd.ExecuteNonQuery();

                DbHelper.InitializeInsertCommand<RbPart>(cmd, x =>
                new
                {
                    x.PartID,
                    x.Name,
                    x.CategoryID,
                    x.IsPrintOrPattern,
                    x.ParentPartID
                });

                foreach (var row in partCsv.Rows)
                {
                    if (IsCancellationRequested)
                        break;

                    if (row.IsHeader)
                        continue;
                    string partID = row[0];

                    int? category = string.IsNullOrEmpty(row[2]) ? default(int?) : int.Parse(row[2]);
                    string parentPartID = null;
                    bool isPrintOrPattern = false;

                    if (!(category == 17 || category == 58))//non-lego and stickers
                    {
                        var match = IsPrintOrPatternRegex.Match(partID);
                        isPrintOrPattern = match.Success;
                        if (isPrintOrPattern)
                        {
                            parentPartID = partID.Substring(0, match.Index);

                            if (IsAssemblyRegex.IsMatch(partID) && !IsAssemblyRegex.IsMatch(parentPartID))
                            {
                                parentPartID += partID.Substring(IsAssemblyRegex.Match(partID).Index);
                            }
                        }
                    }

                    DbHelper.InsertWithParameters(cmd, row[0], row[1], category, isPrintOrPattern, parentPartID);

                }

                trans.Commit();
            }
        }

        private void ImportRebrickableRelationships()
        {
            if (!File.Exists(RelationshipsCsvFile))
                return;

            var partCsv = IO.CsvFile.Read(RelationshipsCsvFile, IO.CsvFile.Separator.Comma);
            partCsv.Rows[0].IsHeader = true;

            using (var trans = Connection.BeginTransaction())
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<RbPartRelation>()}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<RbPartRelation>()}'";
                cmd.ExecuteNonQuery();

                DbHelper.InitializeInsertCommand<RbPartRelation>(cmd);
                DbHelper.OrderCommandParameters(cmd,
                    nameof(RbPartRelation.ParentPartID),
                    nameof(RbPartRelation.ChildPartID),
                    nameof(RbPartRelation.RelationTypeChar));

                foreach (var row in partCsv.Rows)
                {
                    if (IsCancellationRequested)
                        break;

                    if (row.IsHeader)
                        continue;
                    DbHelper.InsertWithParameters(cmd, row[2], row[1], row[0]);

                }
                trans.Commit();
            }
        }

        public const string PARTS_FILENAME = "parts.csv";

        public const string RELATIONSHIPS_FILENAME = "part_relationships.csv";

        public static List<Tuple<string, string>> GetRebrickableDownloadLinks()
        {
            var web = new HtmlWeb();

            var htmlDoc = web.Load("https://rebrickable.com/downloads/");

            var downloadLinkNodes = htmlDoc.DocumentNode.SelectNodes("//a[contains(., \".csv\")]");
            var downloadLinks = new List<Tuple<string, string>>();

            foreach (var linkNode in downloadLinkNodes)
            {
                downloadLinks.Add(new Tuple<string, string>(linkNode.InnerText, linkNode.Attributes["href"].Value));
            }
            return downloadLinks;
        }

        public void DownloadRebrickableCsvFiles(string destinationFolder)
        {
            Directory.CreateDirectory(destinationFolder);

            var links = GetRebrickableDownloadLinks();
            var partsLink = links.FirstOrDefault(x => x.Item1 == PARTS_FILENAME)?.Item2;
            var relastionshipsLink = links.FirstOrDefault(x => x.Item1 == RELATIONSHIPS_FILENAME)?.Item2;

            using (var wc = new WebClient())
            {
                PartsCsvFile = Path.Combine(destinationFolder, PARTS_FILENAME);
                wc.DownloadFile(partsLink, PartsCsvFile);
                RelationshipsCsvFile = Path.Combine(destinationFolder, RELATIONSHIPS_FILENAME);
                wc.DownloadFile(relastionshipsLink, RelationshipsCsvFile);
            }
        }
    }
}
