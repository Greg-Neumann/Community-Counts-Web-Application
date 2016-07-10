namespace CommunityCounts.Models.Master
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ccmaster.district")]
    public partial class district
    {
        public district()
        {
            postcodes = new HashSet<postcode>();
        }

        [Key]
        public int idDistrictCode { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(9)]
        public string DistrictCode { get; set; }

        [Required]
        [StringLength(45)]
        public string Description { get; set; }

        public int idCPDate { get; set; }

        public virtual ICollection<postcode> postcodes { get; set; }
    }
}
