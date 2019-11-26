using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.Rebrickable
{
    [Table("RbPartRelations")]
    public class RbPartRelation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string ParentPartID { get; set; }

        [ForeignKey("ParentPartID")]
        public virtual RbPart ParentPart { get; set; }

        public string ChildPartID { get; set; }

        [ForeignKey("ChildPartID")]
        public virtual RbPart ChildPart { get; set; }

        [Column("RelationType")]
        protected char RelationTypeChar { get; set; }

        [NotMapped]
        public RbRelationType RelationType
        {
            get => (RbRelationType)RelationTypeChar;
            set => RelationTypeChar = (char)value;
        }
    }
}
