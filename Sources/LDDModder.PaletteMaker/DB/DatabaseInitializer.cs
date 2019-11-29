using HtmlAgilityPack;
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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LDDEnvironment = LDDModder.LDD.LDDEnvironment;
using System.Reflection;
using LDDModder.PaletteMaker.Models.Rebrickable;
using LDDModder.Utilities;
using LDDModder.PaletteMaker.Models;

namespace LDDModder.PaletteMaker.DB
{
    static class DatabaseInitializer
    {
        public static void ImportBaseData(string databasePath, CancellationToken cancellationToken)
        {
            using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            {
                conn.Open();

                var lddImporter = new LddDataImporter(conn, LDDEnvironment.Current, cancellationToken);
                lddImporter.ImportAllData();

                if (cancellationToken.IsCancellationRequested)
                    return;

                var rbImporter = new RebrickableDataImporter(conn, LDDEnvironment.Current, cancellationToken);
                rbImporter.ImportAllData();

            }
        }

        public static void ImportRebrickableData(string databasePath, CancellationToken cancellationToken)
        {
            using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            {
                conn.Open();
                var rbImporter = new RebrickableDataImporter(conn, LDDEnvironment.Current, cancellationToken);
                rbImporter.ImportAllData();
            }
        }

        public static void InitializeDefaultMappings(string databasePath)
        {
            using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            //using (var db = new PaletteDbContext(conn))
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
