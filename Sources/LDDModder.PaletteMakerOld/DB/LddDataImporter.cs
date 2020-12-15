using LDDModder.LDD;
using LDDModder.LDD.Files;
using LDDModder.LDD.Primitives;
using LDDModder.PaletteMaker.Models.LDD;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.DB
{
    public class LddDataImporter : DbInitializerModule
    {
        public LddDataImporter(SQLiteConnection connection, LDDEnvironment environment, CancellationToken cancellationToken) : base(connection, environment, cancellationToken)
        {

        }

        public void ImportAllData()
        {
            if (!IsCancellationRequested)
                ImportLddParts();

            if (!IsCancellationRequested)
                ImportLddElements();
        }

        public void ImportLddParts()
        {
            NotifyBeginStep("Importing LDD parts and elements");
            NotifyIndefiniteProgress();
            NotifyProgressStatus("Clearing previous data...");
            using (var trans = Connection.BeginTransaction())
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<LddPart>()}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<LddPart>()}'";
                cmd.ExecuteNonQuery();
                trans.Commit();
            }

            string tmpFolder1 = null, tmpFolder2 = null;

            try
            {
                //Extract Primitives and Assemblies if needed
                if (!Environment.DatabaseExtracted)
                {
                    NotifyProgressStatus("Extracting primitives and assemblies from 'db.lif'");

                    string dbLifPath = Environment.GetLifFilePath(LddLif.DB);
                    if (!File.Exists(dbLifPath))
                        return;

                    using (var dbLif = LifFile.Open(dbLifPath))
                    {
                        var primitivesFolder = dbLif.GetFolder("Primitives");
                        var assembliesFolder = dbLif.GetFolder("Assemblies");

                        if (primitivesFolder == null || assembliesFolder == null)
                            return;

                        tmpFolder1 = FileHelper.GetTempDirectory();
                        primitivesFolder.ExtractContent(tmpFolder1, CancellationToken, null);

                        if (IsCancellationRequested)
                            return;

                        tmpFolder2 = FileHelper.GetTempDirectory();
                        assembliesFolder.ExtractContent(tmpFolder2, CancellationToken, null);
                    }
                }

                if (IsCancellationRequested)
                    return;

                string primitivesDir = tmpFolder1 ?? Environment.GetAppDataSubDir("db\\Primitives");
                ImportPrimitivesFromDirectory(primitivesDir);

                if (IsCancellationRequested)
                    return;

                string assembliesDir = tmpFolder1 ?? Environment.GetAppDataSubDir("db\\Assemblies");
                ImportAssembliesFromDirectory(assembliesDir);

                if (IsCancellationRequested)
                    return;
            }
            finally
            {
                if (!string.IsNullOrEmpty(tmpFolder1))
                    Task.Factory.StartNew(() => FileHelper.DeleteFileOrFolder(tmpFolder1, true, true));

                if (!string.IsNullOrEmpty(tmpFolder2))
                    Task.Factory.StartNew(() => FileHelper.DeleteFileOrFolder(tmpFolder2, true, true));
            }
        }

        private void ImportPrimitivesFromDirectory(string directory)
        {
            NotifyProgressStatus("Fetching primitive files...");
            var primitivesFiles = Directory.GetFiles(directory, "*.xml");
            NotifyTaskStart("Importing primitives...", primitivesFiles.Length);

            using (var trans = Connection.BeginTransaction())
            using (var cmd = Connection.CreateCommand())
            {
                DbHelper.InitializeInsertCommand<LddPart>(cmd, x =>
                new
                {
                    x.DesignID,
                    x.Name,
                    x.Aliases,
                    x.SubMaterials,
                    x.IsAssembly
                });

                int totalProcessed = 0;

                foreach (var primitiveFile in primitivesFiles)
                {
                    if (IsCancellationRequested)
                        break;

                    try
                    {
                        var primitiveInfo = Primitive.Load(primitiveFile);
                        string aliasesStr = string.Join(";", primitiveInfo.Aliases.Where(x => x != primitiveInfo.ID));
                        if (!string.IsNullOrEmpty(aliasesStr))
                            aliasesStr = $";{aliasesStr};";

                        string subMaterialsStr = primitiveInfo.SubMaterials != null ? 
                            string.Join(";", primitiveInfo.SubMaterials) : string.Empty;

                        DbHelper.InsertWithParameters(cmd,
                            primitiveInfo.ID.ToString(),
                            primitiveInfo.Name,
                            aliasesStr,
                            subMaterialsStr,
                            false);
                    }
                    catch (Exception ex)
                    { 
                    
                    }

                    ReportProgress(++totalProcessed, primitivesFiles.Length);
                }

                trans.Commit();
            }
        }

        private void ImportAssembliesFromDirectory(string directory)
        {
            NotifyProgressStatus("Fetching assembly files...");
            var assemblyFiles = Directory.GetFiles(directory, "*.lxfml");
            NotifyTaskStart("Importing assemblies...", assemblyFiles.Length);

            using (var trans = Connection.BeginTransaction())
            using (var mainCmd = Connection.CreateCommand())
            using (var subCmd = Connection.CreateCommand())
            using (var updateCmd = Connection.CreateCommand())
            {
                DbHelper.InitializeInsertCommand<LddPart>(mainCmd, x =>
                new
                {
                    x.DesignID,
                    x.Name,
                    x.Aliases,
                    x.IsAssembly
                });

                DbHelper.InitializeInsertCommand<LddAssemblyPart>(subCmd, x =>
                new
                {
                    x.AssemblyID,
                    x.PartID,
                    x.DefaultMaterials
                });

                int totalProcessed = 0;

                foreach (var assemFile in assemblyFiles)
                {
                    if (IsCancellationRequested)
                        break;

                    try
                    {
                        var assemInfo = LDDModder.LDD.Models.Assembly.Load(assemFile);
                        string assemID = assemInfo.ID.ToString();

                        //skip decomposed assemblies
                        if (assemInfo.Bricks.Count == 1 &&
                            assemInfo.ID.ToString() == assemInfo.Bricks[0].DesignID)
                            continue;



                        if (assemInfo.Bricks.Any(x => x.DesignID == assemInfo.ID.ToString()))
                        {

                            updateCmd.CommandText = $"UPDATE {DbHelper.GetTableName<LddPart>()} SET IsAssembly = 1 WHERE DesignID = '{assemInfo.ID}'";
                            updateCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            DbHelper.InsertWithParameters(
                                mainCmd,
                                assemID,
                                assemInfo.Name,
                                string.Join(";", assemInfo.Aliases),
                                true
                            );
                        }

                        foreach (var subPart in assemInfo.Bricks)
                        {
                            DbHelper.InsertWithParameters(
                                subCmd,
                                assemInfo.ID.ToString(),
                                subPart.Part?.DesignID ?? subPart.DesignID,
                                string.Join(";", subPart.Part.Materials)
                            );
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    ReportProgress(++totalProcessed, assemblyFiles.Length);
                }

                trans.Commit();
            }
        }

        public void ImportLddElements()
        {
            NotifyIndefiniteProgress("Loading LDD palette file...");

            var lddPaletteFile = GetLddPaletteFile(Environment);
            if (lddPaletteFile == null)
                return;

            var lddPalette = lddPaletteFile.Palettes.First();

            var duplicatedElements = lddPalette.Items.GroupBy(x => x.ElementID).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

            var manualLddElements = new List<LddElement>();

            using (var dbContext = new PaletteDbContext(Connection))
            {
                manualLddElements.AddRange(
                    dbContext.LddElements.Where(x => x.Flag != 1)
                );
            }
            var addedElements = new HashSet<string>();

            using (var trans = Connection.BeginTransaction())
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<ElementDecoration>()}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<ElementMaterial>()}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<ElementPart>()}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<LddElement>()}";
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<LddElement>()}'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<ElementPart>()}'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<ElementMaterial>()}'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{DbHelper.GetTableName<ElementDecoration>()}'";
                cmd.ExecuteNonQuery();
                trans.Commit();
            }

            int totalToProcess = lddPalette.Items.Count + manualLddElements.Count;
            NotifyTaskStart("Importing LDD elements...", totalToProcess);

            using (var trans = Connection.BeginTransaction())
            using (var elemCmd = Connection.CreateCommand())
            using (var partCmd = Connection.CreateCommand())
            using (var decCmd = Connection.CreateCommand())
            using (var matCmd = Connection.CreateCommand())
            using (var rowidCmd = Connection.CreateCommand())
            {
                DbHelper.InitializeInsertCommand<LddElement>(elemCmd, x =>
                new
                {
                    x.ElementID,
                    x.DesignID,
                    x.IsAssembly,
                    x.Flag
                });
                //elemCmd.Parameters[$"${nameof(LddElement.Flag)}"].Value = 1;

                DbHelper.InitializeInsertCommand<ElementPart>(partCmd, x =>
                new
                {
                    x.ElementID,
                    x.PartID,
                    x.MaterialID
                });

                DbHelper.InitializeInsertCommand<ElementDecoration>(decCmd, x =>
                new
                {
                    x.ElementPartID,
                    x.SurfaceID,
                    x.DecorationID
                });

                DbHelper.InitializeInsertCommand<ElementMaterial>(matCmd, x =>
                new
                {
                    x.ElementPartID,
                    x.SurfaceID,
                    x.MaterialID
                });

                rowidCmd.CommandText = "select last_insert_rowid()";

                int totalProcessed = 0;

                foreach (var item in lddPalette.Items)
                {
                    if (IsCancellationRequested)
                        break;
   
                    if (string.IsNullOrEmpty(item.ElementID) || duplicatedElements.Contains(item.ElementID))
                    {
                        ReportProgress(++totalProcessed, totalToProcess);
                        continue;
                    }

                    bool isAssembly = item is LDDModder.LDD.Palettes.Assembly;
                    DbHelper.InsertWithParameters(elemCmd, item.ElementID, item.DesignID.ToString(), isAssembly, 1);

                    addedElements.Add(item.ElementID);

                    var partConfigs = new List<ElementPart>();

                    if (item is LDDModder.LDD.Palettes.Assembly assy)
                    {
                        foreach (var part in assy.Parts)
                            partConfigs.Add(AssemblyPartToConfig(part));
                    }
                    else if (item is LDDModder.LDD.Palettes.Brick brick)
                        partConfigs.Add(PaletteBrickToConfig(brick));

                    foreach (var part in partConfigs)
                    {
                        int partID = DbHelper.InsertWithParameters(partCmd, rowidCmd, item.ElementID, part.PartID, part.MaterialID);

                        foreach (var dec in part.Decorations)
                            DbHelper.InsertWithParameters(decCmd, partID, dec.SurfaceID, dec.DecorationID);

                        foreach (var mat in part.SubMaterials)
                            DbHelper.InsertWithParameters(matCmd, partID, mat.SurfaceID, mat.MaterialID);
                    }

                    ReportProgress(++totalProcessed, totalToProcess);
                }

                foreach (var oldElem in manualLddElements)
                {
                    if (addedElements.Contains(oldElem.ElementID))
                        continue;

                    DbHelper.InsertWithParameters(elemCmd, oldElem.ElementID, oldElem.DesignID, oldElem.IsAssembly, oldElem.Flag);

                    foreach (var part in oldElem.Parts)
                    {
                        int partID = DbHelper.InsertWithParameters(partCmd, rowidCmd, oldElem.ElementID, part.PartID, part.MaterialID);
                        foreach (var dec in part.Decorations)
                            DbHelper.InsertWithParameters(decCmd, partID, dec.SurfaceID, dec.DecorationID);

                        foreach (var mat in part.SubMaterials)
                            DbHelper.InsertWithParameters(matCmd, partID, mat.SurfaceID, mat.MaterialID);
                    }

                    ReportProgress(++totalProcessed, totalToProcess);
                }

                trans.Commit();
            }
        }

        private static PaletteFile GetLddPaletteFile(LDDEnvironment environment)
        {
            var appDataPalettes = environment.GetAppDataSubDir("Palettes");

            if (File.Exists(Path.Combine(appDataPalettes, "LDD.lif")))
                return PaletteFile.FromLif(Path.Combine(appDataPalettes, "LDD.lif"));

            if (Directory.Exists(Path.Combine(appDataPalettes, "LDD")))
                return PaletteFile.FromDirectory(Path.Combine(appDataPalettes, "LDD"));

            string dbLifPath = environment.GetLifFilePath(LddLif.DB);

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

        private static ElementPart AssemblyPartToConfig(LDDModder.LDD.Palettes.Assembly.Part part)
        {
            var partConfig = new Models.LDD.ElementPart()
            {
                PartID = part.DesignID.ToString(),
                MaterialID = part.MaterialID,
            };
            if (part.Decorations?.Any() ?? false)
            {
                foreach (var deco in part.Decorations)
                {
                    partConfig.Decorations.Add(new Models.LDD.ElementDecoration()
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
                    partConfig.SubMaterials.Add(new Models.LDD.ElementMaterial()
                    {
                        SurfaceID = subMat.SurfaceID,
                        MaterialID = subMat.MaterialID
                    });
                }
            }
            return partConfig;
        }

        private static ElementPart PaletteBrickToConfig(LDDModder.LDD.Palettes.Brick part)
        {
            var partConfig = new Models.LDD.ElementPart()
            {
                PartID = part.DesignID.ToString(),
                MaterialID = part.MaterialID,
            };
            if (part.Decorations?.Any() ?? false)
            {
                foreach (var deco in part.Decorations)
                {
                    partConfig.Decorations.Add(new Models.LDD.ElementDecoration()
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
                    partConfig.SubMaterials.Add(new Models.LDD.ElementMaterial()
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
