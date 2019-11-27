using HtmlAgilityPack;
using LDDModder.LDD.Files;
using LDDModder.LDD.Primitives;
using LDDModder.PaletteMaker.Models.LDD;
using LDDModder.PaletteMaker.Native;
using LDDModder.PaletteMaker.Rebrickable;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LDDEnvironment = LDDModder.LDD.LDDEnvironment;

namespace LDDModder.PaletteMaker.Models
{
    static class DatabaseInitializer
    {
        public struct StepProgressInfo
        {
            public string Name;
            public int Current;
            public int Total;
            public double Percent => Total > 0 ? (Current / (double)Total) : 0;
        }

        public struct ProgressInfo
        {
            public int Current;
            public int Total;
            public double Percent => Total > 0 ? (Current / (double)Total) : 0;
        }

        private static void CommitAndFreeDB(ref PaletteDbContext dbContext, string dbPath)
        {
            dbContext.SaveChanges();
            if (dbContext.Database.CurrentTransaction != null)
                dbContext.Database.CurrentTransaction.Commit();
            dbContext.Dispose();
            dbContext = new PaletteDbContext($"Data Source={dbPath}");
        }

        public static void ImportBaseData(string databasePath, CancellationToken cancellationToken, 
            IProgress<StepProgressInfo> mainProgress,
            IProgress<ProgressInfo> detailProgress)
        {
            using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            {
                conn.Open();
                var stepProgress = new StepProgressInfo { Name = "LDD Parts", Current = 1, Total = 8 };

                mainProgress?.Report(stepProgress);
                ImportLddPrimitives(conn, cancellationToken, detailProgress);

                if (!cancellationToken.IsCancellationRequested)
                {
                    stepProgress.Current++;
                    stepProgress.Name = "LDD Assemblies";

                    mainProgress?.Report(stepProgress);
                    ImportLddAssemblies(conn, cancellationToken, detailProgress);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    stepProgress.Current++;
                    stepProgress.Name = "LDD Elements";

                    mainProgress?.Report(stepProgress);
                    ImportLddElements(conn, cancellationToken, detailProgress);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    stepProgress.Current++;
                    stepProgress.Name = "Rebrickable Colors";

                    mainProgress?.Report(stepProgress);
                    ImportRebrickableColors(conn, cancellationToken, detailProgress);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    stepProgress.Current++;
                    stepProgress.Name = "Rebrickable Categories";

                    mainProgress?.Report(stepProgress);
                    ImportRebrickableCategories(conn, cancellationToken, detailProgress);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    stepProgress.Current++;
                    stepProgress.Name = "Downloading Rebrickable Files";
                    mainProgress?.Report(stepProgress);
                    detailProgress?.Report(new ProgressInfo() { Current = 0, Total = 1 });

                    string tmpDir = GetTempDirectory();
                    DownloadRebrickableParts(tmpDir);
                    detailProgress?.Report(new ProgressInfo() { Current = 1, Total = 1 });

                    stepProgress.Current++;
                    stepProgress.Name = "Importing Rebrickable Parts";
                    mainProgress?.Report(stepProgress);

                    ImportRebrickableParts(conn, Path.Combine(tmpDir, "parts.csv"), cancellationToken, detailProgress);

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        stepProgress.Current++;
                        stepProgress.Name = "Importing Rebrickable Relationships";
                        mainProgress?.Report(stepProgress);

                        ImportRebrickablePartRelationships(conn, Path.Combine(tmpDir, "part_relationships.csv"), cancellationToken, detailProgress);
                    }

                    Task.Factory.StartNew(() => FileHelper.DeleteFileOrFolder(tmpDir));
                }
            }
        }

        public static void ImportLddPrimitives(SQLiteConnection conn, 
            CancellationToken cancellationToken, 
            IProgress<ProgressInfo> progress)
        {
            using (var trans = conn.BeginTransaction())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM LddParts WHERE IsAssembly = 0";
                cmd.ExecuteNonQuery();
                trans.Commit();
            }

