namespace CommunityCounts.Models.Master
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.refdatatype")]
    public partial class refdatatype
    {
        public refdatatype()
        {
            refdatas = new HashSet<refdata>();
        }

        [Key]
        [Column(TypeName = "char")]
        [StringLength(4)]
        public string TypeCode { get; set; }

        [Required]
        [StringLength(45)]
        public string TypeName { get; set; }

        public virtual ICollection<refdata> refdatas { get; set; }
    }
}
