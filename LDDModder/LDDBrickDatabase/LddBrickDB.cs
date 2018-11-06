using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDBrickDatabase
{
    public class LddBrickDB : DbContext
    {

        

        public DbSet<Brick> Bricks { get; set; }
        public DbSet<Assembly> Assemblies { get; set; }
        public DbSet<BrickModel> BrickModels { get; set; }
        public DbSet<Palette> Palettes { get; set; }
        public DbSet<Decoration> Decorations { get; set; }
        public DbSet<DecorationMapping> DecorationMappings { get; set; }

        public LddBrickDB(SQLiteConnection conn) : base(conn, false)
        {

        }

        //public DbSet<DatabaseInfo> DatabaseInfos { get; set; }

    }
}
