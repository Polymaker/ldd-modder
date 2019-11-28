using LDDModder.PaletteMaker.Models;
using LDDModder.PaletteMaker.Models.LDD;
using LDDModder.PaletteMaker.Models.Rebrickable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.DB
{
    public class PaletteDbContext : DbContext
    {

        #region LDD Elements

        public DbSet<LddPart> LddParts { get; set; }

        public DbSet<LddAssemblyPart> AssemblyParts { get; set; }

        public DbSet<LddElement> LddElements { get; set; }

        public DbSet<ElementPart> Configurations { get; set; }

        #endregion

        #region Rebrickable Elements

        public DbSet<RbTheme> Themes { get; set; }

        public DbSet<RbCategory> Categories { get; set; }

        public DbSet<RbColor> Colors { get; set; }

        public DbSet<RbPart> RbParts { get; set; }

        public DbSet<RbPartRelation> RbPartRelationships { get; set; }

        #endregion

        public DbSet<PartMapping> PartMappings { get; set; }

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
            modelBuilder.Entity<RbPartRelation>()
                    .HasRequired(m => m.ParentPart)
                    .WithMany(t => t.Relationships)
                    .HasForeignKey(m => m.ParentPartID)
                    .WillCascadeOnDelete(false);
        }
    }
}
