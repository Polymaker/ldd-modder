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

        public static void InitializeDefaultMappings(string databasePath)
        {
            using (var conn = new SQLiteConnection($"Data Source={databasePath}"))
            using (var db = new PaletteDbContext(conn))
            {
                

            }
        }
    }
}
