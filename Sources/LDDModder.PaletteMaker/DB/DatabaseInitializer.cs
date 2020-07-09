using System.Data.SQLite;
using System.Threading;
using LDDEnvironment = LDDModder.LDD.LDDEnvironment;
using LDDModder.PaletteMaker.Models;
using System;

namespace LDDModder.PaletteMaker.DB
{
    static class DatabaseInitializer
    {
        [Flags]
        public enum InitializationStep
        {
            LddPartsAndElements = 1,
            RebrickableBaseData = 2,
            RebrickablePartsAndRelationships = 4,
            RebrickableLddMappings = 8,
            RebrickableSets = 16,
            RebrickableSetParts = 32,
            All = LddPartsAndElements | RebrickableBaseData | RebrickablePartsAndRelationships | RebrickableLddMappings | RebrickableSets | RebrickableSetParts
        }

        public static void InitializeOrUpdateDatabase(string databasePath, 
            InitializationStep steps, 
            CancellationToken cancellationToken,
            IDbInitProgressHandler progressHandler)
        {
            using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            {
                conn.Open();

                int totalSteps = 0;

                foreach (InitializationStep step in Enum.GetValues(typeof(InitializationStep)))
                {
                    if (step != InitializationStep.All && steps.HasFlag(step))
                        totalSteps++;
                }

                progressHandler?.OnInitImportTask(totalSteps);

                if (steps.HasFlag(InitializationStep.LddPartsAndElements))
                {
                    var lddImporter = new LddDataImporter(conn, LDDEnvironment.Current, cancellationToken);
                    lddImporter.ProgressHandler = progressHandler;
                    lddImporter.ImportAllData();
                }

                if (cancellationToken.IsCancellationRequested)
                    return;

                if (steps.HasFlag(InitializationStep.RebrickableBaseData) || 
                    steps.HasFlag(InitializationStep.RebrickablePartsAndRelationships) ||
                    steps.HasFlag(InitializationStep.RebrickableSets) ||
                    steps.HasFlag(InitializationStep.RebrickableSetParts))
                {
                    var rbImporter = new RebrickableDataImporter(conn, LDDEnvironment.Current, cancellationToken);
                    rbImporter.ProgressHandler = progressHandler;
                    rbImporter.InventoriesCsvFile = @"C:\Users\JWTurner\Downloads\inventories.csv";
                    rbImporter.InventoryPartsCsvFile = @"C:\Users\JWTurner\Downloads\inventory_parts.csv";
                    
                    if (steps.HasFlag(InitializationStep.RebrickableBaseData))
                        rbImporter.ImportBaseData();

                    if (cancellationToken.IsCancellationRequested)
                        return;

                    if (steps.HasFlag(InitializationStep.RebrickablePartsAndRelationships))
                        rbImporter.ImportPartsAndRelationships();

                    if (cancellationToken.IsCancellationRequested)
                        return;

                    if (steps.HasFlag(InitializationStep.RebrickableSets))
                        rbImporter.ImportSets();

                    if (cancellationToken.IsCancellationRequested)
                        return;

                    if (steps.HasFlag(InitializationStep.RebrickableSetParts))
                        rbImporter.ImportSetParts();
                }

                if (cancellationToken.IsCancellationRequested)
                    return;

                if (steps.HasFlag(InitializationStep.RebrickableLddMappings))
                    InitializeDefaultMappings(databasePath, progressHandler);


            }
        }

        //public static void ImportBaseData(string databasePath, CancellationToken cancellationToken)
        //{
        //    using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
        //    {
        //        conn.Open();

        //        var lddImporter = new LddDataImporter(conn, LDDEnvironment.Current, cancellationToken);
        //        lddImporter.ImportAllData();

        //        if (cancellationToken.IsCancellationRequested)
        //            return;

        //        //var rbImporter = new RebrickableDataImporter(conn, LDDEnvironment.Current, cancellationToken);
        //        //rbImporter.ImportAllData();

        //    }
        //}

