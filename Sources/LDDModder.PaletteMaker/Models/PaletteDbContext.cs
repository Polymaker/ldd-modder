using LDDModder.PaletteMaker.Models.Palettes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Models
{
    public class PaletteDbContext : DbContext
    {
        
        #region LDD Elements

        public DbSet<LegoElement> LegoElements { get; set; }

        public DbSet<PartConfiguration> Configurations { get; set; }

        #endregion

        public PaletteDbContext(System.Data.SQLite.SQLiteConnection conn) : base(conn, false)
        {
            Database.SetInitializer<PaletteDbContext>(null);
        }

        public PaletteDbContext(string nameOrConnectionString) : base(new System.Data.SQLite.SQLiteConnection(nameOrConnectionString), true)
        {
            Database.SetInitializer<PaletteDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
