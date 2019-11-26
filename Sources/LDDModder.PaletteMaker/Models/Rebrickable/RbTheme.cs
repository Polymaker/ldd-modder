using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.Rebrickable
{
    [Table("RbThemes")]
    public class RbTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public string Name { get; set; }

        public int? ParentThemeID { get; set; }

        [ForeignKey("ParentThemeID")]
        public virtual RbTheme ParentTheme { get; set; }
    }
}