        //public static void ImportLddPartsAndAssemblies(string databasePath, CancellationToken cancellationToken)
        //{
        //    using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
        //    {
        //        var lddImporter = new LddDataImporter(conn, LDDEnvironment.Current, cancellationToken);
        //        lddImporter.ImportAllData();
        //    }
        //}

        //public static void ImportRebrickableData(string databasePath, CancellationToken cancellationToken)
        //{
        //    using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
        //    {
        //        conn.Open();
        //        var rbImporter = new RebrickableDataImporter(conn, LDDEnvironment.Current, cancellationToken);
        //        rbImporter.ImportAllData();
        //    }
        //}

        public static void InitializeDefaultMappings(string databasePath, IDbInitProgressHandler progressHandler)
        {
            progressHandler.OnBeginStep("Initializing Rebrickable to LDD mappings");
            progressHandler.OnReportIndefiniteProgress();
            progressHandler.OnReportProgressStatus(string.Empty);

            using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<PartMapping>()} WHERE MatchLevel in (0, 1)";
                    cmd.ExecuteNonQuery();

                    DbHelper.InitializeInsertCommand<PartMapping>(cmd, x => new { x.RebrickableID, x.LegoID, x.MatchLevel, x.IsActive });
                    
                    string insertIntoSQL = cmd.CommandText.Substring(0, cmd.CommandText.IndexOf("VALUES"));
                    cmd.CommandText = insertIntoSQL + " SELECT r.PartID, r.PartID, 0, 1 FROM RbParts r " +
                        "INNER JOIN LddParts l on l.DesignID = r.PartID " +
                        "WHERE r.IsPrintOrPattern = 0";
                    cmd.ExecuteNonQuery(); // Insert exact matches

                    cmd.CommandText = insertIntoSQL + " SELECT r.PartID, l.DesignID, 1, 1 FROM RbParts r " +
                        "INNER JOIN LddParts l on l.Aliases LIKE '%;' || r.PartID || ';%' " +
                        "WHERE LENGTH(l.Aliases) > 0 AND r.IsPrintOrPattern = 0 AND l.DesignID <> r.PartID";
                    cmd.ExecuteNonQuery(); // Insert exact matches on LDD aliases

                    cmd.CommandText = $"DELETE FROM {DbHelper.GetTableName<PartMapping>()} WHERE MatchLevel = 1 " +
                        $"AND RebrickableID in (SELECT pm.RebrickableID from {DbHelper.GetTableName<PartMapping>()} pm WHERE pm.MatchLevel = 0) ";
                    cmd.ExecuteNonQuery(); // remove possible duplicates

                    cmd.CommandText = insertIntoSQL + " SELECT r.PartID, '73200', 1, 1 FROM RbParts r " +
                        "WHERE r.IsPrintOrPattern = 0 and r.PartID like '970c%'";
                    cmd.ExecuteNonQuery(); // Insert minifigs legs alternates

                    cmd.CommandText = insertIntoSQL + "SELECT r1.PartID, '76382', 1, 1 FROM RbParts r1 " +
                        "WHERE r1.IsPrintOrPattern = 0 and r1.PartID like '973c%' " +
                        "UNION " +
                        "SELECT r2.ParentPartID, '76382', 1, 1 FROM RbParts r2 " +
                        "WHERE r2.IsPrintOrPattern = 1 and r2.ParentPartID like '973c%' ";
                    cmd.ExecuteNonQuery(); //Insert minifigs toros alternates

                    cmd.CommandText = insertIntoSQL + " SELECT r.PartID, l.DesignID, 2, 1 FROM RbParts r " +
                        "INNER JOIN LddParts l on l.DesignID = substr(r.PartID, 1, length(r.PartID) - 1) " +
                        "WHERE  r.IsPrintOrPattern = 0 and r.IsAssembly = 0 and (r.PartID like '%a' or r.PartID like '%b' or r.PartID like '%c')";
                    cmd.ExecuteNonQuery(); // Insert possible alternates (e.g.: 3245a, 3245b -> 3245)

                    trans.Commit();
                }
            }
        }
    }
}