            void ImportFromDirectory(string directory)
            {
                var primitivesFiles = Directory.GetFiles(directory, "*.xml");
                var progressTracker = new ProgressInfo { Total = primitivesFiles.Length };
                progress?.Report(progressTracker);

                using (var trans = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO LddParts (DesignID, Name, IsAssembly, Aliases) VALUES ($id, $name, 0, $aliases)";
                    var idParam = cmd.Parameters.Add("$id", System.Data.DbType.String);
                    var nameParam = cmd.Parameters.Add("$name", System.Data.DbType.String);
                    var aliasParam = cmd.Parameters.Add("$aliases", System.Data.DbType.String);

                    foreach (var primitiveFile in primitivesFiles)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        try
                        {
                            var primitiveInfo = Primitive.Load(primitiveFile);
                            idParam.Value = primitiveInfo.ID.ToString();
                            nameParam.Value = primitiveInfo.Name;
                            aliasParam.Value = string.Join(";", primitiveInfo.Aliases);
                            cmd.ExecuteNonQuery();
                        }
                        catch { }

                        progressTracker.Current++;
                        progress?.Report(progressTracker);
                    }

                    trans.Commit();
                }
            }

            if (LDDEnvironment.Current.DatabaseExtracted)
            {
                var primitivesPath = LDDEnvironment.Current.GetAppDataSubDir("db\\Primitives");
                ImportFromDirectory(primitivesPath);
            }
            else
            {
                string dbLifPath = LDDEnvironment.Current.GetDatabaseLifPath();

                if (File.Exists(dbLifPath))
                {
                    using (var dbLif = LifFile.Open(dbLifPath))
                    {
                        var primitivesFolder = dbLif.GetFolder("Primitives");
                        if (primitivesFolder != null)
                        {
                            string tmpFolder = GetTempDirectory();
                            Directory.CreateDirectory(tmpFolder);

                            try
                            {
                                primitivesFolder.ExtractContent(tmpFolder, cancellationToken, null);
                                if (!cancellationToken.IsCancellationRequested)
                                    ImportFromDirectory(tmpFolder);
                            }
                            finally
                            {
                                Task.Factory.StartNew(() => FileHelper.DeleteFileOrFolder(tmpFolder));
                            }
                        }
                    }
                }
            }
        }

        public static void ImportLddAssemblies(SQLiteConnection conn,
            CancellationToken cancellationToken,
            IProgress<ProgressInfo> progress)
        {
            using (var trans = conn.BeginTransaction())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM LddAssemblyParts";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM sqlite_sequence WHERE name='LddAssemblyParts'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM LddParts WHERE IsAssembly = 1";
                cmd.ExecuteNonQuery();
                trans.Commit();
            }

            void ImportFromDirectory(string directory)
            {
                var assemblyFiles = Directory.GetFiles(directory, "*.lxfml");
                var progressTracker = new ProgressInfo { Total = assemblyFiles.Length };
                progress?.Report(progressTracker);

                using (var trans = conn.BeginTransaction())
                using (var mainCmd = conn.CreateCommand())
                using (var subCmd = conn.CreateCommand())
                {
                    mainCmd.CommandText = "INSERT INTO LddParts (DesignID, Name, IsAssembly, Aliases) VALUES ($id, $name, 1, $aliases)";
                    var idParam = mainCmd.Parameters.Add("$id", System.Data.DbType.String);
                    var nameParam = mainCmd.Parameters.Add("$name", System.Data.DbType.String);
                    var aliasParam = mainCmd.Parameters.Add("$aliases", System.Data.DbType.String);

                    subCmd.CommandText = "INSERT INTO LddAssemblyParts (AssemblyID, PartID, DefaultMaterials) VALUES ($assID, $partID, $materials)";
                    var assemIDParam = subCmd.Parameters.Add("$assID", System.Data.DbType.String);
                    var partIDParam = subCmd.Parameters.Add("$partID", System.Data.DbType.String);
                    var materialsParam = subCmd.Parameters.Add("$materials", System.Data.DbType.String);

                    foreach (var assemFile in assemblyFiles)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        try
                        {
                            var assemInfo = LDDModder.LDD.Models.Assembly.Load(assemFile);

                            //skip decomposed assemblies
                            if (assemInfo.Bricks.Count == 1 &&
                                assemInfo.ID.ToString() == assemInfo.Bricks[0].DesignID)
                                continue;

                            idParam.Value = assemInfo.ID.ToString();
                            nameParam.Value = assemInfo.Name;
                            aliasParam.Value = string.Join(";", assemInfo.Aliases);
                            mainCmd.ExecuteNonQuery();

                            foreach (var subPart in assemInfo.Bricks)
                            {
                                assemIDParam.Value = assemInfo.ID.ToString();
                                partIDParam.Value = subPart.Part.DesignID;
                                materialsParam.Value = string.Join(";", subPart.Part.Materials);
                                subCmd.ExecuteNonQuery();
                            }
                        }
                        catch { }

                        progressTracker.Current++;
                        progress?.Report(progressTracker);
                    }

                    trans.Commit();
                }
            }

            if (LDDEnvironment.Current.DatabaseExtracted)
            {
                var primitivesPath = LDDEnvironment.Current.GetAppDataSubDir("db\\Assemblies");
                ImportFromDirectory(primitivesPath);
            }
            else
            {
                string dbLifPath = LDDEnvironment.Current.GetDatabaseLifPath();

                if (File.Exists(dbLifPath))
                {
                    using (var dbLif = LifFile.Open(dbLifPath))
                    {
                        var assembliesFolder = dbLif.GetFolder("Assemblies");
                        if (assembliesFolder != null)
                        {
                            string tmpFolder = GetTempDirectory();
                            Directory.CreateDirectory(tmpFolder);

                            try
                            {
                                assembliesFolder.ExtractContent(tmpFolder, cancellationToken, null);
                                if (!cancellationToken.IsCancellationRequested)
                                    ImportFromDirectory(tmpFolder);
                            }
                            finally
                            {
                                Task.Factory.StartNew(() => FileHelper.DeleteFileOrFolder(tmpFolder));
                            }
                        }
                    }
                }
            }
        }

        public static void ImportLddElements(SQLiteConnection conn,
            CancellationToken cancellationToken,
            IProgress<ProgressInfo> progress)
        {
            var lddPaletteFile = GetLddPalette();
            if (lddPaletteFile == null)
                return;
            var lddPalette = lddPaletteFile.Palettes.First();

            var progressTracker = new ProgressInfo { Total = lddPalette.Items.Count };
            progress?.Report(progressTracker);

            var manualLddElements = new List<LddElement>();

            using(var dbContext = new PaletteDbContext(conn))
            {
                manualLddElements.AddRange(
                    dbContext.LddElements.Where(x => x.Flag != 1)
                );
            }

            using (var trans = conn.BeginTransaction())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Decorations";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM SubMaterials";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM PartConfigs";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM LddElements";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM sqlite_sequence WHERE name='LddElements'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM sqlite_sequence WHERE name='PartConfigs'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM sqlite_sequence WHERE name='Decorations'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM sqlite_sequence WHERE name='SubMaterials'";
                cmd.ExecuteNonQuery();
                trans.Commit();
            }

            using (var trans = conn.BeginTransaction())
            using (var elemCmd = conn.CreateCommand())
            using (var cfgCmd = conn.CreateCommand())
            using (var decCmd = conn.CreateCommand())
            using (var matCmd = conn.CreateCommand())
            using (var rowidCmd = conn.CreateCommand())
            {
                elemCmd.CommandText = "INSERT INTO LddElements (DesignID, ElementID, IsAssembly, Flag) VALUES ($DesignID, $ElementID, $IsAssembly, 1)";
                elemCmd.Parameters.Add("$DesignID", System.Data.DbType.String);
                elemCmd.Parameters.Add("$ElementID", System.Data.DbType.String);
                elemCmd.Parameters.Add("$IsAssembly", System.Data.DbType.Boolean);

                cfgCmd.CommandText = "INSERT INTO PartConfigs (LegoElementID, DesignID, MaterialID) VALUES ($LegoElementID, $DesignID, $MaterialID)";
                cfgCmd.Parameters.Add("$LegoElementID", System.Data.DbType.Int32);
                cfgCmd.Parameters.Add("$DesignID", System.Data.DbType.String);
                cfgCmd.Parameters.Add("$MaterialID", System.Data.DbType.Int32);

                decCmd.CommandText = "INSERT INTO Decorations (ConfigurationID, SurfaceID, DecorationID) VALUES ($ConfigurationID, $SurfaceID, $DecorationID)";
                decCmd.Parameters.Add("$ConfigurationID", System.Data.DbType.Int32);
                decCmd.Parameters.Add("$SurfaceID", System.Data.DbType.Int32);
                decCmd.Parameters.Add("$DecorationID", System.Data.DbType.String);

                matCmd.CommandText = "INSERT INTO SubMaterials (ConfigurationID, SurfaceID, MaterialID) VALUES ($ConfigurationID, $SurfaceID, $MaterialID)";
                matCmd.Parameters.Add("$ConfigurationID", System.Data.DbType.Int32);
                matCmd.Parameters.Add("$SurfaceID", System.Data.DbType.Int32);
                matCmd.Parameters.Add("$MaterialID", System.Data.DbType.Int32);

                rowidCmd.CommandText = "select last_insert_rowid()";

                foreach (var item in lddPalette.Items)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    bool isAssembly = item is LDDModder.LDD.Palettes.Assembly;
                    int elemID = InsertAutoIncObject(elemCmd, rowidCmd, item.DesignID.ToString(), item.ElementID, isAssembly);

                    var partConfigs = new List<Models.LDD.PartConfiguration>();

                    if (item is LDDModder.LDD.Palettes.Assembly assy)
                    {
                        foreach (var part in assy.Parts)
                            partConfigs.Add(AssemblyPartToConfig(part));
                    }
                    else if (item is LDDModder.LDD.Palettes.Brick brick)
                        partConfigs.Add(PaletteBrickToConfig(brick));

                    foreach (var cfg in partConfigs)
                    {
                        int configID = InsertAutoIncObject(cfgCmd, rowidCmd, elemID, cfg.DesignID, cfg.MaterialID);

                        foreach(var dec in cfg.Decorations)
                            InsertAutoIncObject(decCmd, rowidCmd, configID, dec.SurfaceID, dec.DecorationID);

                        foreach (var mat in cfg.SubMaterials)
                            InsertAutoIncObject(matCmd, rowidCmd, configID, mat.SurfaceID, mat.MaterialID);
                    }

                    progressTracker.Current++;
                    progress?.Report(progressTracker);
                }

                trans.Commit();
            }

            //var db = new PaletteDbContext(conn);
            //db.Database.BeginTransaction();

            //var manualElements = db.LddElements.Where(x=>x.Flag != 1)
            //    .Select(x => new Tuple<string, string>(x.DesignID, x.ElementID)).ToList();

            //foreach (var item in lddPalette.Items)
            //{
            //    if (cancellationToken.IsCancellationRequested)
            //        break;

            //    if (manualElements.Any(x=>x.Item1 == item.DesignID.ToString() && x.Item2 == item.ElementID))
            //    {
            //        var existingElem = db.LddElements.FirstOrDefault(x =>
            //            x.DesignID == item.DesignID.ToString() &&
            //            x.ElementID == item.ElementID);
            //        db.LddElements.Remove(existingElem);
            //    }

            //    var legoElem = new Models.LDD.LddElement()
            //    {
            //        DesignID = item.DesignID.ToString(),
            //        ElementID = item.ElementID,
            //        IsAssembly = (item is LDDModder.LDD.Palettes.Assembly),
            //        Flag = 1
            //    };
            //    db.LddElements.Add(legoElem);

            //    if (item is LDDModder.LDD.Palettes.Assembly assy)
            //    {
            //        foreach (var part in assy.Parts)
            //            legoElem.Configurations.Add(AssemblyPartToConfig(part));
            //    }
            //    else if (item is LDDModder.LDD.Palettes.Brick brick)
            //    {
            //        legoElem.Configurations.Add(PaletteBrickToConfig(brick));
            //    }

            //    progressTracker.Current++;
            //    progress?.Report(progressTracker);

            //    if (progressTracker.Current % 1000 == 0)
            //    {
            //        CommitAndFreeDB(ref db, conn.FileName);
            //        db.Database.BeginTransaction();
            //    }
            //}

            //db.SaveChanges();
            //db.Database.CurrentTransaction.Commit();
            //db.Dispose();
        }

        public static void ImportRebrickableColors(SQLiteConnection conn,
            CancellationToken cancellationToken,
            IProgress<ProgressInfo> progress)
        {
            using (var trans = conn.BeginTransaction())
            using (var cmd = conn.CreateCommand())
            using (var cmd2 = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM RbColorMatches";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM RbColors";
                cmd.ExecuteNonQuery();

                var colors = RebrickableAPI.GetAllColors().ToList();
                var progressTracker = new ProgressInfo { Total = colors.Count };
                progress?.Report(progressTracker);

                cmd.CommandText = "INSERT INTO RbColors (ID, Name, IsTransparent, RgbHex) VALUES ($ID, $Name, $IsTransparent, $RgbHex)";
                cmd.Parameters.Add("$ID", System.Data.DbType.Int32);
                cmd.Parameters.Add("$Name", System.Data.DbType.String);
                cmd.Parameters.Add("$IsTransparent", System.Data.DbType.Boolean);
                cmd.Parameters.Add("$RgbHex", System.Data.DbType.String);

                cmd2.CommandText = "INSERT INTO RbColorMatches (RbColorID, Platform, ColorID, ColorName) VALUES ($RbColorID, $Platform, $ColorID, $ColorName)";
                cmd2.Parameters.Add("$RbColorID", System.Data.DbType.Int32);
                cmd2.Parameters.Add("$Platform", System.Data.DbType.String);
                cmd2.Parameters.Add("$ColorID", System.Data.DbType.Int32);
                cmd2.Parameters.Add("$ColorName", System.Data.DbType.String);

                void AddColorMatches(int colorID, PaletteMaker.Rebrickable.Models.ColorIds colorIds, string platform)
                {
                    int colorCount = colorIds?.ExtIds?.Count ?? 0;

                    for (int i = 0; i < colorCount; i++)
                    {
                        cmd2.Parameters[0].Value = colorID;
                        cmd2.Parameters[1].Value = platform;
                        cmd2.Parameters[2].Value = colorIds.ExtIds[i];
                        cmd2.Parameters[3].Value = colorIds.ExtDescrs[i][0];
                        cmd2.ExecuteNonQuery();
                    }
                }

                foreach (var rbColor in colors)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    cmd.Parameters[0].Value = rbColor.Id;
                    cmd.Parameters[1].Value = rbColor.Name;
                    cmd.Parameters[2].Value = rbColor.IsTransparent;
                    cmd.Parameters[3].Value = rbColor.RgbHex;
                    cmd.ExecuteNonQuery();

                    AddColorMatches(rbColor.Id, rbColor.ExternalColorIds.LEGO, "LEGO");
                    AddColorMatches(rbColor.Id, rbColor.ExternalColorIds.BrickLink, "BrickLink");

                    progressTracker.Current++;
                    progress?.Report(progressTracker);
                }

                trans.Commit();
            }
        }

        public static void ImportRebrickableCategories(SQLiteConnection conn,
            CancellationToken cancellationToken,
            IProgress<ProgressInfo> progress)
        {
            using (var trans = conn.BeginTransaction())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM RbCategories";
                cmd.ExecuteNonQuery();

                var categories = RebrickableAPI.GetAllCategories().ToList();
                var progressTracker = new ProgressInfo { Total = categories.Count };
                progress?.Report(progressTracker);

                cmd.CommandText = "INSERT INTO RbCategories (ID, Name) VALUES ($ID, $Name)";
                cmd.Parameters.Add("$ID", System.Data.DbType.Int32);
                cmd.Parameters.Add("$Name", System.Data.DbType.String);

                foreach (var rbCat in categories)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    cmd.Parameters[0].Value = rbCat.Id;
                    cmd.Parameters[1].Value = rbCat.Name;
                    cmd.ExecuteNonQuery();

                    progressTracker.Current++;
                    progress?.Report(progressTracker);
                }

                trans.Commit();
            }
        }

        public static void InitRebrickableElements(string databasePath, bool clearAll = false)
        {
            var db = new PaletteDbContext($"Data Source={databasePath}");

            db.Database.Connection.Open();
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM RbColorMatches";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM RbColors";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM RbCategories";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM RbThemes";
                cmd.ExecuteNonQuery();
            }

            var colors = RebrickableAPI.GetAllColors();

            void AddColorMatches(Rebrickable.RbColor color,
                LDDModder.PaletteMaker.Rebrickable.Models.ColorIds colorIds,
                string platform)
            {
                int colorCount = colorIds?.ExtIds?.Count ?? 0;

                for (int i = 0; i < colorCount; i++)
                {
                    var match = new Rebrickable.RbColorMatch()
                    {
                        Platform = platform,
                        ColorID = colorIds.ExtIds[i],
                        ColorName = colorIds.ExtDescrs[i][0]
                    };

                    color.ColorMatches.Add(match);
                }
            }

            foreach (var rbColor in colors)
            {
                if (!db.Colors.Any(x => x.ID == rbColor.Id))
                {
                    var color = new Rebrickable.RbColor()
                    {
                        ID = rbColor.Id,
                        Name = rbColor.Name,
                        IsTransparent = rbColor.IsTransparent,
                        RgbHex = rbColor.RgbHex
                    };

                    AddColorMatches(color, rbColor.ExternalColorIds.BrickLink, "BrickLink");
                    AddColorMatches(color, rbColor.ExternalColorIds.LEGO, "LEGO");
                    //AddColorMatches(color, rbColor.ExternalColorIds.LDraw, "LDraw");
                    db.Colors.Add(color);
                }
            }
            CommitAndFreeDB(ref db, databasePath);

            var categories = RebrickableAPI.GetAllCategories().ToList();

            foreach (var rbCat in categories)
            {
                if (!db.Categories.Any(x => x.ID == rbCat.Id))
                {
                    db.Categories.Add(new Rebrickable.RbCategory()
                    {
                        ID = rbCat.Id,
                        Name = rbCat.Name
                    });
                }
            }

            CommitAndFreeDB(ref db, databasePath);

            var themes = RebrickableAPI.GetAllThemes();

            foreach (var rbTheme in themes)
            {
                if (!db.Themes.Any(x => x.ID == rbTheme.Id))
                {
                    db.Themes.Add(new Rebrickable.RbTheme()
                    {
                        ID = rbTheme.Id,
                        Name = rbTheme.Name,
                        ParentThemeID = rbTheme.ParentID
                    });
                }
            }

            db.SaveChanges();
            db.Dispose();
        }

        public static Regex IsPrintOrPatternRegex = new Regex("((?<=\\d)p|pr|pb|px|pat)\\d", RegexOptions.Compiled);
        public static Regex IsAssemblyRegex = new Regex("c\\d\\d[a-c]?$", RegexOptions.Compiled);

        public static void ImportRebrickableParts(SQLiteConnection conn, 
            string partCsvPath,
            CancellationToken cancellationToken,
            IProgress<ProgressInfo> progress)
        {
            var partCsv = IO.CsvFile.Read(partCsvPath, IO.CsvFile.Separator.Comma);
            partCsv.Rows[0].IsHeader = true;

            var progressTracker = new ProgressInfo { Total = partCsv.Rows.Count - 1 };
            progress?.Report(progressTracker);

            using (var trans = conn.BeginTransaction())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM RbParts";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO RbParts (PartID, Name, CategoryID, ParentPartID, IsPrintOrPattern) VALUES ($id, $name, $category, $parentPartID, $IsPrintOrPattern)";
                cmd.Parameters.Add("$id", System.Data.DbType.String);
                cmd.Parameters.Add("$name", System.Data.DbType.String);
                cmd.Parameters.Add("$category", System.Data.DbType.Int32);
                cmd.Parameters.Add("$parentPartID", System.Data.DbType.String);
                cmd.Parameters.Add("$IsPrintOrPattern", System.Data.DbType.Boolean);

                foreach (var row in partCsv.Rows)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    if (row.IsHeader)
                        continue;
                    string partID = row[0];

                    int ? category = string.IsNullOrEmpty(row[2]) ? default(int?) : int.Parse(row[2]);
                    string parentPart = null;
                    bool isPrintOrPattern = false;

                    if (!(category == 17 || category == 58))//non-lego and stickers
                    {
                        var match = IsPrintOrPatternRegex.Match(partID);
                        isPrintOrPattern = match.Success;
                        if (isPrintOrPattern)
                        {
                            parentPart = partID.Substring(0, match.Index);
                            if (IsAssemblyRegex.IsMatch(partID) && !IsAssemblyRegex.IsMatch(parentPart))
                            {
                                parentPart += partID.Substring(IsAssemblyRegex.Match(partID).Index);
                            }
                        }

                        //parentPart = GetBasePartID(row[0]);
                    }

                    InsertObject(cmd, row[0], row[1], category, parentPart, isPrintOrPattern);

                    progressTracker.Current++;
                    progress?.Report(progressTracker);
                }

                trans.Commit();
            }
        }
        static string[] PartIdSeparators = new string[] { "pr", "pb", "px", "pat" };

        static string GetBasePartID(string partID)
        {

            return null;
        }

        public static void ImportRebrickablePartRelationships(SQLiteConnection conn, 
            string partCsvPath,
            CancellationToken cancellationToken,
            IProgress<ProgressInfo> progress)
        {
            var partCsv = IO.CsvFile.Read(partCsvPath, IO.CsvFile.Separator.Comma);
            partCsv.Rows[0].IsHeader = true;

            var progressTracker = new ProgressInfo { Total = partCsv.Rows.Count - 1 };
            progress?.Report(progressTracker);

            using (var trans = conn.BeginTransaction())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM RbPartRelations";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM sqlite_sequence WHERE name='RbPartRelations'";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO RbPartRelations (ParentPartID, ChildPartID, RelationType) VALUES ($parentID, $childID, $type)";
                var parentParam = cmd.Parameters.Add("$parentID", System.Data.DbType.String);
                var childParam = cmd.Parameters.Add("$childID", System.Data.DbType.String);
                var typeParam = cmd.Parameters.Add("$type", System.Data.DbType.String);

                foreach (var row in partCsv.Rows)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    if (row.IsHeader)
                        continue;
                    parentParam.Value = row[2];
                    childParam.Value = row[1];
                    typeParam.Value = row[0];
                    cmd.ExecuteNonQuery();

                    progressTracker.Current++;
                    progress?.Report(progressTracker);
                }
                trans.Commit();
            }
        }

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

        public static void DownloadRebrickableParts(string destinationFolder)
        {
            Directory.CreateDirectory(destinationFolder);

            var links = GetRebrickableDownloadLinks();
            var partsLink = links.FirstOrDefault(x => x.Item1 == "parts.csv")?.Item2;
            var relastionshipsLink = links.FirstOrDefault(x => x.Item1 == "part_relationships.csv")?.Item2;

            using (var wc = new WebClient())
            {
                wc.DownloadFile(partsLink, Path.Combine(destinationFolder, "parts.csv"));
                wc.DownloadFile(relastionshipsLink, Path.Combine(destinationFolder, "part_relationships.csv"));
            }
        }

        private static PaletteFile GetLddPalette()
        {
            var appDataPalettes = LDDEnvironment.Current.GetAppDataSubDir("Palettes");

            if (File.Exists(Path.Combine(appDataPalettes, "LDD.lif")))
                return PaletteFile.FromLif(Path.Combine(appDataPalettes, "LDD.lif"));

            if (Directory.Exists(Path.Combine(appDataPalettes, "LDD")))
                return PaletteFile.FromDirectory(Path.Combine(appDataPalettes, "LDD"));

            string dbLifPath = LDDEnvironment.Current.GetDatabaseLifPath();

            if (File.Exists(dbLifPath))
            {
                using (var lif = LifFile.Open(dbLifPath))
                {
                    var paletteEntry = lif.GetAllFiles().FirstOrDefault(x => x.Name == "LDD.lif");
                    if (paletteEntry != null)
                    {
                        using (var paletteLif = LifFile.Read(paletteEntry.GetStream()))
                            return PaletteFile.FromLif(paletteLif);
                    }
                }
            }

            return null;
        }

        private static int InsertAutoIncObject(SQLiteCommand insertCmd, SQLiteCommand rowidCmd, params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
                insertCmd.Parameters[i].Value = values[i];
            insertCmd.ExecuteNonQuery();

            long rowid = (long)rowidCmd.ExecuteScalar();
            return (int)rowid;
        }

        private static void InsertObject(SQLiteCommand insertCmd, params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
                insertCmd.Parameters[i].Value = values[i];
            insertCmd.ExecuteNonQuery();
        }

        private static PartConfiguration AssemblyPartToConfig(LDDModder.LDD.Palettes.Assembly.Part part)
        {
            var partConfig = new Models.LDD.PartConfiguration()
            {
                DesignID = part.DesignID.ToString(),
                MaterialID = part.MaterialID,
            };
            if (part.Decorations?.Any() ?? false)
            {
                foreach (var deco in part.Decorations)
                {
                    partConfig.Decorations.Add(new Models.LDD.Decoration()
                    {
                        SurfaceID = deco.SurfaceID,
                        DecorationID = deco.DecorationID
                    });
                }
            }
            if (part.SubMaterials?.Any() ?? false)
            {
                foreach (var subMat in part.SubMaterials)
                {
                    partConfig.SubMaterials.Add(new Models.LDD.SubMaterial()
                    {
                        SurfaceID = subMat.SurfaceID,
                        MaterialID = subMat.MaterialID
                    });
                }
            }
            return partConfig;
        }

        private static PartConfiguration PaletteBrickToConfig(LDDModder.LDD.Palettes.Brick part)
        {
            var partConfig = new Models.LDD.PartConfiguration()
            {
                DesignID = part.DesignID.ToString(),
                MaterialID = part.MaterialID,
            };
            if (part.Decorations?.Any() ?? false)
            {
                foreach (var deco in part.Decorations)
                {
                    partConfig.Decorations.Add(new Models.LDD.Decoration()
                    {
                        SurfaceID = deco.SurfaceID,
                        DecorationID = deco.DecorationID
                    });
                }
            }
            if (part.SubMaterials?.Any() ?? false)
            {
                foreach (var subMat in part.SubMaterials)
                {
                    partConfig.SubMaterials.Add(new Models.LDD.SubMaterial()
                    {
                        SurfaceID = subMat.SurfaceID,
                        MaterialID = subMat.MaterialID
                    });
                }
            }
            return partConfig;
        }

        private static string GetTempDirectory()
        {
            return Path.Combine(Path.GetTempPath(), LDDModder.Utilities.StringUtils.GenerateUID(8));
        }
    }
}
