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
        public string RelationType { get; set; }

        [NotMapped]
        public RbRelationType RelationTypeFlag
        {
            get => (RbRelationType)RelationType[0];
            set 
            {
                if ((char)value != '\0')
                    RelationType = ((char)value).ToString();
            }
        }
    }
}
