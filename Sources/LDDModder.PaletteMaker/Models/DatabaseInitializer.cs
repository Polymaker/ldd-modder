using LDDModder.LDD.Files;
using LDDModder.LDD.Primitives;
using LDDModder.PaletteMaker.Models.LDD;
using LDDModder.PaletteMaker.Rebrickable;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Models
{
    static class DatabaseInitializer
    {
        public static void InitDb(string databasePath, bool clearAll = false)
        {
            InitLddParts(databasePath);
            InitLddElements(databasePath, clearAll);
            InitRebrickableElements(databasePath, clearAll);
        }

        private static void CommitAndFreeDB(ref PaletteDbContext dbContext, string dbPath)
        {
            dbContext.SaveChanges();
            dbContext.Dispose();
            dbContext = new PaletteDbContext($"Data Source={dbPath}");
        }

        public static void InitLddParts(string databasePath)
        {
            if (LDDModder.LDD.LDDEnvironment.Current.DatabaseExtracted)
            {
                var primitivesPath = LDDModder.LDD.LDDEnvironment.Current.GetAppDataSubDir("db\\Primitives");
                var assembliesPath = LDDModder.LDD.LDDEnvironment.Current.GetAppDataSubDir("db\\Assemblies");

                using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
                {
                    conn.Open();

                    var primitivesFiles = Directory.GetFiles(primitivesPath, "*.xml");
                    var assemblyFiles = Directory.GetFiles(assembliesPath, "*.lxfml");

                    using (var trans = conn.BeginTransaction())
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM LddAssemblyParts";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "DELETE FROM LddParts";
                        cmd.ExecuteNonQuery();
                        trans.Commit();
                    }

                    using (var trans = conn.BeginTransaction())
                    using (var cmd = conn.CreateCommand())
                    {
                        InsertLddPrimitives(cmd, primitivesFiles);
                        trans.Commit();
                    }

                    using (var trans = conn.BeginTransaction())
                    {
                        InsertLddAssemblies(conn, assemblyFiles);
                        trans.Commit();
                    }
                }
            }
            
        }

        private static void InsertLddPrimitives(SQLiteCommand cmd, string[] primitivesFiles)
        {
            int processedCount = 0;

            cmd.CommandText = "INSERT INTO LddParts (DesignID, Name, IsAssembly, Aliases) VALUES ($id, $name, $isAssembly, $aliases)";
            var idParam = cmd.Parameters.Add("$id", System.Data.DbType.String);
            var nameParam = cmd.Parameters.Add("$name", System.Data.DbType.String);
            var assyParam = cmd.Parameters.Add("$isAssembly", System.Data.DbType.Boolean);
            var aliasParam = cmd.Parameters.Add("$aliases", System.Data.DbType.String);

            foreach (var primitiveFile in primitivesFiles)
            {
                try
                {
                    var primitiveInfo = Primitive.Load(primitiveFile);
                    idParam.Value = primitiveInfo.ID.ToString();
                    nameParam.Value = primitiveInfo.Name;
                    assyParam.Value = false;
                    aliasParam.Value = string.Join(";", primitiveInfo.Aliases);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }

                if ((processedCount++) % 100 == 0)
                    Debug.WriteLine($"Total part processed: {processedCount} of {primitivesFiles.Length}");
            }
        }

        private static void InsertLddAssemblies(SQLiteConnection conn, string[] assemblyFiles)
        {
            int processedCount = 0;
            var mainCmd = conn.CreateCommand();
            mainCmd.CommandText = "INSERT INTO LddParts (DesignID, Name, IsAssembly, Aliases) VALUES ($id, $name, $isAssembly, $aliases)";
            var idParam = mainCmd.Parameters.Add("$id", System.Data.DbType.String);
            var nameParam = mainCmd.Parameters.Add("$name", System.Data.DbType.String);
            var assyParam = mainCmd.Parameters.Add("$isAssembly", System.Data.DbType.Boolean);
            var aliasParam = mainCmd.Parameters.Add("$aliases", System.Data.DbType.String);

            var subCmd = conn.CreateCommand();
            subCmd.CommandText = "INSERT INTO LddAssemblyParts (AssemblyID, PartID, DefaultMaterials) VALUES ($assID, $partID, $materials)";
            var assemIDParam = subCmd.Parameters.Add("$assID", System.Data.DbType.String);
            var partIDParam = subCmd.Parameters.Add("$partID", System.Data.DbType.String);
            var materialsParam = subCmd.Parameters.Add("$materials", System.Data.DbType.String);

            foreach(var assemFile in assemblyFiles)
            {
                try
                {
                    var assemInfo = LDDModder.LDD.Models.Assembly.Load(assemFile);
                    idParam.Value = assemInfo.ID.ToString();
                    nameParam.Value = assemInfo.Name;
                    assyParam.Value = true;
                    aliasParam.Value = string.Join(";", assemInfo.Aliases);
                    mainCmd.ExecuteNonQuery();

                    foreach(var subPart in assemInfo.Bricks)
                    {
                        assemIDParam.Value = assemInfo.ID.ToString();
                        partIDParam.Value = subPart.Part.DesignID;
                        materialsParam.Value = string.Join(";", subPart.Part.Materials);
                        subCmd.ExecuteNonQuery();
                    }

                    if ((processedCount++) % 100 == 0)
                        Debug.WriteLine($"Total assembly processed: {processedCount} of {assemblyFiles.Length}");
                }
                catch (Exception ex)
                { 
                }
            }

            mainCmd.Dispose();
            subCmd.Dispose();
        }

        public static void InitLddElements(string databasePath, bool clearAll = false)
        {
            var lddPalette = GetLddPalette();
            if (lddPalette == null)
                return;

            int processedCount = 0;
            var db = new PaletteDbContext($"Data Source={databasePath}");

            foreach (var item in lddPalette.Palettes[0].Items)
            {
                var legoElem = db.LddElements.FirstOrDefault(x => x.DesignID == item.DesignID.ToString() && x.ElementID == item.ElementID);

                if (legoElem == null)
                {
                    legoElem = new Models.LDD.LddElement()
                    {
                        DesignID = item.DesignID.ToString(),
                        ElementID = item.ElementID,
                        IsAssembly = (item is LDDModder.LDD.Palettes.Assembly)
                    };
                    db.LddElements.Add(legoElem);

                    if (item is LDDModder.LDD.Palettes.Assembly assy)
                    {
                        foreach (var part in assy.Parts)
                            legoElem.Configurations.Add(AssemblyPartToConfig(part));
                    }
                    else if (item is LDDModder.LDD.Palettes.Brick brick)
                    {
                        legoElem.Configurations.Add(PaletteBrickToConfig(brick));
                    }
                }

                processedCount++;
                if (processedCount % 100 == 0)
                {
                    Debug.WriteLine($"Total processed: {processedCount} of {lddPalette.Palettes[0].Items.Count}");
                    CommitAndFreeDB(ref db, databasePath);
                }
            }

            db.SaveChanges();
            db.Dispose();
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

        public static void InitRebrickableParts(string databasePath, string partCsvPath)
        {
            var partCsv = IO.CsvFile.Read(partCsvPath, IO.CsvFile.Separator.Comma);
            partCsv.Rows[0].IsHeader = true;
            int totalParts = partCsv.Rows.Count - 1;
            int totalProcessed = 0;

            using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM RbParts";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO RbParts (PartID, Name, CategoryID) VALUES ($id, $name, $category)";
                    var idParam = cmd.Parameters.Add("$id", System.Data.DbType.String);
                    var nameParam = cmd.Parameters.Add("$name", System.Data.DbType.String);
                    var ctgParam = cmd.Parameters.Add("$category", System.Data.DbType.Int32);

                    foreach (var row in partCsv.Rows)
                    {
                        if (row.IsHeader)
                            continue;
                        idParam.Value = row[0];
                        nameParam.Value = row[1];
                        ctgParam.Value = string.IsNullOrEmpty(row[2]) ? default(int?) : int.Parse(row[2]);
                        cmd.ExecuteNonQuery();

                        if ((++totalProcessed) % 100 == 0)
                            Debug.WriteLine($"Total part processed: {totalProcessed} of {totalParts}");
                    }
                    trans.Commit();
                }
               
            }
        }

        public static void InitRebrickablePartRelationships(string databasePath, string partCsvPath)
        {
            var partCsv = IO.CsvFile.Read(partCsvPath, IO.CsvFile.Separator.Comma);
            partCsv.Rows[0].IsHeader = true;
            int totalParts = partCsv.Rows.Count - 1;
            int totalProcessed = 0;

            using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            {
                conn.Open();

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
                        if (row.IsHeader)
                            continue;
                        parentParam.Value = row[2];
                        childParam.Value = row[1];
                        typeParam.Value = row[0];
                        cmd.ExecuteNonQuery();

                        if ((++totalProcessed) % 100 == 0)
                            Debug.WriteLine($"Total relationship processed: {totalProcessed} of {totalParts}");
                    }
                    trans.Commit();
                }

            }
        }

        private static PaletteFile GetLddPalette()
        {
            var appDataPalettes = LDDModder.LDD.LDDEnvironment.Current.GetAppDataSubDir("Palettes");
            if (File.Exists(Path.Combine(appDataPalettes, "LDD.lif")))
                return PaletteFile.FromLif(Path.Combine(appDataPalettes, "LDD.lif"));
            if (Directory.Exists(Path.Combine(appDataPalettes, "LDD")))
                return PaletteFile.FromDirectory(Path.Combine(appDataPalettes, "LDD"));
            string dbLifPath = Path.Combine(LDDModder.LDD.LDDEnvironment.Current.ApplicationDataPath, "db.lif");

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


        private static Models.LDD.PartConfiguration AssemblyPartToConfig(LDDModder.LDD.Palettes.Assembly.Part part)
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

        private static Models.LDD.PartConfiguration PaletteBrickToConfig(LDDModder.LDD.Palettes.Brick part)
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

    }
}
