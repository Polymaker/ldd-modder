using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LDDModder.PaletteMaker.Models.Rebrickable
{
    [Table("RbColors")]
    public class RbColor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public string Name { get; set; }

        public string RgbHex { get; set; }

        public bool IsTransparent { get; set; }

        public virtual ICollection<RbColorMatch> ColorMatches { get; set; }

        [NotMapped]
        public string ColorCode => $"#FF{RgbHex}";

        public RbColor()
        {
            ColorMatches = new List<RbColorMatch>();
        }

        [NotMapped]
        public string DisplayName => $"{Name} ({ID})";
    }
}
